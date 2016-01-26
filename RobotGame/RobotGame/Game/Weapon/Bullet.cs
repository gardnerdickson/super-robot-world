using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Exceptions;
using RobotGame.Game.Audio;
using RobotGame.Game.Enemy;

namespace RobotGame.Game.Weapon
{
    class Bullet : Projectile
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private SpriteKey onCollisionParticleSpriteKey;
        private ParticleEmitter particleEmitter;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Bullet(int damage, Vector2 position, Vector2 velocity, SpriteKey spriteKey, SpriteKey onCollisionParticleSpriteKey, ParticleEmitter particleEmitter, ProjectileSource projectileSource)
            : base(damage, position, velocity, PhysicsMode.None, projectileSource)
        {
            this.sprite = new Sprite(spriteKey);
            this.particleEmitter = particleEmitter;
            this.onCollisionParticleSpriteKey = onCollisionParticleSpriteKey;

            if (this.particleEmitter != null)
            {
                this.particleEmitter.Start();
            }
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update(GameTime gameTime)
        {
            if (this.particleEmitter != null)
            {
                this.particleEmitter.Position = this.position;
            }
            CheckExplosiveProjectileCollisions(PlayerProjectileList);
            CheckExplosiveProjectileCollisions(EnemyProjectileList);
            base.Update(gameTime);
        }

        public override void Collide(GameActor actor)
        {
            new Particle(this.position, this.onCollisionParticleSpriteKey, 0.0f, 255, 255, 1f, 1f);
            base.Collide(actor);
        }

        public override void Remove()
        {
            if (this.particleEmitter != null)
            {
                this.particleEmitter.Stop();
                this.particleEmitter.Remove();
            }
            base.Remove();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void CheckExplosiveProjectileCollisions(List<GameActor> projectileList)
        {
            foreach (Projectile projectile in projectileList)
            {
                Vector2 pointA = this.position;
                Vector2 pointB = new Vector2(this.position.X - this.velocity.X, this.position.Y - this.velocity.Y);
                if (projectile is ExplosiveProjectile && (CollisionUtil.CheckPerPixelCollision(this, projectile) || CollisionUtil.CheckIntersectionCollision(new Line(pointA, pointB), projectile.Bounds)))
                {
                    projectile.Collide(this);
                    this.Collide(projectile);
                }
            }
        }
    }
}
