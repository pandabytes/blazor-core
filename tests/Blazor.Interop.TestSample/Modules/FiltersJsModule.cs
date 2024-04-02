using Microsoft.JSInterop;

namespace Blazor.Interop.TestSample.Modules;

public sealed class FiltersJsModule : BaseJsModule
{
  protected override string ModulePath => $"{ModulePrefixPath}/js/math.js";

  public FiltersJsModule(IJSRuntime jSRuntime) : base(jSRuntime) {}

  public async Task<int[]> FilterNumberByAsync(int[] array, Func<int, bool> predicate)
    => await Module.InvokeAsync<int[]>("filterNumberBy", array, predicate);

  public async Task LogCallbackAsync(int[] array, Action<int> callback)
    => await Module.InvokeVoidAsync("logCallback", array, callback);
}
