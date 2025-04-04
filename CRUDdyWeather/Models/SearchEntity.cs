using CRUDdyWeather.Controllers;
using CRUDdyWeather.Enums;
using CRUDdyWeather.Services;

namespace CRUDdyWeather.Models;

public class SearchEntity
{
    private double Lat { get; set; }
    private double Lng { get; set; }
    private ForcastType Ftype { get; set; }
    private string? DumpJSON { get; set; }

    public SearchEntity(double lat, double lng, ForcastType type)
    {
        this.Lat = lat;
        this.Lng = lng;
        this.Ftype = type;
        this.DumpJSON = UrlCaller.UrlBuilder(lat,lng,type);
       
    }
}

   


