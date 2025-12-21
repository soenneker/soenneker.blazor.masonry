using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Asyncs.Initializers;

namespace Soenneker.Blazor.Masonry;

///<inheritdoc cref="IMasonryInterop"/>
public sealed class MasonryInterop : IMasonryInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncInitializer<bool> _scriptInitializer;

    private const string _modulePath = "Soenneker.Blazor.Masonry/js/masonryinterop.js";
    private const string _moduleName = "MasonryInterop";

    public MasonryInterop(IJSRuntime jSRuntime, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncInitializer<bool>(async (useCdn, token) =>
        {
            if (useCdn)
            {
                await _resourceLoader.LoadScriptAndWaitForVariable("https://cdn.jsdelivr.net/npm/masonry-layout@4.2.2/dist/masonry.pkgd.min.js", "Masonry",
                    "sha256-Nn1q/fx0H7SNLZMQ5Hw5JLaTRZp0yILA/FRexe19VdI=", cancellationToken: token);
            }
            else
            {
                await _resourceLoader.LoadScriptAndWaitForVariable("_content/Soenneker.Blazor.Masonry/js/masonry.pkgd.min.js", "Masonry",
                    cancellationToken: token);
            }

            await _resourceLoader.ImportModuleAndWaitUntilAvailable(_modulePath, _moduleName, 100, token);
        });
    }

    public ValueTask Warmup(bool useCdn = true, CancellationToken cancellationToken = default)
    {
        return _scriptInitializer.Init(useCdn, cancellationToken);
    }

    public async ValueTask Init(string elementId, string? containerSelector = null, string itemSelector = ".masonry-item", string? columnWidthSelector = null,
        bool percentPosition = true, float transitionDurationSecs = .2F, bool useCdn = true, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(useCdn, cancellationToken);

        containerSelector ??= $"#{elementId}";

        var transitionDurationStr = $"{transitionDurationSecs}s";

        await _jsRuntime.InvokeVoidAsync($"{_moduleName}.init", cancellationToken, elementId, containerSelector, itemSelector, columnWidthSelector,
            percentPosition, transitionDurationStr);
    }

    public ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.createObserver", cancellationToken, elementId);
    }

    public async ValueTask Layout(string elementId, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(true, cancellationToken);

        await _jsRuntime.InvokeVoidAsync($"{_moduleName}.layout", cancellationToken, elementId);
    }

    public ValueTask Destroy(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.destroy", cancellationToken, elementId);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_modulePath);

        await _scriptInitializer.DisposeAsync();
    }
}