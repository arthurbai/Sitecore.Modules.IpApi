// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LookupProvider.cs" company="">
//   
// </copyright>
// <summary>
//   The lookup provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Swissworx.Modules.Analytics.Lookups.IpApi
{
    using System;

    using Sitecore.Analytics.Lookups;
    using Sitecore.Analytics.Model;
    using Sitecore.CES.Client;
    using Sitecore.CES.GeoIp.Caching;
    using Sitecore.CES.GeoIp.Pipelines.HandleLookupError;
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines;

    /// <summary>The lookup provider.</summary>
    public class LookupProvider : LookupProviderBase
    {
        #region Constants

        /// <summary>The handle lookup error pipeline name.</summary>
        private const string HandleLookupErrorPipelineName = "ces.geoIp.handleLookupError";

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="LookupProvider"/> class.</summary>
        /// <param name="geoIpConnector">The geo IP connector.</param>
        /// <remarks>We need to implement this constructor for Sitecore to work.</remarks>
        public LookupProvider(ResourceConnector<WhoIsInformation> geoIpConnector)
        {
        }

        #endregion

        #region Properties

        /// <summary>Gets the replacement IP address when the local IP has been detected.</summary>
        private string LocalIpReplacement => Settings.GetSetting("Swissworx.Modules.IpApi.LocalIpReplacement", "198.154.118.185");

        #endregion

        #region Public Methods and Operators

        /// <summary>Retrieves the geo location information by IP.</summary>
        /// <param name="ip">The IP address.</param>
        /// <returns>The <see cref="WhoIsInformation"/>.</returns>
        public override WhoIsInformation GetInformationByIp(string ip)
        {
            Assert.ArgumentNotNullOrEmpty(ip, nameof(ip));
            if (ip == "127.0.0.1" || ip == "::1")
            {
                ip = this.LocalIpReplacement;
            }

            GeoIpCache geoIpCache = GeoIpCacheManager.GeoIpCache;
            WhoIsInformation information = geoIpCache.Get(ip);
            if (information != null)
            {
                return information;
            }

            try
            {
                GeoVisitorInformation info = new IpApiServiceAgent().GetVisitorInformation(ip);
                information = new WhoIsInformation
                                  {
                                      City = info.City,
                                      Country = info.Country,
                                      Isp = info.Isp,
                                      Latitude = info.Lat,
                                      Longitude = info.Lon,
                                      PostalCode = info.Zip,
                                      Region = info.Region
                                  };
                geoIpCache.Add(ip, information);
                return information;
            }
            catch (Exception ex)
            {
                HandleLookupErrorArgs handleLookupErrorArgs = new HandleLookupErrorArgs(ex);
                CorePipeline.Run(HandleLookupErrorPipelineName, handleLookupErrorArgs);
                if (handleLookupErrorArgs.Fallback == null)
                {
                    throw;
                }

                if (handleLookupErrorArgs.CacheFallback)
                {
                    geoIpCache.Add(ip, handleLookupErrorArgs.Fallback);
                }

                return handleLookupErrorArgs.Fallback;
            }
        }

        #endregion
    }
}