[![](https://img.shields.io/nuget/v/Soenneker.Blazor.Masonry.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Masonry/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.masonry/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.masonry/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blazor.Masonry.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Masonry/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.Masonry 🧱
### A lightweight, responsive **Blazor** component for Masonry (the cascading grid layout library) — perfect for image grids, cards, and dynamic content.

## ✨ Features

- Fully compatible with Bootstrap grid classes
- Script auto-loading (CDN or embedded)
- No manual cleanup required

## Installation

```
dotnet add package Soenneker.Blazor.Masonry
```

## Usage

1. Register the interop within DI (`Program.cs`)

```csharp
public static async Task Main(string[] args)
{
    ...
    builder.Services.AddMasonryInteropAsScoped();
}
```

## 🧩 Components

### `Masonry`

Wraps a group of items in a Masonry layout.

```razor
<Masonry AutoRender="true" class="row"> // or if not Bootstrap, a different class
    ...
</Masonry>
```

**Parameters:**

- `AutoRender` – Automatically initializes layout after render

### `MasonryItem`

Wraps a single item in the layout and adds the required class.

```razor
<MasonryItem class="col-md-6">
    <div class="card">...</div>
</MasonryItem>
```

Automatically appends `masonry-item` to the `class` attribute.

---

## 🚀 Example

```razor
<Masonry AutoRender="false" @ref="_masonry" class="row">
    @foreach (var card in _cards)
    {
        <MasonryItem class="col-lg-4 col-md-6 mb-4">
            <div class="card">
                <img src="@card.ImageUrl" class="card-img-top" />
                <div class="card-body">
                    <h5>@card.Title</h5>
                    <p>@card.Text</p>
                </div>
            </div>
        </MasonryItem>
    }
</Masonry>

@code {
    private Masonry _masonry = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Example delay for images to load
            await Task.Delay(3000); 
            await _masonry.Init();
        }
    }
}
```

---

## 🔄 Initialization

- If `AutoRender` is `true`, Masonry initializes on first render.
- If `false`, call `Init()` manually when ready (e.g., after images load).
- **No disposal needed** – cleanup is handled automatically when navigating away.

## 🧠 Manual Interop Usage

If you want full control without using the `Masonry` component, you can use the provided `IMasonryInterop` service directly.

```csharp
@inject IMasonryInterop MasonryInterop
```

### Warmup (Script Load)

```csharp
await MasonryInterop.Warmup(); // Loads Masonry script from CDN
```

Or use the embedded version:

```csharp
await MasonryInterop.Warmup(useCdn: false);
```

### Initialize Layout

```csharp
await MasonryInterop.Init("myElementId");
```

Advanced options:

```csharp
await MasonryInterop.Init(
    elementId: "gallery",
    containerSelector: "#gallery",
    itemSelector: ".masonry-item",
    percentPosition: true,
    transitionDurationSecs: 0.25f,
    useCdn: true
);
```

### Force Layout Recalculation

```csharp
await MasonryInterop.Layout("gallery");
```

### Set Up Mutation Observer

```csharp
await MasonryInterop.CreateObserver("gallery");
```

### Destroy Instance

```csharp
await MasonryInterop.Destroy("gallery");
```

> 💡 **Note:** The `Masonry` component handles these automatically. Use manual interop when building custom wrappers or integrations.