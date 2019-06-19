using System.Collections.Generic;

namespace DFC.Composite.Shell.Moc.OneCol.Models
{
    public class BreadcrumbViewModel
    {
        public string Title { get; set; } = "Unknown trade";
        public IList<BreadcrumbPathViewModel> Paths { get; set; }
    }
}
