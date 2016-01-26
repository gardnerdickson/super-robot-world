using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using RobotGame.Engine;
using RobotGame.Exceptions;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Enemy
{
    public enum EnemyOrientation
    {
        Up,
        Down,
        Left,
        Right,
    }

    abstract class AbstractEnemy : SentientActor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float GRAVITY_FORCE = Config.ENEMY_GRAVITY_FORCE;
        private const float ENEMY_DRAW_DEPTH = Config.ENEMY_DRAW_DEPTH;
        private const float POWERUP_SPAWN_X_VELOCITY = Config.POWERUP_SPAWN_X_VELOCITY;
        private const float POWERUP_SPAWN_Y_VELOCITY = Config.POWERUP_SPAWN_Y_VELOCITY;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static List<GameActor> enemyList = new List<GameActor>();
        
        private string name;

        private string onDeathPowerupSpawnType;
        private PhysicsMode onDeathPowerupPhysicsMode;

        protected bool mapBoundsCorrectionEnabled = false;
        protected bool collideWithEnvironment = true;
        protected int pointValue;

        // Properties ------------------------------------------------------------------------------------- Properties

        public static List<GameActor> EnemyList
        {
            get { return enemyList; }
        }

        public string Name
        {
            get { return this.name; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public AbstractEnemy(Vector2 position, PhysicsMode physicsMode, String name, string onDeathPowerupSpawnType, PhysicsMode onDeathPowerupPhysicsMode)
            : base(position, Vector2.Zero, physicsMode)
        {
            this.name = name;

            this.drawDepth = ENEMY_DRAW_DEPTH;

            this.onDeathPowerupSpawnType = onDeathPowerupSpawnType;
            this.onDeathPowerupPhysicsMode = onDeathPowerupPhysicsMode;

            this.physicsController.GravityForce = GRAVITY_FORCE;

            EnemyList.Add(this);
        }

        public AbstractEnemy(Vector2 position, PhysicsMode physicsMode, String name)
            : this(position, physicsMode, name, null, PhysicsMode.None)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public abstract void PerformAI(Player player);

        public override void TakeDamage(int damage)
        {
            this.health -= damage;
            if (this.health <= 0)
            {
                OnDeath();
                this.Remove();
                SoundManager.PlaySoundEffect(SoundKey.EnemyDeath);
            }
            else
            {
                this.sprite.ShowDamage();
                SoundManager.PlayRandomSoundEffect(SoundKey.EnemyTakeDamage1, SoundKey.EnemyTakeDamage2, SoundKey.EnemyTakeDamage3);
            }
        }

        public override void Collide(GameActor actor)
        {
            base.Collide(actor);
        }

        public override void Update(GameTime gameTime)
        {
            this.PerformAI((Player)Player.PlayerList[0]);

            this.ApplyPhysics();
            this.velocity.Y = MathHelper.Clamp(this.velocity.Y, -MAX_VERTICAL_SPEED, MAX_VERTICAL_SPEED);

            this.Move(this.velocity);
            if (mapBoundsCorrectionEnabled)
            {
                CheckMapBoundsAndCorrect();
            }

            if (Level.GetInstance().IsBoundsOnMap(this.Bounds))
            {
                if (collideWithEnvironment)
                {
                    this.HandleMapCollisions();
                    this.HandleMoverCollisions();
                }
            }
            else
            {
                this.Remove();
            }

            this.CheckActorCollisions();

            base.Update(gameTime);
        }

        public static bool IsVectorInRange(Vector2 direction, float range, EnemyOrientation orientation)
        {
            float directionDegrees = MathHelper.ToDegrees((float)Math.Atan2(direction.X, -direction.Y));

            if (orientation == EnemyOrientation.Up)
            {
                return directionDegrees > -(range / 2) && directionDegrees < (range / 2);
            }
            else if (orientation == EnemyOrientation.Down)
            {
                return (directionDegrees < -180 + (range / 2)) || (directionDegrees > 180 - (range / 2));
            }
            else if (orientation == EnemyOrientation.Left)
            {
                return directionDegrees > -90 - (range / 2) && directionDegrees < -90 + (range / 2);
            }
            else
            {
                return directionDegrees > 90 - (range / 2) && directionDegrees < 90 + (range / 2);
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected virtual void OnDeath()
        {
            SpawnPowerup();
            GivePointsToPlayer();
        }

        protected virtual bool IsFacingActorHorizontally(GameActor actor)
        {
            if (this.position.X - actor.Position.X > 0 && this.velocity.X < 0 ||
                this.position.X - actor.Position.X < 0 && this.velocity.X > 0)
            {
                return true;
            }
            return false;
        }

        protected virtual bool IsFacingActorVertically(GameActor actor)
        {
            if (this.position.Y - actor.Position.Y > 0 && this.velocity.Y < 0 ||
                this.position.Y - actor.Position.Y < 0 && this.velocity.Y > 0)
            {
                return true;
            }
            return false;
        }
        
        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void CheckActorCollisions()
        {
            for (int i = 0; i < Projectile.PlayerProjectileList.Count; i++)
            {
                Projectile projectile = (Projectile)Projectile.PlayerProjectileList[i];
                if (!projectile.Dead)
                {
                    if (CollisionUtil.CheckPerPixelCollision(this, projectile))
                    {
                        this.TakeDamage(projectile.Damage);
                        projectile.Collide(this);
                    }
                }
            }
        }

        private void SpawnPowerup()
        {
            if (this.onDeathPowerupSpawnType != null)
            {
                ActorDirector.GetInstance().SpawnPowerup(this.onDeathPowerupSpawnType, this.onDeathPowerupPhysicsMode, this.position,
                                          (this.onDeathPowerupPhysicsMode == PhysicsMode.Gravity) ? new Vector2(POWERUP_SPAWN_X_VELOCITY, POWERUP_SPAWN_Y_VELOCITY) : Vector2.Zero);
            }
        }

        private void GivePointsToPlayer()
        {
            Player player = (Player) Player.PlayerList[0];
            player.Points += this.pointValue;
        }
    }
}
