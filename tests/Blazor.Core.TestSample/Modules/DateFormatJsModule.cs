using Blazor.Core.Interop;
using Blazor.Core.Interop.Callbacks;
using Microsoft.JSInterop;

namespace Blazor.Core.TestSample.Modules;

public sealed class DateFormatJsModule : BaseJsModule
{
  protected override string ModulePath => $"./js/{nameof(Modules)}/date-format.js";

  public DateFormatJsModule(IJSRuntime jSRuntime) : base(jSRuntime) {}

  public async Task<string> FormatCurrentDateTimeAsync(Func<DateTime, string> formatter)
  {
    var callbackInterop = new FuncCallbackInterop<DateTime, string>(formatter);
    _callbackInterops.Add(callbackInterop);
    return await Module.InvokeAsync<string>("formatCurrentDateTime", callbackInterop);
  }
}
