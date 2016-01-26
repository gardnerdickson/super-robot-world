using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotGame.Game;
using RobotGame.Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RobotGame.Exceptions;
using RobotGame.Game.Input;

namespace RobotGame
{
    class Credits
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const string MENU_BLACK_ASSET = Config.MENU_BLACK_ASSET;
        private const string MENU_FONT_ASSET = Config.MENU_FONT_ASSET;
        private const string SPECIAL_THANKS_BACKGROUND_ASSET = Config.SPECIAL_THANKS_BACKGROUND_ASSET;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private List<Menu> creditMenus;
        private int currentMenu;

        // Properties ------------------------------------------------------------------------------------- Properties
        
        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Credits(int screenWidth, int screenHeight)
        {
            currentMenu = 0;

            creditMenus = new List<Menu>();
            creditMenus.Add(new TimedMenu(MENU_BLACK_ASSET, 3000, MENU_FONT_ASSET, false, screenWidth, screenHeight));
            creditMenus.Add(new TimedMenu(MENU_BLACK_ASSET, 7000, MENU_FONT_ASSET, false, screenWidth, screenHeight));
            creditMenus.Add(new TimedMenu(MENU_BLACK_ASSET, 1000, MENU_FONT_ASSET, false, screenWidth, screenHeight));
            creditMenus.Add(new TimedMenu(MENU_BLACK_ASSET, 7000, MENU_FONT_ASSET, false, screenWidth, screenHeight));
            creditMenus.Add(new TimedMenu(MENU_BLACK_ASSET, 1000, MENU_FONT_ASSET, false, screenWidth, screenHeight));
            creditMenus.Add(new TimedMenu(SPECIAL_THANKS_BACKGROUND_ASSET, 10000, MENU_FONT_ASSET, false, screenWidth, screenHeight));
            creditMenus.Add(new TimedMenu(MENU_BLACK_ASSET, 1000, MENU_FONT_ASSET, false, screenWidth, screenHeight));
            creditMenus.Add(new InteractiveMenu(MENU_BLACK_ASSET, RobotGame.RobotGameInputDevice, MENU_FONT_ASSET, screenWidth, screenHeight));

            creditMenus[0].AddMessage("");
            creditMenus[0].AddMessage("");
            creditMenus[0].AddMessage("");
            creditMenus[0].AddMessage("");
            creditMenus[0].AddMessage("");
            creditMenus[0].AddMessage("");
            creditMenus[0].AddMessage("");
            creditMenus[0].AddMessage("");
            creditMenus[0].AddMessage("");

            creditMenus[1].AddMessage("SUPER ROBOT WORLD");
            creditMenus[1].AddMessage("");
            creditMenus[1].AddMessage("");
            creditMenus[1].AddMessage("GARDNER DICKSON");
            creditMenus[1].AddMessage("Programming");
            creditMenus[1].AddMessage("Design");
            creditMenus[1].AddMessage("Art");
            creditMenus[1].AddMessage("");
            creditMenus[1].AddMessage("");

            creditMenus[2].AddMessage("SUPER ROBOT WORLD");
            creditMenus[2].AddMessage("");
            creditMenus[2].AddMessage("");
            creditMenus[2].AddMessage("");
            creditMenus[2].AddMessage("");
            creditMenus[2].AddMessage("");
            creditMenus[2].AddMessage("");
            creditMenus[2].AddMessage("");
            creditMenus[2].AddMessage("");

            creditMenus[3].AddMessage("SUPER ROBOT WORLD");
            creditMenus[3].AddMessage("");
            creditMenus[3].AddMessage("");
            creditMenus[3].AddMessage("CHRIS BUTLER");
            creditMenus[3].AddMessage("Music");
            creditMenus[3].AddMessage("");
            creditMenus[3].AddMessage("JESSE REID");
            creditMenus[3].AddMessage("Sound Effects");
            creditMenus[3].AddMessage("");

            creditMenus[4].AddMessage("SUPER ROBOT WORLD");
            creditMenus[4].AddMessage("");
            creditMenus[4].AddMessage("");
            creditMenus[4].AddMessage("");
            creditMenus[4].AddMessage("");
            creditMenus[4].AddMessage("");
            creditMenus[4].AddMessage("");
            creditMenus[4].AddMessage("");
            creditMenus[4].AddMessage("");

            creditMenus[5].AddMessage("SUPER ROBOT WORLD");
            creditMenus[5].AddMessage("");
            creditMenus[5].AddMessage("");
            creditMenus[5].AddMessage("SPECIAL THANKS");
            creditMenus[5].AddMessage("");
            creditMenus[5].AddMessage("");
            creditMenus[5].AddMessage("");
            creditMenus[5].AddMessage("");
            creditMenus[5].AddMessage("");

            creditMenus[6].AddMessage("SUPER ROBOT WORLD");
            creditMenus[6].AddMessage("");
            creditMenus[6].AddMessage("");
            creditMenus[6].AddMessage("");
            creditMenus[6].AddMessage("");
            creditMenus[6].AddMessage("");
            creditMenus[6].AddMessage("");
            creditMenus[6].AddMessage("");
            creditMenus[6].AddMessage("");

            creditMenus[7].AddMessage("SUPER ROBOT WORLD");
            creditMenus[7].AddMessage("");
            creditMenus[7].AddMessage("");
            creditMenus[7].AddMessage("");
            creditMenus[7].AddMessage("Thanks for playing!");
            creditMenus[7].AddMessage("");
            creditMenus[7].AddMessage("");
            creditMenus[7].AddMessage("");
            creditMenus[7].AddMessage("");
        }

        public void LoadContent(ContentManager content)
        {
            foreach (Menu menu in creditMenus)
            {
                menu.LoadContent(content);
            }
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public int Update()
        {
            if (creditMenus[currentMenu].Update() == 0)
            {
                currentMenu++;

                if (currentMenu > creditMenus.Count - 1)
                {
                    //currentMenu = 0;
                    //foreach (Menu menu in creditMenus)
                    //{
                    //    menu.Reset();
                    //}
                    currentMenu--;
                    return 0;
                }
            }
            return -1;
        }

        public void Reset()
        {
            currentMenu = 0;
            foreach (Menu menu in creditMenus)
            {
                menu.Reset();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            creditMenus[currentMenu].Draw(spriteBatch);
#if DEBUG
            //Debug.AddDebugInfo("Credits menu draw depth: " + creditMenus[currentMenu].DrawDepth);
#endif
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

    }
}
