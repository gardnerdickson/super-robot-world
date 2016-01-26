using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using System.Collections.Generic;
using RobotGame.Exceptions;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Weapon
{
    public class ExplosiveProjectile : Projectile
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members
        
        protected int blastRadius;
        protected int blastDamage;

        private float rotationIncrement;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public ExplosiveProjectile(int blastDamage, int blastRadius, Vector2 position, Vector2 velocity, PhysicsMode physicsMode, float mass, float gravityForce, SpriteKey spriteKey, float rotationIncrement, ProjectileSource projectileSource)
            : base(0, position, velocity, physicsMode, projectileSource)
        {
            this.blastDamage = blastDamage;
            this.blastRadius = blastRadius;

            this.mass = mass;
            this.sprite = new Sprite(spriteKey);
            this.rotationIncrement = rotationIncrement;

            this.physicsController.GravityForce = gravityForce;
        }

        public ExplosiveProjectile(int blastDamage, int blastRadius, Vector2 position, Vector2 velocity, SpriteKey spriteKey, ProjectileSource projectileSource)
            : this(blastDamage, blastRadius, position, velocity, PhysicsMode.None, 0.0f, 0.0f, spriteKey, 0.0f, projectileSource)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Collide(GameActor actor)
        {
            new Blast(this.position, this.Bounds, SpriteKey.Explosion, SpriteKey.ExplosionSmoke, this.blastDamage, new Circle(this.position, this.blastRadius));
            base.Collide(actor);
        }

        public override void Update(GameTime gameTime)
        {
            this.rotation += this.rotationIncrement;
            base.Update(gameTime);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
