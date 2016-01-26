using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game
{
    class ParticleEmitterFactory
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private float spawnFrequency;
        private Vector2 particleVelocity;
        private float particleMaxVelocityX;
        private float particleMaxVelocityY;
        private Vector2 particleAcceleration;
        private SpriteKey particleSpriteKey;
        private float particleRotationSpeed;
        private int particleFadeStart;
        private int particleFadeEnd;
        private float scaleStart;
        private float scaleEnd;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public ParticleEmitterFactory(float spawnFrequency, SpriteKey spriteKey, float particleScaleStart, float particleScaleEnd)
            : this(spawnFrequency, Vector2.Zero, 0f, 0f, Vector2.Zero, spriteKey, 0f, 255, 0, particleScaleStart, particleScaleEnd)
        { }

        public ParticleEmitterFactory(float spawnFrequency, SpriteKey spriteKey, float particleRotationSpeed,
                               int particleFadeStart, int particleFadeEnd, float particleScaleStart, float particleScaleEnd)
            : this(spawnFrequency, Vector2.Zero, 0f, 0f, Vector2.Zero, spriteKey, particleRotationSpeed, particleFadeStart, particleFadeEnd, particleScaleStart, particleScaleEnd)
        { }


        public ParticleEmitterFactory(float spawnFrequency, SpriteKey spriteKey, float particleRotationSpeed, float particleScaleStart, float particleScaleEnd)
            : this(spawnFrequency, Vector2.Zero, 0f, 0f, Vector2.Zero, spriteKey, particleRotationSpeed, 255, 0, particleScaleStart, particleScaleEnd)
        { }

        public ParticleEmitterFactory(float spawnFrequency, Vector2 particleVelocity, float particleMaxVelocityX, float particleMaxVelocityY, Vector2 particleAcceleration,
                               SpriteKey particleSpriteKey, float particleRotationSpeed, int particleFadeStart, int particleFadeEnd, float particleScaleStart, float particleScaleEnd)
        {
            this.spawnFrequency = spawnFrequency;
            this.particleVelocity = particleVelocity;
            this.particleMaxVelocityX = particleMaxVelocityX;
            this.particleMaxVelocityY = particleMaxVelocityY;
            this.particleAcceleration = particleAcceleration;
            this.particleSpriteKey = particleSpriteKey;
            this.particleRotationSpeed = particleRotationSpeed;
            this.particleFadeStart = particleFadeStart;
            this.particleFadeEnd = particleFadeEnd;
            this.scaleStart = particleScaleStart;
            this.scaleEnd = particleScaleEnd;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public ParticleEmitter CreateParticleEmitter(Vector2 position)
        {
            return new ParticleEmitter(this.spawnFrequency, position, this.particleVelocity, this.particleMaxVelocityX, this.particleMaxVelocityY, this.particleAcceleration,
                                       this.particleSpriteKey, this.particleRotationSpeed, this.particleFadeStart, this.particleFadeEnd, this.scaleStart, this.scaleEnd);

        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
