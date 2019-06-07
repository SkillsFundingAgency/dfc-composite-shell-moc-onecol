using System.Collections.Generic;

namespace DFC.Composite.Shell.Moc.OneCol.Models
{
    public class BreadcrumbViewModel
    {
        public IEnumerable<string> Paths { get; set; }
        public string ThisLocation { get; set; }
    }
}
