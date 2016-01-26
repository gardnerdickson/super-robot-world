using System;
using RobotGame.Game.Waypoint;
using RobotGame.Game.Weapon;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Enemy
{
    class SimpleTurret : Turret
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int START_HEALTH = 50;
        private const int MASS = 150;
        
        // Data Members --------------------------------------------------------------------------------- Data Members
        
        private bool enabled;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public SimpleTurret(Vector2 position, EnemyOrientation orientation, float speed, string name, string onDeathPowerupSpawnType, PhysicsMode onDeathPowerupPhysicsMode, MapWaypoint[] waypoints, float fireDuration, float fireDelay, bool invincible, bool laserCollideWithMovers, SpriteKey bodySprite, SpriteKey nozzleSprite)
            : base(position, orientation, speed, name, onDeathPowerupSpawnType, invincible, onDeathPowerupPhysicsMode, waypoints, bodySprite, nozzleSprite)
        {
            this.health = START_HEALTH;
            this.pointValue = START_HEALTH;
            this.mass = MASS;

            if (invincible)
            {
                this.laserOffset = new Vector2(24, 24);
            }
            else
            {
                this.laserOffset = new Vector2(50, 50);
            }
            
            this.laserWeapon = new FixedDirectionLaserWeapon(50, new SimpleDelayLogic(fireDelay), fireDuration, laserCollideWithMovers);
            this.laserWeapon.Direction = this.fireDirection;

            this.enabled = true;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void TakeDamage(int damage)
        {
            if (!this.invincible)
            {
                base.TakeDamage(damage);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (Camera.GetInstance().IsActorOnScreen(this))
            {
                this.nozzleSprite.Draw(spriteBatch, this.position, (float)Math.Atan2(this.fireDirection.Y, this.fireDirection.X), NOZZLE_DRAW_DEPTH, SpriteEffects.None);
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void AttackPlayer(Player player)
        {
            if (this.enabled)
            {
                this.laserWeapon.Position = this.position + (this.laserOffset * this.fireDirection);
                this.laserWeapon.TryFire(-1);
            }
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

    }
}
