using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Powerup
{
    class PowerupJetPack : AbstractPowerup
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float JETPACK_FUEL_MAX = Config.JETPACK_FUEL_MAX;

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public PowerupJetPack(Vector2 position, Vector2 Velocity, PhysicsMode physicsMode)
            : base(position, Velocity, physicsMode)
        {
            this.sprite = new Sprite(SpriteKey.JetpackPowerup);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void ApplyPowerup(Player player)
        {
            player.JetPackEnabled = true;
            player.JetPackFuel = JETPACK_FUEL_MAX;
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
