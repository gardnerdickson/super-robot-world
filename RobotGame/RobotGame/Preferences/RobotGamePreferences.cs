using System;
using RobotGame.Game.Input;

namespace RobotGame.Preferences
{
    public interface RobotGamePreferences
    {
        void Load(string filename);

        bool GetShowPrefsWindowOnStartup();
        int GetBackBufferWidth();
        int GetBackBufferHeight();
        bool GetFullScreen();
        InputDeviceType GetInputDevice();

        void SetShowPrefsWindowOnStartup(bool showPrefsWindowOnStartup);
        void SetBackBufferWidth(int backBufferWidth);
        void SetBackBufferHeight(int backBufferHeight);
        void SetFullScreen(bool fullScreen);
        void SetInputDevice(InputDeviceType inputDevice);

        void Save();
    }
}
