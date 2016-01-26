using System;
using Microsoft.Xna.Framework;

namespace RobotGame.Game
{
    public enum WaypointOrientation
    {
        Horizontal,
        Vertical,
        TwoDimensional
    }

    public class MapWaypoint
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private Vector2 position;
        private WaypointOrientation orientation;

        // Properties ------------------------------------------------------------------------------------- Properties

        public Vector2 Position
        {
            get { return this.position; }
        }

        public WaypointOrientation Orientation
        {
            get { return this.orientation; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public MapWaypoint(Vector2 position, WaypointOrientation orientation)
        {
            this.position = position;
            this.orientation = orientation;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public bool PassedWaypoint(Vector2 actorPosition, Vector2 actorDirection)
        {
            if (orientation == WaypointOrientation.TwoDimensional)
            {
                //Vector2 waypointVector = this.position - actorPosition;
                //if (waypointVector == Vector2.Zero || (Vector2.Normalize(waypointVector) != actorDirection && Math.Acos(Vector2.Dot(actorDirection, Vector2.Normalize(waypointVector))) > 0.9d))
                //{
                //    return true;
                //}

                float distance = Vector2.Distance(this.position, actorPosition);
                if (distance < 5f)
                {
                    return true;
                }

            }
            else if (orientation == WaypointOrientation.Horizontal)
            {
                if (actorDirection.X > 0 && actorPosition.X > this.position.X)
                {
                    return true;
                }
                else if (actorDirection.X < 0 && actorPosition.X < this.position.X)
                {
                    return true;
                }
            }
            else
            {
                if (actorDirection.Y > 0 && actorPosition.Y > this.position.Y)
                {
                    return true;
                }
                else if (actorDirection.Y < 0 && actorPosition.Y < this.position.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public Vector2 DirectionToWaypoint(Vector2 actorPosition)
        {
            if (orientation == WaypointOrientation.TwoDimensional)
            {
                return this.position - actorPosition;
            }
            else if (orientation == WaypointOrientation.Horizontal)
            {
                Vector2 direction;
                if (actorPosition.X - this.position.X > 0)
                {
                    direction = new Vector2(-1f, 0f);
                }
                else
                {
                    direction = new Vector2(1f, 0f);
                }
                return direction;
            }
            else
            {
                Vector2 direction;
                if (actorPosition.Y - this.position.Y > 0)
                {
                    direction = new Vector2(0f, -1f);
                }
                else
                {
                    direction = new Vector2(0f, 1f);
                }
                return direction;
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
