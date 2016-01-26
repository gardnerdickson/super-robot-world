using System;
using RobotGame.Engine;
using RobotGame.Game.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RobotGame.Game
{
    class InteractiveMenu : Menu
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected InputDevice inputDevice;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public InteractiveMenu(string textureAsset, InputDevice inputDevice, string messageFontAsset, int? screenWidth, int? screenHeight)
            : base(textureAsset, messageFontAsset, screenWidth, screenHeight)
        {
            this.inputDevice = inputDevice;
        }

        public InteractiveMenu(string textureAsset, InputDevice inputDevice)
            : this(textureAsset, inputDevice, null, null, null)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override int Update()
        {
            if (this.inputDevice.GetMenuSelect() || this.inputDevice.GetMenuBack())
            {
                return 0;
            }
            return -1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (messages.Count > 0)
            {
                int messageFontHeight = 0;
                if (messageFont != null)
                {
                    messageFontHeight = this.messageFont.LineSpacing;
                }

                int offsetY = (int)(this.screenHeight / 2) - (int)((messageFontHeight * messages.Count) / 2);
                int offsetX;

                foreach (string message in messages)
                {
                    if (this.alignment == TextAlignment.Center)
                    {
                        offsetX = (int)(this.screenWidth / 2) - (int)(this.messageFont.MeasureString(message).X / 2);
                    }
                    else
                    {
                        offsetX = Config.TITLE_SAFE_OFFSET_X + LEFT_ALIGNMENT_OFFSET_X;
                    }

                    spriteBatch.DrawString(this.messageFont, message, Camera.GetInstance().Position + new Vector2(offsetX, offsetY), Color.White, 0f, Vector2.Zero, this.textScale, SpriteEffects.None, TEXT_DRAW_DEPTH);
                    offsetY += this.messageFont.LineSpacing;
                }
            }

        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
