using System;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;
using Microsoft.Xna.Framework;

namespace RobotGame.Game
{
    public struct RobotGameState
    {
        public int Level;
        public int LevelCheckpointPriority;
        public Vector2 CheckpointRespawnPosition;
        public int PlayerPoints;
        public int PlayerLives;
        public int PlayerDeaths;
        public int PlayerHealth;
        public bool PlayerJetpackEnabled;
        public AbstractWeapon GrenadeLauncherPrototype;
        public AbstractWeapon HomingMissileLauncherPrototype;
        public InventoryState GrenadeLauncherInventoryState;
        public InventoryState HomingMissileLauncherInventoryState;
        public int SecondaryWeaponAmmo;
        public int ContinuesUsed;

        public RobotGameState(int level, int levelCheckpointPriority, Vector2 checkpointRespawnPosition, int playerPoints, int playerLives, int playerDeaths, int playerHealth, bool playerJetpackEnabled,
                              AbstractWeapon grenadeLauncherPrototype, AbstractWeapon homingMissileLauncherPrototype, InventoryState grenadeLauncherInventoryState,
                              InventoryState homingMissileLauncherInventoryState, int secondaryWeaponAmmo, int continuesUsed)
        {
            this.Level = level;
            this.LevelCheckpointPriority = levelCheckpointPriority;
            this.CheckpointRespawnPosition = checkpointRespawnPosition;
            this.PlayerPoints = playerPoints;
            this.PlayerHealth = playerHealth;
            this.PlayerLives = playerLives;
            this.PlayerDeaths = playerDeaths;
            this.PlayerJetpackEnabled = playerJetpackEnabled;
            this.GrenadeLauncherPrototype = grenadeLauncherPrototype;
            this.HomingMissileLauncherPrototype = homingMissileLauncherPrototype;
            this.GrenadeLauncherInventoryState = grenadeLauncherInventoryState;
            this.HomingMissileLauncherInventoryState = homingMissileLauncherInventoryState;
            this.SecondaryWeaponAmmo = secondaryWeaponAmmo;
            this.ContinuesUsed = continuesUsed;
        }
    }
}
