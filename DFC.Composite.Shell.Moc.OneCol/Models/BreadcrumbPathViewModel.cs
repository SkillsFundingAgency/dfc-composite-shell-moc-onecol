using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Composite.Shell.Moc.OneCol.Models
{
    public class BreadcrumbPathViewModel: BaseViewModel
    {
        public string Route { get; set; }
        public string Title { get; set; }
        public bool IsLastItem { get; set; } = false;
    }
}
