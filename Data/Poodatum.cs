using System;
using System.Collections.Generic;

namespace PooLandApp.Data
{
    public partial class Poodatum
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; } = null!;
        public string? Photo { get; set; } = null!;
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
