using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace RobotGame.Game.Enemy
{
    class CoreShield : AbstractEnemy
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int START_HEALTH = Config.CORE_SHIELD_HEALTH;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private float rotationArmLength;
        private Vector2 rotationArmPosition;
        private float rotationIncrement;
        private Vector2 direction;

        private Color tint;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public CoreShield(string name, Vector2 rotationArmPosition, float rotationArmLength, float startRotation, float rotationIncrement)
            : base(Vector2.Zero, PhysicsMode.None, name)
        {
            this.health = START_HEALTH;
            this.sprite = new Sprite(SpriteKey.CoreShield);
            this.rotationArmLength = rotationArmLength;
            this.rotationIncrement = rotationIncrement;
            this.rotationArmPosition = rotationArmPosition;

            this.pointValue = START_HEALTH;

            this.direction = new Vector2((float)Math.Cos(startRotation), (float)Math.Sin(startRotation));
            this.direction.Normalize();

            this.collideWithEnvironment = false;

            this.tint = Color.White;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void PerformAI(Player player)
        {
            // Increment rotation
            this.direction = Vector2.Transform(this.direction, Matrix.CreateRotationZ(this.rotationIncrement));
            this.direction.Normalize();
            this.position = rotationArmPosition + direction * rotationArmLength;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            
            float tintValue = ((float)this.health / (float)START_HEALTH) * 255;
            this.tint.G = (byte)tintValue;
            this.tint.B = (byte)tintValue;
        }

        protected override void OnDeath()
        {
            Explode(SpriteKey.CoreShieldExplosion, 2, 2);
            base.OnDeath();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.sprite.Draw(spriteBatch, this.position, this.rotation, this.drawDepth, SpriteEffects.None, this.tint);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
