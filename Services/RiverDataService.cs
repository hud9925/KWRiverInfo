using KWFishingHQ.Interfaces;
using PuppeteerSharp;

namespace KWFishingHQ.Services
{
    public class RiverDataService : IRiverDataService
    {
        private readonly HttpClient _httpClient;
        public RiverDataService(HttpClient httpClient)
        {
            _httpClient = httpClient; // Injected HttpClient
        }
        //public string ScrapeResult { get;  private set;}

        public async Task<(string Temperature, string Wind, string CloudCover)> ScrapeRiverDataTemperatureAsync()
        {
            // scraping air temperature, wind speed and direction, cloud cover values from accuweather.com, for Waterloo
            try {
                await new BrowserFetcher().DownloadAsync();
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true
                });

                var page = await browser.NewPageAsync();
                await page.SetUserAgentAsync("danielhu");
                await page.GoToAsync("https://www.accuweather.com/en/ca/waterloo/n2j/hourly-weather-forecast/55073");
        
                await page.WaitForSelectorAsync("div.temp");
                var temperatureElement = await page.QuerySelectorAsync("div.temp");
                var temperatureText = await temperatureElement.EvaluateFunctionAsync<string>("el => el.textContent");

                await page.WaitForSelectorAsync("div.panel.no-realfeel-phrase p:nth-of-type(1) span.value");
                var windElement = await page.QuerySelectorAsync("div.panel.no-realfeel-phrase p:nth-of-type(1) span.value");
                var windText = await windElement.EvaluateFunctionAsync<string>("el => el.textContent");

                await page.WaitForSelectorAsync("body > div > div.two-column-page-content > div.page-column-1 > div.page-content.content-module > div.hourly-wrapper.content-module > div > div.accordion-item-content > div > div:nth-child(2) > div > p:nth-child(3) > span");
                var cloudCover = await page.QuerySelectorAsync("body > div > div.two-column-page-content > div.page-column-1 > div.page-content.content-module > div.hourly-wrapper.content-module > div > div.accordion-item-content > div > div:nth-child(2) > div > p:nth-child(3) > span");
                var cloudText = await cloudCover.EvaluateFunctionAsync<string>("el => el.textContent");

                await browser.CloseAsync();

                return (temperatureText, windText, cloudText);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return ("Error", "e", "Error");
            }
            
        }
        public async Task<(string Pressure, string SunRise, string SunSet, string UVindex)> ScrapeWaterlooWeatherData()
        {
            // scraping logic for additional weather data from weatherwx.com for waterloo ontario
            try {
                await new BrowserFetcher().DownloadAsync();
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true
                });

                var page = await browser.NewPageAsync();
                await page.SetUserAgentAsync("danielhu");
                await page.GoToAsync("https://www.weatherwx.com/forecasts/ca/on/waterloo.html");

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
                return (pressureText, SunRiseText, sunsetText, UVText);
            }
            catch (Exception ex){
                Console.WriteLine($"Error occurred: {ex.Message}");
                return ("Error", "e", "Error", "e"); 
            }
        }
    }
}