namespace KWFishingHQ.Interfaces
{
    public interface IRiverDataService
    {
        Task<(string Temperature, string Wind, string CloudCover)> ScrapeRiverDataTemperatureAsync();
        Task<(string Pressure, string SunRise, string SunSet, string UVindex)> ScrapeWaterlooWeatherData();
    }
}