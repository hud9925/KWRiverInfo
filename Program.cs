using KWFishingHQ.Interfaces;
using KWFishingHQ.Services;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc.Routing;

var builder = WebApplication.CreateBuilder(args);

// Azure key vault configuration
string keyVaultUrl = builder.Configuration["AzureKeyVault:Url"];
var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
builder.Services.AddSingleton(secretClient);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<IRiverDataService, RiverDataService>(); // scraper
var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
