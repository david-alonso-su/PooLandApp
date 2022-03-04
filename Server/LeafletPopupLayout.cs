using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using PooLandApp.Data;

namespace PooLandApp.Server;
public class LeafletPopupLayout
{
    public static IDbContextFactory<PooLandDbContext> DbFactory { get; set; }
    public static ILogger Logger { get; set; }
    public LeafletPopupLayout(IDbContextFactory<PooLandDbContext> dbFactory, ILogger logger) 
    {
        DbFactory = dbFactory;
        Logger = logger;
    }

    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }

    public string? ButtonText { get; set; }

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
                <div class=""row mt-2"">
                    <div class=""col text-center"">
                        <button id=""popup-button-submit"" onclick=""popupButtonClick('{2}')"" type=""button"" class=""btn btn-outline-primary btn-sm"">{3}</button>
                    </div>
                </div>
            </div>

            ";

    public string GetHtml() { return String.Format(html, Description, Photo, Id, ButtonText); }

    [JSInvokable]
    public static void PopupButtonClick(string id) 
    {
        DisablePoo.DbFactory = DbFactory;
        DisablePoo.Logger = Logger;
        DisablePoo.Disable(Convert.ToInt32(id));
    }

}

public static class DisablePoo 
{
    public static IDbContextFactory<PooLandDbContext> DbFactory;
    public static ILogger Logger { get; set; }
    public static void Disable(int Id)
    {
        try
        {
            using var pooContext = DbFactory?.CreateDbContext();
            var poo = pooContext?.Poodata.FirstOrDefault(x => x.Id == Id);
            if (poo != null)
            {
                poo.Visible = false;
            }
            pooContext?.SaveChanges();
        }
        catch (Exception ex)
        {
            Logger.LogDebug(ex.ToString());
        }
    }
}

