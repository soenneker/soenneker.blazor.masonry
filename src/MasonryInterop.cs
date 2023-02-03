using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Blazor.Masonry.Abstract;

namespace Soenneker.Blazor.Masonry;

///<inheritdoc cref="IMasonryInterop"/>
public class MasonryInterop : IMasonryInterop
{
    private readonly IJSRuntime _jsRuntime;

    public MasonryInterop(IJSRuntime jSRuntime)
    {
        _jsRuntime = jSRuntime;
    }

    public ValueTask Init(string selector = ".div", bool percentPosition = true, float transitionDurationSecs = .2F)
    {
        var transitionDurationStr = $"{transitionDurationSecs}s";

        return _jsRuntime.InvokeVoidAsync("initMasonry", selector, percentPosition, transitionDurationStr);
    }
}