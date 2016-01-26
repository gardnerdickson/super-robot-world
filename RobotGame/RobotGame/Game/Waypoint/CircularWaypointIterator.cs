using System;
using Microsoft.Xna.Framework;

namespace RobotGame.Game.Waypoint
{
    public class CircularWaypointIterator : WaypointIterator
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private MapWaypoint[] waypoints;
        private int currentWaypoint;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public CircularWaypointIterator(MapWaypoint[] waypoints)
        {
            this.waypoints = waypoints;
            this.currentWaypoint = 0;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Next()
        {
            this.currentWaypoint++;
            if (this.currentWaypoint == this.waypoints.Length)
            {
                this.currentWaypoint = 0;
            }
        }

        public void First()
        {
            this.currentWaypoint = 0;
        }

        public MapWaypoint Current()
        {
            return this.waypoints[this.currentWaypoint];
        }

        public MapWaypoint PeekFirst()
        {
            return this.waypoints[0];
        }

        public MapWaypoint PeekLast()
        {
            return this.waypoints[this.waypoints.Length - 1];
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
