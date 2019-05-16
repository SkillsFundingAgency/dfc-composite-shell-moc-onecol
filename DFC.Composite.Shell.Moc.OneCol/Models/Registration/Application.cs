using System.Collections.Generic;

namespace DFC.Composite.Shell.Moc.OneCol.Models.Registration
{
    public class Application
    {
        public string Path { get; set; }

        public string TopNavigationText { get; set; }

        public int TopNavigationOrder { get; set; }

        public Api.PathApiDto.PageLayouts Layout { get; set; }

        public string OfflineHtml { get; set; }

        public string SitemapUrl { get; set; }

        public IEnumerable<Region> Regions { get; set; }

    }
}
