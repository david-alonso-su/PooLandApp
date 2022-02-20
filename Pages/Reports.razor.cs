namespace PooLandApp.Pages
{
    public partial class Report
    {
        public class FormData
        {
            public string? Description { get; set; }
            public string? strLatitude
            {
                get
                {
                    return Latitude.ToString();
                }
                set
                {
                    Latitude = Convert.ToSingle(value);
                }
            }
            public string? strLongitude
            {
                get
                {
                    return Longitude.ToString();
                }
                set
                {
                    Longitude = Convert.ToSingle(value);
                }
            }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
            public string? Photo { get; set; }
            public short IsCaptchaValid { get; set; }
        }
    }
}
