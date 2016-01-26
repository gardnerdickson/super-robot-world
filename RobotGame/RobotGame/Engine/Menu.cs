using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RobotGame.Game.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RobotGame.Engine
{
    public abstract class Menu
    {
        public enum TextAlignment
        {
            Left,
            Center
        }

        // Constants ---------------------------------------------------------------------------------------- Contants

        protected const float DRAW_DEPTH = Config.MENU_DRAW_DEPTH;
        protected const float TEXT_DRAW_DEPTH = Config.MENU_TEXT_DRAW_DEPTH;
        protected const int LEFT_ALIGNMENT_OFFSET_X = Config.MENU_LEFT_ALIGNMENT_OFFSET_X;

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected Nullable<int> screenWidth;
        protected Nullable<int> screenHeight;

        protected string textureAsset;
        protected Texture2D texture;

        protected TextAlignment alignment;
        protected float textScale;

        protected string messageFontAsset;

        protected List<string> messages;

        protected SpriteFont messageFont;

        protected int backgroundAlpha;

        protected float drawDepth;

        // Properties ------------------------------------------------------------------------------------- Properties

        public List<string> Messages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }

        public int BackgroundAlpha
        {
            get { return this.backgroundAlpha; }
            set { this.backgroundAlpha = value; }
        }

        public TextAlignment Alignment
        {
            get { return this.alignment; }
            set { this.alignment = value; }
        }

        public float TextScale
        {
            get { return this.textScale; }
            set { this.textScale = value; }
        }

        public float DrawDepth
        {
            get { return this.drawDepth; }
            set { this.drawDepth = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Menu(string textureAsset, string messageFontAsset, int? screenWidth, int? screenHeight)
        {
            this.textureAsset = textureAsset;

            this.alignment = TextAlignment.Center;
            this.textScale = 1.0f;

            this.messageFontAsset = messageFontAsset;
            this.messages = new List<string>();

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            this.backgroundAlpha = 255;

            this.drawDepth = DRAW_DEPTH;
        }

        public Menu(string textureAsset)
            : this(textureAsset, null, null, null)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods
        
        public virtual void LoadContent(ContentManager contentManager)
        {
            this.texture = contentManager.Load<Texture2D>(this.textureAsset);

            if (this.messageFontAsset != null)
            {
                this.messageFont = contentManager.Load<SpriteFont>(this.messageFontAsset);
            }
        }

        public abstract int Update();

        public virtual void Reset() { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, Camera.GetInstance().Position, null, new Color(255, 255, 255, this.backgroundAlpha), 0f, Vector2.Zero, 1f, SpriteEffects.None, this.drawDepth);
        }

        public void AddMessage(string message)
        {
            this.messages.Add(message);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
