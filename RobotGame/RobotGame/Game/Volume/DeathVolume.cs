using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Enemy;
using RobotGame.Game.Powerup;

namespace RobotGame.Game.Volume
{
    class DeathVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public DeathVolume(Rectangle bounds)
            : base(bounds)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update()
        {
            SentientActor player = (SentientActor) Player.PlayerList[0];
            if (CollisionUtil.CheckIntersectionCollision(player.Bounds, this.bounds) != Rectangle.Empty)
            {
                ((Player)player).Invincible = false;
                player.TakeDamage(player.Health);
            }

            foreach (SentientActor enemy in AbstractEnemy.EnemyList)
            {
                if (CollisionUtil.CheckIntersectionCollision(enemy.Bounds, this.bounds) != Rectangle.Empty)
                {
                    enemy.TakeDamage(enemy.Health);
                }
            }

            foreach (AbstractPowerup powerup in AbstractPowerup.PowerupList)
            {
                if (CollisionUtil.CheckIntersectionCollision(powerup.Bounds, this.bounds) != Rectangle.Empty)
                {
                    powerup.Remove();
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
