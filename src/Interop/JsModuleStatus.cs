namespace Blazor.Core.Interop;

/// <summary>
/// Status of the JS module.
/// </summary>
public enum JsModuleStatus
{
  /// <summary>
  /// Indicate the module has not been imported yet.
  /// Consider the module unusable until it's imported.
  /// </summary>
  NotImported,
  /// <summary>
  /// Indicate the module has been imported
  /// and is ready to be used.
  /// </summary>
  Imported,
  /// <summary>
  /// Indicate the module has been disposed
  /// and it should be considered unusable.
  /// </summary>
  Disposed,
}
