using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Powerup
{
    class PowerupAmmo : AbstractPowerup
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int AMMO_VALUE = Config.AMMO_POWERUP_VALUE;

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public PowerupAmmo(Vector2 position, Vector2 velocity, PhysicsMode physicsMode)
            : base(position, velocity, physicsMode)
        {
            this.sprite = new Sprite(SpriteKey.AmmoPowerup);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void ApplyPowerup(Player player)
        {
            player.SecondaryWeaponInventory.Ammo += AMMO_VALUE;
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
