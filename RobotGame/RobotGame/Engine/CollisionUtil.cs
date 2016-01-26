using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RobotGame.Engine
{
    public struct Line
    {
        public Vector2 Point1;
        public Vector2 Point2;

        public Line(Vector2 point1, Vector2 point2)
        {
            this.Point1 = point1;
            this.Point2 = point2;
        }
    }

    class CollisionUtil
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int ALPHA_THRESHOLD = Config.ALPHA_THRESHOLD;

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        private CollisionUtil() { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static bool CheckPerPixelCollision(Actor actorA, Actor actorB)
        {
            Rectangle collisionRect = Rectangle.Intersect(actorA.Bounds, actorB.Bounds);

            // Avoid going into per pixel collision detection if we don't have to
            if (collisionRect == Rectangle.Empty)
            {
                return false;
            }

            int pixelCount = collisionRect.Width * collisionRect.Height;

            // Get the pixels from each sprite that are intersecting
            Color[] pixelsA = new Color[pixelCount];
            Color[] pixelsB = new Color[pixelCount];
            actorA.Sprite.Texture.GetData<Color>(0, actorA.NormalizeBounds(collisionRect), pixelsA, 0, pixelCount);
            actorB.Sprite.Texture.GetData<Color>(0, actorB.NormalizeBounds(collisionRect), pixelsB, 0, pixelCount);

            for (int i = 0; i < pixelCount; i++)
            {
                if (pixelsA[i].A > ALPHA_THRESHOLD && pixelsB[i].A > ALPHA_THRESHOLD)
                {
                    return true;
                }
            }

            return false;
        }

        public static float CheckIntersectionCollision(Circle circle, Rectangle rect)
        {
            float intersectionValueA = CheckVertexCollision(circle, rect);
            float intersectionValueB = CheckNoVertexCollision(circle, rect);

            return (intersectionValueA > intersectionValueB) ? intersectionValueA : intersectionValueB;
        }

        public static Rectangle CheckIntersectionCollision(Rectangle rectA, Rectangle rectB)
        {
            return Rectangle.Intersect(rectA, rectB);
        }

        public static bool CheckIntersectionCollision(Line pointLine, Rectangle rect)
        {
            Line[] rectLines = new Line[4];
            rectLines[0] = new Line(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top));
            rectLines[1] = new Line(new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom));
            rectLines[2] = new Line(new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Left, rect.Bottom));
            rectLines[3] = new Line(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Left, rect.Top));

            foreach (Line rectLine in rectLines)
            {
                double d = (rectLine.Point2.Y - rectLine.Point1.Y) * (pointLine.Point2.X - pointLine.Point1.X) -
                           (rectLine.Point2.X - rectLine.Point1.X) * (pointLine.Point2.Y - pointLine.Point1.Y);

                double n_a = (rectLine.Point2.X - rectLine.Point1.X) * (pointLine.Point1.Y - rectLine.Point1.Y) -
                             (rectLine.Point2.Y - rectLine.Point1.Y) * (pointLine.Point1.X - rectLine.Point1.X);

                double n_b = (pointLine.Point2.X - pointLine.Point1.X) * (pointLine.Point1.Y - rectLine.Point1.Y) -
                             (pointLine.Point2.Y - pointLine.Point1.Y) * (pointLine.Point1.X - rectLine.Point1.X);

                if (d != 0)
                {
                    double ua = n_a / d;
                    double ub = n_b / d;

                    if (ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        public static Vector2 GetIntersectionDepth(Rectangle rectA, Rectangle rectB)
        {
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
            {
                return Vector2.Zero;
            }

            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private static float CheckVertexCollision(Circle circle, Rectangle rect)
        {
            if (circle.Center.X >= rect.Left && circle.Center.X <= rect.Right &&
                circle.Center.Y >= rect.Top && circle.Center.Y <= rect.Bottom)
            {
                return 1.0f;
            }

            Vector2[] rectangleVertices = new Vector2[]
            {
                new Vector2(rect.Left, rect.Top),
                new Vector2(rect.Right, rect.Top),
                new Vector2(rect.Left, rect.Bottom),
                new Vector2(rect.Right, rect.Bottom),
            };

            float radiusSquared = circle.Radius * circle.Radius;
            float shortestIntersectionLengthSquared = -1;
            foreach (Vector2 vertex in rectangleVertices)
            {
                Vector2 intersectionVector = vertex - circle.Center;
                float intersectionLengthSquared = intersectionVector.LengthSquared();
                if ((intersectionLengthSquared > 0) && (intersectionLengthSquared < radiusSquared))
                {
                    if (shortestIntersectionLengthSquared == -1 || intersectionLengthSquared < shortestIntersectionLengthSquared)
                    {
                        shortestIntersectionLengthSquared = intersectionLengthSquared;
                    }
                }
            }

            if (shortestIntersectionLengthSquared == -1)
            {
                return 0.0f;
            }

            return 1 - (shortestIntersectionLengthSquared / radiusSquared);
        }

        private static float CheckNoVertexCollision(Circle circle, Rectangle rect)
        {
            Vector2 circleRightPoint = new Vector2(circle.Center.X + circle.Radius, circle.Center.Y);
            Vector2 circleTopPoint = new Vector2(circle.Center.X, circle.Center.Y - circle.Radius);
            Vector2 circleLeftPoint = new Vector2(circle.Center.X - circle.Radius, circle.Center.Y);
            Vector2 circleBottomPoint = new Vector2(circle.Center.X, circle.Center.Y + circle.Radius);

            float shortestIntersectionLength = -1;
            List<float> intersectionLengthList = new List<float>();

            if (rect.Left <= circleRightPoint.X && rect.Left >= circle.Center.X &&
                rect.Top <= circleRightPoint.Y && rect.Bottom >= circleRightPoint.Y)
            {
                float intersectionLength = circle.Radius - (circleRightPoint.X - rect.Left);
                intersectionLengthList.Add(intersectionLength);
            }
            if (rect.Right >= circleLeftPoint.X && rect.Right <= circle.Center.X &&
                rect.Top <= circleLeftPoint.Y && rect.Bottom >= circleLeftPoint.Y)
            {
                float intersectionLength = circle.Radius - Math.Abs(circleLeftPoint.X - rect.Right);
                intersectionLengthList.Add(intersectionLength);
            }
            if (rect.Bottom >= circleTopPoint.Y && rect.Bottom <= circle.Center.Y &&
                rect.Left <= circleTopPoint.X && rect.Right >= circleTopPoint.X)
            {
                float intersectionLength = circle.Radius - Math.Abs(circleTopPoint.Y - rect.Bottom);
                intersectionLengthList.Add(intersectionLength);
            }
            if (rect.Top <= circleBottomPoint.Y && rect.Top >= circle.Center.Y &&
                rect.Left <= circleBottomPoint.X && rect.Right >= circleBottomPoint.X)
            {
                float intersectionLength = circle.Radius - (circleBottomPoint.Y - rect.Top);
                intersectionLengthList.Add(intersectionLength);
            }

            foreach (float length in intersectionLengthList)
            {
                if (shortestIntersectionLength == -1 || length < shortestIntersectionLength)
                {
                    shortestIntersectionLength = length;
                }
            }

            if (shortestIntersectionLength == -1)
            {
                return 0.0f;
            }

            return 1 - (shortestIntersectionLength / circle.Radius);
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

    }
}
