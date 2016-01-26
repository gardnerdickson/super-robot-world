using System;
using RobotGame.Game.Weapon;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Audio;
using RobotGame.Exceptions;

namespace RobotGame.Game.Enemy
{
    class Wasp : AbstractEnemy
    {
        enum Mode
        {
            Fire,
            Transition,
            CalculateTargetPosition,
        }

        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int ATTACK_RECTANGLE_WIDTH = Config.WASP_ATTACK_RECTANGLE_WIDTH;
        private const int ATTACK_RECTANGLE_HEIGHT = Config.WASP_ATTACK_RECTANGLE_HEIGHT;
        private const int ATTACK_RECTANGLE_OFFSET_Y = Config.WASP_ATTACK_RECTANGLE_OFFSET_Y;

        private const float ADDITIONAL_FIRE_DELAY = Config.WASP_ADDITIONAL_FIRE_DELAY;
        private const float BURST_DELAY = Config.WASP_BURST_DELAY;
        private static int BURST_AMOUNT = Config.WASP_BURST_AMOUNT;
        private const int PROJECTILE_DAMAGE = Config.WASP_PROJECTILE_DAMAGE;
        private const float PROJECTILE_SPEED = Config.WASP_PROJECTILE_SPEED;
        private const int PROJECTILE_SPAWN_OFFSET = Config.WASP_PROJECTILE_SPAWN_OFFSET;
        private const float PARTICLE_SPAWN_FREQUENCY = Config.WASP_PARTICLE_SPAWN_FREQUENCY;

        private const int TRANSITION_TICKS = Config.WASP_TRANSITION_TICKS;
        private const float MINIMUM_TRANSITION_DISTANCE = Config.WASP_MINIMUM_TRANSITION_DISTANCE;

        private const int START_HEALTH = Config.WASP_START_HEALTH;

        private const float DRAW_DEPTH = Config.AERIAL_PAWN_DRAW_DEPTH;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback CalculatePositionCallback;

        private static Random random = new Random();

        private Mode mode;

        private Vector2 targetPosition;
        private float transitionSpeed;
        
