using CRUDdyWeather.Enums;

public class WeatherViewModel
{
    public string Name { get; set; }
    public ForcastType Ftype { get; set; }

    // Forecast data
    public CurrentWeather CurrentWeather { get; set; }
    public List<DailyForecast> DailyForecasts { get; set; }
    public List<HourlyForecast> HourlyForecasts { get; set; }

    public WeatherViewModel(string name, ForcastType ftype)
    {
        Name = name;
        Ftype = ftype;
        CurrentWeather = new CurrentWeather();
        DailyForecasts = new List<DailyForecast>();
        HourlyForecasts = new List<HourlyForecast>();
    }
}

public class CurrentWeather
{
    public string Temperature { get; set; }
    public string WeatherDescription { get; set; }
    public string Humidity { get; set; }
    public string WindSpeed { get; set; }
}

public class DailyForecast
{
    public string Date { get; set; }
    public string TemperatureHigh { get; set; }
    public string TemperatureLow { get; set; }
    public string WeatherDescription { get; set; }
}

public class HourlyForecast
{
    public string Time { get; set; }
    public string Temperature { get; set; }
    public string WeatherDescription { get; set; }
}


