using System;
using RobotGame.Game.Waypoint;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Weapon;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Enemy
{
    class TrackingTurret : Turret
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int START_HEALTH = 50;
        private const int MASS = 150;

        private const int HORIZONTAL_ATTACK_THRESHOLD = 1000;
        private const int VERTICAL_ATTACK_THRESHOLD = 800;
        private const float LERP_AMOUNT = 0.07f;
        private const float NOZZLE_RANGE = 150; // degrees

        // Data Members --------------------------------------------------------------------------------- Data Members

        private Vector2 direction;

        private Vector2 nozzleOffset;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public TrackingTurret(Vector2 position, EnemyOrientation orientation, float speed, string name, string onDeathPowerupSpawnType, bool invincible, PhysicsMode onDeathPoweurpPhysicsMode, MapWaypoint[] waypoints, SpriteKey bodySprite, SpriteKey nozzleSprite)
            : base(position, orientation, speed, name, onDeathPowerupSpawnType, invincible, onDeathPoweurpPhysicsMode, waypoints, bodySprite, nozzleSprite)
        {
            this.health = START_HEALTH;
            this.pointValue = START_HEALTH;
            this.mass = MASS;

            this.laserWeapon = new TrackingLaserWeapon(50, true);

            if (this.invincible)
            {
                this.laserOffset = new Vector2(45, 45);
            }
            else
            {
                this.laserOffset = new Vector2(58, 58);
            }

            this.nozzleOffset = new Vector2(20, 20);
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
                this.nozzleSprite.Draw(spriteBatch, this.position + this.nozzleOffset * this.direction, (float)Math.Atan2(this.direction.Y, this.direction.X), NOZZLE_DRAW_DEPTH, SpriteEffects.None);
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void AttackPlayer(Player player)
        {
            Vector2 vectorToTarget = player.Position - this.position;
            Vector2 directionToTarget = Vector2.Normalize(vectorToTarget);

            if (Math.Abs(vectorToTarget.X) < HORIZONTAL_ATTACK_THRESHOLD && Math.Abs(vectorToTarget.Y) < VERTICAL_ATTACK_THRESHOLD)
            {
                Vector2 directionVector = Vector2.Normalize(Vector2.Lerp(this.direction, directionToTarget, LERP_AMOUNT));
                
                if (IsVectorInRange(directionVector, NOZZLE_RANGE, this.orientation))
                {
                    this.direction = directionVector;

                    this.laserWeapon.Direction = this.direction;
                    this.laserWeapon.Position = this.position + this.laserOffset * this.direction;

                    this.laserWeapon.TryFire(-1);
                }
            }
            else
            {
                ((LaserWeapon)this.laserWeapon).Stop();
                this.direction = this.fireDirection;
            }
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
