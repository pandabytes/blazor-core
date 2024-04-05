using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.Interop.TestSample;
using Blazor.Interop.TestSample.Modules;
using Blazor.Interop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
  .AddSingleton<DateFormatJsModule>()
  .AddSingleton<DotnetCallbackJsModule>()
  .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var webHost = builder.Build();

var dotnetCallbackModule = webHost.Services.GetRequiredService<DotnetCallbackJsModule>();
await dotnetCallbackModule.ImportModuleAsync();
await dotnetCallbackModule.DisposeAsync();

await webHost.RunAsync();
