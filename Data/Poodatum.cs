using NetTopologySuite.Geometries;

namespace PooLandApp.Data;
public partial class Poodatum
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; } = null!;
    public string? Photo { get; set; } = null!;
    public Point Location { get; set; }
    public bool Visible { get; set; }
    public int NeighborhoodId { get; set; }
    public Neighborhood Neighborhood { get; set; }
}
