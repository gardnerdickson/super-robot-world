using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace RobotGame.Engine
{
    class SimpleBackground : Background
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private string assetName;
        private float scrollRate;
        private int tilesX;
        private int tilesY;

        private Texture2D texture;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public SimpleBackground(string assetName, float scrollRate, int tilesX, int tilesY, float scale, int startOffsetX, int startOffsetY, Color color, float drawDepthOverride)
            : base(scale, startOffsetX, startOffsetY, color, drawDepthOverride)
        {
            this.assetName = assetName;
            this.scrollRate = scrollRate;
            this.tilesX = tilesX;
            this.tilesY = tilesY;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void LoadTexture(ContentManager contentManager)
        {
            this.texture = contentManager.Load<Texture2D>(this.assetName);
        }

        public override void Draw(SpriteBatch spriteBatch, int offsetX, int offsetY, float drawDepth)
        {
            float depth = (this.drawDepthOverride != -1) ? this.drawDepthOverride : drawDepth;
            for (int i = 0; i < this.tilesX; i++)
            {
                for (int j = 0; j < this.tilesY; j++)
                {
                    spriteBatch.Draw(this.texture, new Vector2(offsetX * scrollRate + (i * texture.Width * this.scale) + startOffsetX, offsetY * scrollRate + (j * texture.Height * this.scale) + startOffsetY),
                                     null, this.color, 0.0f, Vector2.Zero, this.scale, SpriteEffects.None, depth);
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
