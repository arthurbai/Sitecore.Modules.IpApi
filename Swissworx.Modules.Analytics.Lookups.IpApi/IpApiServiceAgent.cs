// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IpApiServiceAgent.cs" company="">
//   
// </copyright>
// <summary>
//   The IP API service agent.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Swissworx.Modules.Analytics.Lookups.IpApi
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Sitecore.Configuration;
    using Sitecore.Diagnostics;

    /// <summary>The IP API service agent.</summary>
    internal class IpApiServiceAgent
    {
        #region Properties

        /// <summary>Gets the API key.</summary>
        private string ApiKey
        {
            get
            {
                string key = Settings.GetSetting("Swissworx.Modules.IpApi.ApiKey");
                Assert.ArgumentNotNullOrEmpty(key, "Swissworx.Modules.IpApi.ApiKey");
                return key;
            }
        }

        /// <summary>The service timeout.</summary>
        private int ServiceTimeout => Settings.GetIntSetting("Swissworx.Modules.IpApi.ServiceTimeout", 5);

        /// <summary>Gets the service URL.</summary>
        private string ServiceUrl
        {
            get
            {
                string url = Settings.GetSetting("Swissworx.Modules.IpApi.ServiceUrl");
                Assert.ArgumentNotNullOrEmpty(url, "Swissworx.Modules.IpApi.ServiceUrl");
                return url;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Retrieves the visitors geo information.</summary>
        /// <param name="ipAddress">The visitors IP address.</param>
        /// <returns>The <see cref="GeoVisitorInformation"/>.</returns>
        public GeoVisitorInformation GetVisitorInformation(string ipAddress)
        {
            Assert.ArgumentNotNullOrEmpty(ipAddress, nameof(ipAddress));

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ServiceUrl);
                client.Timeout = TimeSpan.FromSeconds(this.ServiceTimeout);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Task<HttpResponseMessage> asyncTask = client.GetAsync($"/json/{ipAddress}?key={this.ApiKey}");
                asyncTask.Wait();
                HttpResponseMessage response = asyncTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    Task<string> task = response.Content.ReadAsStringAsync();
                    task.Wait();
                    GeoVisitorInformation geoInfo = JsonConvert.DeserializeObject<GeoVisitorInformation>(task.Result);
                    return geoInfo;
                }

                throw new ApplicationException($"Visitor lookup failed for IP '{ipAddress}'");
            }
        }

        #endregion
    }
}