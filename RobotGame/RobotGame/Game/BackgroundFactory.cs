using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RobotGame.Game
{
    public abstract class BackgroundFactory
    {
        protected float scale;
        protected int startOffsetX;
        protected int startOffsetY;
        protected Color color;
        protected float drawDepthOverride;

        public BackgroundFactory(float scale, int startOffsetX, int startOffsetY, Color color, float drawDepthOverride)
        {
            this.scale = scale;
            this.startOffsetX = startOffsetX;
            this.startOffsetY = startOffsetY;
            this.color = color;
            this.drawDepthOverride = drawDepthOverride;
        }

        public abstract Background CreateInstance();
    }

    public class SimpleBackgroundFactory : BackgroundFactory
    {
        private string assetName;
        private float scrollRate;
        private int tilesX;
        private int tilesY;

        public SimpleBackgroundFactory(string assetName, float scrollRate, int tilesX, int tilesY, float scale, int startOffsetX, int startOffsetY, Color color)
            : this(assetName, scrollRate, tilesX, tilesY, scale, startOffsetX, startOffsetY, color, -1)
        { }

        public SimpleBackgroundFactory(string assetName, float scrollRate, int tilesX, int tilesY, float scale, int startOffsetX, int startOffsetY, Color color, float drawDepthOverride)
            : base(scale, startOffsetX, startOffsetY, color, drawDepthOverride)
        {
            this.assetName = assetName;
            this.scrollRate = scrollRate;
            this.tilesX = tilesX;
            this.tilesY = tilesY;
        }

        public override Background CreateInstance()
        {
            return new SimpleBackground(this.assetName, this.scrollRate, this.tilesX, this.tilesY, this.scale, this.startOffsetX, startOffsetY, this.color, this.drawDepthOverride);
        }
    }

    public class AnimatedBackgroundFactory : BackgroundFactory
    {
        private float velocityX;
        private float scrollRate;
        private string assetName;
        private int tiles;

        public AnimatedBackgroundFactory(string assetName, float velocityX, float scrollRate, int tiles, float scale, int startOffsetX, int startOffsetY, Color color)
            : this(assetName, velocityX, scrollRate, tiles, scale, startOffsetX, startOffsetY, color, -1)
        { }

        public AnimatedBackgroundFactory(string assetName, float velocityX, float scrollRate, int tiles, float scale, int startOffsetX, int startOffsetY, Color color, float drawDepthOverride)
            : base(scale, startOffsetX, startOffsetY, color, drawDepthOverride)
        {
            this.velocityX = velocityX;
            this.scrollRate = scrollRate;
            this.assetName = assetName;
            this.tiles = tiles;
        }

        public override Background CreateInstance()
        {
            return new AnimatedBackground(this.assetName, this.velocityX, this.scrollRate, this.tiles, this.scale, this.startOffsetX, this.startOffsetY, this.color, this.drawDepthOverride);
        }
    }

    public class SkyBackgroundFactory : BackgroundFactory
    {
        private string assetName;

        public SkyBackgroundFactory(string assetName, Color color)
            : this(assetName, color, -1)
        { }

        public SkyBackgroundFactory(string assetName, Color color, float drawDepthOverride)
            : base(1f, 0, 0, color, drawDepthOverride)
        {
            this.assetName = assetName;
        }

        public override Background CreateInstance()
        {
            return new SkyBackground(this.assetName, this.color, this.drawDepthOverride);
        }
    }
}
