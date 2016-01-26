using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotGame.Game.Input;

namespace RobotGame.Game
{
    class PressStartMenu : InteractiveMenu
    {
        public PressStartMenu(string textureAsset, InputDevice inputDevice, string messageFontAsset, int? screenWidth, int? screenHeight)
            : base(textureAsset, inputDevice, messageFontAsset, screenWidth, screenHeight)
        { }

        public override int Update()
        {
            if (this.inputDevice.GetPause())
            {
                return 0;
            }
            return -1;
        }
    }
}
