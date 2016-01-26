using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace RobotGame.Engine
{
    public abstract class Background
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected float scale;
        protected int startOffsetX;
        protected int startOffsetY;
        protected Color color;
        protected float drawDepthOverride;
        
        // Properties ------------------------------------------------------------------------------------- Properties
        
        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Background(float scale, int startOffsetX, int startOffsetY, Color color, float drawDepthOverride)
        {
            this.scale = scale;
            this.startOffsetX = startOffsetX;
            this.startOffsetY = startOffsetY;
            this.color = color;
            this.drawDepthOverride = drawDepthOverride;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public abstract void LoadTexture(ContentManager contentManager);

        public virtual void Update() { }

        public abstract void Draw(SpriteBatch spriteBatch, int offsetX, int offsetY, float drawDepth);

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
