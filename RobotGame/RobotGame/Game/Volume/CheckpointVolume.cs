using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Weapon;

namespace RobotGame.Game.Volume
{
    class CheckpointVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private bool activated;
        private Vector2 respawnPosition;
        private int priority;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public CheckpointVolume(Rectangle bounds, Vector2 respawnPosition, int priority)
            : base(bounds)
        {
            this.activated = false;
            this.priority = priority;
            this.respawnPosition = respawnPosition;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update()
        {
            if (!activated)
            {
                Player player = (Player)Player.PlayerList[0];
                if (CollisionUtil.CheckIntersectionCollision(this.bounds, player.Bounds) != Rectangle.Empty)
                {
                    this.activated = true;
                    if (RobotGame.GameState.LevelCheckpointPriority < this.priority)
                    {
                        RobotGame.GameState.LevelCheckpointPriority = this.priority;
                        RobotGame.GameState.CheckpointRespawnPosition = this.respawnPosition;
                        RobotGame.GameState.SecondaryWeaponAmmo = player.SecondaryWeaponInventory.Ammo;
                        RobotGame.GameState.GrenadeLauncherInventoryState = player.SecondaryWeaponInventory.GetState(WeaponInventory.GRENADE_LAUNCHER_INDEX);
                        RobotGame.GameState.HomingMissileLauncherInventoryState = player.SecondaryWeaponInventory.GetState(WeaponInventory.HOMING_MISSILE_LAUNCHER_INDEX);
                        RobotGame.GameState.PlayerJetpackEnabled = player.JetPackEnabled;
                        RobotGame.GameState.PlayerPoints = player.Points;
                    }
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
