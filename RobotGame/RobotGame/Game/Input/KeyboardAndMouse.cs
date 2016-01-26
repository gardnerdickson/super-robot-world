using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Input
{
    class KeyboardAndMouse : InputDevice
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const Keys MOVE_LEFT = Config.KEYBOARD_MOVE_LEFT;
        private const Keys MOVE_RIGHT = Config.KEYBOARD_MOVE_RIGHT;
        private const Keys MOVE_UP = Config.KEYBOARD_MOVE_UP;
        private const Keys MOVE_DOWN = Config.KEYBOARD_MOVE_DOWN;
        private const Keys JUMP = Config.KEYBOARD_JUMP;
        private const Keys WEAPON_SWITCH = Config.KEYBOARD_WEAPON_SWITCH;
        private const Keys PAUSE = Config.KEYBOARD_PAUSE;
        private const Keys MENU_SELECT = Config.KEYBOARD_MENU_SELECT;
        private const Keys MENU_GO_BACK = Config.KEYBOARD_MENU_GO_BACK;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;

        private KeyboardState lastKeyboardState;
        private MouseState lastMouseState;

        private bool pauseEnabled = true;

        // Properties ------------------------------------------------------------------------------------- Properties

        public bool PauseEnabled
        {
            set { this.pauseEnabled = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Update()
        {
            this.lastKeyboardState = this.currentKeyboardState;
            this.lastMouseState = this.currentMouseState;

            this.currentKeyboardState = Keyboard.GetState();
            this.currentMouseState = Mouse.GetState();
        }

        public float GetHorizontalMove()
        {
            float move = 0.0f;
            if (this.currentKeyboardState.IsKeyDown(MOVE_LEFT))
            {
                move = -1.0f;
            }
            else if (this.currentKeyboardState.IsKeyDown(MOVE_RIGHT))
            {
                move = 1.0f;
            }
            return move;
        }

        public float GetVerticalMove()
        {
            float move = 0.0f;
            if (this.currentKeyboardState.IsKeyDown(MOVE_UP) && this.lastKeyboardState.IsKeyUp(MOVE_UP))
            {
                move = -1.0f;
            }
            else if (this.currentKeyboardState.IsKeyDown(MOVE_DOWN) && this.lastKeyboardState.IsKeyUp(MOVE_DOWN))
            {
                move = 1.0f;
            }
            return move;
        }

        public bool GetJump(bool allowHold)
        {
            if (allowHold)
            {
                return this.currentKeyboardState.IsKeyDown(JUMP);
            }
            return this.currentKeyboardState.IsKeyDown(JUMP) && this.lastKeyboardState.IsKeyUp(JUMP);
        }

        public bool GetPrimaryFire(bool allowHold)
        {
            if (allowHold)
            {
                return this.currentMouseState.LeftButton == ButtonState.Pressed;
            }
            return this.currentMouseState.LeftButton == ButtonState.Pressed && this.lastMouseState.LeftButton == ButtonState.Released;
        }

        public bool GetSecondaryFire(bool allowHold)
        {
            if (allowHold)
            {
                return this.currentMouseState.RightButton == ButtonState.Pressed;
            }
            return this.currentMouseState.RightButton == ButtonState.Pressed && this.lastMouseState.RightButton == ButtonState.Released;
        }

        public Vector2 GetNozzleDirection(Vector2 playerPosition)
        {
            Vector2 crosshairPosition = HUD.GetInstance().CrosshairPosition;
            Vector2 direction = crosshairPosition - playerPosition;

            return Vector2.Normalize(direction);
        }

        public bool GetWeaponSwitch()
        {
            return this.currentKeyboardState.IsKeyDown(WEAPON_SWITCH) && !this.lastKeyboardState.IsKeyDown(WEAPON_SWITCH);
        }

        public bool GetPause()
        {
            if (pauseEnabled)
            {
                return this.currentKeyboardState.IsKeyDown(PAUSE) && !this.lastKeyboardState.IsKeyDown(PAUSE);
            }
            else
            {
                return false;
            }
        }

        public bool GetMenuSelect()
        {
            return this.currentKeyboardState.IsKeyDown(MENU_SELECT) && !this.lastKeyboardState.IsKeyDown(MENU_SELECT);
        }

        public bool GetMenuBack()
        {
            return this.currentKeyboardState.IsKeyDown(MENU_GO_BACK) && !this.lastKeyboardState.IsKeyDown(MENU_GO_BACK);
        }

        public void EnableTakeDamageFeedback() { }

        public void EnableJetpackFeedback() { }

        public void EnableLandFeedback() { }

        public void DisableTakeDamageFeedback() { }

        public void DisableJetpackFeedback() { }

        public void DisableLandFeedback() { }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

    }
}
