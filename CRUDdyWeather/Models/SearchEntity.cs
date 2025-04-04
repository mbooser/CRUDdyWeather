using CRUDdyWeather.Enums;

public class SearchEntity
{
    public string Name { get; set; } = "";
    public double Lat { get; set; }
    public double Lng { get; set; }
    public ForcastType Ftype { get; set; } = ForcastType.Current;
    public string DumpJSON { get; set; } = "";

    public SearchEntity()
    {
        
    }
}
