using CRUDdyWeather.Enums;

/// <summary>
/// Model for holding basic data of a search
/// Variety of Forcast is determined by ForcastType Enum
/// All data stored in DumpJSON for later parsing.
/// </summary>
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
