using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RobotGame.Engine
{
    class AnimatedBackground : Background
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private float velocityX;
        private float scrollRate;

        private BackgroundTile[] backgroundTiles;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public AnimatedBackground(string assetName, float velocityX, float scrollRate, int tiles, float scale, int startOffsetX, int startOffsetY, Color color, float drawDepthOverride)
            : base(scale, startOffsetX, startOffsetY, color, drawDepthOverride)
        {
            this.velocityX = velocityX;
            this.scrollRate = scrollRate;

            this.backgroundTiles = new BackgroundTile[tiles];

            for (int i = 0; i < this.backgroundTiles.Length; i++)
            {
                backgroundTiles[i] = new BackgroundTile(assetName);
            }
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void LoadTexture(ContentManager contentManager)
        {
            for (int i = 0; i < this.backgroundTiles.Length; i++)
            {
                backgroundTiles[i].LoadTexture(contentManager);

                int textureOffsetX = 0;
                int index = i;
                while (index > 0)
                {
                    textureOffsetX += backgroundTiles[--index].Width;
                }

                backgroundTiles[i].Position += new Vector2((textureOffsetX * this.scale) + this.startOffsetX, this.startOffsetY);
            }
        }

        public override void Update()
        {

            foreach (BackgroundTile backgroundTile in this.backgroundTiles)
            {
                backgroundTile.Update(new Vector2(this.velocityX, 0f));

                if (this.velocityX < 0)
                {
                    if (backgroundTile.Position.X + backgroundTile.Width * this.scale < 0)
                    {
                        backgroundTile.Position = new Vector2(backgroundTile.Position.X + backgroundTile.Width * this.backgroundTiles.Length * this.scale, backgroundTile.Position.Y);
                    }
                }
                else if (this.velocityX > 0)
                {
                    if (backgroundTile.Position.X * this.scale > Level.GetInstance().Width)
                    {
                        backgroundTile.Position = new Vector2(-backgroundTile.Width * this.scale, backgroundTile.Position.Y);
                    }
                }
            }
            
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch, int offsetX, int offsetY, float drawDepth)
        {
            Vector2 offset = new Vector2(offsetX * scrollRate, offsetY * scrollRate);

            float depth = (this.drawDepthOverride != -1) ? this.drawDepthOverride : drawDepth;
            foreach (BackgroundTile backgroundTile in this.backgroundTiles)
            {
                backgroundTile.Draw(spriteBatch, offset, this.color, this.scale, depth);
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
