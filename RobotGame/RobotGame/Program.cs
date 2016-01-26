using System;

using System.IO;
#if WINDOWS
using System.Windows.Forms;
#endif
using System.Collections.Generic;

using RobotGame.Game;
using RobotGame.Game.Input;
using RobotGame.Preferences;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;


namespace RobotGame
{
#if WINDOWS || XBOX
    static class Program
    {
        private const string PREFERENCES_FILE = "Preferences.xml";

        private const int DEFAULT_BACK_BUFFER_WIDTH = Config.DEFAULT_BACK_BUFFER_WIDTH;
        private const int DEFAULT_BACK_BUFFER_HEIGHT = Config.DEFAULT_BACK_BUFFER_HEIGHT;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            int backBufferWidth = DEFAULT_BACK_BUFFER_WIDTH;
            int backBufferHeight = DEFAULT_BACK_BUFFER_HEIGHT;
            bool fullScreen;
            InputDeviceType inputDevice;

#if WINDOWS
            // Load the preferences file
            RobotGamePreferences preferences = new XmlPreferences();
            preferences.Load(PREFERENCES_FILE);

            if (preferences.GetShowPrefsWindowOnStartup())
            {
                PreferencesForm preferencesForm = new PreferencesForm(preferences);
                preferencesForm.StartPosition = FormStartPosition.CenterScreen;
                DialogResult result = preferencesForm.ShowDialog();

                // Don't start the game if the user clicks 'Cancel'
                if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            
            inputDevice = preferences.GetInputDevice();
            fullScreen = preferences.GetFullScreen();
            if (!fullScreen)
            {
                backBufferWidth = preferences.GetBackBufferWidth();
                backBufferHeight = preferences.GetBackBufferHeight();
            }

            // Persist any changed preferences to the file
            preferences.Save();

#elif XBOX
            fullScreen = true;
            inputDevice = InputDeviceType.Controller;
#endif

            // START THE SHOW!
            RobotGame game = new RobotGame(backBufferWidth, backBufferHeight, fullScreen, inputDevice);
            game.Run();
        }
    }
#endif
}

