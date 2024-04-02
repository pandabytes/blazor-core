using Blazor.Interop.CallbackInterops;
using Microsoft.JSInterop;

namespace Blazor.Interop.TestSample.Modules;

public sealed class FiltersJsModule : BaseJsModule
{
  protected override string ModulePath => $"./js/{nameof(Modules)}/filters.js";

  public FiltersJsModule(IJSRuntime jSRuntime) : base(jSRuntime) {}

  public async Task<int[]> FilterNumberByAsync(int[] array, Func<int, bool> predicate)
  {
    var callbackInterop = new FuncCallbackInterop<int, bool>(predicate);
    _callbackInterops.Add(callbackInterop);

    return await Module.InvokeAsync<int[]>("filterNumberBy", array, callbackInterop);
  }

  public async Task LogCallbackAsync(int[] array, Action<int> callback)
  {
    var callbackInterop = new ActionCallbackInterop<int>(callback);
    _callbackInterops.Add(callbackInterop);

    await Module.InvokeVoidAsync("logCallback", array, callbackInterop);
  }
}
