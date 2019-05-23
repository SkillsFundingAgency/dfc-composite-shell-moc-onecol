using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.AsyncInitialization;
using DFC.Composite.Shell.Moc.OneCol.Models.Api;
using DFC.Composite.Shell.Moc.OneCol.Models.Registration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DFC.Composite.Shell.Moc.OneCol.Services
{
    public class RegisterApplicationInitialiser : IAsyncInitializer
    {
        private readonly ApplicationRegistration _applicationRegistration;
        private readonly ILogger<RegisterApplicationInitialiser> _logger;

        private string PathUrl => $"{_applicationRegistration.RegisterPathUrl}/{_applicationRegistration.Application.Path.ToLower()}";

        private string RegionsUrl => string.Format(_applicationRegistration.RegisterRegionUrl, _applicationRegistration.Application.Path.ToLower());

        private string RegionUrl(RegionApiDto.PageRegions pageRegion)
        {
            return $"{RegionsUrl}/{pageRegion.ToString().ToLower()}";
        }

        public RegisterApplicationInitialiser(ApplicationRegistration applicationRegistration, ILogger<RegisterApplicationInitialiser> logger)
        {
            _applicationRegistration = applicationRegistration;

            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation($"Registering application: {_applicationRegistration.Application.Path}");

            //TODO: Ian: uncomment the following when REST APIs are available to be called
            //// get any current registration so that it may be overwritten
            //var pathDto = await GetAsync<PathApiDto>(PathUrl);

            //if (pathDto != null)
            //{
            //    // if exists then overwrite
            //    var pathApiDto = new PathApiDto()
            //    {
            //        DocumentId = pathDto.DocumentId.Value,
            //        Path = _applicationRegistration.Application.Path,
            //        TopNavigationText = _applicationRegistration.Application.TopNavigationText,
            //        TopNavigationOrder = _applicationRegistration.Application.TopNavigationOrder,
            //        Layout = _applicationRegistration.Application.Layout,
            //        OfflineHtml = _applicationRegistration.Application.OfflineHtml,
            //        SitemapUrl = _applicationRegistration.Application.SitemapUrl
            //    };

            //    _logger.LogInformation($"Overwriting application: {_applicationRegistration.Application.Path}");

            //    _ = await PutAsync(PathUrl, pathApiDto);
            //}
            //else
            //{
            //    // does not exist - first use - create it
            //    var pathApiDto = new PathApiDto()
            //    {
            //        DocumentId = null,
            //        Path = _applicationRegistration.Application.Path,
            //        TopNavigationText = _applicationRegistration.Application.TopNavigationText,
            //        TopNavigationOrder = _applicationRegistration.Application.TopNavigationOrder,
            //        Layout = _applicationRegistration.Application.Layout,
            //        OfflineHtml = _applicationRegistration.Application.OfflineHtml,
            //        SitemapUrl = _applicationRegistration.Application.SitemapUrl
            //    };

            //    _logger.LogInformation($"Creating application: {_applicationRegistration.Application.Path}");

            //    _ = await PostAsync(PathUrl, pathApiDto);
            //}

            //// get all the existing Regions, then either create, update or delete based on new registration data
            //_logger.LogInformation($"Getting regions for application: {_applicationRegistration.Application.Path}");

            //var regionsApiDto = await GetAsync<IEnumerable<RegionApiDto>>(RegionsUrl);

            //_logger.LogInformation($"Got {regionsApiDto.Count()} regions for application: {_applicationRegistration.Application.Path}");

            //if (regionsApiDto?.Count() > 0)
            //{
            //    // some regions exist - decide whether to create, update or delete existing based on this replacement registration
            //    var regionsToCreate = _applicationRegistration.Application.Regions.Where(e => !regionsApiDto.Any(a => a.PageRegion == e.PageRegion)).ToList();

            //    if (regionsToCreate?.Count() > 0)
            //    {
            //        PostNewRegions(regionsToCreate);
            //    }

            //    var regionsToUpdate = _applicationRegistration.Application.Regions.Where(e => regionsApiDto.Any(a => a.PageRegion == e.PageRegion)).ToList();

            //    if (regionsToUpdate?.Count() > 0)
            //    {
            //        PutExistingRegions(regionsApiDto, regionsToUpdate);
            //    }

            //    var regionsApiDtoToDelete = regionsApiDto.Where(e => !_applicationRegistration.Application.Regions.Any(a => a.PageRegion == e.PageRegion)).ToList();

            //    if (regionsApiDtoToDelete?.Count() > 0)
            //    {
            //        DeleteExistingRegions(regionsApiDtoToDelete);
            //    }
            //}
            //else
            //{
            //    // no regions exist - post them all
            //    var regionsToPost = _applicationRegistration.Application.Regions.ToList();

            //    if (regionsToPost?.Count() > 0)
            //    {
            //        PostNewRegions(regionsToPost);
            //    }
            //}

            _logger.LogInformation($"Registered application: {_applicationRegistration.Application.Path}");
        }

        #region Define the update calls to the Rest API

        private void PostNewRegions(List<Region> regions)
        {
            _logger.LogInformation($"Creating regions for application: {_applicationRegistration.Application.Path}");

            regions.ForEach(async f =>
            {
                var regionDto = new RegionApiDto()
                {
                    Path = _applicationRegistration.Application.Path,
                    PageRegion = f.PageRegion,
                    RegionEndpoint = f.RegionEndpoint,
                    HeathCheckRequired = f.HeathCheckRequired,
                    OfflineHtml = f.OfflineHtml
                };

                _ = await PostAsync(RegionUrl(regionDto.PageRegion), regionDto);
            });

            _logger.LogInformation($"Created {regions?.Count()} regions for application: {_applicationRegistration.Application.Path}");
        }

        private void PutExistingRegions(IEnumerable<RegionApiDto> regionsApiDto, List<Region> regions)
        {
            _logger.LogInformation($"Replacing regions for application: {_applicationRegistration.Application.Path}");

            regions.ForEach(async f =>
            {
                var regionApiDto = regionsApiDto.Where(w => w.PageRegion == f.PageRegion).FirstOrDefault();

                regionApiDto.RegionEndpoint = f.RegionEndpoint;
                regionApiDto.HeathCheckRequired = f.HeathCheckRequired;
                regionApiDto.OfflineHtml = f.OfflineHtml;

                _ = await PutAsync(RegionUrl(regionApiDto.PageRegion), regionApiDto);
            });

            _logger.LogInformation($"Replaced {regions?.Count()} regions for application: {_applicationRegistration.Application.Path}");
        }

        private void DeleteExistingRegions(List<RegionApiDto> regionsApiDo)
        {
            _logger.LogInformation($"Deleting regions for application: {_applicationRegistration.Application.Path}");

            regionsApiDo.ForEach(async f => await DeleteAsync(RegionUrl(f.PageRegion)));

            _logger.LogInformation($"Deleted {regionsApiDo?.Count()} regions for application: {_applicationRegistration.Application.Path}");
        }

        #endregion

        #region Define generic functions for calling Rest API

        private async Task<T> GetAsync<T>(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (var content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        var pathApiDto = JsonConvert.DeserializeObject<T>(data);

                        return pathApiDto;
                    }
                }
            }

            return default;
        }

        private async Task<T> PostAsync<T>(string url, T model)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(url, model);

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    using (var content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        model = JsonConvert.DeserializeObject<T>(data);

                        return model;
                    }
                }
            }

            return default;
        }

        private async Task<T> PutAsync<T>(string url, T model)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsJsonAsync(url, model);

                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (var content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        model = JsonConvert.DeserializeObject<T>(data);

                        return model;
                    }
                }
            }

            return default;
        }

        private async Task DeleteAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(url);

                response.EnsureSuccessStatusCode();
            }
        }

        #endregion

    }
}
