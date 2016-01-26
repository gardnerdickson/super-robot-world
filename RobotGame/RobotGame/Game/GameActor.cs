using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Engine;
using RobotGame.Game.Mover;
using RobotGame.Game.Volume;
using RobotGame.Game.Powerup;
using RobotGame.Game.Weapon;
using RobotGame.Game.Enemy;

namespace RobotGame.Game
{
    public enum PhysicsMode
    {
        None,
        Gravity,
        GravityAndFriction
    }

    public enum EnvironmentState
    {
        OnGround,
        InAir
    }

    public abstract class GameActor : Actor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        protected const float MAX_HORIZONTAL_SPEED = Config.MAX_HORIZONTAL_SPEED;
        protected const float MAX_VERTICAL_SPEED = Config.MAX_VERTICAL_SPEED;

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected Vector2 velocity;
        protected float mass;
        protected Vector2 force;
        protected PhysicsMode physicsMode;
        
        protected PhysicsController physicsController;

        // Properties ------------------------------------------------------------------------------------- Properties

        public Vector2 Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
        }

        public float Mass
        {
            get { return this.mass; }
        }

        public Vector2 Force
        {
            get { return this.force; }
            set { this.force = value; }
        }
        
        // Contructors ----------------------------------------------------------------------------------- Contructors

        public GameActor(Vector2 position, Vector2 velocity, PhysicsMode physicsMode)
            : base(position)
        {
            this.velocity = velocity;
            this.physicsMode = physicsMode;

            this.physicsController = new PhysicsController(MAX_HORIZONTAL_SPEED, MAX_VERTICAL_SPEED);
        }

        public GameActor(Vector2 position)
            : this(position, Vector2.Zero, PhysicsMode.None)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public virtual void Collide(GameActor actor) { }
        
        public override void Update(GameTime gameTime)
        {
            // Reset all of the forces from this game tick
            this.force = Vector2.Zero;

            base.Update(gameTime);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected virtual void DirectionOrientateHorizontal()
        {
            // Intentionally do nothing if the velocity in X is 0.
            if (this.velocity.X < 0f)
            {
                this.flip = SpriteEffects.FlipHorizontally;
            }
            else if (this.velocity.X > 0f)
            {
                this.flip = SpriteEffects.None;
            }
        }

        protected void DirectionOrientateVertical()
        {
            // Intentionally do nothing if the velocity in Y is 0.
            if (this.velocity.Y < 0f)
            {
                this.flip = SpriteEffects.FlipVertically;
            }
            else if (this.velocity.Y > 0f)
            {
                this.flip = SpriteEffects.None;
            }
        }

        protected void Move(Vector2 amount)
        {
            this.position += amount;
            this.position = new Vector2((float)Math.Round(this.position.X), (float)Math.Round(this.position.Y));
        }

        protected void ApplyPhysics()
        {
            Vector2 physicsVector = Vector2.Zero;

            if (this.physicsMode == PhysicsMode.Gravity)
            {
                physicsVector = this.physicsController.ApplyGravity(this.mass, this.force);
            }

            this.velocity += physicsVector;
        }

        protected bool IsActorVisible(GameActor actor)
        {
            Vector2 lineToActor = actor.Position - this.Position;

            Rectangle tileArea = new Rectangle();

            tileArea.X = (int)(lineToActor.X <= 0 ? actor.Position.X : this.Position.X);
            tileArea.Y = (int)(lineToActor.Y <= 0 ? actor.Position.Y : this.Position.Y);
            tileArea.Width = Math.Abs((int)lineToActor.X);
            tileArea.Height = Math.Abs((int)lineToActor.Y);

            int leftTile = (int)Math.Floor((float)tileArea.Left / Level.GetInstance().TileWidth);
            int rightTile = (int)Math.Ceiling((float)tileArea.Right / Level.GetInstance().TileWidth) - 1;
            int topTile = (int)Math.Floor((float)tileArea.Top / Level.GetInstance().TileHeight);
            int bottomTile = (int)Math.Ceiling((float)tileArea.Bottom / Level.GetInstance().TileHeight) - 1;

            int levelWidth = Level.GetInstance().TilesX;
            int levelHeight = Level.GetInstance().TilesY;
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    if (y > 0 && y < levelHeight && x > 0 && x < levelWidth)
                    {
                        if (Level.GetInstance().IsCollisionTile(x, y) || Level.GetInstance().IsDestructibleTile(x, y))
                        {
                            bool collisionOccurred = CollisionUtil.CheckIntersectionCollision(new Line(this.position, actor.Position), Level.GetInstance().GetTileBounds(x, y));
                            if (collisionOccurred)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        protected virtual bool HandleMapCollisions()
        {
            bool mapCollisionOccurred = false;

            // Get the range of tiles that we're intersecting with
            int leftTile = (int)Math.Floor((float)this.Bounds.Left / Level.GetInstance().TileWidth);
            int rightTile = (int)Math.Ceiling((float)this.Bounds.Right / Level.GetInstance().TileWidth) - 1;
            int topTile = (int)Math.Floor((float)this.Bounds.Top / Level.GetInstance().TileHeight);
            int bottomTile = (int)Math.Ceiling((float)this.Bounds.Bottom / Level.GetInstance().TileHeight) - 1;

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

                            //float absDepthX = Math.Abs(intersectionDepth.X);
                            //float absDepthY = Math.Abs(intersectionDepth.Y);

                            //if (absDepthY < absDepthX)
                            //{
                            //    // Resolve the collision on the Y axis
                            //    this.position = new Vector2(this.position.X, this.position.Y + intersectionDepth.Y);

                            //    this.velocity.Y = 0.0f;
                            //}
                            //else
                            //{
                            //    // Resolve the collision on the X axis
                            //    this.position = new Vector2(this.position.X + intersectionDepth.X, this.position.Y);
                            //}

                            
                            // FIX: for explosive projectiles spawning explosions inside the map
                            float absVelocityX = Math.Abs(this.velocity.X);
                            float absVelocityY = Math.Abs(this.velocity.Y);

                            if (absVelocityY >= absVelocityX)
                            {
                                // Resolve the collision on the Y axis
                                this.position = new Vector2(this.position.X, this.position.Y + intersectionDepth.Y);

                                this.velocity.Y = 0.0f;
                            }
                            else
                            {
                                // Resolve the collision on the X axis
                                this.position = new Vector2(this.position.X + intersectionDepth.X, this.position.Y);
                            }
                        }
                    }
                }
            }

            return mapCollisionOccurred;
        }

        protected virtual bool HandleMoverCollisions()
        {
            bool mapCollisionOccurred = false;

            // Handle collision with movers
            List<GameActor> moverList = AbstractMover.MoverList;
            foreach (AbstractMover mover in moverList)
            {
                Vector2 intersectionDepth = CollisionUtil.GetIntersectionDepth(this.Bounds, mover.Bounds);

                if (intersectionDepth != Vector2.Zero)
                {
                    mapCollisionOccurred = true;

                    float absDepthX = Math.Abs(intersectionDepth.X);
                    float absDepthY = Math.Abs(intersectionDepth.Y);

                    if (absDepthY < absDepthX)
                    {
                        // Resolve the collision on the Y axis
                        this.position = new Vector2(this.position.X, this.position.Y + intersectionDepth.Y);

                        if (intersectionDepth.Y < 0f)
                        {
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

        protected virtual bool CheckMapIntersection(Rectangle actorBounds)
        {
            // Get the range of tiles that we're intersecting with
            int leftTile = (int)Math.Floor((float)actorBounds.Left / Level.GetInstance().TileWidth);
            int rightTile = (int)Math.Ceiling((float)actorBounds.Right / Level.GetInstance().TileWidth) - 1;
            int topTile = (int)Math.Floor((float)actorBounds.Top / Level.GetInstance().TileHeight);
            int bottomTile = (int)Math.Ceiling((float)actorBounds.Bottom / Level.GetInstance().TileHeight) - 1;

            // Check if any of the tiles we're intersecting with are collision tiles
            for (int y = topTile; y <= bottomTile; y++)
            {
                for (int x = leftTile; x <= rightTile; x++)
                {
                    if (Level.GetInstance().IsCollisionTile(x, y) || Level.GetInstance().IsDestructibleTile(x, y) || Level.GetInstance().IsForegroundTile(x, y))
                    {
                        Rectangle tileBounds = Level.GetInstance().GetTileBounds(x, y);
                        Vector2 intersectionDepth = CollisionUtil.GetIntersectionDepth(actorBounds, tileBounds);

                        if (intersectionDepth != Vector2.Zero)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
