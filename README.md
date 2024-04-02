# Blazor Interop

This library provides a base class that consumers can use to
implement their own JS modules (`BaseJsModule`).

More importantly this library provides a TS module that
serializes/deserializeC# callbacks (`Func`, `Action`, etc.) to JS.
This allows C# code to pass let's say a `Func<>` to JS, and JS code
can invoke the C# callback. To use this functionality you must
have a reference to a `DotnetCallbackJsModule` object and then
call its `ImportModuleAsync()` to import the `dotnet-callback.js` module.

# Example
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

Then in your application code (most likely in Blazor), use the module like this:
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
    await MathModule.ImportModuleAsync();

    _sum = await MathModule.AddAsync(3, 2);
  }
}
```
