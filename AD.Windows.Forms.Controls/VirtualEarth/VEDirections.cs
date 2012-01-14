using System;
using System.Collections.Generic;

namespace AD.Windows.Forms.Controls
{
    #region VERouteLeg class

    public class VERouteLeg
    {
        #region Properties
        private string duration = String.Empty;

        /// <summary>
        /// Time between two waypoints in a route  (format is hh:mm:ss)
        /// </summary>
        public string Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private decimal distance = 0;

        /// <summary>
        /// Distance in miles or kilometers of this part of the route
        /// </summary>
        public decimal Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        private string description = String.Empty;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        #endregion

        #region Constructors
        public VERouteLeg()
        {
        }

        public VERouteLeg(string description, string duration, decimal distance)
        {
            this.description = description;
            this.duration = duration;
            this.distance = distance;
        }
        #endregion
    }

    #endregion

    #region VERouteDirections class

    public class RouteDirections
    {
        private List<VERouteLeg> _routeLegs = new List<VERouteLeg>();
        private string _totalDuration = String.Empty;
        private decimal _totalDistance = 0;

        public RouteDirections() { }

        public RouteDirections(string totalDuration, decimal totalDistance)
        {
            _totalDuration = totalDuration;
            _totalDistance = totalDistance;
        }

        public List<VERouteLeg> RouteLegs
        {
            get { return _routeLegs; }
            set { _routeLegs = value; }
        }

        /// <summary>
        /// Total time of a route (format is hh:mm:ss)
        /// </summary>
        public string TotalDuration
        {
            get { return _totalDuration; }
            set { _totalDuration = value; }
        }

        // Total distance (in miles or kilometers)
        public decimal TotalDistance
        {
            get { return _totalDistance; }
            set { _totalDistance = value; }
        }

        /// <summary>
        /// Adds a part of the route to the directions 
        /// </summary>
        /// <param name="routeLeg">The leg to add</param>
        public void AddRouteLeg(VERouteLeg routeLeg)
        {
            _routeLegs.Add(routeLeg);
        }
    }

    #endregion
}
