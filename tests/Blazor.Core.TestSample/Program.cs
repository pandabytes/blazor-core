using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.Core.TestSample;
using Blazor.Core.TestSample.Modules;
using Blazor.Core.Interop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
  .AddSingleton<DateFormatJsModule>()
  .AddSingleton<DotnetCallbackJsModule>()
  .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var webHost = builder.Build();

var dotnetCallbackModule = webHost.Services.GetRequiredService<DotnetCallbackJsModule>();
await dotnetCallbackModule.ImportAsync();
await dotnetCallbackModule.DisposeAsync();

await webHost.RunAsync();
