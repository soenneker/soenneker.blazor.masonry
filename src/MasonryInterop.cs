using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;

namespace Soenneker.Blazor.Masonry;

///<inheritdoc cref="IMasonryInterop"/>
public sealed class MasonryInterop : IMasonryInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncInitializer<bool> _scriptInitializer;

    private const string _modulePath = "Soenneker.Blazor.Masonry/js/masonryinterop.js";
    private const string _moduleName = "MasonryInterop";

    private readonly CancellationScope _cancellationScope = new();

    public MasonryInterop(IJSRuntime jSRuntime, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncInitializer<bool>(Initialize);
    }

    private async ValueTask Initialize(bool useCdn, CancellationToken token)
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
    }

    public ValueTask Warmup(bool useCdn = true, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _scriptInitializer.Init(useCdn, linked);
    }

    public async ValueTask Init(string elementId, string? containerSelector = null, string itemSelector = ".masonry-item", string? columnWidthSelector = null,
        bool percentPosition = true, float transitionDurationSecs = .2F, bool useCdn = true, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _scriptInitializer.Init(useCdn, linked);

            containerSelector ??= $"#{elementId}";

            var transitionDurationStr = $"{transitionDurationSecs}s";

            await _jsRuntime.InvokeVoidAsync("MasonryInterop.init", linked, elementId, containerSelector, itemSelector, columnWidthSelector,
                percentPosition, transitionDurationStr);
        }
    }

    public ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("MasonryInterop.createObserver", linked, elementId);
    }

    public async ValueTask Layout(string elementId, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _scriptInitializer.Init(true, linked);
            await _jsRuntime.InvokeVoidAsync("MasonryInterop.layout", linked, elementId);
        }
    }

    public ValueTask Destroy(string elementId, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("MasonryInterop.destroy", linked, elementId);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_modulePath);

        await _scriptInitializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}