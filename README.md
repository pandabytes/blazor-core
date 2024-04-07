# Blazor Core

# Blazor.Interop
This library provides a base class that consumers can use to
implement their own JS modules (`BaseJsModule`).

More importantly this library provides a TS module that
serializes/deserializeC# callbacks (`Func`, `Action`, etc.) to JS.
This allows C# code to pass let's say a `Func<>` to JS, and JS code
can invoke the C# callback. To use this functionality you must
have a reference to a `DotNetCallbackJsModule` object and then
call its `ImportAsync()` to import the `dotnet-callback.js` module.
Then call `RegisterAttachReviverAsync()`.

Your code in `Program.cs` may look like this.
```cs
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
  .AddSingleton<DotNetCallbackJsModule>()
  .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var webHost = builder.Build();

// Only need to import and register once
var dotnetCallbackModule = webHost.Services.GetRequiredService<DotNetCallbackJsModule>();
await dotnetCallbackModule.ImportAsync();
await dotnetCallbackModule.RegisterAttachReviverAsync();
await dotnetCallbackModule.DisposeAsync();

await webHost.RunAsync();
```

Alternatively you can just call the extension method `RegisterAttachReviverAsync` to do
what's described above.
```cs
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
  .AddSingleton<DotNetCallbackJsModule>()
  .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var webHost = builder.Build();

// Only need to import and register once
await webHost.Services.RegisterAttachReviverAsync();

await webHost.RunAsync();
```

## Example
Your custom module may look like this.

In your `math.ts`:
```ts
export function add(a: number, b: number): number {
  return a + b;
}
```

Define your module like this, `MathJsModule.cs`:
```cs
public sealed class MathJsModule : BaseJsModule
{
  /// <inheritdoc/>
  protected override string ModulePath { get; }

  public MathJsModule(IJSRuntime jSRuntime) : base(jSRuntime)
  {
    var customPath = "js/math.js";
    ModulePath = $"{ModulePrefixPath}/{customPath}";
  }

  public async Task<int> AddAsync(int a, int b)
    => await Module.InvokeAsync<int>("add", a, b);
}
```

Then in your application code (most likely in Blazor), 
add the module class to your DI container, and use the module like this:
```razor
@inject MathJsModule MathModule

<p>Sum is @_sum</p>

@code
{
  private int _sum;

  protected override async Task OnInitializedAsync()
  {
    await base.OnInitializedAsync();

    // You must first load the module otherwise
    // using it will cause exception
    await MathModule.ImportAsync();

    _sum = await MathModule.AddAsync(3, 2);
  }
}
```
