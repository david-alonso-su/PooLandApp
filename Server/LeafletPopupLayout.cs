namespace PooLandApp.Server
{
    public class LeafletPopupLayout
    {

        public string? Description { get; set; }
        public string? Photo { get; set; }

        //private string html ="asdfadf";
        private string html = @"
            <div class=""container"">
                <div class=""row"">
                    <div class=""col"">
                        <p>{0}</p>
                     </div>
                </div>
                <div class=""row"">
                    <div class=""col"">
                        <img src=""{1}""  class=""rounded mx-auto d-block"" />
                    </div>
                </div>
            </div>";

        public string GetHtml() { return String.Format(html, Description, Photo); }
    }
}