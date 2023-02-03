using System.Threading.Tasks;

namespace Soenneker.Blazor.Masonry.Abstract;

public interface IMasonryInterop 
{
    ValueTask Init(string selector = ".div", bool percentPosition = true, float transitionDurationSecs = .2F);
}