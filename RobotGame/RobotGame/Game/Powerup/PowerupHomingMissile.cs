using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Weapon;

namespace RobotGame.Game.Powerup
{
    class PowerupHomingMissile : AbstractPowerup
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float HOMING_MISSILE_LAUNCHER_DELAY = Config.PLAYER_SECONDARY_FIRE_DELAY;
        private const float HOMING_MISSILE_LAUNCH_SPEED = Config.PLAYER_GRENADE_SPEED;
        private const int AMMO_VALUE = Config.AMMO_POWERUP_VALUE;

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public PowerupHomingMissile(Vector2 position, Vector2 velocity, PhysicsMode physicsMode)
            : base(position, velocity, physicsMode)
        {
            this.sprite = new Sprite(SpriteKey.HomingMissilePowerup);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void ApplyPowerup(Player player)
        {
            player.SecondaryWeaponInventory.Ammo += AMMO_VALUE;

            // If the player already has a grenade launcher, leave its inventory state alone
            if (player.SecondaryWeaponInventory.GetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX) != InventoryState.Unavailable)
            {
                return;
            }
            
            if (player.SecondaryWeaponInventory.GetSelectedWeapon() == null)
            {
                player.SecondaryWeaponInventory.SetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX, InventoryState.Selected);
            }
            else
            {
                player.SecondaryWeaponInventory.SetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX, InventoryState.Available);
            }
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
