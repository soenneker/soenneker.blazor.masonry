using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;

namespace Soenneker.Blazor.Masonry;

///<inheritdoc cref="IMasonryInterop"/>
public sealed class MasonryInterop : IMasonryInterop
{
    private const string _modulePath = "/_content/Soenneker.Blazor.Masonry/js/masonryinterop.js";

    private readonly IResourceLoader _resourceLoader;
    private readonly IModuleImportUtil _moduleImportUtil;

    private readonly AsyncInitializer<bool> _scriptInitializer;

    private readonly CancellationScope _cancellationScope = new();

    public MasonryInterop(IResourceLoader resourceLoader, IModuleImportUtil moduleImportUtil)
    {
        _resourceLoader = resourceLoader;
        _moduleImportUtil = moduleImportUtil;

        _scriptInitializer = new AsyncInitializer<bool>(Initialize);
    }

    private static string NormalizeContentUri(string uri)
    {
        if (string.IsNullOrEmpty(uri) || uri.Contains("://", StringComparison.Ordinal))
            return uri;

        return uri[0] == '/' ? uri : "/" + uri;
    }

    private async ValueTask Initialize(bool useCdn, CancellationToken token)
    {
        if (useCdn)
        {
            await _resourceLoader.LoadScriptAndWaitForVariable(
                "https://cdn.jsdelivr.net/npm/masonry-layout@4.2.2/dist/masonry.pkgd.min.js",
                "Masonry",
                "sha256-Nn1q/fx0H7SNLZMQ5Hw5JLaTRZp0yILA/FRexe19VdI=",
                cancellationToken: token);
        }
        else
        {
            await _resourceLoader.LoadScriptAndWaitForVariable(NormalizeContentUri("_content/Soenneker.Blazor.Masonry/js/masonry.pkgd.min.js"), "Masonry",
                cancellationToken: token);
        }

        _ = await _moduleImportUtil.GetContentModuleReference(_modulePath, token);
    }

    public async ValueTask Warmup(bool useCdn = true, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
            await _scriptInitializer.Init(useCdn, linked);
    }

    public async ValueTask Init(string elementId, string? containerSelector = null, string itemSelector = ".masonry-item", string? columnWidthSelector = null,
        bool percentPosition = true, float transitionDurationSecs = .2F, bool useCdn = true, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await _scriptInitializer.Init(useCdn, linked);

            containerSelector ??= $"#{elementId}";

            var transitionDurationStr = $"{transitionDurationSecs}s";

            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("init", linked, elementId, containerSelector, itemSelector, columnWidthSelector, percentPosition,
                transitionDurationStr);
        }
    }

    public async ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("createObserver", linked, elementId);
        }
    }

    public async ValueTask Layout(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await _scriptInitializer.Init(true, linked);
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("layout", linked, elementId);
        }
    }

    public async ValueTask Destroy(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("destroy", linked, elementId);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _moduleImportUtil.DisposeContentModule(_modulePath);

        await _scriptInitializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}
