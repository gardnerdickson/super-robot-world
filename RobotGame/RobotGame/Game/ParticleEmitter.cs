using System;
using RobotGame.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RobotGame.Game
{
    class ParticleEmitter : GameActor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members
        
        private TimerCallback SpawnParticleCallback;

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

        private bool enabled;

        // Properties ------------------------------------------------------------------------------------- Properties

        public bool IsEnabled
        {
            get { return this.enabled; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public ParticleEmitter(float spawnFrequency, Vector2 position, SpriteKey spriteKey, float particleScaleStart, float particleScaleEnd)
            : this(spawnFrequency, position, Vector2.Zero, 0f, 0f, Vector2.Zero, spriteKey, 0f, 255, 0, particleScaleStart, particleScaleEnd)
        { }

        public ParticleEmitter(float spawnFrequency, Vector2 position, SpriteKey spriteKey, float particleRotationSpeed,
                               int particleFadeStart, int particleFadeEnd, float particleScaleStart, float particleScaleEnd)
            : this(spawnFrequency, position, Vector2.Zero, 0f, 0f, Vector2.Zero, spriteKey, particleRotationSpeed, particleFadeStart, particleFadeEnd, particleScaleStart, particleScaleEnd)
        { }


        public ParticleEmitter(float spawnFrequency, Vector2 position, SpriteKey spriteKey, float particleRotationSpeed, float particleScaleStart, float particleScaleEnd)
            : this(spawnFrequency, position, Vector2.Zero, 0f, 0f, Vector2.Zero, spriteKey, particleRotationSpeed, 255, 0, particleScaleStart, particleScaleEnd)
        { }

        public ParticleEmitter(float spawnFrequency, Vector2 position, Vector2 particleVelocity, float particleMaxVelocityX, float particleMaxVelocityY, Vector2 particleAcceleration,
                               SpriteKey particleSpriteKey, float particleRotationSpeed, int particleFadeStart, int particleFadeEnd, float particleScaleStart, float particleScaleEnd)
            : base(position, Vector2.Zero, PhysicsMode.None)
        {
            this.SpawnParticleCallback += new TimerCallback(spawn_particle);
            
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

        public void Start()
        {
            this.enabled = true;
            TimerManager.GetInstance().RegisterTimer(this.spawnFrequency, this.SpawnParticleCallback, null);
        }

        public void Stop()
        {
            this.enabled = false;
        }

        public ParticleEmitter Clone()
        {
            return new ParticleEmitter(this.spawnFrequency, this.position, this.particleVelocity, this.particleMaxVelocityX, this.particleMaxVelocityY, this.particleAcceleration,
                                       this.particleSpriteKey, this.particleRotationSpeed, this.particleFadeStart, this.particleFadeEnd, this.scaleStart, this.scaleEnd);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void spawn_particle(Object param)
        {
            if (this.enabled)
            {
                new Particle(this.position, this.particleVelocity, this.particleMaxVelocityX, this.particleMaxVelocityY, this.particleAcceleration,
                             this.particleSpriteKey, this.particleRotationSpeed, this.particleFadeStart, this.particleFadeEnd, this.scaleStart, this.scaleEnd);
                TimerManager.GetInstance().RegisterTimer(this.spawnFrequency, this.SpawnParticleCallback, null);
            }
        }
    }
}
