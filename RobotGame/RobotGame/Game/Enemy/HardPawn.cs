using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;
using RobotGame.Exceptions;

namespace RobotGame.Game.Enemy
{
    class HardPawn : AbstractEnemy
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int START_HEALTH = Config.HARD_PAWN_START_HEALTH;
        private const float MASS = Config.HARD_PAWN_MASS;
        private const float GRAVITY_FORCE = Config.HARD_PAWN_GRAVITY_FORCE;
        private const float JUMP_FORCE = Config.HARD_PAWN_JUMP_FORCE;
        private const float HORIZONTAL_SPEED = Config.HARD_PAWN_HORIZONTAL_SPEED;
        private const float PROJECTILE_PROXIMITY_THRESHOLD = Config.HARD_PAWN_PROJECTILE_HORIZONAL_PROXIMITY_THRESHOLD;
        private const float PROJECTILE_ANGLE_THRESHOLD = Config.HARD_PAWN_PROJECTILE_ANGLE_THRESHOLD;
        private const float JUMP_VELOCITY = Config.HARD_PAWN_JUMP_VELOCITY;
        private const float JUMP_DISABLE_TIME = Config.HARD_PAWN_JUMP_DISABLE_TIME;
        private const float ATTACK_RANGE = Config.HARD_PAWN_ATTACK_RANGE;
        private const float DIRECTION_CHANGE_DELAY = Config.HARD_PAWN_DIRECTION_CHANGE_DELAY;
        private const float DIRECTION_CHANGE_RANDOM_RANGE = Config.HARD_PAWN_DIRECTION_CHANGE_DELAY_RANDOM_RANGE;
        private const float FIRE_DELAY = Config.HARD_PAWN_FIRE_DELAY;
        private const float PROJECTILE_SPEED = Config.HARD_PAWN_PROJECTILE_BULLET_SPEED;
        private const int PROJECTILE_DAMAGE = Config.HARD_PAWN_PROJECTILE_DAMAGE;
        private const float PROJECTILE_SPAWN_OFFSET = Config.PAWN_PROJECTILE_SPAWN_OFFSET;
        private const float BURST_FIRE_DELAY = Config.HARD_PAWN_BURST_FIRE_DELAY;
        private const int BURST_FIRE_AMOUNT = Config.HARD_PAWN_BURST_FIRE_AMOUNT;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static Random random = new Random();

        private TimerCallback JumpTimerNotify;
        private TimerCallback ChangeDirectionTimerNotify;

        private AbstractWeapon weapon;

