using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RobotGame.Game;
using RobotGame.Game.Enemy;

namespace RobotGame.Engine
{
    public abstract class Actor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        public const double FLICKER_INTERVAL = Config.FLICKER_INTERVAL;

        // Data Members --------------------------------------------------------------------------------- Data Members

        public static List<Actor> ActorList = new List<Actor>();

        protected Sprite sprite;
        protected Vector2 position;
        protected float rotation;
        protected SpriteEffects flip;
        protected float drawDepth;

        protected bool visible;
        protected bool dead;

        protected bool flicker;

        private TimerCallback VisibleCallback;

        // Properties ------------------------------------------------------------------------------------- Properties

        public bool Dead
        {
            get { return this.dead; }
        }

        public bool Flicker
        {
            set
            {
                this.flicker = value;
                
                if (value)
                {
                    TimerManager.GetInstance().RegisterTimer(FLICKER_INTERVAL, this.visibility_toggle, null);
                }
            }
        }

        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Sprite Sprite
        {
            get { return this.sprite; }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)this.position.X - (int)this.sprite.Origin.X,
                                     (int)this.position.Y - (int)this.sprite.Origin.Y,
                                     this.sprite.Width,
                                     this.sprite.Height);
            }
        }

        public Rectangle NormalizeBounds(Rectangle rect)
        {
            return new Rectangle(rect.X - (int)this.Position.X + this.Sprite.GetFrameBounds().X + (int)this.Sprite.Origin.X,
                                 rect.Y - (int)this.Position.Y + this.Sprite.GetFrameBounds().Y + (int)this.Sprite.Origin.Y,
                                 rect.Width,
                                 rect.Height);
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Actor(Vector2 position)
        {
            this.position = position;

            this.dead = false;
            this.visible = true;
            this.flicker = false;
            this.rotation = 0f;
            this.flip = SpriteEffects.None;
            this.drawDepth = 1.0f;

            this.VisibleCallback += new TimerCallback(visibility_toggle);

            ActorList.Add(this);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void visibility_toggle(Object param)
        {
            this.visible = !this.visible;
            if (this.flicker)
            {
                TimerManager.GetInstance().RegisterTimer(FLICKER_INTERVAL, this.VisibleCallback, null);
            }
            else
            {
                this.visible = true;
            }
        }

        public virtual void Remove()
        {
            this.dead = true;
        }
   
        public virtual void Update(GameTime gameTime)
        {
            if (this.sprite != null)
            {
                this.sprite.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.sprite != null && this.visible && Camera.GetInstance().IsActorOnScreen(this))
            {
                this.sprite.Draw(spriteBatch, this.position, this.rotation, this.drawDepth, flip);
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
