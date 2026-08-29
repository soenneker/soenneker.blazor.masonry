[![](https://img.shields.io/nuget/v/Soenneker.Blazor.Masonry.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Masonry/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.masonry/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.masonry/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blazor.Masonry.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Masonry/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.masonry/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.masonry/actions/workflows/codeql.yml)

# Soenneker.Blazor.Masonry

Blazor components and interop for arranging variable-height items with [Masonry](https://masonry.desandro.com/).

## Installation

```bash
dotnet add package Soenneker.Blazor.Masonry
```

```csharp
using Soenneker.Blazor.Masonry.Registrars;

builder.Services.AddMasonryInteropAsScoped();
```

Add the component namespace to `_Imports.razor`:

```razor
@using Soenneker.Blazor.Masonry
```

## Automatic layout

```razor
<Masonry class="row" SizerClass="col-sm-6 col-lg-4">
    @foreach (Card card in cards)
    {
        <MasonryItem class="col-sm-6 col-lg-4 mb-3">
            <article class="card">
                <h2>@card.Title</h2>
                <p>@card.Description</p>
            </article>
        </MasonryItem>
    }
</Masonry>
```

`MasonryItem` always appends the `masonry-item` class. When `SizerClass` is supplied, the container renders an empty `.masonry-sizer` element with those width classes and uses it as the column width. The library does not include Bootstrap; the example classes work only when the application supplies Bootstrap or equivalent CSS.

Automatic initialization runs after the first render. It works best when item dimensions are already stable.

## Images and changing content

Images can change item height after Masonry's first measurement. Initialize only after the images are ready, or call `Layout()` from the image load path:

```razor
<Masonry @ref="_masonry"
         AutoRender="false"
         SizerClass="gallery-column">
    @foreach (Photo photo in photos)
    {
        <MasonryItem class="gallery-column">
            <img src="@photo.Url" alt="@photo.Alt" @onload="ImageLoaded" />
        </MasonryItem>
    }
</Masonry>

@code {
    private Masonry? _masonry;
    private int _loadedImages;
    private bool _initialized;

    private async Task ImageLoaded()
    {
        _loadedImages++;

        if (_loadedImages == photos.Count)
        {
            await _masonry!.Init();
            _initialized = true;
        }
        else if (_initialized)
            await _masonry!.Layout();
    }
}
```

When adding or removing rendered items after initialization, call `Layout()` after Blazor has rendered the new DOM. The built-in observer watches removal of the entire Masonry container for cleanup; it does not automatically re-layout child mutations or image loads.

## Asset source

The component loads a pinned Masonry script from jsDelivr with an integrity check. For direct interop usage, `Warmup(useCdn: false)` or `Init(..., useCdn: false)` selects the packaged script instead. The scoped loader initializes once, so use the same source choice throughout a scope.

Most applications should use the components. `IMasonryInterop` is available to custom wrapper authors and supports `Init`, `Layout`, `CreateObserver`, and `Destroy` by element ID. Invalid selectors and calls to `Layout` before initialization surface as JavaScript interop errors rather than being silently ignored.
