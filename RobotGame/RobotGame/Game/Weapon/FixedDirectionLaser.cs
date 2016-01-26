using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using System.Collections.Generic;
using RobotGame.Game.Mover;

namespace RobotGame.Game.Weapon
{
    class FixedDirectionLaser : Laser
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private Line defaultLaserLine;

        private Point destructibleTile;

        private bool collideWithMovers;

        // Properties ------------------------------------------------------------------------------------- Properties
        
        // Contructors ----------------------------------------------------------------------------------- Contructors

        public FixedDirectionLaser(int damage, Vector2 position, Vector2 direction, bool collideWithMovers)
            : base(damage, position, direction)
        {
            this.defaultLaserLine = new Line();
            this.defaultLaserLine.Point1 = this.position;

            this.collideWithMovers = collideWithMovers;

            CalculateDefaultLaserLine();

            this.laserLine = this.defaultLaserLine;
        }

        public FixedDirectionLaser(int damage, Vector2 position, Vector2 direction)
            : this(damage, position, direction, true)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update(GameTime gameTime)
        {
            if (this.destructibleTile.X != -1 && !Level.GetInstance().IsDestructibleTile(destructibleTile.X, destructibleTile.Y))
            {
                CalculateDefaultLaserLine();
            }

            if (collideWithMovers)
            {
                this.laserLine = CollideLineWithMovers(this.defaultLaserLine);
            }

            base.Update(gameTime);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void CalculateDefaultLaserLine()
        {
            this.destructibleTile = new Point(-1, -1);
            Point collidingTile = new Point(-1, -1);
            this.defaultLaserLine = CollideLineWithEnvironment(this.defaultLaserLine, false, true, ref collidingTile);

            if (collidingTile.X != -1)
            {
                Rectangle tileBounds = Level.GetInstance().GetTileBounds(collidingTile.X, collidingTile.Y);
                if (this.direction.Y == 1)
                {
                    this.defaultLaserLine.Point2.Y = tileBounds.Top;
                }
                else if (this.direction.Y == -1)
                {
                    this.defaultLaserLine.Point2.Y = tileBounds.Bottom;
                }
                else if (this.direction.X == 1)
                {
                    this.defaultLaserLine.Point2.X = tileBounds.Left;
                }
                else
                {
                    this.defaultLaserLine.Point2.X = tileBounds.Right;
                }


                if (Level.GetInstance().IsDestructibleTile(collidingTile.X, collidingTile.Y))
                {
                    this.destructibleTile = collidingTile;
                }
            }

        }
    }
}
