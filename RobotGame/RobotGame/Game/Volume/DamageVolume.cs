using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Volume
{
    class DamageVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private int amount;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public DamageVolume(Rectangle bounds, int amount)
            : base(bounds)
        {
            this.amount = amount;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update()
        {
            SentientActor player = (SentientActor)Player.PlayerList[0];
            if (CollisionUtil.CheckIntersectionCollision(player.Bounds, this.bounds) != Rectangle.Empty)
            {
                player.TakeDamage(amount);
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
