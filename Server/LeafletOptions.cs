namespace PooLandApp.Server;
public class LeafletOptions
{
    public string UrlTemplate { get; set; }
    public string Attribution { get; set; }
    public LatitudeLongitude Center { get; set; }
    public float Zoom { get; set; }
    public MaxBound MaxBounds { get; set; }
}
public class LatitudeLongitude
{
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}
public class MaxBound
{
    public LatitudeLongitude Up { get; set; }
    public LatitudeLongitude Down { get; set; }
}