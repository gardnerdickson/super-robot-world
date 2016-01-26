using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace RobotGame.Engine
{
    public class SpriteSheet : IDisposable
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private int tilesX;
        private int tilesY;
        private int frameCount;
        private double frameInterval;

        private int tileWidth;
        private int tileHeight;

        private SpriteDamageMode damageMode;
        private Texture2D normalTexture;
        private Texture2D damageTexture;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        public int TilesX
        {
            get { return this.tilesX; }
        }

        public int TilesY
        {
            get { return this.tilesY; }
        }

        public int FrameCount
        {
            get { return this.frameCount; }
        }

        public double FrameInterval
        {
            get { return this.frameInterval; }
        }

        public int TileWidth
        {
            get { return this.tileWidth; }
        }

        public int TileHeight
        {
            get { return this.tileHeight; }
        }

        public Texture2D NormalTexture
        {
            get { return this.normalTexture; }
        }

        public Texture2D DamageTexture
        {
            get { return this.damageTexture; }
        }

        public SpriteDamageMode DamageMode
        {
            get { return this.damageMode; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public SpriteSheet(string assetName, int tilesX, int tilesY, int frameCount, double frameInterval,
                           SpriteDamageMode damageMode, RobotGameContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.tilesX = tilesX;
            this.tilesY = tilesY;
            this.frameCount = frameCount;
            this.frameInterval = frameInterval;
            this.damageMode = damageMode;

            this.normalTexture = contentManager.Load<Texture2D>(assetName);

            if (this.damageMode == SpriteDamageMode.DamageTexture)
            {
                this.damageTexture = MakeDamageTexture(this.normalTexture, graphicsDevice);
            }

            this.tileWidth = this.normalTexture.Width / this.tilesX;
            this.tileHeight = this.normalTexture.Height / this.tilesY;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Dispose()
        {
            this.normalTexture.Dispose();
            this.damageTexture.Dispose();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private static Texture2D MakeDamageTexture(Texture2D texture, GraphicsDevice graphicsDevice)
        {
            Color[] pixelData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(pixelData);

            for (int i = 0; i < pixelData.Length; i++)
            {
                byte offset = 200;
                byte r = (byte)Math.Min(pixelData[i].R + offset, 255);
                byte g = (byte)Math.Min(pixelData[i].G + offset, 255);
                byte b = (byte)Math.Min(pixelData[i].B + offset, 255);

                pixelData[i] = new Color(r, g, b, pixelData[i].A);
            }

            Texture2D damageTexture = new Texture2D(graphicsDevice, texture.Width, texture.Height);
            damageTexture.SetData<Color>(pixelData);

            return damageTexture;
        }
    }
}
