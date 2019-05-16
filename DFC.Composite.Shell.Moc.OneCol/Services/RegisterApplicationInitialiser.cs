using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.AsyncInitialization;
using DFC.Composite.Shell.Moc.OneCol.Models.Api;
using DFC.Composite.Shell.Moc.OneCol.Models.Registration;
using Newtonsoft.Json;

namespace DFC.Composite.Shell.Moc.OneCol.Services
{
    public class RegisterApplicationInitialiser : IAsyncInitializer
    {
        private readonly ApplicationRegistration _applicationRegistration;

        private string PathUrl => $"{_applicationRegistration.RegisterPathUrl}/{_applicationRegistration.Application.Path}";
        private string RegionsUrl => $"{_applicationRegistration.RegisterPathUrl}/{_applicationRegistration.Application.Path}/regions";
        private string RegionUrl(RegionApiDto.PageRegions pageRegion)
        {
            return $"{_applicationRegistration.RegisterPathUrl}/{_applicationRegistration.Application.Path}/regions/{pageRegion.ToString()}";
        }

        public RegisterApplicationInitialiser(ApplicationRegistration applicationRegistration)
        {
            _applicationRegistration = applicationRegistration;
        }

        public async Task InitializeAsync()
        {
            //TODO: Ian: uncomment the following code only when PATH& REGION API have been implemented and we have an endpoint to call
            //// get any current registration so they may be overwritten
            //var pathDto = await GetExistingPathRegistrationAsync();

            //if (pathDto != null)
            //{
            //    // if exists then overwrite
            //    _ = await PutPathRegistrationAsync(pathDto.DocumentId.Value);
            //}
            //else
            //{
            //    // does not exist - first use - create it
            //    _ = await PostPathRegistrationAsync();
            //}

            //// get all the existing Regions, then either create, update or delete based on new registration data
            //var regionsApiDto = await GetExistingRegionsForPathAsync();

            //List<Region> regionsToPost = null;
            //List<Region> regionsToPut = null;
            //List<RegionApiDto> regionsApiDtoToDelete = null;

            //if (regionsApiDto?.Count() > 0)
            //{
            //    // some regions exist - decide whether to create, update or delete existing based on this replacement registration
            //    regionsToPost = _applicationRegistration.Application.Regions.Where(e => !regionsApiDto.Any(a => a.PageRegion == e.PageRegion)).ToList();
            //    regionsToPut = _applicationRegistration.Application.Regions.Where(e => regionsApiDto.Any(a => a.PageRegion == e.PageRegion)).ToList();
            //    regionsApiDtoToDelete = regionsApiDto.Where(e => !_applicationRegistration.Application.Regions.Any(a => a.PageRegion == e.PageRegion)).ToList();
            //}
            //else
            //{
            //    // no regions exist - post them all
            //    regionsToPost = _applicationRegistration.Application.Regions.ToList();
            //}

            //if (regionsToPost?.Count() > 0)
            //{
            //    regionsToPost.ForEach(async f => _ = await PostRegionRegistrationAsync(f));
            //}

            //if (regionsToPut?.Count() > 0)
            //{
            //    regionsToPut.ForEach(async f => _ = await PutRegionRegistrationAsync(f, regionsApiDto.Where(w => w.PageRegion == f.PageRegion).FirstOrDefault()));
            //}

            //if (regionsApiDtoToDelete?.Count() > 0)
            //{
            //    regionsApiDtoToDelete.ForEach(async f => await DeleteRegionRegistrationAsync(f));
            //}
        }

        private async Task<PathApiDto> GetExistingPathRegistrationAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(PathUrl);

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (var content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        var pathApiDto = JsonConvert.DeserializeObject<PathApiDto>(data);

                        return pathApiDto;
                    }
                }
            }

            return null;
        }

        private async Task<PathApiDto> PostPathRegistrationAsync()
        {
            var pathApiDto = new PathApiDto()
            {
                DocumentId = null,
                Path = _applicationRegistration.Application.Path,
                TopNavigationText = _applicationRegistration.Application.TopNavigationText,
                TopNavigationOrder = _applicationRegistration.Application.TopNavigationOrder,
                Layout = _applicationRegistration.Application.Layout,
                OfflineHtml = _applicationRegistration.Application.OfflineHtml,
                SitemapUrl = _applicationRegistration.Application.SitemapUrl
            };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(PathUrl, pathApiDto);

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    using (var content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        pathApiDto = JsonConvert.DeserializeObject<PathApiDto>(data);

                        return pathApiDto;
                    }
                }
            }

            return null;
        }

        private async Task<PathApiDto> PutPathRegistrationAsync(Guid documentid)
        {
            var pathApiDto = new PathApiDto()
            {
                DocumentId = documentid,
                Path = _applicationRegistration.Application.Path,
                TopNavigationText = _applicationRegistration.Application.TopNavigationText,
                TopNavigationOrder = _applicationRegistration.Application.TopNavigationOrder,
                Layout = _applicationRegistration.Application.Layout,
                OfflineHtml = _applicationRegistration.Application.OfflineHtml,
                SitemapUrl = _applicationRegistration.Application.SitemapUrl
            };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsJsonAsync(PathUrl, pathApiDto);

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (var content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        pathApiDto = JsonConvert.DeserializeObject<PathApiDto>(data);

                        return pathApiDto;
                    }
                }
            }

            return null;
        }

        private async Task<IEnumerable<RegionApiDto>> GetExistingRegionsForPathAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(RegionsUrl);

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (var content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        var regionsApiDto = JsonConvert.DeserializeObject<IEnumerable<RegionApiDto>>(data);

                        return regionsApiDto;
                    }
                }
            }

            return null;
        }

        private async Task<RegionApiDto> PostRegionRegistrationAsync(Region region)
        {
            using (var httpClient = new HttpClient())
            {
                var regionDto = new RegionApiDto()
                {
                    Path = _applicationRegistration.Application.Path,
                    PageRegion = region.PageRegion,
                    RegionEndpoint = region.RegionEndpoint,
                    HeathCheckRequired = region.HeathCheckRequired,
                    OfflineHtml = region.OfflineHtml
                };

                var response = await httpClient.PostAsJsonAsync(RegionUrl(regionDto.PageRegion), regionDto);

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    using (var content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        regionDto = JsonConvert.DeserializeObject<RegionApiDto>(data);

                        return regionDto;
                    }
                }
            }

            return null;
        }

        private async Task<RegionApiDto> PutRegionRegistrationAsync(Region region, RegionApiDto regionApiDto)
        {
            regionApiDto.HeathCheckRequired = region.HeathCheckRequired;
            regionApiDto.RegionEndpoint = region.RegionEndpoint;
            regionApiDto.OfflineHtml = region.OfflineHtml;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsJsonAsync(RegionUrl(regionApiDto.PageRegion), regionApiDto);

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (var content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        regionApiDto = JsonConvert.DeserializeObject<RegionApiDto>(data);

                        return regionApiDto;
                    }
                }
            }

            return null;
        }

        private async Task DeleteRegionRegistrationAsync(RegionApiDto regionApiDto)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(RegionUrl(regionApiDto.PageRegion));

                response.EnsureSuccessStatusCode();
            }
        }

    }
}
