using System.Collections.Generic;
using DFC.Composite.Shell.Moc.OneCol.Data;

namespace DFC.Composite.Shell.Moc.OneCol.Services
{
    public interface ITradeService
    {
        Trade GetTrade(int id);
        List<Trade> GetTrades(string category = null, bool filter16Plus = false, bool filter18Plus = false, bool filter21Plus = false, string searchClue = null);
        List<Category> GetCategories();
    }
}
