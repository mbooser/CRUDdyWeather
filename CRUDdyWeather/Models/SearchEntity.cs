using CRUDdyWeather.Enums;

public class SearchEntity
{
    public string Name { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public ForcastType Ftype { get; set; }
    public string DumpJSON { get; set; }

    public SearchEntity(double lat, double lng, ForcastType type)
    {
        this.Lat = lat;
        this.Lng = lng;
        this.Ftype = type;
        this.DumpJSON = "";
    }
}
