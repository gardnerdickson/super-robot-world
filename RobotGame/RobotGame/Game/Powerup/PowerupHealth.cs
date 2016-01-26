using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Powerup
{
    class PowerupHealth : AbstractPowerup
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int HEALTH_VALUE = Config.HEALTH_POWERUP_VALUE;

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public PowerupHealth(Vector2 position, Vector2 velocity, PhysicsMode physicsMode)
            : base(position, velocity, physicsMode)
        {
            this.sprite = new Sprite(SpriteKey.HealthPowerup);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void ApplyPowerup(Player player)
        {
            player.Health = Math.Min(player.Health + HEALTH_VALUE, Player.PLAYER_HEALTH_MAX);
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
