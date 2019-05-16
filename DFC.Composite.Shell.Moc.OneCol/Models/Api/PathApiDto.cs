using System;

namespace DFC.Composite.Shell.Moc.OneCol.Models.Api
{
    public class PathApiDto
    {

        public enum PageLayouts
        {
            None = 0,
            FullWidth = 1,
            SidebarRight = 2,
            SidebarLeft = 3
        }

        public Guid? DocumentId { get; set; }

        public string Path { get; set; }

        public string TopNavigationText { get; set; }

        public int TopNavigationOrder { get; set; }

        public PageLayouts Layout { get; set; }

        public bool IsOnline { get; set; }

        public string OfflineHtml { get; set; }

        public string SitemapUrl { get; set; }

        public DateTime DateOfRegistration { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
