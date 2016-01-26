using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RobotGame.Engine
{
    class SkyBackground : Background
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private string assetName;
        private Texture2D texture;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public SkyBackground(string assetName, Color color, float drawDepthOverride)
            : base(1f, 0, 0, color, drawDepthOverride)
        {
            this.assetName = assetName;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void LoadTexture(ContentManager contentManager)
        {
            this.texture = contentManager.Load<Texture2D>(this.assetName);
        }

        public override void Draw(SpriteBatch spriteBatch, int offsetX, int offsetY, float drawDepth)
        {
            spriteBatch.Draw(this.texture, new Vector2(offsetX, offsetY), null, this.color, 0f, Vector2.Zero, 1f, SpriteEffects.None, (this.drawDepthOverride != -1) ? this.drawDepthOverride : drawDepth);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
