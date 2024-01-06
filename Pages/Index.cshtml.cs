using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KWFishingHQ.Interfaces;
namespace KWFishingHQ.Pages;


public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IRiverDataService _riverDataService;
    public string? Temperature { get; private set; }
    public string? Wind { get; private set; }
    public string? CloudCover { get; private set; }

    public string? Pressure { get; private set;}

    public string? SunRise { get; private set; }

    public string? SunSet { get; private set; }

    public string? UVindex { get; private set; }

    public IndexModel(IRiverDataService riverDataService)
    {
        _riverDataService = riverDataService;
    }
    public async Task OnGetAsync(){
        //var (temp, wind, cloud) = await _riverDataService.ScrapeRiverDataTemperatureAsync();
        var (temp, wind, cloud, pressure, sunrise, sunset, UV) = await _riverDataService.ScrapeWaterlooWeatherData();
        Temperature = temp;
        Wind = wind;
        CloudCover = cloud;
        Pressure = pressure;
        SunRise = sunrise;
        SunSet = sunset;
        UVindex = UV;
    }
}
