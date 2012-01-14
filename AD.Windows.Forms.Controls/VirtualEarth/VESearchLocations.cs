using System;

namespace AD.Windows.Forms.Controls
{
    /// <summary>
    /// This calls represents a location on the map. 
    /// </summary>
    public class VESearchLocation
    {
        /// <summary>
        /// ID of the pushpin once it is displayed on the map. Use this ID to delete or modify a pushpin
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The search string for the location
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// The search string for the business
        /// </summary>
        public string What { get; set; }

        /// <summary>
        /// Title displayed on the pushpin
        /// </summary>
        public string PushPinTitle { get; set; }

        /// <summary>
        /// Description displayed when the pushpin is selected
        /// </summary>
        public string PushPinDescription { get; set; }

        /// <summary>
        /// Location of the image used as pushpin symbol on the map
        /// </summary>
        public string PushPinImage { get; set; }

        /// <summary>
        /// Title of the layer to add the pushpin to, leave empty to not add the pushpin to a layer
        /// </summary>
        public string PushPinLayer { get; set; }

        /// <summary>
        /// Longtitude of the found location, do not set it by code: it gets filled during the search operation
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Latitude of the found location, do not set it by code: it gets filled during the search operation
        /// </summary>
        public double? Latitude { get; set; }

        public VESearchLocation()
        {
            Where = string.Empty;
            What = string.Empty;
            PushPinDescription = string.Empty;
            PushPinImage = string.Empty;
            PushPinLayer = string.Empty;
        }

        public VESearchLocation(double longitude, double latitude)
            : this()
        {
            Id = string.Empty;
            Longitude = longitude;
            Latitude = latitude;
            PushPinTitle = string.Empty;
        }

        public VESearchLocation(string id, double longitude, double latitude)
            : this()
        {
            Id = id;
            Longitude = longitude;
            Latitude = latitude;
            PushPinTitle = string.Empty;
        }

        public VESearchLocation(string id, double longitude, double latitude, string pushPinTitle)
            : this()
        {
            Id = id;
            Longitude = longitude;
            Latitude = latitude;
            PushPinTitle = pushPinTitle;
        }
    }
}
