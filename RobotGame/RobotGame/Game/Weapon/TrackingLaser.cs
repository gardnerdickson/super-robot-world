using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Game.Enemy;
using System.Collections.Generic;
using RobotGame.Exceptions;
using Microsoft.Xna.Framework.Input;
using RobotGame.Game.Mover;

namespace RobotGame.Game.Weapon
{
    class TrackingLaser : Laser
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private Point collidingDestructibleTile;
        private bool collidingWithMover;
        private bool collideWithMovers;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public TrackingLaser(int damage, Vector2 position, Vector2 direction, bool collideWithMovers)
            : base(damage, position, direction)
        {
            this.collideWithMovers = collideWithMovers;
            this.collidingDestructibleTile = new Point(-1, -1);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update(GameTime gameTime)
        {
            float distance = Vector2.Distance(this.laserLine.Point1, this.position);
            this.laserLine.Point1 = this.position;

            bool collidingDestructibleTileDestroyed = this.collidingDestructibleTile.X != -1 && !Level.GetInstance().IsDestructibleTile(this.collidingDestructibleTile.X, this.collidingDestructibleTile.Y);
            if (distance > 0.02f || this.collidingWithMover || collidingDestructibleTileDestroyed)
            {
                this.collidingDestructibleTile = new Point(-1, -1);
                Point collidingTile = new Point(-1, -1);
                this.laserLine = CollideLineWithEnvironment(this.laserLine, false, false, ref collidingTile);
                if (collidingTile.X != -1 && Level.GetInstance().IsDestructibleTile(collidingTile.X, collidingTile.Y))
                {
                    this.collidingDestructibleTile = collidingTile;
                }
            }

            this.collidingWithMover = CheckMoverCollision(this.laserLine);

            base.Update(gameTime);
        }
        
        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
