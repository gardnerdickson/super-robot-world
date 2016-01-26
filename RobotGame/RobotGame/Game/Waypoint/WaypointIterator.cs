using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RobotGame.Game.Waypoint
{
    public interface WaypointIterator
    {
        void Next();
        void First();
        MapWaypoint Current();

        MapWaypoint PeekFirst();
        MapWaypoint PeekLast();
    }
}
