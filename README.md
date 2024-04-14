# Blazor Core

# Blazor.Interop
This library provides a base class that consumers can use to
implement their own JS modules (`BaseJsModule`).

## Callback Interop
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

// Only need to import and register once and can be disposed right away
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

await webHost.Services.RegisterAttachReviverAsync();
await webHost.RunAsync();
```

Then you can use it like this.
```cs
// Action
Action action = () => Console.WriteLine("Hello World!");
ActionCallbackInterop actionCallbackInterop = new(action);

// Func
Func<int, Task<int>> func = (number) => Task.FromResult(0);
FuncCallbackInterop<int, Task<int>> funcCallbackInterop = new(func);

// Then pass the callback interop objects to a IJSRuntime's or
// IJSObjectReference's Invoke<Void>Async method. The callback
// interop objects will be correctly serialized to JS callback
IJSRuntime jsRuntime = ...
await jsRuntime.InvokeVoidAsync("myFunction", action, func);

// Make sure to clean up the callback interop objects when
// they're no longer in use. Note that if you dispose them
// before JS code calls them, an exception will be thrown.
// So make sure to only dispose once you're sure JS code
// has called them.
actionCallbackInterop.Dispose();
funcCallbackInterop.Dispose();
```

## Define Custom Module Example
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
@implements IAsyncDisposable
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

  // Make sure to dispose the module object
  public async ValueTask DisposeAsync()
  {
    await MathModule.DisposeAsync();
  }
}
```
