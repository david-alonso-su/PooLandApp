using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;
using BlazorLeaflet.Models;
using PooLandApp.Data;

namespace PooLandApp.MapData;
public static class GISFunctions
{
    public static bool IsPositionInsideBounds(Coordinate coordinate, Geometry geometry)
    {
        var pointLocator = new PointLocator();
        if (pointLocator.Intersects(coordinate, geometry))
            return true;
        else
            return false;
    }
    public static bool IsPositionInsideBounds(LatLng position, Geometry geometry) 
    {
        var coordinate = new Coordinate(position.Lat, position.Lng);
        return IsPositionInsideBounds(coordinate, geometry);

    }
    public static bool IsPositionInsideBounds(Point point, Geometry geometry)
    {
        var coordinate = new Coordinate(point.X, point.Y);
        return IsPositionInsideBounds(coordinate, geometry);

    }
    public static bool IsPositionInNeighborhoods(LatLng position, DbSet<Neighborhood> neighborhoods, out string neighborhoodName)
    {
        neighborhoodName = "Out of Boundaries";
        var result = false;
        
        foreach (var n in neighborhoods)
        {
            if (IsPositionInsideBounds(position, n.Coordinates))
            {
                result = true;
                neighborhoodName = n.Name;
                break;
            }
        }
        return result;
    }
    public static Neighborhood? FindNeighborhood(Point position, DbSet<Neighborhood> neighborhoods) 
    {
        Neighborhood? neighborhood = null;

        foreach (var n in neighborhoods)
        {
            if (IsPositionInsideBounds(position, n.Coordinates))
            {
                neighborhood = n;
                break;
            }
        }

        return neighborhood;
    }
}
