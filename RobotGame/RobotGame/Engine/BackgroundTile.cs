using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RobotGame.Engine
{
    class BackgroundTile
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private string assetName;
        private Vector2 position;

        private Texture2D texture;

        // Properties ------------------------------------------------------------------------------------- Properties

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public int Width
        {
            get { return this.texture.Width; }
        }

        public int Height
        {
            get { return this.texture.Height; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public BackgroundTile(string assetName)
        {
            this.assetName = assetName;
            this.position = Vector2.Zero;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void LoadTexture(ContentManager contentManager)
        {
            this.texture = contentManager.Load<Texture2D>(this.assetName);
        }

        public void Update(Vector2 velocity)
        {
            this.position += velocity;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset, Color color, float scale, float drawDepth)
        {
            spriteBatch.Draw(this.texture, this.position + offset, null, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, drawDepth);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
