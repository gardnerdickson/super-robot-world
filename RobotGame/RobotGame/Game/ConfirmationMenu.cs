using System;
using RobotGame.Game.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RobotGame.Game.Audio;

namespace RobotGame.Game
{
    class ConfirmationMenu : OptionMenu
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private static string[] CONFIRMATION_MENU_OPTIONS = Config.CONFIRMATION_MENU_OPTIONS;
        private static string CONFIRMATION_MENU_MESSAGE = Config.CONFIRMATION_MENU_MESSAGE;
        private const int CONFIRMATION_MENU_YES = Config.CONFIRMATION_MENU_YES;
        private const int CONFIRMATION_MENU_NO = Config.CONFIRMATION_MENU_NO;

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public ConfirmationMenu(string textureAsset, InputDevice inputDevice, string messageFontAsset, string optionFontAsset, int screenWidth, int screenHeight)
            : base(textureAsset, inputDevice, messageFontAsset, optionFontAsset, null, screenWidth, screenHeight)
        {
            this.currentOption = 1;

            this.AddMessage(CONFIRMATION_MENU_MESSAGE);

            AddOption(CONFIRMATION_MENU_OPTIONS[CONFIRMATION_MENU_YES]);
            AddOption(CONFIRMATION_MENU_OPTIONS[CONFIRMATION_MENU_NO]);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods
        
        public override int Update()
        {
            float verticalMove = this.inputDevice.GetVerticalMove();
            if (verticalMove > 0 && this.currentOption != options.Count - 1)
            {
                this.currentOption++;
                SoundManager.PlaySoundEffect(SoundKey.MenuToggle);
            }
            else if (verticalMove < 0 && this.currentOption != 0)
            {
                this.currentOption--;
                SoundManager.PlaySoundEffect(SoundKey.MenuToggle);
            }

            if (this.inputDevice.GetMenuSelect())
            {
                if (this.currentOption == 0)
                {
                    SoundManager.PlaySoundEffect(SoundKey.MenuIn);
                }
                else
                {
                    SoundManager.PlaySoundEffect(SoundKey.MenuOut);
                }
                return this.currentOption;
            }
            else if (this.inputDevice.GetMenuBack())
            {
                SoundManager.PlaySoundEffect(SoundKey.MenuOut);
                return CONFIRMATION_MENU_NO;
            }

            return -1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            DrawMenu(spriteBatch);
        }

        public override void Reset()
        {
            this.currentOption = 1;
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
