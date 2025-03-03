using System;
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

    private readonly AsyncSingleton _scriptInitializer;

    private const string _modulePath = "Soenneker.Blazor.Masonry/js/masonryinterop.js";
    private const string _moduleNamespace = "MasonryInterop";

    public MasonryInterop(IJSRuntime jSRuntime, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton(async (token, obj) =>
        {
            var useCdn = true;

            if (obj.Length > 0)
                useCdn = (bool)obj[0];

            await _resourceLoader.ImportModuleAndWaitUntilAvailable(_modulePath, _moduleNamespace, 100, token).NoSync();

            if (useCdn)
            {
                await _resourceLoader.LoadScriptAndWaitForVariable("https://cdn.jsdelivr.net/npm/masonry-layout@4.2.2/dist/masonry.pkgd.min.js", "Masonry",
                                         "sha256-Nn1q/fx0H7SNLZMQ5Hw5JLaTRZp0yILA/FRexe19VdI=", cancellationToken: token)
                                     .NoSync();
            }
            else
            {
                await _resourceLoader
                      .LoadScriptAndWaitForVariable("_content/Soenneker.Blazor.Masonry/js/masonry.pkgd.min.js", "Masonry", cancellationToken: token)
                      .NoSync();
            }

            return new object();
        });
    }

    public async ValueTask Warmup(bool useCdn = true, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(cancellationToken, useCdn).NoSync();
    }

    public async ValueTask Init(string id, string containerSelector = ".container", string itemSelector = ".row", bool percentPosition = true,
        float transitionDurationSecs = .2F, bool useCdn = true, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(cancellationToken, useCdn).NoSync();

        var transitionDurationStr = $"{transitionDurationSecs}s";

        await _jsRuntime.InvokeVoidAsync($"{_moduleNamespace}.init", cancellationToken, id, containerSelector, itemSelector, percentPosition, transitionDurationStr)
                        .NoSync();
    }

    public async ValueTask Layout(string id, CancellationToken cancellationToken = default)
    {
        await _jsRuntime.InvokeVoidAsync($"{_moduleNamespace}.layout", cancellationToken, id).NoSync();
    }

    public async ValueTask Destroy(string id, CancellationToken cancellationToken = default)
    {
        await _jsRuntime.InvokeVoidAsync($"{_moduleNamespace}.destroy", cancellationToken, id).NoSync();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await _resourceLoader.DisposeModule(_modulePath).NoSync();

        await _scriptInitializer.DisposeAsync().NoSync();
    }
}
