using System;
using RobotGame.Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RobotGame.Game
{
    class TimedMenu : Menu
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback timerCallback;

        private double lifeTime;
        private int state;
        private bool allowAdvance;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        public double LifeTime
        {
            get { return this.lifeTime; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public TimedMenu(string textureAsset, double lifeTime, string messageFontAsset, bool allowAdvance, int? screenWidth, int? screenHeight)
            : base(textureAsset, messageFontAsset, screenWidth, screenHeight)
        {
            this.lifeTime = lifeTime;
            this.state = -1;
            this.allowAdvance = allowAdvance;

            this.timerCallback = new TimerCallback(menu_state_change);
        }

        public TimedMenu(string textureAsset, double lifeTime)
            : this(textureAsset, lifeTime, null, false, null, null)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public double TimeLeft()
        {
            return TimerManager.GetInstance().GetRemainingTime(this.timerCallback);
        }

        public override int Update()
        {
            if (!TimerManager.GetInstance().IsTimerRegistered(this.timerCallback) && this.state == -1)
            {
                TimerManager.GetInstance().RegisterTimer(lifeTime, this.timerCallback, null);
            }

            if (allowAdvance && RobotGame.RobotGameInputDevice.GetMenuSelect())
            {
                this.state = 0;
            }

            return state;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (messages.Count > 0 && messageFont != null)
            {
                int offsetY = (int)(this.screenHeight / 2) - (int)((messageFont.LineSpacing * messages.Count) / 2);

                foreach (string message in messages)
                {
                    int offsetX;
                    if (this.alignment == TextAlignment.Center)
                    {
                        offsetX = (int)(this.screenWidth / 2) - (int)(this.messageFont.MeasureString(message).X / 2);
                    }
                    else
                    {
                        offsetX = LEFT_ALIGNMENT_OFFSET_X;
                    }
                    spriteBatch.DrawString(this.messageFont, message, Camera.GetInstance().Position + new Vector2(offsetX, offsetY), Color.White, 0f, Vector2.Zero, this.textScale, SpriteEffects.None, TEXT_DRAW_DEPTH);
                    offsetY += messageFont.LineSpacing;
                }
            }
        }

        public override void Reset()
        {
            this.state = -1;
        }
                
        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void menu_state_change(object param)
        {
            this.state = 0;
        }
    }
}
