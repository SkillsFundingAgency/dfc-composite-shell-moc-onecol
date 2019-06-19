using System.Collections.Generic;
using DFC.Composite.Shell.Moc.OneCol.Data;

namespace DFC.Composite.Shell.Moc.OneCol.Models
{
    public class TradeIndexViewModel : BaseViewModel
    {
        public IEnumerable<Trade> Trades { get; set; }
    }
}
