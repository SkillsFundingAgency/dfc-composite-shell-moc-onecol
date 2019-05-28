using System.Collections.Generic;
using DFC.Composite.Shell.Moc.OneCol.Data;
using DFC.Composite.Shell.Moc.OneCol.Models;
using DFC.Composite.Shell.Moc.OneCol.Services;
using Microsoft.AspNetCore.Mvc;


namespace DFC.Composite.Shell.Moc.OneCol.Controllers
{
    public class TradeController : Controller
    {
        public const string Filter16Plus = "16Plus";
        public const string Filter18Plus = "18Plus";
        public const string Filter21Plus = "21Plus";
        public static readonly List<string> Filters = new List<string> { Filter16Plus, Filter18Plus, Filter21Plus };

        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpGet]
        public IActionResult Head()
        {
            var vm = new HeadViewModel();

            return View(vm);
        }

        [HttpGet]
        public IActionResult BodyTop()
        {
            var vm = new BodyTopViewModel();

            return View(vm);
        }

        [HttpGet]
        public IActionResult Breadcrumb()
        {
            var vm = new BreadcrumbViewModel();

            return View(vm);
        }

        [HttpGet]
        public IActionResult Index(string category, string filter, string searchClue)
        {
            var vm = new TradeIndexViewModel();
            bool filter16Plus = (string.Compare(filter, Filter16Plus, true) == 0);
            bool filter18Plus = (string.Compare(filter, Filter18Plus, true) == 0);
            bool filter21Plus = (string.Compare(filter, Filter21Plus, true) == 0);

            vm.Trades = _tradeService.GetTrades(category, filter16Plus, filter18Plus, filter21Plus, searchClue);

            return View(vm);
        }

        [HttpGet]
        public IActionResult BodyFooter()
        {
            var vm = new BodyFooterViewModel();

            return View(vm);
        }

        [HttpGet]
        public IActionResult Search(string searchClue)
        {
            if (!string.IsNullOrEmpty(searchClue))
            {
                return RedirectToAction(nameof(Index), new { searchClue });
            }

            var vm = new SearchViewModel();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(SearchViewModel search)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(search.Clue))
                {
                    return RedirectToAction(nameof(Index), new { searchClue = search.Clue });
                }
            }

            return View(search);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _tradeService.GetTrade(id);
            var vm = new TradeEditViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                MinimumAge = model.MinimumAge,
                City = model.City,
                Category = model.Category
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Trade trade)
        {
            if (ModelState.IsValid)
            {
                bool isAuthenticated = User.Identity.IsAuthenticated;

                return RedirectToAction(nameof(Index));
            }

            return View(trade);
        }
    }
}
