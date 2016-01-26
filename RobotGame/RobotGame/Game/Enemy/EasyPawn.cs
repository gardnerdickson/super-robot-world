using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Enemy
{
    class EasyPawn : AbstractEnemy
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int START_HEALTH = Config.EASY_PAWN_START_HEALTH;
        private const float MASS = Config.EASY_PAWN_MASS;
        private const float HORIZONTAL_SPEED = Config.EASY_PAWN_HORIZONTAL_SPEED;
        private const float ATTACK_RANGE = Config.EASY_PAWN_ATTACK_RANGE;
        private const float FIRE_DELAY = Config.EASY_PAWN_FIRE_DELAY;
        private const float CHANGE_DIRECTION_DELAY = Config.EASY_PAWN_CHANGE_DIRECTION_DELAY;
        private const float PROJECTILE_SPEED = Config.EASY_PAWN_PROJECTILE_BULLET_SPEED;
        private const int PROJECTILE_DAMAGE = Config.EASY_PAWN_PROJECTILE_DAMAGE;
        private const float PROJECTILE_SPAWN_OFFSET = Config.PAWN_PROJECTILE_SPAWN_OFFSET;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback ChangeDirectionCallback;

        private AbstractWeapon weapon;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public EasyPawn(Vector2 position, string name, int direction, string onDeathPowerupSpawnType, PhysicsMode onDeathPowerupPhysicsMode)
            : base(position, PhysicsMode.Gravity, name, onDeathPowerupSpawnType, onDeathPowerupPhysicsMode)
        {
            this.sprite = new Sprite(SpriteKey.EasyPawn);
            this.health = START_HEALTH;
            this.pointValue = START_HEALTH;
            this.mass = MASS;
            this.velocity = new Vector2(HORIZONTAL_SPEED * direction, 0f);

            ProjectileFactory bulletFactory = new BulletFactory(PROJECTILE_DAMAGE, SpriteKey.EasyPawnProjectile, SpriteKey.EnemyBulletCollisionParticle, null, ProjectileSource.Enemy);
            FireLogic simpleFireLogic = new SimpleFireLogic(false);
            this.weapon = new ProjectileLauncher(bulletFactory, new SimpleDelayLogic(FIRE_DELAY), simpleFireLogic, PROJECTILE_SPEED, SoundKey.EnemyFire, SoundKey.FireNoAmmo);

            this.ChangeDirectionCallback = new TimerCallback(change_direction_callback);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void PerformAI(Player player)
        {
            // If we are facing the player and are close enough to the player, fire
            if (IsFacingActorHorizontally(player))
            {
                if (Math.Abs(this.position.X - player.Position.X) <= ATTACK_RANGE)
                {
                    FireWeapon();
                }
            }
            else
            {
                if (!TimerManager.GetInstance().IsTimerRegistered(this.ChangeDirectionCallback))
                {
                    TimerManager.GetInstance().RegisterTimer(CHANGE_DIRECTION_DELAY, this.ChangeDirectionCallback, null);
                }
            }

            DirectionOrientateHorizontal();
        }

        public override void Remove()
        {
            base.Remove();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void OnDeath()
        {
            Explode(SpriteKey.EasyPawnExplosion, SpriteSheetFactory.EASY_PAWN_EXPLOSION_TILES_X, SpriteSheetFactory.EASY_PAWN_EXPLOSION_TILES_Y);
            base.OnDeath();
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void FireWeapon()
        {
            Vector2 fireDirection = new Vector2(this.velocity.X, 0f);
            fireDirection.Normalize();

            this.weapon.Direction = fireDirection;
            this.weapon.Position = this.position + (fireDirection * PROJECTILE_SPAWN_OFFSET);

            this.weapon.TryFire(-1);
        }

        private void change_direction_callback(Object param)
        {
            if (!IsFacingActorHorizontally(this))
            {
                this.velocity.X *= -1;
            }
        }
    }
}
