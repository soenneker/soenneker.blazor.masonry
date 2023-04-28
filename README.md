[![](https://img.shields.io/nuget/v/Soenneker.Blazor.Masonry.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Masonry/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.masonry/publish.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.masonry/actions/workflows/publish.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blazor.Masonry.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Masonry/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.Masonry
### A Blazor interop library that integrates [Masonry](https://masonry.desandro.com/), the cascading grid layout library

## Installation

```
Install-Package Soenneker.Blazor.Masonry
```

## Usage

1. Insert the interop script, and the library script in `wwwroot/index.html` at the bottom of your `<body>`

```html
<script src="_content/Soenneker.Blazor.Masonry/masonry.js"></script>
<script async src="https://cdn.jsdelivr.net/npm/masonry-layout@4.2.2/dist/masonry.pkgd.min.js" integrity="sha256-Nn1q/fx0H7SNLZMQ5Hw5JLaTRZp0yILA/FRexe19VdI=" crossorigin="anonymous"></script>
```

2. Register the interop within DI (`Program.cs`)

```csharp
public static async Task Main(string[] args)
{
    ...
    builder.Services.AddMasonry();
}
```

3. Inject `IMasonryInterop` within your `App.Razor` file


```csharp
@using Soenneker.Blazor.Masonry.Abstract
@inject IMasonryInterop MasonryInterop
```


4. Use [Bootstrap Rows](https://getbootstrap.com/docs/5.0/examples/masonry/) and columns on the page (`<div class='row'></div>`). Other selectors can be passed into the interop, `.row` is default.

5. Initialize Masonry within your Razor code in the `OnAfterRenderAsync` override

```csharp
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    await MasonryInterop.Init();
}
```