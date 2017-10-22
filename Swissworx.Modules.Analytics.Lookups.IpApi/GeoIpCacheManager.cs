// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoIpCacheManager.cs" company="">
//   
// </copyright>
// <summary>
//   The geo IP cache manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Swissworx.Modules.Analytics.Lookups.IpApi
{
    using Sitecore.CES.GeoIp.Caching;
    using Sitecore.Diagnostics;

    /// <summary>The geo IP cache manager.</summary>
    internal static class GeoIpCacheManager
    {
        #region Static Fields

        /// <summary>The sync lock.</summary>
        private static readonly object SyncLock = new object();

        /// <summary>The geo IP cache.</summary>
        private static GeoIpCache geoIpCache = new GeoIpCache();

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the geo IP cache.</summary>
        public static GeoIpCache GeoIpCache
        {
            get => geoIpCache;

            set
            {
                Assert.IsNotNull(value, typeof(GeoIpCache));
                lock (SyncLock)
                {
                    geoIpCache = value;
                }
            }
        }

        #endregion
    }
}