using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Blazor.Masonry;

///<inheritdoc cref="IMasonryInterop"/>
public class MasonryInterop : IMasonryInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IModuleImportUtil _moduleImportUtil;

    private readonly AsyncSingleton<object> _scriptInitializer;

    public MasonryInterop(IJSRuntime jSRuntime, IModuleImportUtil moduleImportUtil)
    {
        _jsRuntime = jSRuntime;
        _moduleImportUtil = moduleImportUtil;

        _scriptInitializer = new AsyncSingleton<object>(async objects => {

            var cancellationToken = (CancellationToken)objects[0];

            await _moduleImportUtil.Import("Soenneker.Blazor.Masonry/masonryinterop.js", cancellationToken);
            await _moduleImportUtil.WaitUntilLoadedAndAvailable("Soenneker.Blazor.Masonry/masonryinterop.js", "MasonryInitializer", 100, cancellationToken);

            return new object();
        });
    }

    public async ValueTask Init(string containerSelector = ".container", string itemSelector = ".row", bool percentPosition = true, float transitionDurationSecs = .2F, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Get(cancellationToken).NoSync();

        var transitionDurationStr = $"{transitionDurationSecs}s";

        await _jsRuntime.InvokeVoidAsync("MasonryInitializer.init", cancellationToken, containerSelector, itemSelector, percentPosition, transitionDurationStr);
    }

    public ValueTask DisposeAsync()
    {
        return _moduleImportUtil.DisposeModule("Soenneker.Blazor.Masonry/masonryinterop.js");
    }
}