using System;
using RobotGame.Game.Input;
using RobotGame.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace RobotGame.Game
{
    class PauseMenu : OptionMenu
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public PauseMenu(string textureAsset, InputDevice inputDevice, string messageFontAsset, string optionFontAsset, int screenWidth, int screenHeight)
            : base(textureAsset, inputDevice, messageFontAsset, optionFontAsset, null, screenWidth, screenHeight)
        { }
        
        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override int Update()
        {
            if (this.inputDevice.GetPause() && !this.confirmationMenuEnabled)
            {
                return 0;
            }
            else
            {
                return base.Update();
            }
        }
        
        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
