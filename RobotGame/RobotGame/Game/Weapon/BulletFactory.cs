using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    class BulletFactory : ProjectileFactory
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private int damage;
        private SpriteKey spriteKey;
        private SpriteKey onCollisionParticleSpriteKey;
        private ParticleEmitterFactory particleEmitterFactory;
        private ProjectileSource projectileSource;

        // Properties ------------------------------------------------------------------------------------- Properties

        public int Damage
        {
            get { return this.damage; }
            set { this.damage = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public BulletFactory(int damage, SpriteKey spriteKey, SpriteKey onCollisionParticleSpriteKey, ParticleEmitterFactory particleEmitterFactory, ProjectileSource projectileSource)
        {
            this.damage = damage;
            this.spriteKey = spriteKey;
            this.onCollisionParticleSpriteKey = onCollisionParticleSpriteKey;
            this.particleEmitterFactory = particleEmitterFactory;
            this.projectileSource = projectileSource;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods
        
        public GameActor CreateProjectile(Vector2 position, Vector2 velocity)
        {
            ParticleEmitter particleEmitter = this.particleEmitterFactory != null ? this.particleEmitterFactory.CreateParticleEmitter(position) : null;
            return new Bullet(this.damage, position, velocity, this.spriteKey, this.onCollisionParticleSpriteKey, particleEmitter, this.projectileSource);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
