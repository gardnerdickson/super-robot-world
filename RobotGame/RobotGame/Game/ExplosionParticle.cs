using System;
using RobotGame.Engine;
using Microsoft.Xna.Framework;

namespace RobotGame.Game
{
    class ExplosionParticle : GameActor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float LIFETIME = Config.EXPLOSION_PARTICLE_LIFETIME;
        private const float FADE_AGE = Config.EXPLOSION_PARTICLE_FADE_AGE;
        private const float PARTICLE_EMITTER_STOP_AGE = Config.EXPLOSION_PARTICLE_EMITTER_STOP_AGE;

        private const float DRAW_DEPTH = Config.ENEMY_EXPLOSION_DRAW_DEPTH;

        private const float GRAVITY_FORCE = 16f;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private ParticleEmitter particleEmitter;

        private double age = 0f;
        private float rotationIncrement;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public ExplosionParticle(Vector2 position, Vector2 velocity, float mass, Sprite sprite, int spriteFrame, float rotationIncrement, ParticleEmitter particleEmitter)
            : base(position, velocity, PhysicsMode.Gravity)
        {
            this.sprite = sprite;
            this.particleEmitter = particleEmitter;
            this.mass = mass;
            this.rotationIncrement = rotationIncrement;
            this.sprite.CurrentFrame = spriteFrame;
            this.drawDepth = DRAW_DEPTH;

            this.physicsController.GravityForce = GRAVITY_FORCE;

            if (this.particleEmitter != null)
            {
                this.particleEmitter.Start();
            }
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update(GameTime gameTime)
        {
            this.rotation += this.rotationIncrement;
            if (this.particleEmitter != null)
            {
                this.particleEmitter.Position = this.position;
            }
            this.age += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.age > FADE_AGE)
            {
                float fadePercentage = (((float)this.age - FADE_AGE) / (LIFETIME - FADE_AGE));
                float alpha = MathHelper.Lerp(255, 0, fadePercentage);
                this.sprite.Alpha = (int)alpha;
            }

            if (this.age > PARTICLE_EMITTER_STOP_AGE && this.particleEmitter != null)
            {
                this.particleEmitter.Stop();
            }
            ApplyPhysics();
            Move(this.velocity);

            if (this.age > LIFETIME)
            {
                Remove();
            }

            base.Update(gameTime);
        }

        public override void Remove()
        {
            if (this.particleEmitter != null)
            {
                this.particleEmitter.Remove();
            }
            base.Remove();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
