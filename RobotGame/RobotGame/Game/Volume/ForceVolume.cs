using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Enemy;
using RobotGame.Game.Weapon;

namespace RobotGame.Game.Volume
{
    class ForceVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private Vector2 force;
         
        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public ForceVolume(Rectangle bounds, Vector2 force)
            : base(bounds)
        {
            this.force = force;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update()
        {
            GameActor player = Player.PlayerList[0];
            if (CollisionUtil.CheckIntersectionCollision(player.Bounds, this.bounds) != Rectangle.Empty)
            {
                player.Force += this.force;
            }
            foreach (GameActor enemy in AbstractEnemy.EnemyList)
            {
                if (CollisionUtil.CheckIntersectionCollision(enemy.Bounds, this.bounds) != Rectangle.Empty)
                {
                    enemy.Force += this.force;
                }
            }
            foreach (Projectile projectile in Projectile.PlayerProjectileList)
            {
                if (CollisionUtil.CheckIntersectionCollision(projectile.Bounds, this.bounds) != Rectangle.Empty)
                {
                    projectile.Force += this.force;
                }
            }
            foreach (Projectile projectile in Projectile.EnemyProjectileList)
            {
                if (CollisionUtil.CheckIntersectionCollision(projectile.Bounds, this.bounds) != Rectangle.Empty)
                {
                    projectile.Force += this.force;
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
