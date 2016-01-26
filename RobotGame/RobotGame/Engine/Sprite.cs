using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotGame.Engine
{
    public enum SpriteDamageMode
    {
        None,
        DamageTexture
    }

    public class Sprite
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const double DAMAGE_TEXTURE_DURATION = Config.DAMAGE_TEXTURE_DURATION;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback ShowDamageCallback;

        private SpriteKey spriteSheetKey;
        private int currentFrame;
        private double frameTimeRemaining;

        private bool showDamage;
        private bool animate;
        private bool reverseFrameIteration = false;

        private int alpha;
        private float scale;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        public int FrameCount
        {
            get { return SpriteSheetFactory.GetSpriteSheet(this.spriteSheetKey).FrameCount; }
        }

        public int CurrentFrame
        {
            get { return this.currentFrame; }
            set { this.currentFrame = value; }
        }

        public Texture2D Texture
        {
            get { return this.SpriteSheet.NormalTexture; }
        }

        public int Width
        {
            get { return this.SpriteSheet.TileWidth; }
        }

        public int Height
        {
            get { return this.SpriteSheet.TileHeight; }
        }

        public Vector2 Origin
        {
            get { return new Vector2(this.SpriteSheet.TileWidth / 2, this.SpriteSheet.TileHeight / 2); }
        }

        public double FrameTimeRemaining
        {
            get { return this.frameTimeRemaining; }
        }
        
        public int Alpha
        {
            get { return this.alpha; }
            set { this.alpha = value; }
        }

        public float Scale
        {
            get { return this.scale; }
            set { this.scale = value; }
        }

        public bool Animate
        {
            get { return this.animate; }
            set { this.animate = value; }
        }

        public bool ReverseFrameIteration
        {
            get { return this.reverseFrameIteration; }
            set { this.reverseFrameIteration = value; }
        }

        public SpriteSheet SpriteSheet
        {
            get { return SpriteSheetFactory.GetSpriteSheet(this.spriteSheetKey); }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Sprite(SpriteKey spriteSheetKey)
            : this(spriteSheetKey, true)
        { }

        public Sprite(SpriteKey spriteSheetKey, bool animate)
        {
            this.spriteSheetKey = spriteSheetKey;
            this.animate = animate;

            this.currentFrame = 0;
            this.frameTimeRemaining = this.SpriteSheet.FrameInterval;

            this.alpha = 255;
            this.scale = 1.0f;

            this.ShowDamageCallback = new TimerCallback(damage_disable);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public Rectangle GetFrameBounds()
        {
            int x = this.currentFrame % this.SpriteSheet.TilesX * this.SpriteSheet.TileWidth;
            int y = this.currentFrame / this.SpriteSheet.TilesX * this.SpriteSheet.TileHeight;

            return new Rectangle(x, y, this.SpriteSheet.TileWidth, this.SpriteSheet.TileHeight);
        }

        public void ShowDamage()
        {
            this.showDamage = true;

            if (!TimerManager.GetInstance().IsTimerRegistered(this.ShowDamageCallback))
            {
                TimerManager.GetInstance().RegisterTimer(DAMAGE_TEXTURE_DURATION, this.ShowDamageCallback, null);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (animate)
            {
                this.frameTimeRemaining -= gameTime.ElapsedGameTime.TotalMilliseconds;

                if (reverseFrameIteration)
                {
                    if (this.frameTimeRemaining <= 0)
                    {
                        this.currentFrame--;
                        if (this.currentFrame < 0)
                        {
                            this.currentFrame = this.SpriteSheet.FrameCount - 1;
                        }
                        this.frameTimeRemaining = this.SpriteSheet.FrameInterval;
                    }
                }
                else
                {
                    if (this.frameTimeRemaining <= 0)
                    {
                        this.currentFrame++;
                        if (this.currentFrame >= this.SpriteSheet.FrameCount)
                        {
                            this.currentFrame = 0;
                        }
                        this.frameTimeRemaining = this.SpriteSheet.FrameInterval;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, float drawDepth, SpriteEffects flip)
        {
            this.Draw(spriteBatch, position, rotation, drawDepth, flip, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, float drawDepth, SpriteEffects spriteEffects, Color color)
        {
            SpriteSheet spriteSheet = SpriteSheetFactory.GetSpriteSheet(this.spriteSheetKey);
            Texture2D texture;
            if (this.showDamage && spriteSheet.DamageMode == SpriteDamageMode.DamageTexture)
            {
                texture = spriteSheet.DamageTexture;
            }
            else
            {
                texture = spriteSheet.NormalTexture;
            }
            
            spriteBatch.Draw(texture, position, GetFrameBounds(), new Color(color.R, color.G, color.B, this.alpha), rotation, this.Origin, this.scale, spriteEffects, drawDepth);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void damage_disable(Object param)
        {
            this.showDamage = false;
        }
    }
}
