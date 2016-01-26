using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using Microsoft.Xna.Framework.Input;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Enemy
{
    class AerialBomber : AbstractEnemy
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int START_HEALTH = Config.AERIAL_BOMBER_START_HEALTH;
        private const float AERIAL_BOMBER_MAX_HORIZONTAL_SPEED = Config.AERIAL_BOMBER_MAX_HORIZONTAL_SPEED;
        private const float ACCELERATION = Config.AERIAL_BOMBER_ACCELERATION;
        private const float HORIZONTAL_ATTACK_RANGE = Config.AERIAL_BOMBER_HORIZONTAL_ATTACK_RANGE;
        private const float VERTICAL_ATTACK_RANGE = Config.AERIAL_BOMBER_VERTICAL_ATTACK_RANGE;
        private const float PROJECTILE_FIRE_DELAY = Config.AERIAL_BOMBER_FIRE_DELAY;
        private const int PROJECTILE_BLAST_DAMAGE = Config.AERIAL_BOMBER_PROJECTILE_BLAST_DAMAGE;
        private const int PROJECTILE_BLAST_RADIUS = Config.AERIAL_BOMBER_PROJECTILE_BLAST_RADIUS;
        private const float PROJECTILE_LAUNCH_SPEED = Config.AERIAL_BOMBER_PROJECTILE_LAUNCH_SPEED;
        private const float PROJECTILE_MASS = Config.AERIAL_BOMBER_PROJECTILE_MASS;
        //private const float PROJECTILE_GRAVITY_FORCE = Config.ENEMY_PROJECTILE_GRAVITY_FORCE;
        private const float PROJECTILE_GRAVITY_FORCE = 17f;
        private const float PROJECTILE_ROTATION_INCREMENT = Config.AERIAL_BOMBER_PROJECTILE_ROTATION_INCREMENT;
        private const float PROJECTILE_SPAWN_OFFSET = Config.AERIAL_BOMBER_PROJECTILE_SPAWN_OFFSET;
        private const double CHANGE_DIRECTION_DELAY = Config.AERIAL_BOMBER_CHANGE_DIRECTION_DELAY;
        private const int CHANGE_DIRECTION_RANDOM_RANGE = Config.AERIAL_BOMBER_CHANGE_DIRECTION_RANDOM_RANGE;

        private static readonly Vector2 FIRE_DIRECTION = new Vector2(0f, 1f);

        // Data Members --------------------------------------------------------------------------------- Data Members

        private Vector2 acceleration;

        private bool waitingToChangeDirection;
        private bool changeDirectionEnabled;

        private AbstractWeapon weapon;

        private TimerCallback ChangeDirectionTimerNotify;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public AerialBomber(Vector2 position, string name, string onDeathPowerupSpawnType, PhysicsMode onDeathPowerupPhysicsMode)
            : base(position, PhysicsMode.None, name, onDeathPowerupSpawnType, onDeathPowerupPhysicsMode)
        {
            this.sprite = new Sprite(SpriteKey.AerialBomber);
            this.health = START_HEALTH;
            this.pointValue = START_HEALTH;
            this.acceleration = new Vector2(ACCELERATION, 0f);
            this.mapBoundsCorrectionEnabled = true;

            this.changeDirectionEnabled = false;

            this.waitingToChangeDirection = false;

            this.ChangeDirectionTimerNotify += new TimerCallback(change_direction_enable);

            DelayLogic simpleDelayLogic = new SimpleDelayLogic(PROJECTILE_FIRE_DELAY);
            ProjectileFactory projectileFactory = new ExplosiveProjectileFactory(PROJECTILE_BLAST_DAMAGE, PROJECTILE_BLAST_RADIUS, PROJECTILE_MASS, PROJECTILE_GRAVITY_FORCE,
                                                                                 PROJECTILE_ROTATION_INCREMENT, SpriteKey.AerialBomberProjectile, ProjectileSource.Enemy);
            FireLogic simpleFireLogic = new SimpleFireLogic(false);
            this.weapon = new ProjectileLauncher(projectileFactory, simpleDelayLogic, simpleFireLogic, PROJECTILE_LAUNCH_SPEED, SoundKey.EnemyFire, SoundKey.FireNoAmmo);
        }

        public AerialBomber(Vector2 position, string name)
            : this(position, name, null, PhysicsMode.None)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods
        
        public override void PerformAI(Player player)
        {
            if (!this.waitingToChangeDirection)
            {
                if (!IsFacingActorHorizontally(player))
                {
                    // Inject some randomness into the change direction behaviour
                    Random random = new Random();
                    int randomNumber = random.Next(-CHANGE_DIRECTION_RANDOM_RANGE, CHANGE_DIRECTION_RANDOM_RANGE + 1);

                    TimerManager.GetInstance().RegisterTimer(CHANGE_DIRECTION_DELAY + randomNumber, this.ChangeDirectionTimerNotify, null);
                    this.waitingToChangeDirection = true;
                }
            }
            else if (this.waitingToChangeDirection && this.changeDirectionEnabled)
            {
                if (!IsFacingActorHorizontally(player))
                {
                    this.acceleration *= -1;
                }

                this.waitingToChangeDirection = false;
                this.changeDirectionEnabled = false;
            }

            this.velocity += this.acceleration;
            this.velocity.X = MathHelper.Clamp(this.velocity.X, -AERIAL_BOMBER_MAX_HORIZONTAL_SPEED, AERIAL_BOMBER_MAX_HORIZONTAL_SPEED);

            // If we are close enough to the player, drop bomb
            Vector2 distance = this.position - player.Position;
            
            if (Math.Abs(distance.X) <= HORIZONTAL_ATTACK_RANGE && Math.Abs(distance.Y) <= VERTICAL_ATTACK_RANGE)
            {
                FireWeapon();
            }
        }

        public override void Remove()
        {
            this.weapon.FireLogic.Interrupt();
            base.Remove();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void OnDeath()
        {
            Explode(SpriteKey.AerialBomberExplosion, SpriteSheetFactory.AERIAL_BOMBER_EXPLOSION_TILES_X, SpriteSheetFactory.AERIAL_BOMBER_EXPLOSION_TILES_Y);
            base.OnDeath();
        }

        protected override bool IsFacingActorHorizontally(GameActor actor)
        {
            if (this.position.X - actor.Position.X > 0 && this.acceleration.X < 0 ||
                this.position.X - actor.Position.X < 0 && this.acceleration.X > 0)
            {
                return true;
            }
            return false;
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void change_direction_enable(Object param)
        {
            this.changeDirectionEnabled = true;
        }

        private void FireWeapon()
        {
            Vector2 fireDirection = Vector2.Normalize(FIRE_DIRECTION);
            
            this.weapon.Direction = fireDirection;
            this.weapon.Position = this.position + (fireDirection * PROJECTILE_SPAWN_OFFSET);

            this.weapon.TryFire(1);
        }
    }
}
