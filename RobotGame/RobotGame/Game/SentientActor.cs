using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game;
using RobotGame.Game.Mover;
using System.Collections.Generic;

namespace RobotGame.Game
{
    abstract class SentientActor : GameActor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants
        
        private const int EXPLOSION_PARTICLE_MASS = Config.ENEMY_EXPLOSION_PARTICLE_MASS;
        private const int EXPLOSION_PARTICLE_MASS_RANDOM_RANGE = Config.ENEMY_EXPLOSION_PARTICLE_MASS_RANDOM_RANGE;
        private const float EXPLOSION_PARTICLE_SPEED = Config.ENEMY_EXPLOSION_PARTICLE_SPEED;
        private const float EXPLOSION_PARTICLE_ROTATION = Config.ENEMY_EXPLOSION_PARTICLE_ROTATION;

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected EnvironmentState environmentState;

        protected int health;

        // Properties ------------------------------------------------------------------------------------- Properties

        public int Health
        {
            get { return this.health; }
            set { this.health = value; }
        }

        public EnvironmentState EnvironmentState
        {
            get { return this.environmentState; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public SentientActor(Vector2 position, Vector2 velocity, PhysicsMode physicsMode)
            : base(position, velocity, physicsMode)
        { }

        public SentientActor(Vector2 position)
            : this(position, Vector2.Zero, PhysicsMode.None)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public abstract void TakeDamage(int damage);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        //protected Vector2 Jump(float amount)
        //{
        //    return new Vector2(0f, PhysicsController.ConvertVelocityToForce(amount));
        //}

        protected void Explode(SpriteKey spriteKey, int tilesX, int tilesY)
        {
            Explosions.CreateExplosion(this.position, spriteKey, tilesX, tilesY, SpriteKey.ExplosionSmoke,
                                       EXPLOSION_PARTICLE_MASS, EXPLOSION_PARTICLE_MASS_RANDOM_RANGE, EXPLOSION_PARTICLE_SPEED);
        }

        protected void CheckMapBoundsAndCorrect()
        {
            // Make sure the actor doesn't go off the edge of the map
            if ((this.position.X - this.sprite.Origin.X) < Level.GetInstance().TileWidth)
            {
                this.position = new Vector2(Level.GetInstance().TileWidth + this.sprite.Origin.X, this.position.Y);
            }
            if ((this.position.X + this.sprite.Origin.X) > (Level.GetInstance().Width - Level.GetInstance().TileWidth))
            {
                this.position = new Vector2((Level.GetInstance().Width - Level.GetInstance().TileWidth) - this.sprite.Origin.X, this.position.Y);
            }

            if ((this.position.Y - this.sprite.Origin.Y) < Level.GetInstance().TileHeight)
            {
                this.position = new Vector2(this.position.X, Level.GetInstance().TileHeight + this.sprite.Origin.Y);
            }
            if ((this.position.Y + this.sprite.Origin.Y) > (Level.GetInstance().Height - Level.GetInstance().TileHeight))
            {
                this.position = new Vector2(this.position.X, (Level.GetInstance().Height - Level.GetInstance().TileHeight) - this.sprite.Origin.Y);
            }
        }

        protected bool CheckMapBounds(GameActor actor)
        {
            if ((actor.Position.X - actor.Sprite.Origin.X) < Level.GetInstance().TileWidth ||
                (actor.Position.X + actor.Sprite.Origin.X) > (Level.GetInstance().Width - Level.GetInstance().TileWidth) ||
                (actor.Position.Y - actor.Sprite.Origin.Y) < Level.GetInstance().TileHeight ||
                (actor.Position.Y + actor.Sprite.Origin.Y) > (Level.GetInstance().Height - Level.GetInstance().TileHeight))
            {
                return true;
            }
            return false;
        }

        protected bool CheckMapBounds(Vector2 position)
        {
            if (position.X < Level.GetInstance().TileWidth ||
                position.X > (Level.GetInstance().Width - Level.GetInstance().TileWidth) ||
                position.Y < Level.GetInstance().TileHeight ||
                position.Y > (Level.GetInstance().Height - Level.GetInstance().TileHeight))
            {
                return true;
            }
            return false;
        }

        protected override bool HandleMapCollisions()
        {
            bool mapCollisionOccurred = false;

            // Reset the OnGound state
            environmentState = EnvironmentState.InAir;

            // Get the range of tiles that we're intersecting with
            int leftTile = (int)Math.Floor((float)this.Bounds.Left / Level.GetInstance().TileWidth);
            int rightTile = (int)Math.Ceiling((float)this.Bounds.Right / Level.GetInstance().TileWidth) - 1;
            int topTile = (int)Math.Floor((float)this.Bounds.Top / Level.GetInstance().TileHeight);
            int bottomTile = (int)Math.Ceiling((float)this.Bounds.Bottom / Level.GetInstance().TileHeight) - 1;

            Vector2 longestCollisionDepth = Vector2.Zero;
            bool colliding = true;
            int count = 0;

            while (colliding)
            {
                if (count > 60)
                {
                    this.TakeDamage(this.Health);
                    return true;
                }
                
                colliding = false;
                longestCollisionDepth = Vector2.Zero;

                // Check if any of the tiles we're intersecting with are collision tiles
                for (int y = topTile; y <= bottomTile; y++)
                {
                    for (int x = leftTile; x <= rightTile; x++)
                    {
                        if (Level.GetInstance().IsCollisionTile(x, y) || Level.GetInstance().IsDestructibleTile(x, y))
                        {
                            Rectangle tileBounds = Level.GetInstance().GetTileBounds(x, y);
                            Vector2 intersectionDepth = CollisionUtil.GetIntersectionDepth(this.Bounds, tileBounds);

                            if (intersectionDepth != Vector2.Zero)
                            {
                                mapCollisionOccurred = true;
                                colliding = true;

                                if (intersectionDepth.Length() > longestCollisionDepth.Length())
                                {
                                    longestCollisionDepth = intersectionDepth;
                                }
                            }
                        }
                    }
                }

                float absDepthX = Math.Abs(longestCollisionDepth.X);
                float absDepthY = Math.Abs(longestCollisionDepth.Y);

                if (mapCollisionOccurred)
                {
                    if (absDepthY < absDepthX)
                    {
                        // Resolve the collision on the Y axis
                        this.position = new Vector2(this.position.X, this.position.Y + longestCollisionDepth.Y);

                        // If the actor crossed the tile boudary from the top, set the actor's state to "OnGround"
                        if (longestCollisionDepth.Y < 0f)
                        {
                            environmentState = EnvironmentState.OnGround;

                            if (this.velocity.Y > 0f)
                            {
                                this.velocity.Y = 0f;
                            }
                        }
                        else // The actor crossed the tile boundary from the bottom.
                        {
                            if (this.velocity.Y < 0f)
                            {
                                this.velocity.Y = 0f;
                            }
                        }
                    }
                    else
                    {
                        // Resolve the collision on the X axis
                        this.position = new Vector2(this.position.X + longestCollisionDepth.X, this.position.Y);
                    }
                }

                count++;
            }

            return mapCollisionOccurred;
        }

        protected override bool HandleMoverCollisions()
        {
            bool mapCollisionOccurred = false;

            // Handle collisions with movers
            List<GameActor> moverList = AbstractMover.MoverList;
            foreach (AbstractMover mover in moverList)
            {
                Vector2 intersectionDepth = CollisionUtil.GetIntersectionDepth(this.Bounds, mover.Bounds);

                if (intersectionDepth != Vector2.Zero)
                {
                    mapCollisionOccurred = true;

                    mover.Collide(this);

                    float absDepthX = Math.Abs(intersectionDepth.X);
                    float absDepthY = Math.Abs(intersectionDepth.Y);

                    if (absDepthY < absDepthX)
                    {
                        // Resolve the collision on the Y axis
                        this.position = new Vector2(this.position.X, this.position.Y + intersectionDepth.Y);

                        if (intersectionDepth.Y < 0f)
                        {
                            environmentState = EnvironmentState.OnGround;

                            if (this.velocity.Y > 0f)
                            {
                                this.velocity.Y = 0f;
                                Move(mover.Velocity);
                            }
                        }
                        else
                        {
                            if (this.velocity.Y < 0f)
                            {
                                this.velocity.Y = 0f;
                            }
                        }
                    }
                    else
                    {
                        // Resolve the collision on the X axis
                        this.position = new Vector2(this.position.X + intersectionDepth.X, this.position.Y);
                    }
                }
            }

            return mapCollisionOccurred;
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
