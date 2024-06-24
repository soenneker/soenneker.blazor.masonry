using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;

namespace Soenneker.Blazor.Masonry;

///<inheritdoc cref="IMasonryInterop"/>
public class MasonryInterop : IMasonryInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IModuleImportUtil _moduleImportUtil;

    private bool _initialized;

    public MasonryInterop(IJSRuntime jSRuntime, IModuleImportUtil moduleImportUtil)
    {
        _jsRuntime = jSRuntime;
        _moduleImportUtil = moduleImportUtil;
    }

    private async ValueTask EnsureInitialized(CancellationToken cancellationToken = default)
    {
        if (_initialized)
            return;

        _initialized = true;

        await _moduleImportUtil.Import("Soenneker.Blazor.Masonry/js/masonryinterop.js", cancellationToken);
        await _moduleImportUtil.WaitUntilLoaded("Soenneker.Blazor.Masonry/js/masonryinterop.js", cancellationToken);
    }

    public async ValueTask Init(string containerSelector = ".container", string itemSelector = ".row", bool percentPosition = true, float transitionDurationSecs = .2F, CancellationToken cancellationToken = default)
    {
        await EnsureInitialized(cancellationToken);

        var transitionDurationStr = $"{transitionDurationSecs}s";

        await _jsRuntime.InvokeVoidAsync("MasonryInitializer.init", cancellationToken, containerSelector, itemSelector, percentPosition, transitionDurationStr);
    }
}