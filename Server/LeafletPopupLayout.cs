namespace PooLandApp.Server
{
    public class LeafletPopupLayout
    {

        public string? Description { get; set; }
        public string? Photo { get; set; }

        //private string html ="asdfadf";
        private string html = "<div><p>{0}</p><img src=\"{1}\"/></div>";

        public string GetHtml() { return String.Format(html, Description, Photo); }
    }
}