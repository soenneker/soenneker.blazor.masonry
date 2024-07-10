using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Blazor.Masonry;

///<inheritdoc cref="IMasonryInterop"/>
public class MasonryInterop : IMasonryInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncSingleton<object> _scriptInitializer;

    public MasonryInterop(IJSRuntime jSRuntime, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton<object>(async (token, _) => {

            await _resourceLoader.ImportModuleAndWaitUntilAvailable("Soenneker.Blazor.Masonry/masonryinterop.js", "MasonryInterop", 100, token).NoSync();
            await _resourceLoader.LoadScriptAndWaitForVariable("https://cdn.jsdelivr.net/npm/masonry-layout@4.2.2/dist/masonry.pkgd.min.js", "Masonry","sha256-Nn1q/fx0H7SNLZMQ5Hw5JLaTRZp0yILA/FRexe19VdI=", cancellationToken: token).NoSync();
        
            return new object();
        });
    }

    public async ValueTask Init(string containerSelector = ".container", string itemSelector = ".row", bool percentPosition = true, float transitionDurationSecs = .2F, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Get(cancellationToken).NoSync();

        var transitionDurationStr = $"{transitionDurationSecs}s";

        await _jsRuntime.InvokeVoidAsync("MasonryInterop.init", cancellationToken, containerSelector, itemSelector, percentPosition, transitionDurationStr);
    }

    public ValueTask DisposeAsync()
    {
        return _resourceLoader.DisposeModule("Soenneker.Blazor.Masonry/masonryinterop.js");
    }
}