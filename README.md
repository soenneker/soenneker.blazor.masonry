[![](https://img.shields.io/nuget/v/Soenneker.Blazor.Masonry.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Masonry/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.masonry/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.masonry/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blazor.Masonry.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Masonry/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.Masonry
### A Blazor interop library that integrates [Masonry](https://masonry.desandro.com/), the cascading grid layout library

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
    builder.Services.AddMasonry();
}
```

2. Inject `IMasonryInterop` within your `App.Razor` file


```csharp
@using Soenneker.Blazor.Masonry.Abstract
@inject IMasonryInterop MasonryInterop
```

3. Use [Bootstrap Rows](https://getbootstrap.com/docs/5.0/examples/masonry/) and columns on the page (`<div class='row'></div>`). Other selectors can be passed into the interop, `.row` is default.

4. Initialize Masonry within your Razor code in the `OnAfterRenderAsync` override

```csharp
protected override async Task OnAfterRenderAsync(bool firstRender)
{
	if (firstRender)
		await MasonryInterop.Init();
}
```