using NetTopologySuite.Geometries;

namespace PooLandApp.Data;
public partial  class Neighborhood
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Polygon Coordinates { get; set; }
}