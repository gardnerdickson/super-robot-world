using System;
using Microsoft.Xna.Framework;

namespace RobotGame.Game.Input
{
    public enum InputDeviceType
    {
        KeyboardAndMouse,
        Controller
    }

    public interface InputDevice
    {
        bool PauseEnabled { set; }

        void Update();

        float GetHorizontalMove();
        float GetVerticalMove();
        bool GetJump(bool allowHold);
        bool GetPrimaryFire(bool allowHold);
        bool GetSecondaryFire(bool allowHold);
        Vector2 GetNozzleDirection(Vector2 playerPosition);
        bool GetWeaponSwitch();
        bool GetPause();
        bool GetMenuSelect();
        bool GetMenuBack();

        void EnableTakeDamageFeedback();
        void EnableLandFeedback();
        void EnableJetpackFeedback();
        void DisableTakeDamageFeedback();
        void DisableLandFeedback();
        void DisableJetpackFeedback();
    }
}