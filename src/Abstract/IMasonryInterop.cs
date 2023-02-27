using System.Threading.Tasks;

namespace Soenneker.Blazor.Masonry.Abstract;

/// <summary>
/// A small Blazor interop library that integrates Masonry (https://masonry.desandro.com), the cascading grid layout library
/// </summary>
public interface IMasonryInterop 
{
    /// <summary>
    /// Initialize Masonry within your Razor code in the `OnAfterRenderAsync` override
    /// </summary>
    ValueTask Init(string selector = ".row", bool percentPosition = true, float transitionDurationSecs = .2F);
}