
using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Volume
{
    class EventTriggerVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TriggerKey key;
        private bool activated;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public EventTriggerVolume(Rectangle bounds, TriggerKey key)
            : base(bounds)
        {
            this.key = key;
            this.activated = false;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update()
        {
            if (!this.activated)
            {
                Player player = (Player)Player.PlayerList[0];
                if (CollisionUtil.CheckIntersectionCollision(this.bounds, player.Bounds) != Rectangle.Empty)
                {
                    this.activated = true;
                    Triggers.TriggerEvent(key);
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
