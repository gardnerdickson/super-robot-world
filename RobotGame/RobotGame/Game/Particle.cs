using System;
using RobotGame.Engine;
using Microsoft.Xna.Framework;

namespace RobotGame.Game
{
    class Particle : GameActor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float PARTICLE_DRAW_DEPTH = Config.PARTICLE_DRAW_DEPTH;
        
        // Data Members --------------------------------------------------------------------------------- Data Members

        private int fadeStart;
        private int fadeEnd;
        private float rotationSpeed;
        private float scaleStart;
        private float scaleEnd;

        private float maxVelocityX;
        private float maxVelocityY;
        private Vector2 acceleration;

        private float particleLifetime;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Particle(Vector2 position, SpriteKey spriteKey, float rotationSpeed)
            : this(position, Vector2.Zero, 0.0f, 0.0f, Vector2.Zero, spriteKey, rotationSpeed)
        { }

        public Particle(Vector2 position, SpriteKey spriteKey, float rotationSpeed, int fadeStart, int fadeEnd, float scaleStart, float scaleEnd)
            : this(position, Vector2.Zero, 0f, 0f, Vector2.Zero, spriteKey, rotationSpeed, fadeStart, fadeEnd, scaleStart, scaleEnd)
        { }

        public Particle(Vector2 position, Vector2 velocity, float maxVelocityX, float maxVelocityY, Vector2 acceleration, SpriteKey spriteKey, float rotationSpeed)
            : this(position, velocity, maxVelocityX, maxVelocityY, acceleration, spriteKey, rotationSpeed, 255, 0, 1.0f, 1.0f)
        { }

        public Particle(Vector2 position, Vector2 velocity, float maxVelocityX, float maxVelocityY, Vector2 acceleration,
                        SpriteKey spriteKey, float rotationSpeed, float scaleStart, float scaleEnd)
            : this(position, velocity, maxVelocityX, maxVelocityY, acceleration, spriteKey, rotationSpeed, 255, 0, scaleStart, scaleEnd)
        { }                        

        public Particle(Vector2 position, Vector2 velocity, float maxVelocityX, float maxVelocityY, Vector2 acceleration,
                        SpriteKey spriteKey, float rotationSpeed, int fadeStart, int fadeEnd, float scaleStart, float scaleEnd)
            : base(position, velocity, PhysicsMode.None)
        {
            if (fadeStart < 0 || fadeStart > 255 || fadeEnd < 0 || fadeEnd > 255)
            {
                throw new InvalidOperationException("fadeStart and fadeEnd values must be between 0 and 255");
            }

            this.maxVelocityX = maxVelocityX;
            this.maxVelocityY = maxVelocityY;
            this.acceleration = acceleration;
            this.sprite = new Sprite(spriteKey);
            this.rotationSpeed = rotationSpeed;
            this.fadeStart = fadeStart;
            this.fadeEnd = fadeEnd;
            this.scaleStart = scaleStart;
            this.scaleEnd = scaleEnd;
            this.sprite.Scale = this.scaleStart;

            this.drawDepth = PARTICLE_DRAW_DEPTH;

            this.particleLifetime = (float)(this.sprite.SpriteSheet.FrameInterval * (this.sprite.FrameCount - 1));
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update(GameTime gameTime)
        {
            if (this.sprite.CurrentFrame == this.sprite.FrameCount - 1)
            {
                this.Remove();
            }
            else
            {
                this.velocity += this.acceleration;
                this.velocity = new Vector2(MathHelper.Clamp(this.velocity.X, -maxVelocityX, maxVelocityX),
                                            MathHelper.Clamp(this.velocity.Y, -maxVelocityY, maxVelocityY));

                this.Move(this.velocity);

                this.rotation += this.rotationSpeed;
            }

            if (this.particleLifetime != 0)
            {
                float particleAge = (float)(this.sprite.CurrentFrame * this.sprite.SpriteSheet.FrameInterval) +
                                        (float)(this.sprite.SpriteSheet.FrameInterval - this.sprite.FrameTimeRemaining);
                float agePercentage = (particleAge / particleLifetime);

                this.sprite.Alpha = (int)MathHelper.Lerp(this.fadeStart, this.fadeEnd, agePercentage);
                this.sprite.Scale = MathHelper.Lerp(this.scaleStart, this.scaleEnd, agePercentage);
            }

            base.Update(gameTime);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
