
using Microsoft.AspNetCore.Mvc.RazorPages;
using KWFishingHQ.Interfaces;
using System.Security.Principal;
public class RiverDataModel: PageModel
{
    private readonly IRiverDataService _riverDataService;
    
    public string? Temperature { get; private set; }
    public string? Wind { get; private set; }
    public string? CloudCover { get; private set; }

    public string? pressure { get; private set; }

    public string? sunrise { get; private set; }

    public string? sunset { get; private set; }
    public string? UV{get; private set;}
    public RiverDataModel(IRiverDataService riverDataService)
    {
        _riverDataService = riverDataService;
    }
    public async Task OnGetAync(){
        //(Temperature, Wind, CloudCover) = await _riverDataService.ScrapeRiverDataTemperatureAsync();
        (Temperature, Wind, CloudCover, pressure, sunrise, sunset, UV) = await _riverDataService.ScrapeWaterlooWeatherData();
    }

    
}


