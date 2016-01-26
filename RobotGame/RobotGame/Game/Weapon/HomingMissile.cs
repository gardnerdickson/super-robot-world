using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Engine;
using RobotGame.Exceptions;
using RobotGame.Game.Enemy;
using RobotGame.Game.Audio;


namespace RobotGame.Game.Weapon
{
    enum HomingMissilePhase
    {
        Launch,
        Seek,
        TargetLost,
    }

    class HomingMissile : ExplosiveProjectile
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float MASS = Config.HOMING_MISSILE_PROJECTILE_MASS;
        private const float GRAVITY_FORCE = Config.HOMING_MISSILE_GRAVITY_FORCE;
        private const float TURN_INCREMENT = Config.HOMING_MISSILE_TURN_INCREMENT;
        private const float MAX_SEEK_SPEED = Config.HOMING_MISSILE_MAX_SEEK_SPEED;
        private const float SEEK_ACCELERATION = Config.HOMING_MISSILE_SEEK_ACCELERATION;
        private const float LERP_AMOUNT = Config.HOMING_MISSILE_LERP_AMOUNT;
        private const float ROTATION_AMOUNT = Config.HOMING_MISSILE_ROTATION_AMOUNT;
        private const float PARTICLE_ROTATION_AMOUNT = Config.HOMING_MISSILE_PARTICLE_ROTATION;
        private const float PARTICLE_INTERVAL = Config.HOMING_MISSILE_SEEK_EFFECT_INTERVAL;
        private const float PARTICLE_SPAWN_FREQUENCY = Config.HOMING_MISSILE_PARTICLE_SPAWN_FREQUENCY;
        private const float PARTICLE_SCALE_START = Config.HOMING_MISSILE_PARTICLE_SCALE_START;
        private const float PARTICLE_SCALE_END = Config.HOMING_MISSILE_PARTICLE_SCALE_END;
        private const float SELF_DESTRUCT_TIMEOUT = Config.HOMING_MISSILE_SELF_DESTRUCT_TIMEOUT;

        private const int TARGET_ARROW_OFFSET = 5;
        private const float TARGET_ARROW_DRAW_DEPTH = Config.PARTICLE_DRAW_DEPTH;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private GameActor target;
        private int targetRadius;
        private HomingMissilePhase homingMissilePhase;
        private ParticleEmitter smokeEmitter;

        private Sprite targetArrow;

        // Properties ------------------------------------------------------------------------------------- Properties

        public HomingMissilePhase HomingMissilePhase
        {
            get { return this.homingMissilePhase; }
            set
            {
                if (value == HomingMissilePhase.Seek)
                {
                    this.physicsMode = PhysicsMode.None;
                    this.sprite = new Sprite(SpriteKey.HomingMissileSeek);
                    this.smokeEmitter.Start();
                }
                else if (value == HomingMissilePhase.TargetLost)
                {
                    this.physicsMode = PhysicsMode.Gravity;
                    this.sprite = new Sprite(SpriteKey.HomingMissile);
                    this.smokeEmitter.Stop();
                }

                this.homingMissilePhase = value;
            }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public HomingMissile(int blastDamage, int blastRadius, int targetRadius, Vector2 position, Vector2 velocity, ProjectileSource projectileSource)
            : base(blastDamage, blastRadius, position, velocity, PhysicsMode.Gravity, MASS, GRAVITY_FORCE ,SpriteKey.HomingMissile, 0.0f, projectileSource)
        {
            this.homingMissilePhase = HomingMissilePhase.Launch;
            this.targetRadius = targetRadius;
            
            this.smokeEmitter = new ParticleEmitter(PARTICLE_SPAWN_FREQUENCY, this.position, SpriteKey.ExplosionSmoke, PARTICLE_ROTATION_AMOUNT, 150, 0, PARTICLE_SCALE_START, PARTICLE_SCALE_END);

            this.targetArrow = new Sprite(SpriteKey.TargetArrow);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods
        
        public bool HasTarget()
        {
            return this.target != null;
        }

        public override void Remove()
        {
            this.smokeEmitter.Stop();
            this.smokeEmitter.Remove();
            SoundManager.StopSoundEffect(SoundKey.HomingMissileSeekMode);
            base.Remove();
        }

        public override void Update(GameTime gameTime)
        {
            float maxIntersectionDepth = -1f;
            if (this.homingMissilePhase == HomingMissilePhase.Launch)
            {
                // Rotate the sprite
                this.rotation += ROTATION_AMOUNT;

                this.target = null;
                foreach (GameActor gameActor in AbstractEnemy.EnemyList)
                {
                    AbstractEnemy enemy = (AbstractEnemy)gameActor;
                    if (!enemy.Dead)
                    {
                        float intersectionDepth = CollisionUtil.CheckIntersectionCollision(new Circle(this.position, this.targetRadius), enemy.Bounds);
                        if (intersectionDepth != 0 && intersectionDepth > maxIntersectionDepth)
                        {
                            // If the level is not in the way of the homing missile's path to the enemy, make the enemy the target
                            Turret turret = enemy as Turret;
                            if (IsActorVisible(enemy) && (turret == null || !turret.Invincible))
                            {
                                maxIntersectionDepth = intersectionDepth;
                                this.target = enemy;
                            }
                        }
                    }
                }
            }
            else if (this.homingMissilePhase == HomingMissilePhase.Seek)
            {
                if (!HasTarget())
                {
                    string message = "Homing missile is in 'Seek' state without a target";
                    LogError(message, typeof(HomingMissile));
                    throw new InvalidStateException(message);
                }

                if (this.target.Dead)
                {
                    this.HomingMissilePhase = HomingMissilePhase.TargetLost;
                }


                // Adjust the HomingMissile's direction towards the enemy
                float currentSpeed = this.velocity.Length();

                Vector2 currentDirection = Vector2.Normalize(this.velocity);
                Vector2 directionToTarget = Vector2.Normalize(this.target.Position - this.position);

                Vector2 newDirection = Vector2.Lerp(currentDirection, directionToTarget, LERP_AMOUNT);
                this.velocity = newDirection * Math.Min(currentSpeed + SEEK_ACCELERATION, MAX_SEEK_SPEED);

                this.rotation = (float)Math.Atan2(directionToTarget.Y, directionToTarget.X) + (float)(Math.PI / 2);
            }
            else
            {
                // Let projectile fall.
            }

            this.smokeEmitter.Position = this.position;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (HasTarget())
            {
                this.targetArrow.Draw(spriteBatch, new Vector2(this.target.Position.X, this.target.Position.Y - this.targetArrow.Height / 2 - this.target.Sprite.Height / 2 - TARGET_ARROW_OFFSET), 0f, TARGET_ARROW_DRAW_DEPTH, SpriteEffects.None);
            }

            base.Draw(spriteBatch);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
        
        private void LogError(string message, Type clazz)
        {
#if WINDOWS
            Logging.LogError(message, clazz, Environment.StackTrace);
#endif
        }

    }
}