        private AbstractWeapon weapon;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Wasp(Vector2 position, string name, string onDeathPowerupSpawnType, PhysicsMode onDeathPowerupPhysicsMode)
            : base(position, PhysicsMode.None, name, onDeathPowerupSpawnType, onDeathPowerupPhysicsMode)
        {
            this.sprite = new Engine.Sprite(SpriteKey.Wasp);
            this.health = START_HEALTH;
            this.pointValue = START_HEALTH;

            this.mode = Mode.CalculateTargetPosition;

            CalculatePositionCallback = new TimerCallback(calculate_target_position);

            DelayLogic simpleDelayLogic = new SimpleDelayLogic(0);
            ParticleEmitterFactory particleEmitterFactory = new ParticleEmitterFactory(PARTICLE_SPAWN_FREQUENCY, SpriteKey.AerialPawnProjectileParticle, 0f, 1f, 1f);
            ProjectileFactory bulletFactory = new BulletFactory(PROJECTILE_DAMAGE, SpriteKey.AerialPawnProjectile, SpriteKey.EnemyBulletCollisionParticle,
                                                                    particleEmitterFactory, ProjectileSource.Enemy);
            FireLogic burstFireLogic = new BurstFireLogic(false, BURST_DELAY, new int[]{BURST_AMOUNT});

            this.weapon = new ProjectileLauncher(bulletFactory, simpleDelayLogic, burstFireLogic, PROJECTILE_SPEED, SoundKey.EnemyFire, SoundKey.FireNoAmmo);

            this.collideWithEnvironment = false;

            this.drawDepth = DRAW_DEPTH;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void PerformAI(Player player)
        {
            if (this.mode == Mode.CalculateTargetPosition)
            {
                // Determine attack rectangle
                int x = (int)player.Position.X - (int)(ATTACK_RECTANGLE_WIDTH / 2);
                int y = (int)player.Position.Y + ATTACK_RECTANGLE_OFFSET_Y - (int)(ATTACK_RECTANGLE_WIDTH / 2);
                Rectangle attackRectangle = new Rectangle(x, y, ATTACK_RECTANGLE_WIDTH, ATTACK_RECTANGLE_HEIGHT);

                if (CheckMapBounds(new Vector2(attackRectangle.Center.X, attackRectangle.Center.Y)))
                {
                    y = (int)player.Position.Y - ATTACK_RECTANGLE_OFFSET_Y - (int)(ATTACK_RECTANGLE_WIDTH / 2);
                    attackRectangle = new Rectangle(x, y, ATTACK_RECTANGLE_WIDTH, ATTACK_RECTANGLE_HEIGHT);
                }
                
                int tries = 0;
                bool targetPositionInEnvironment = true;
                float transitionDistance = 0;
                float maxTransitionDistance = 0;
                while ((targetPositionInEnvironment || transitionDistance < MINIMUM_TRANSITION_DISTANCE) && tries < 5)
                {
                    // Determine position inside attack rectangle to move to
                    int randomX = random.Next(attackRectangle.Left + 1, attackRectangle.Right);
                    int randomY = random.Next(attackRectangle.Top + 1, attackRectangle.Bottom);
                    
                    Vector2 randomTargetPosition = new Vector2(randomX, randomY);
                    Rectangle targetBounds = new Rectangle((int)randomTargetPosition.X - (this.Bounds.Width / 2),
                                                           (int)randomTargetPosition.Y - (this.Bounds.Height / 2),
                                                           this.Bounds.Width, this.Bounds.Height);

                    transitionDistance = Vector2.Distance(this.position, randomTargetPosition);

                    if (Level.GetInstance().IsBoundsOnMap(targetBounds))
                    {
                        targetPositionInEnvironment = CheckMapIntersection(targetBounds);
                    }
                    else
                    {
                        targetPositionInEnvironment = true;
                    }

                    if (!targetPositionInEnvironment && transitionDistance > maxTransitionDistance)
                    {
                        maxTransitionDistance = transitionDistance;
                        this.targetPosition = randomTargetPosition;
                    }

                    tries++;
                }

                this.transitionSpeed = Vector2.Distance(this.position, targetPosition) / TRANSITION_TICKS;

                this.mode = Mode.Transition;
            }
            else if (this.mode == Mode.Transition)
            {
                if (Vector2.Distance(this.position, this.targetPosition) <= this.transitionSpeed)
                {
                    this.velocity = Vector2.Zero;
                    this.mode = Mode.Fire;
                }
                else
                {
                    Vector2 direction = Vector2.Normalize(this.targetPosition - this.position);
                    this.velocity = direction * this.transitionSpeed;
                }
            }
            else
            {
                if (!TimerManager.GetInstance().IsTimerRegistered(this.CalculatePositionCallback))
                {
                    Vector2 launchDirection = Vector2.Normalize(player.Position - this.position);
                    if (IsActorVisible(player))
                    {
                        FireWeapon(launchDirection);
                    }

                    float fireTime = (BURST_DELAY * BURST_AMOUNT) + ADDITIONAL_FIRE_DELAY;
                    TimerManager.GetInstance().RegisterTimer(fireTime, CalculatePositionCallback, null);
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void OnDeath()
        {
            Explode(SpriteKey.WaspExplosion, SpriteSheetFactory.WASP_EXPLOSION_TILES_X, SpriteSheetFactory.WASP_EXPLOSION_TILES_Y);
            base.OnDeath();
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        public void FireWeapon(Vector2 launchDirection)
        {
            this.weapon.Direction = launchDirection;
            this.weapon.Position = this.position + (launchDirection * PROJECTILE_SPAWN_OFFSET);

            this.weapon.TryFire(1);
        }

        public void calculate_target_position(Object param)
        {
            this.mode = Mode.CalculateTargetPosition;
        }
    }
}
