using System;
using Microsoft.Xna.Framework;
using RobotGame.Exceptions;
using System.Collections.Generic;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    public enum ProjectileSource
    {
        Player,
        Enemy
    }

    public abstract class Projectile : GameActor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float PROJECTILE_DRAW_DEPTH = Config.PROJECTILE_DRAW_DEPTH;
        private const float MAX_HORIZONTAL_VELOCITY = Config.PROJECTILE_MAX_HORIZONTAL_SPEED;
        private const float MAX_VERTICAL_VELOCITY = Config.PROJECTILE_MAX_VERTICAL_SPEED;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static List<GameActor> playerProjectileList = new List<GameActor>();
        private static List<GameActor> enemyProjectileList = new List<GameActor>();

        private int damage;

        protected ProjectileSource projectileSource;

        // Properties ------------------------------------------------------------------------------------- Properties

        public static List<GameActor> PlayerProjectileList
        {
            get { return playerProjectileList; }
        }

        public static List<GameActor> EnemyProjectileList
        {
            get { return enemyProjectileList; }
        }

        public int Damage
        {
            get { return this.damage; }
            set { this.damage = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Projectile(int damage, Vector2 position, Vector2 velocity, PhysicsMode physicsMode, ProjectileSource projectileSource)
            : base(position, velocity, physicsMode)
        {
            this.drawDepth = PROJECTILE_DRAW_DEPTH;
            this.damage = damage;
            
            // Add the projectile to an actor list so it starts getting updated
            if (projectileSource == ProjectileSource.Player)
            {
                playerProjectileList.Add(this);
            }
            else if (projectileSource == ProjectileSource.Enemy)
            {
                enemyProjectileList.Add(this);
            }
            this.projectileSource = projectileSource;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods
        
        public override void Collide(GameActor actor)
        {
            this.Remove();
        }

        public override void Update(GameTime gameTime)
        {
            if (Level.GetInstance().IsBoundsOnMap(this.Bounds))
            {
                bool collidedWithMap = HandleMapCollisions();
                bool collidedWithMover = HandleMoverCollisions();

                if (collidedWithMap || collidedWithMover)
                {
                    this.Collide(null);
                }
            }
            else
            {
                this.Remove();
            }

            this.ApplyPhysics();

            this.velocity.X = MathHelper.Clamp(this.velocity.X, -MAX_HORIZONTAL_VELOCITY, MAX_HORIZONTAL_VELOCITY);
            this.velocity.Y = MathHelper.Clamp(this.velocity.Y, -MAX_VERTICAL_VELOCITY, MAX_VERTICAL_VELOCITY);

            this.Move(this.velocity);

            base.Update(gameTime);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
