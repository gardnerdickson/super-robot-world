using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Weapon;

namespace RobotGame.Game.Powerup
{
    class PowerupGrenade : AbstractPowerup
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float GRENADE_LAUNCHER_DELAY = Config.PLAYER_SECONDARY_FIRE_DELAY;
        private const float GRENADE_LAUNCH_SPEED = Config.PLAYER_GRENADE_SPEED;
        private const int AMMO_VALUE = Config.AMMO_POWERUP_VALUE;

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public PowerupGrenade(Vector2 position, Vector2 velocity, PhysicsMode physicsMode)
            : base(position, velocity, physicsMode)
        {
            this.sprite = new Sprite(SpriteKey.GrenadePowerup);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void ApplyPowerup(Player player)
        {
            player.SecondaryWeaponInventory.Ammo += AMMO_VALUE;

            // If the player already has a grenade launcher, leave its inventory state alone
            if (player.SecondaryWeaponInventory.GetState(WeaponInventory.GRENADE_LAUNCHER_INDEX) != InventoryState.Unavailable)
            {
                return;
            }

            if (player.SecondaryWeaponInventory.GetSelectedWeapon() == null)
            {
                player.SecondaryWeaponInventory.SetState(WeaponInventory.GRENADE_LAUNCHER_INDEX, InventoryState.Selected);
            }
            else
            {
                player.SecondaryWeaponInventory.SetState(WeaponInventory.GRENADE_LAUNCHER_INDEX, InventoryState.Available);
            }

        }

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
