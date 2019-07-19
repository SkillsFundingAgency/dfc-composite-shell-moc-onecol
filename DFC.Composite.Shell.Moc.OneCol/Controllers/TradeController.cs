using System.Collections.Generic;
using System.Linq;
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
        [Route("trade/head/{**data}")]
        public IActionResult Head(string data)
        {
            var vm = new HeadViewModel
            {
                Title = string.IsNullOrWhiteSpace(data) ? "Index" : data,
                Contents = null
            };

            return View(vm);
        }

        [HttpGet]
        [Route("trade/bodytop/{**data}")]
        public IActionResult BodyTop(string data)
        {
            var vm = new BodyTopViewModel
            {
                Title = nameof(BodyTop),
                Contents = null
            };

            return View(vm);
        }

        [HttpGet]
        [Route("trade/breadcrumb/{**data}")]
        public IActionResult Breadcrumb(string data)
        {
            var vm = new BreadcrumbViewModel()
            {
                Title = data,
                Paths = new List<BreadcrumbPathViewModel>() {
                    new BreadcrumbPathViewModel()
                    {
                        Route = "/",
                        Title = "Home"
                    },
                    new BreadcrumbPathViewModel()
                    {
                        Route = "/trade/index",
                        Title = "Trades"
                    }
                }
            };

            if (!string.IsNullOrWhiteSpace(data))
            {
                vm.Paths.Add(
                    new BreadcrumbPathViewModel()
                    {
                        Route = $"/trade/{data}",
                        Title = data
                    }
                );

                vm.Paths.Last().IsLastItem = true;
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Index(string category, string filter, string searchClue)
        {
            bool filter16Plus = (string.Compare(filter, Filter16Plus, true) == 0);
            bool filter18Plus = (string.Compare(filter, Filter18Plus, true) == 0);
            bool filter21Plus = (string.Compare(filter, Filter21Plus, true) == 0);
            var vm = new TradeIndexViewModel()
            {
                Title = nameof(Index)
            };

            vm.Trades = _tradeService.GetTrades(category, filter16Plus, filter18Plus, filter21Plus, searchClue);

            return View(vm);
        }

        [HttpGet]
        public IActionResult Health()
        {
            return Ok();
        }

        [HttpGet]
        [Route("trade/bodyfooter/{**data}")]
        public IActionResult BodyFooter(string data)
        {
            var vm = new BodyFooterViewModel
            {
                Title = nameof(BodyFooter),
                Contents = null
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Search(string searchClue)
        {
            if (!string.IsNullOrEmpty(searchClue))
            {
                return RedirectToAction(nameof(Index), new { searchClue });
            }

            var vm = new SearchViewModel()
            {
                Title = nameof(Search)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("trade/search")]
        [Route("/search")]
        public IActionResult Search(SearchViewModel search)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(search.Clue))
                {
                    return RedirectToAction(nameof(Index), new { searchClue = search.Clue });
                }
            }

            search.Title = nameof(Search);

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
        [Route("trade/edit")]
        [Route("/edit")]
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
