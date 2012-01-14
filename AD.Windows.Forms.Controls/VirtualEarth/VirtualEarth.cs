using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AD.Windows.Forms.Controls
{
    [ComVisible(true)]
    public partial class VirtualEarthMap : UserControl
    {
        private const string VE_DOC_NAME = "ve.htm";

        #region Private properties

        private string _htmlDocument;
        private bool _centerAtLastResult = false;
        private bool _displayResults = false;
        private bool _finishedFindingLocations = false;
        private int _searchLocationIndex = 0;
        private List<VESearchLocation> _locations = new List<VESearchLocation>();
        private RouteDirections _routeDirections = null;

        #endregion

        #region Event handler delegates

        public delegate void VE_FinishedFindingLocationsDelegate();

        #endregion

        #region Event definitions

        public virtual event VE_FinishedFindingLocationsDelegate OnFinishedFindingLocations;
        
        #endregion

        public VirtualEarthMap()
        {
            InitializeComponent();

            _htmlDocument = _htmlDocument = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"\Temp\", VE_DOC_NAME);
        }

        public void StartVirtualEarthMap()
        {
            string html = LoadMapFileResource();
            using (TextWriter writer = new StreamWriter(_htmlDocument, false, System.Text.Encoding.ASCII))
            {
                writer.Write(html);
                writer.Close();
            }
            browser.ObjectForScripting = this;
            browser.Url = new Uri(_htmlDocument);
        }

        private string LoadMapFileResource()
        {
            string veHTMLDocument = string.Empty;

            using (Stream resourceStream = Assembly.GetAssembly(GetType()).GetManifestResourceStream("AD.Windows.Forms.Controls.VirtualEarth.VirtualEarth.html"))
            {
                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    veHTMLDocument = reader.ReadToEnd();
                    reader.Close();
                }
                resourceStream.Close();
            }
            return veHTMLDocument;
        }

        #region Properties

        /// <summary>
        /// Flag indicating that a call to VE_FindLocations is finished
        /// </summary>
        public bool FinishedFindingLocations
        {
            get
            {
                return _finishedFindingLocations;
            }
        }

        private VEDistanceUnit _distanceUnit = VEDistanceUnit.Kilometers;

        [Browsable(true)]
        [Description("The distance unit used.")]
        [DefaultValue(VEDistanceUnit.Kilometers)]
        [Category("Virtual Earth")]
        public VEDistanceUnit DistanceUnit
        {
            get { return _distanceUnit; }
            set
            {
                _distanceUnit = value;

                if (!this.DesignMode && browser.Document != null)
                {
                    browser.Document.InvokeScript("setDistanceUnit", new object[] { (int)_distanceUnit });
                }
            }
        }

        private VEDisambiguation _disambiguationMode = VEDisambiguation.Default;

        [Browsable(true)]
        [Description("The disambiguation mode.")]
        [DefaultValue(VEDisambiguation.Default)]
        [Category("Virtual Earth")]
        public VEDisambiguation DisambiguationMode
        {
            get { return _disambiguationMode; }
            set
            {
                _disambiguationMode = value;

                if (!this.DesignMode && browser.Document != null)
                {
                    browser.Document.InvokeScript("setDisambiguationMode", new object[] { (int)_disambiguationMode });
                }
            }
        }

        private VEDashboardStyle _dashboardStyle = VEDashboardStyle.Normal;

        [Browsable(true)]
        [Description("The size of the dashboard.")]
        [DefaultValue(VEDashboardStyle.Normal)]
        [Category("Virtual Earth")]
        public VEDashboardStyle DashboardStyle
        {
            get { return _dashboardStyle; }
            set
            {
                _dashboardStyle = value;

                if (!this.DesignMode && browser.Document != null)
                {
                    browser.Document.InvokeScript("setDashboardStyle", new object[] { (int)_dashboardStyle });
                }
            }
        }

        private VEMapStyle _mapStyle = VEMapStyle.Road;

        [Browsable(true)]
        [Description("Defines the way the map is shown.")]
        [DefaultValue(VEMapStyle.Road)]
        [Category("Virtual Earth")]
        public VEMapStyle MapStyle
        {
            get { return _mapStyle; }
            set
            {
                _mapStyle = value;

                if (!this.DesignMode && browser.Document != null)
                {
                    browser.Document.InvokeScript("setMapStyle", new object[] { (int)_mapStyle });
                }
            }
        }

        private bool _showDashboard = true;

        [Browsable(true)]
        [Description("Show or hide the dashboard.")]
        [DefaultValue(true)]
        [Category("Virtual Earth")]
        public bool ShowDashboard
        {
            get { return _showDashboard; }
            set
            {
                _showDashboard = value;

                if (!this.DesignMode && browser.Document != null)
                {
                    browser.Document.InvokeScript("setDashboardVisibility", new object[] { _showDashboard });
                }
            }
        }

        private string _mapLocation = "";

        [Browsable(true)]
        [Description("Set the location displayed on the map.")]
        [DefaultValue("")]
        [Category("Virtual Earth")]
        public string MapLocation
        {
            get { return _mapLocation; }
            set
            {
                _mapLocation = value;

                if (!this.DesignMode && browser.Document != null)
                {
                    browser.Document.InvokeScript("findLocation", new object[] { _mapLocation });
                }
            }
        }

        private int _zoomLevel = 10;

        [Browsable(true)]
        [Description("Set the zoomlevel. (1 to 19)")]
        [DefaultValue(10)]
        [Category("Virtual Earth")]
        public int ZoomLevel
        {
            get
            {
                if (!this.DesignMode && browser.Document != null)
                {
                    return (int)browser.Document.InvokeScript("getZoomLevel");
                }
                else
                {
                    return _zoomLevel;
                }
            }
            set
            {
                _zoomLevel = value;

                if (!this.DesignMode && browser.Document != null)
                {
                    browser.Document.InvokeScript("find", new object[] { _zoomLevel });
                }
            }
        }

        public List<VESearchLocation> SearchLocations
        {
            get { return _locations; }
        }

        #endregion

        #region Virtual Earth map control methods
        /// <summary>
        /// Set zoom level of the map
        /// </summary>
        /// <param name="level">The level to zoom to (1 to 19)</param>
        public void VE_SetZoomLevel(int level)
        {
            if (level < 1 || level > 19)
            {
                throw new ArgumentException("Level must be a number between 1 and 19.");
            }

            if (!this.DesignMode && browser.Document != null)
            {
                browser.Document.InvokeScript("setZoomLevel", new object[] { level });
            }
        }

        /// <summary>
        /// Zoom in 1 level on the map
        /// </summary>
        public void VE_ZoomIn()
        {
            if (!this.DesignMode && browser.Document != null)
            {
                browser.Document.InvokeScript("zoomIn");
            }
        }

        /// <summary>
        /// Zoom out 1 level on the map
        /// </summary>
        public void VE_ZoomOut()
        {
            if (!this.DesignMode && browser.Document != null)
            {
                browser.Document.InvokeScript("zoomOut");
            }
        }

        /// <summary>
        /// Add a shape layer to the map
        /// </summary>
        /// <param name="title">Title of the layers</param>
        /// <param name="description">Description of the layer</param>
        /// <remarks>Shapelayers are identified by their title</remarks> 
        public void VE_AddShapeLayer(string title, string description)
        {
            if (!this.DesignMode && browser.Document != null)
            {
                browser.Document.InvokeScript("addShapeLayer", new object[] { title, description });
            }
        }

        /// <summary>
        /// Remove as shapelayer and its contents from the map
        /// </summary>
        /// <param name="title">The title of the layer to remove</param>
        /// <returns>false if the layer cannot be found</returns>
        /// <remarks>Shapelayers are identified by their title</remarks> 
        public bool VE_DeleteShapeLayer(string title)
        {
            if (!this.DesignMode && browser.Document != null)
            {
                return (bool)browser.Document.InvokeScript("deleteShapeLayer", new object[] { title });
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Delete all shapelayers and their content from the map
        /// </summary>
        public void VE_DeleteAllShapeLayers()
        {
            browser.Document.InvokeScript("deleteAllShapeLayers");
        }

        /// <summary>
        /// Set the visibility of a shapelayer
        /// </summary>
        /// <param name="title">The title of the shapelayer to modify</param>
        /// <param name="visible">true to show the shapelayer, false to hide</param>
        /// <returns>false if the layer cannot be found</returns>
        /// <remarks>Shapelayers are identified by their title</remarks> 
        public bool VE_SetShapeLayerVisible(string title, bool visible)
        {
            return (bool)browser.Document.InvokeScript("setShapeLayerVisible", new object[] { title, visible });
        }

        /// <summary>
        /// Search for multiple locations and display their location on the map using a pushpin.
        /// </summary>
        /// <param name="locations">List of locations to search for</param>
        /// <param name="centerAtLastResult">if true, the map will be centered at the last found location</param> 
        /// <param name="displayResults">if true, results will be shown on the map as pushpins</param> 
        /// <remarks>
        /// Use this function to find locations by where or what. If the latitude and Longitude is known, use
        /// the AddPushPin function instead.
        /// After a succesful call the latitude and Longitude of the locations in the list is set
        /// If a location is not found, the latitude and Longitude will be null
        /// </remarks> 
        public void VE_FindLocations(List<VESearchLocation> locations, bool centerAtLastResult, bool displayResults, MethodInvoker callbackFunction)
        {
            _locations = locations;
            _centerAtLastResult = centerAtLastResult;
            _displayResults = displayResults;

            if (locations.Count > 0)
            {
                _finishedFindingLocations = false;

                _searchLocationIndex = 0;
                VESearchLocation searchLocation = locations[_searchLocationIndex];
                browser.Document.InvokeScript("find", new object[] { searchLocation.What, searchLocation.Where, centerAtLastResult });
            }

            if (callbackFunction != null)
            {
                OnFinishedFindingLocations += new VE_FinishedFindingLocationsDelegate(callbackFunction);
            }
        }

        /// <summary>
        /// Callback function for the find call to the Virtual Earth control, adds the latitude and Longitude to the locations
        /// </summary>
        /// <param name="Lat">Latitude of the found location, or null when the location is not found</param>
        /// <param name="Long">Longitude of the found location, or null when the location is not found</param>
        /// <param name="errorMsg">Error message if an error has occured</param>
        /// <remarks>This method should not be called directly, instead use the VE_DisplayLocations(..) method</remarks> 
        public void OnJavascriptLocationFound(object Lat, object Long, object errorMsg)
        {
            _locations[_searchLocationIndex].Latitude = (double?)Lat;
            _locations[_searchLocationIndex].Longitude = (double?)Long;

            if (_locations != null && _locations.Count > _searchLocationIndex + 1)
            {
                VESearchLocation searchLocation = _locations[++_searchLocationIndex];
                browser.Document.InvokeScript("find", new object[] { searchLocation.What, searchLocation.Where });
            }
            else if (_locations != null && _displayResults)
            {   
                // all locations are search for, now display the results if specified
                DisplayLocations();
                _finishedFindingLocations = true;
            }
            else if (_centerAtLastResult && _locations.Count > 0 && !_displayResults)
            {   
                // if the results will not be displayed but the map should be centered at the last location, then do that now
                VE_SetCenter(_locations[_locations.Count - 1]);
                _finishedFindingLocations = true;
            }
            else
            {
                _finishedFindingLocations = true;
            }

            // Trigger public event
            if (_finishedFindingLocations)
            {
                OnFinishedFindingLocations();
            }
        }

        /// <summary>
        /// Add pushpins for all locations in the collection
        /// </summary>
        private void DisplayLocations()
        {
            foreach (VESearchLocation location in _locations)
            {
                if (location.Latitude != null && location.Longitude != null)
                {
                    object id = browser.Document.InvokeScript("addPushPin",
                        new object[] { 
                            location.Latitude, 
                            location.Longitude, 
                            location.PushPinTitle, 
                            location.PushPinLayer, 
                            location.PushPinImage, 
                            location.PushPinDescription });

                    location.Id = id == null ? "" : id.ToString();
                }
            }

            // if specified, the last result will be centered on the map
            if (_centerAtLastResult && _locations.Count > 0)
            {
                VE_SetCenter(_locations[_locations.Count - 1]);
            }
        }

        /// <summary>
        /// Centers the map at the specified location (based on latitude/Longitude or where/what)
        /// </summary>
        /// <param name="location">The location the map will center at</param>
        public void VE_SetCenter(VESearchLocation location)
        {
            // set center by coordinates...
            if (location.Latitude != null && location.Longitude != null)
            {
                browser.Document.InvokeScript("setCenter",
                        new object[] 
                        { 
                            location.Longitude, 
                            location.Latitude 
                        });
            } // ...or by what / where
            else if (location.Where != String.Empty || location.What != String.Empty)
            {
                _locations = new List<VESearchLocation>();
                SearchLocations.Add(location);
                VE_FindLocations(_locations, true, false, null);
            }
        }

        /// <summary>
        /// Initiate route calculations
        /// </summary>
        /// <param name="locations">The waypoints of the route (max 25)</param>
        /// <param name="displayRoute">Display the route on the map yes or no</param>
        /// <param name="setBestMapView">Center the map to have best view of the route yes or no</param>
        /// <param name="routeMode">Walking or driving directions</param>
        /// <param name="distanceUnit">Miles or kilometers</param>
        /// <returns>RouteDirection object </returns> 
        public RouteDirections VE_GetDirections(List<VESearchLocation> locations, bool displayRoute, bool setBestMapView, VERouteMode routeMode, VEDistanceUnit distanceUnit)
        {
            List<VESearchLocation> locationsWithLatLongCoordinates = new List<VESearchLocation>();
            List<VESearchLocation> locationsWithoutLatLongCoordinates = new List<VESearchLocation>();

            if (locations.Count > 25)
            {
                throw new Exception("Maximum number of waypoints in a route is 25.");
            }

            // split locations in those with and without latitude and longtidude information
            foreach (VESearchLocation location in locations)
            {
                if (location.Latitude != null && location.Longitude != null)
                {
                    locationsWithLatLongCoordinates.Add(location);
                }
                else
                {
                    locationsWithoutLatLongCoordinates.Add(location);
                }
            }

            // if there are locations without latitude and Longitude information, find the lat/long coordinates using where or what
            if (locationsWithoutLatLongCoordinates.Count > 0)
            {
                VE_FindLocations(locationsWithoutLatLongCoordinates, false, false, null);

                while (!_finishedFindingLocations)
                {
                    Application.DoEvents();
                }

                locationsWithLatLongCoordinates.AddRange(_locations);
            }

            browser.Document.InvokeScript("clearRouteWaypoints");

            // add the waypoints by latitude and Longitude
            foreach (VESearchLocation searchLocation in locationsWithLatLongCoordinates)
            {
                if (searchLocation.Latitude != null && searchLocation.Longitude != null)
                {
                    browser.Document.InvokeScript("addRouteWaypoint",
                        new object[] 
                        { 
                            searchLocation.Longitude, 
                            searchLocation.Latitude 
                        });
                }
            }

            _routeDirections = null;

            browser.Document.InvokeScript("getDirections", new object[] { displayRoute, setBestMapView, (int)routeMode, distanceUnit });

            // wait for asynchronous call to getDirections callback function to finish
            while (_routeDirections == null)
            {
                Application.DoEvents();
            }

            return _routeDirections;
        }

        /// <summary>
        /// Callback function for VE_GetDirections
        /// </summary>
        /// <param name="time">Total time in seconds</param>
        /// <param name="distance">Total distance in miles or kilometers</param>
        /// <param name="description">Route description</param>
        public void OnDirectionsFinished(object duration, object distance, object description)
        {
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            _routeDirections = new RouteDirections((string)duration, decimal.Parse(distance.ToString(), nfi));

            foreach (string routeLeg in ((string)description).Trim('|').Split('|'))
            {
                string[] routeLegDetails = routeLeg.Split('~');
                _routeDirections.AddRouteLeg(new VERouteLeg(routeLegDetails[0], routeLegDetails[2], decimal.Parse(routeLegDetails[1], nfi)));
            }
        }

        /// <summary>
        /// Add a pushpin to the map based on Latitude and Longitude coordinates
        /// </summary>
        /// <param name="location">The location and appearance of the pushpin</param>
        /// <returns>The unique identifier of the added puspin (empty when to location could not be found)</returns>
        public string VE_AddPushPin(VESearchLocation location)
        {
            if (location.Latitude == null && location.Longitude == null)
            {
                throw new Exception("Specify the Latitude and Longitude of the pushpin.");
            }

            object id = browser.Document.InvokeScript("addPushPin",
                        new object[] { 
                            location.Latitude, 
                            location.Longitude, 
                            location.PushPinTitle, 
                            location.PushPinLayer, 
                            location.PushPinImage, 
                            location.PushPinDescription });

            location.Id = id == null ? "" : id.ToString();

            return location.Id;
        }

        /// <summary>
        /// Removes a pushpin from the map
        /// </summary>
        /// <param name="id">The unique identifier of the pushpin to remove</param>
        public void VE_DeletePushPin(string id)
        {
            browser.Document.InvokeScript("deletePushPin", new object[] { id });
        }

        /// <summary>
        /// Delete all pushpins, lines and polygones from the map
        /// </summary>
        public void VE_DeleteAllShapes()
        {
            browser.Document.InvokeScript("deleteAllShapes");
        }

        /// <summary>
        /// Delete current route directions from the map
        /// </summary>
        public void VE_DeleteDirections()
        {
            browser.Document.InvokeScript("deleteDirections");
        }

        #endregion

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            browser.Document.InvokeScript("startVE");
        }
    }
}
