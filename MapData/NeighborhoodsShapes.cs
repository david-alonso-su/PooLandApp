using BlazorLeaflet;
using PooLandApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using BlazorLeaflet.Models;

namespace PooLandApp.MapData;

public static class NeighborhoodsShapes
{
    public static async void Create(Map mapComponent, DbSet<Neighborhood> neighborhoods)
    {
        foreach (var shape in await LoadShapes(neighborhoods))
        {
            mapComponent.AddLayer(new Polygon
            {
                Shape = new[] { shape.Value.ToArray() },
                Popup = new Popup
                {
                    Content = shape.Key
                }
            });
        }
    }

    private static async Task<Dictionary<string, List<PointF>>> LoadShapes(DbSet<Neighborhood> neighborhoods)
    {
        var shapes = new Dictionary<string, List<PointF>>();
        foreach (var neighborhood in neighborhoods)
        {
            var points = new List<PointF>();
            foreach (var point in neighborhood.Coordinates.Coordinates)
            {
                points.Add(new PointF((float)point.X, (float)point.Y));
            }
            shapes.Add(neighborhood.Name, points);
        }
        return shapes;
    }
}
