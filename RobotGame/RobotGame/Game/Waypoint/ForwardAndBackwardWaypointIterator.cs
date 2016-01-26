using System;
using Microsoft.Xna.Framework;

namespace RobotGame.Game.Waypoint
{
    class ForwardAndBackwardWaypointIterator : WaypointIterator
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private MapWaypoint[] waypoints;
        private int currentWaypoint;

        private bool forwardIncrement = true;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public ForwardAndBackwardWaypointIterator(MapWaypoint[] waypoints)
        {
            this.waypoints = waypoints;
            this.currentWaypoint = 0;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Next()
        {
            if (forwardIncrement)
            {
                this.currentWaypoint++;
                if (this.currentWaypoint == this.waypoints.Length)
                {
                    this.currentWaypoint -= 2;
                    this.forwardIncrement = false;
                }
            }
            else
            {
                this.currentWaypoint--;
                if (this.currentWaypoint < 0)
                {
                    this.currentWaypoint += 2;
                    this.forwardIncrement = true;
                }
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
