using System;

namespace DFC.Composite.Shell.Moc.OneCol.Models.Api
{
    public class RegionApiDto
    {
        public enum PageRegions
        {
            None = 0,
            Head,
            Breadcrumb,
            BodyTop,
            Body,
            SidebarRight,
            SidebarLeft,
            BodyFooter
        }

        public Guid? DocumentId { get; set; }

        public string Path { get; set; }

        public PageRegions PageRegion { get; set; }

        public bool IsHealthy { get; set; }

        public string RegionEndpoint { get; set; }

        public bool HeathCheckRequired { get; set; }

        public string OfflineHtml { get; set; }

        public DateTime DateOfRegistration { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
