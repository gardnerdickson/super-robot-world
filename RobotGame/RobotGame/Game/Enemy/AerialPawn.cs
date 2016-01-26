using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;
using Microsoft.Xna.Framework.Input;
using RobotGame.Exceptions;

namespace RobotGame.Game.Enemy
{
    class AerialPawn : AbstractEnemy
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float DRAW_DEPTH = Config.AERIAL_PAWN_DRAW_DEPTH;

        private const int START_HEALTH = Config.AERIAL_PAWN_START_HEALTH;
        private const int RANGE = Config.AERIAL_PAWN_VERTICAL_RANGE;
        private const float HORIZONTAL_SPEED = Config.AERIAL_PAWN_HORIZONTAL_SPEED;
        private const float VERTICAL_SPEED = Config.AERIAL_PAWN_VERTICAL_SPEED;

        private const float FIRE_DELAY = Config.AERIAL_PAWN_FIRE_DELAY;
        private const int PROJECTILE_DAMAGE = Config.AERIAL_PAWN_PROJECTILE_DAMAGE;
        private const float PROJECTILE_SPEED = Config.AERIAL_PAWN_PROJECTILE_SPEED;
        private const int PROJECTILE_SPAWN_OFFSET = Config.AERIAL_PAWN_PROJECTILE_SPAWN_OFFSET;
        private const float PARTICLE_SPAWN_FREQUENCY = Config.AERIAL_PAWN_PROJECTILE_PARTICLE_SPAWN_FREQUENCY;

        private const float HORIZONTAL_ATTACK_THRESHOLD = Config.AERIAL_PAWN_HORIZONTAL_ATTACK_THRESHOLD;
        private const float VERTICAL_ATTACK_THRESHOLD = Config.AERIAL_PAWN_VERTICAL_ATTACK_THRESHOLD;
        
        // Data Members --------------------------------------------------------------------------------- Data Members

        private int maxY;
        private int minY;

        private int target;

        private AbstractWeapon weapon;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public AerialPawn(Vector2 position, string name, int direction, string onDeathPowerupSpawnType, PhysicsMode onDeathPowerupPhysicsMode)
            : base(position, PhysicsMode.None, name, onDeathPowerupSpawnType, onDeathPowerupPhysicsMode)
        {
            this.sprite = new Sprite(SpriteKey.AerialPawn);
            this.health = START_HEALTH;
            this.pointValue = START_HEALTH;

            this.minY = (int)position.Y - RANGE / 2;
            this.maxY = (int)position.Y + RANGE / 2;
            this.target = minY;

            this.velocity = new Vector2(HORIZONTAL_SPEED * direction, -VERTICAL_SPEED);

            DelayLogic simpleDelayLogic = new SimpleDelayLogic(FIRE_DELAY);
            ParticleEmitterFactory particleEmitterFactory = new ParticleEmitterFactory(PARTICLE_SPAWN_FREQUENCY, SpriteKey.AerialPawnProjectileParticle, 0f, 1.0f, 1.0f);
            ProjectileFactory projectileFactory = new BulletFactory(PROJECTILE_DAMAGE, SpriteKey.AerialPawnProjectile, SpriteKey.EnemyBulletCollisionParticle,
                                                                    particleEmitterFactory, ProjectileSource.Enemy);
            FireLogic simpleFireLogic = new SimpleFireLogic(false);

            this.weapon = new ProjectileLauncher(projectileFactory, simpleDelayLogic, simpleFireLogic, PROJECTILE_SPEED, SoundKey.EnemyFire, SoundKey.FireNoAmmo);

            this.collideWithEnvironment = false;

            this.drawDepth = DRAW_DEPTH;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods
        
        public override void PerformAI(Player player)
        {
            UpdateTargetPosition();

            // Check if we should fire at the player
            if (Math.Abs(player.Position.X - this.position.X) <= HORIZONTAL_ATTACK_THRESHOLD &&
                Math.Abs(player.Position.Y - this.position.Y) <= VERTICAL_ATTACK_THRESHOLD)
            {
                FireWeapon(Vector2.Normalize(player.Position - this.position));
            }
        }
        
        public override void Remove()
        {
            base.Remove();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void OnDeath()
        {
            Explode(SpriteKey.AerialPawnExplosion, SpriteSheetFactory.AERIAL_PAWN_EXPLOSION_TILES_X, SpriteSheetFactory.AERIAL_PAWN_EXPLOSION_TILES_Y);
            base.OnDeath();
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void UpdateTargetPosition()
        {
            if (this.target == minY)
            {
                if (this.position.Y < minY)
                {
                    this.target = this.maxY;
                    this.velocity.Y = VERTICAL_SPEED;
                }
            }
            else
            {
                if (this.position.Y > maxY)
                {
                    this.target = this.minY;
                    this.velocity.Y = -VERTICAL_SPEED;
                }
            }
        }

        private void FireWeapon(Vector2 launchDirection)
        {
            if (Level.GetInstance().IsBoundsOnMap(this.Bounds))
            {
                if (!this.CheckMapIntersection(this.Bounds))
                {
                    this.weapon.Direction = launchDirection;
                    this.weapon.Position = this.position + (launchDirection * PROJECTILE_SPAWN_OFFSET);

                    this.weapon.TryFire(1);
                }
            }
            else
            {
                this.Remove();
            }
        }
    }
}
