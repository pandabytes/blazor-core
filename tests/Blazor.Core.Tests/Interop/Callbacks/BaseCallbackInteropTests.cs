using System.Reflection;
using Microsoft.JSInterop;

namespace Blazor.Core.Tests.Interop.Callbacks;

public abstract class BaseCallbackInteropTests
{
  protected static void AssertCallbackInterop(BaseCallbackInterop callbackInterop, object callback)
  {
    var valueProp = callbackInterop.DotNetRef?.GetType().GetProperty("Value");
    var value = valueProp?.GetValue(callbackInterop.DotNetRef);
    
    var valueType = value?.GetType();
    var callbackField = valueType?.GetField("_callback", BindingFlags.Instance | BindingFlags.NonPublic);
    var invokeMethod = valueType?.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
    var jsInvokableAttrb =  invokeMethod?.GetCustomAttribute<JSInvokableAttribute>();

    Assert.NotNull(callbackField);
    Assert.NotNull(jsInvokableAttrb);

    // Use equality comparision for value type like struct and primitive
    if (callback.GetType().IsValueType)
    {
      Assert.Equal(callback, callbackField.GetValue(value));
    }
    else // If it's reference type then check its reference the same
    {
      Assert.Same(callback, callbackField.GetValue(value));
    }
  }
}
