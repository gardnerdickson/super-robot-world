using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace RobotGame.Engine
{
    public class GuideUtils
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        private GuideUtils()
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static bool UserCanPurchaseGame(PlayerIndex index)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[index];
            if (gamer == null)
                return false;

            return gamer.Privileges.AllowPurchaseContent;
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
