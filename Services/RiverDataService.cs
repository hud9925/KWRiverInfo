using KWFishingHQ.Interfaces;
using PuppeteerSharp;
using Azure.Security.KeyVault.Secrets;

namespace KWFishingHQ.Services
{
    public class RiverDataService : IRiverDataService
    {
        private readonly HttpClient _httpClient;
        private readonly SecretClient _secretClient;
        private readonly string _apiKey;

        public RiverDataService(HttpClient httpClient, SecretClient secretClient)
        {
            _httpClient = httpClient; // Injected HttpClient
            _secretClient = secretClient;
            var secret = _secretClient.GetSecret("RiverDataAPIKey");
            _apiKey = secret.Value.Value;
        }
    
        public async Task<(string Temperature, string Wind, string CloudCover, string Pressure, string SunRise, string SunSet, string UVindex)> ScrapeWaterlooWeatherData()
        {
            var apiKeySecret = await _secretClient.GetSecretAsync("RiverDataAPIKey");
            var apiKey = apiKeySecret.Value.Value;
    
            // The Browserless.io WebSocket endpoint
            var browserWSEndpoint = $"wss://chrome.browserless.io?token={apiKey}";
            // scraping logic for additional weather data from weatherwx.com for waterloo ontario
            try {
                var browser = await Puppeteer.ConnectAsync(new ConnectOptions
                    {
                        BrowserWSEndpoint = browserWSEndpoint
                    });
                // await new BrowserFetcher().DownloadAsync();
                // var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                // {
                //     Headless = true
                // });

                var page = await browser.NewPageAsync();
                // disable Images, CSS and JavaScript from Scrapped website 
                await page.SetRequestInterceptionAsync(true);
                page.Request += (sender, e) => {
                    if (e.Request.ResourceType == ResourceType.Image || 
                        e.Request.ResourceType == ResourceType.StyleSheet || 
                        e.Request.ResourceType == ResourceType.Script) {
                        e.Request.AbortAsync();
                    } else {
                        e.Request.ContinueAsync();
                    }
                };

                await page.SetUserAgentAsync("danielhu");
                await page.GoToAsync("https://www.weatherwx.com/forecasts/ca/on/waterloo.html");

                await page.SetUserAgentAsync("danielhu");
                await page.GoToAsync("https://www.weatherwx.com/forecasts/ca/on/waterloo.html");

                await page.WaitForSelectorAsync("#tab-all > div > div:nth-child(2) > article > div.lan.C > table > tbody > tr > td:nth-child(1) > table > tbody > tr:nth-child(3) > td");
                var tempElement = await page.QuerySelectorAsync("#tab-all > div > div:nth-child(2) > article > div.lan.C > table > tbody > tr > td:nth-child(1) > table > tbody > tr:nth-child(3) > td");
                var temperatureText = await tempElement.EvaluateFunctionAsync<string>("el => el.textContent");

                await page.WaitForSelectorAsync("#tab-all > div > div:nth-child(2) > article > div.lan.C > table > tbody > tr > td:nth-child(2) > table > tbody > tr:nth-child(4) > td:nth-child(2)");
                var windElement = await page.QuerySelectorAsync("#tab-all > div > div:nth-child(2) > article > div.lan.C > table > tbody > tr > td:nth-child(2) > table > tbody > tr:nth-child(4) > td:nth-child(2)");
                var windText = await windElement.EvaluateFunctionAsync<string>("el => el.textContent");

                await page.WaitForSelectorAsync("#tab-all > div > div:nth-child(3) > article > table > tbody > tr > td.tdclasstd > div > table > tbody > tr:nth-child(1) > td:nth-child(2)");
                var cloudElement = await page.QuerySelectorAsync("#tab-all > div > div:nth-child(3) > article > table > tbody > tr > td.tdclasstd > div > table > tbody > tr:nth-child(1) > td:nth-child(2)");
                var cloudText = await cloudElement.EvaluateFunctionAsync<string>("el => el.textContent");

                await page.WaitForSelectorAsync("#tab-all > div > div:nth-child(2) > article > div.lan.C > table > tbody > tr > td:nth-child(2) > table > tbody > tr:nth-child(5) > td:nth-child(2)");
                var pressureElement = await page.QuerySelectorAsync("#tab-all > div > div:nth-child(2) > article > div.lan.C > table > tbody > tr > td:nth-child(2) > table > tbody > tr:nth-child(5) > td:nth-child(2)");
                var pressureText = await pressureElement.EvaluateFunctionAsync<string>("el => el.textContent");

                await page.WaitForSelectorAsync("#tab-all > div > div:nth-child(3) > article > table > tbody > tr > td.tdclasstd > div > table > tbody > tr:nth-child(3) > td:nth-child(2)");
                var SunRiseElement = await page.QuerySelectorAsync("#tab-all > div > div:nth-child(3) > article > table > tbody > tr > td.tdclasstd > div > table > tbody > tr:nth-child(3) > td:nth-child(2)");
                var SunRiseText = await SunRiseElement.EvaluateFunctionAsync<string>("el => el.textContent");

                await page.WaitForSelectorAsync("#tab-all > div > div:nth-child(3) > article > table > tbody > tr > td.tdclasstd > div > table > tbody > tr:nth-child(4) > td:nth-child(2)");
                var sunsetElement = await page.QuerySelectorAsync("#tab-all > div > div:nth-child(3) > article > table > tbody > tr > td.tdclasstd > div > table > tbody > tr:nth-child(4) > td:nth-child(2)");
                var sunsetText = await sunsetElement.EvaluateFunctionAsync<string>("el => el.textContent");

                await page.WaitForSelectorAsync("#tab-all > div > div:nth-child(3) > article > table > tbody > tr > td.tdclasstd > div > table > tbody > tr:nth-child(2) > td:nth-child(2)");
                var UVElement = await page.QuerySelectorAsync("#tab-all > div > div:nth-child(3) > article > table > tbody > tr > td.tdclasstd > div > table > tbody > tr:nth-child(2) > td:nth-child(2)");
                var UVText = await UVElement.EvaluateFunctionAsync<string>("el => el.textContent");
                return (temperatureText, windText, cloudText, pressureText, SunRiseText, sunsetText, UVText);
            }
            catch (Exception ex){
                Console.WriteLine($"Error occurred: {ex.Message}");
                return ("Error", "e", "Error", "e", "e", "e", "e"); 
            }
        }
    }
}