        private bool jumpEnabled;
        private bool changeDirectionEnabled;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public HardPawn (Vector2 position, string name, int direction, string onDeathPowerupSpawnType, PhysicsMode onDeathPowerupPhysicsMode)
            : base(position, PhysicsMode.Gravity, name, onDeathPowerupSpawnType, onDeathPowerupPhysicsMode)
        {
            this.sprite = new Sprite(SpriteKey.HardPawn);
            this.health = START_HEALTH;
            this.pointValue = START_HEALTH;
            this.mass = MASS;
            this.velocity = new Vector2(HORIZONTAL_SPEED * direction, 0f);
            this.mapBoundsCorrectionEnabled = true;

            ProjectileFactory projectileFactory = new BulletFactory(PROJECTILE_DAMAGE, SpriteKey.HardPawnProjectile, SpriteKey.EnemyBulletCollisionParticle, null,ProjectileSource.Enemy);

            int[] burstFireAmounts = new int[] { BURST_FIRE_AMOUNT };
            FireLogic burstFireLogic = new BurstFireLogic(false, BURST_FIRE_DELAY, burstFireAmounts);
            this.weapon = new ProjectileLauncher(projectileFactory, new SimpleDelayLogic(FIRE_DELAY), burstFireLogic, PROJECTILE_SPEED, SoundKey.EnemyFire, SoundKey.FireNoAmmo);

            this.jumpEnabled = true;

            this.physicsController.GravityForce = GRAVITY_FORCE;

            this.JumpTimerNotify += new TimerCallback(jump_enable);
            this.ChangeDirectionTimerNotify += new TimerCallback(change_direction_enable);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void PerformAI(Player player)
        {
            bool facingPlayer = IsFacingActorHorizontally(player);
            
            // If we can see a projectile coming at us, we jump to try to avoid it
            if (this.jumpEnabled && facingPlayer)
            {
                foreach (GameActor projectile in Projectile.PlayerProjectileList)
                {
                    Vector2 projectileVelocityNormalized = Vector2.Normalize(projectile.Velocity);
                    Vector2 horizontalVector = new Vector2(1, 0);

                    float angleRadians1 = (float)Math.Acos(Vector2.Dot(projectileVelocityNormalized, horizontalVector));
                    float angleRadians2 = (float)Math.Acos(Vector2.Dot(projectileVelocityNormalized, -horizontalVector));
                    float angleDegrees1 = MathHelper.ToDegrees(angleRadians1);
                    float angleDegrees2 = MathHelper.ToDegrees(angleRadians2);

                    // If the projectile is within 100 pixels from us horizontally, is at the same verical height as us, and
                    // has a velocity angle of less than 30 from the horizontal, then jump.
                    if (Math.Abs(this.position.X - projectile.Position.X) < PROJECTILE_PROXIMITY_THRESHOLD &&
                        Math.Abs(this.position.Y - projectile.Position.Y) <= this.sprite.Height / 2 &&
                        (angleDegrees1 <= PROJECTILE_ANGLE_THRESHOLD || angleDegrees2 <= PROJECTILE_ANGLE_THRESHOLD))
                    {
                        this.Jump();
                    }
                }
            }

            // Update weapon position and direction
            Vector2 fireDirection = new Vector2(this.velocity.X, 0f);
            fireDirection.Normalize();
            this.weapon.Direction = fireDirection;
            this.weapon.Position = this.position + (fireDirection * PROJECTILE_SPAWN_OFFSET);

            // If we are facing the player and are close enough to the player, fire
            if (facingPlayer)
            {
                if (Math.Abs(this.position.X - player.Position.X) <= ATTACK_RANGE)
                {
                    FireWeapon();
                }
            }
            else
            {
                if (!(TimerManager.GetInstance().IsTimerRegistered(this.ChangeDirectionTimerNotify)))
                {
                    double directionChangeRandomValue = random.NextDouble() * DIRECTION_CHANGE_RANDOM_RANGE;
                    directionChangeRandomValue = (random.Next(10) < 5) ? directionChangeRandomValue * -1 : directionChangeRandomValue;
                    TimerManager.GetInstance().RegisterTimer(DIRECTION_CHANGE_DELAY + directionChangeRandomValue, this.ChangeDirectionTimerNotify, null);
                }
            }

            if (this.changeDirectionEnabled)
            {
                this.changeDirectionEnabled = false;
                if (!facingPlayer)
                {
                    this.velocity.X *= -1;
                }
            }

            DirectionOrientateHorizontal();
        }

        public override void Remove()
        {
            this.weapon.FireLogic.Interrupt();
            base.Remove();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void OnDeath()
        {
            Explode(SpriteKey.HardPawnExplosion, SpriteSheetFactory.EASY_PAWN_EXPLOSION_TILES_X, SpriteSheetFactory.EASY_PAWN_EXPLOSION_TILES_Y);
            base.OnDeath();
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void jump_enable(Object param)
        {
            this.jumpEnabled = true;
        }

        private void change_direction_enable(Object param)
        {
            this.changeDirectionEnabled = true;
        }

        private void Jump()
        {
            if (this.environmentState == EnvironmentState.OnGround)
            {
                this.environmentState = EnvironmentState.InAir;
                this.force += new Vector2(0f, -JUMP_FORCE);
                this.jumpEnabled = false;
                TimerManager.GetInstance().RegisterTimer(JUMP_DISABLE_TIME, this.JumpTimerNotify, null);
            }
        }

        private void FireWeapon()
        {
            this.weapon.TryFire(-1);
        }
    }
}
