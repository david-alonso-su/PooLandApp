using System;
using System.Collections.Generic;

namespace PooLandApp.Data
{
    public partial class Poodatum
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; } = null!;
        public string Photo { get; set; } = null!;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
