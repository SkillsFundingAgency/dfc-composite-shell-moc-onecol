using Microsoft.AspNetCore.Html;

namespace DFC.Composite.Shell.Moc.OneCol.Models
{
    public class BaseViewModel
    {
        public string Title { get; set; } = "Unknown Trade title";

        public HtmlString Contents { get; set; } = new HtmlString("Unknown Help content");
    }
}
