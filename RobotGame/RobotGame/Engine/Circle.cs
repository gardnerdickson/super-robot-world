using System;
using Microsoft.Xna.Framework;

namespace RobotGame.Engine
{
    struct Circle
    {
        public Vector2 Center;
        public float Radius;

        public Circle(Vector2 center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        // TODO: Do I need a method for getting the area?
    }
}
