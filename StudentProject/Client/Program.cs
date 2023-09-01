using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;
using ThirdApp.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddLocalization();
await builder.Build().RunAsync();
var host = builder.Build();
var js = host.Services.GetRequiredService<IJSRuntime>();
var culture = await js.InvokeAsync<string>("GetFromLocalStorage", "culture");
await host.RunAsync();
CultureInfo selectedCulture;
if (culture == null)
{
    selectedCulture = new CultureInfo("en-US");
}
else
{
    selectedCulture = new CultureInfo(culture);
}
CultureInfo.DefaultThreadCurrentCulture = selectedCulture;
CultureInfo.DefaultThreadCurrentUICulture = selectedCulture;