// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoVisitorInformation.cs" company="">
//   
// </copyright>
// <summary>
//   The geo visitor information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Swissworx.Modules.Analytics.Lookups.IpApi
{
    using System;
    using System.Diagnostics;

    /// <summary>The geo visitor information.</summary>
    [Serializable]
    [DebuggerDisplay("City={City}, Country={Country}, Lon={Lon}, Lat={Lat}, Status={Status}")]
    public class GeoVisitorInformation
    {
        #region Public Properties

        /// <summary>Gets the empty.</summary>
        public static GeoVisitorInformation Empty => new GeoVisitorInformation();

        /// <summary>Gets or sets the city.</summary>
        public string City { get; set; }

        /// <summary>Gets or sets the country.</summary>
        public string Country { get; set; }

        /// <summary>Gets or sets the country code.</summary>
        public string CountryCode { get; set; }

        /// <summary>Gets or sets the ISP.</summary>
        public string Isp { get; set; }

        /// <summary>Gets or sets the latitude.</summary>
        public double Lat { get; set; }

        /// <summary>Gets or sets the longitude.</summary>
        public double Lon { get; set; }

        /// <summary>Gets or sets the region.</summary>
        public string Region { get; set; }

        /// <summary>Gets or sets the region name.</summary>
        public string RegionName { get; set; }

        /// <summary>Gets or sets the status.</summary>
        public string Status { get; set; }

        /// <summary>Gets or sets the zip.</summary>
        public string Zip { get; set; }

        #endregion
    }
}