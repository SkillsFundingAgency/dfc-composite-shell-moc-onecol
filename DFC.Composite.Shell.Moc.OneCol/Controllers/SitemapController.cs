using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.Composite.Shell.Moc.OneCol.Models.Sitemap;
using DFC.Composite.Shell.Moc.OneCol.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DFC.Composite.Shell.Moc.OneCol.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ILogger<SitemapController> _logger;
        private readonly ITradeService _TradeService;

        public SitemapController(ILogger<SitemapController> logger, ITradeService TradeService)
        {
            _logger = logger;
            _TradeService = TradeService;
        }

        [HttpGet]
        public async Task<ContentResult> Sitemap()
        {
            try
            {
                _logger.LogInformation("Generating Sitemap");

                const string TradeControllerName = "Trade";
                var sitemap = new Sitemap();

                // add the defaults
                sitemap.Add(new SitemapLocation() { Url = Url.Action(nameof(TradeController.Index), TradeControllerName, null, Request.Scheme), Priority = 1 });
                sitemap.Add(new SitemapLocation() { Url = Url.Action(nameof(TradeController.Search), TradeControllerName, null, Request.Scheme), Priority = 1 });

                // add the filters
                TradeController.Filters.ForEach(f => sitemap.Add(new SitemapLocation() { Url = Url.Action(nameof(TradeController.Index), TradeControllerName, new { Filter = f }, Request.Scheme), Priority = 1 }));

                // add the categories
                var categories = GetTradeCategories();

                categories.ForEach(c => sitemap.Add(new SitemapLocation() { Url = Url.Action(nameof(TradeController.Index), TradeControllerName, new { Category = c }, Request.Scheme), Priority = 1 }));

                // extract the sitemap
                string xmlString = sitemap.WriteSitemapToString();

                _logger.LogInformation("Generated Sitemap");

                return Content(xmlString, "application/xml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Sitemap)}: {ex.Message}");
            }

            return null;
        }

        private List<string> GetTradeCategories()
        {
            var categories = _TradeService.GetCategories();

            var results = categories.Select(s => s.Name)
                                    .OrderBy(o => o)
                                    .ToList();
            return results;
        }
    }
}
