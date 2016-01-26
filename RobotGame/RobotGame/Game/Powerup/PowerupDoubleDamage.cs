using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Powerup
{
    class PowerupDoubleDamage : AbstractPowerup
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const double DOUBLE_DAMAGE_DURATION = Config.DOUBLE_DAMAGE_DURATION;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback TimerNotify;

        private Player player;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public PowerupDoubleDamage(Vector2 position, Vector2 velocity, PhysicsMode physicsMode)
            : base(position, velocity, physicsMode)
        {
            this.sprite = new Sprite(SpriteKey.DoubleDamagePowerup);

            this.TimerNotify = new TimerCallback(timer_notify);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void ApplyPowerup(Player player)
        {
            this.player = player;
            this.player.DoubleDamageEnabled = true;
            TimerManager.GetInstance().RegisterTimer(DOUBLE_DAMAGE_DURATION, this.TimerNotify, null);
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        public void timer_notify(Object param)
        {
            this.player.DoubleDamageEnabled = false;
        }
    }
}
