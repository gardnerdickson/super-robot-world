using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Input
{
    class Controller : InputDevice
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float THUMBSTICK_MOVE_THRESHOLD = Config.THUMBSTICK_MOVE_THRESHOLD;
        private const Buttons JUMP = Config.GAMEPAD_JUMP;
        private const Buttons PRIMARY_FIRE = Config.GAMEPAD_PRIMARY_FIRE;
        private const Buttons SECONDARY_FIRE = Config.GAMEPAD_SECONDARY_FIRE;
        private const Buttons WEAPON_SWITCH = Config.GAMEPAD_WEAPON_SWITCH;
        private const Buttons PAUSE = Config.GAMEPAD_PAUSE;
        private const Buttons MENU_SELECT = Config.GAMEPAD_MENU_SELECT;
        private const Buttons MENU_GO_BACK = Config.GAMEPAD_MENU_GO_BACK;

        private const float TAKE_DAMAGE_LF_VIBRATION = Config.TAKE_DAMAGE_LF_VIBRATION;
        private const float TAKE_DAMAGE_HF_VIBRATION = Config.TAKE_DAMAGE_HF_VIBRATION;
        private const float JETPACK_LF_VIBRATION = Config.JETPACK_LF_VIBRATION;
        private const float JETPACK_HF_VIBRATION = Config.JETPACK_HF_VIBRATION;
        private const float LAND_LF_VIBRATION = Config.LAND_LF_VIBRATION;
        private const float LAND_HF_VIBRATION = Config.LAND_HF_VIBRATION;

        private const int NUM_VIBRATIONS = 3;
        private const int JETPACK_VIBRATION_INDEX = 0;
        private const int LAND_VIBRATION_INDEX = 1;
        private const int TAKE_DAMAGE_VIBRATION_INDEX = 2;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private VibrationSetting[] vibrationSettings;

        private PlayerIndex activePlayerIndex;
        private GamePadState currentGamePadState;
        private GamePadState lastGamePadState;

        private Vector2 previousNozzleAngle;

        private bool pauseEnabled = true;

        private bool playerIndexChanged = false;

        // Properties ------------------------------------------------------------------------------------- Properties

        public bool PauseEnabled
        {
            set { this.pauseEnabled = value; }
        }

        public bool PlayerIndexChanged
        {
            get { return this.playerIndexChanged; }
        }

        public PlayerIndex ActivePlayerIndex
        {
            get { return this.activePlayerIndex; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Controller()
        {
            this.previousNozzleAngle = new Vector2(1, 0);
            this.activePlayerIndex = PlayerIndex.One;

            this.vibrationSettings = new VibrationSetting[NUM_VIBRATIONS];
            this.vibrationSettings[TAKE_DAMAGE_VIBRATION_INDEX] = new VibrationSetting(TAKE_DAMAGE_LF_VIBRATION, TAKE_DAMAGE_HF_VIBRATION, false);
            this.vibrationSettings[LAND_VIBRATION_INDEX] = new VibrationSetting(LAND_LF_VIBRATION, LAND_HF_VIBRATION, false);
            this.vibrationSettings[JETPACK_VIBRATION_INDEX] = new VibrationSetting(JETPACK_LF_VIBRATION, JETPACK_HF_VIBRATION, false);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Update()
        {
            this.lastGamePadState = this.currentGamePadState;

            PlayerIndex oldIndex = this.activePlayerIndex;

            this.currentGamePadState = GamePad.GetState(this.activePlayerIndex);

            // Try to recover from controller disconnect
            if (!this.currentGamePadState.IsConnected)
            {
                this.currentGamePadState = new GamePadState();
                for (PlayerIndex index = PlayerIndex.Four; index >= PlayerIndex.One; index--)
                {
                    GamePadState gamePadState = GamePad.GetState(index);
                    GamePadCapabilities gamePadCapabilities = GamePad.GetCapabilities(index);

                    if (gamePadState.IsConnected && gamePadCapabilities.GamePadType == GamePadType.GamePad)
                    {
                        this.activePlayerIndex = index;
                        this.currentGamePadState = gamePadState;
                    }
                }
            }

            // If start is pressed on another controller, switch to that controller
            for (PlayerIndex index = PlayerIndex.Four; index >= PlayerIndex.One; index--)
            {
                GamePadState gamePadState = GamePad.GetState(index);
                GamePadCapabilities gamePadCapabilities = GamePad.GetCapabilities(index);

                if (gamePadState.IsButtonDown(Buttons.Start) && gamePadCapabilities.GamePadType == GamePadType.GamePad)
                {
                    this.activePlayerIndex = index;
                    this.currentGamePadState = gamePadState;
                }
            }

            this.playerIndexChanged = this.activePlayerIndex != oldIndex || (!this.lastGamePadState.IsConnected && this.currentGamePadState.IsConnected);


#if DEBUG
            if (this.currentGamePadState.IsButtonDown(Buttons.X) && this.lastGamePadState.IsButtonUp(Buttons.X))
            {
                this.playerIndexChanged = true;
            }
#endif
        }

        public float GetHorizontalMove()
        {
            float thumbstickMove = this.currentGamePadState.ThumbSticks.Left.X;
            if (Math.Abs(thumbstickMove) >= THUMBSTICK_MOVE_THRESHOLD)
            {
                return thumbstickMove;
            }
            else if (this.currentGamePadState.IsButtonDown(Buttons.DPadLeft))
            {
                return -1.0f;
            }
            else if (this.currentGamePadState.IsButtonDown(Buttons.DPadRight))
            {
                return 1.0f;
            }

            return 0.0f;
        }

        public float GetVerticalMove()
        {
            float thumbstickMove = this.currentGamePadState.ThumbSticks.Left.Y;
            float lastThumbstickMove = this.lastGamePadState.ThumbSticks.Left.Y;

            if (Math.Abs(thumbstickMove) >= THUMBSTICK_MOVE_THRESHOLD && Math.Abs(lastThumbstickMove) < THUMBSTICK_MOVE_THRESHOLD)
            {
                return -thumbstickMove;
            }
            else if (this.currentGamePadState.IsButtonDown(Buttons.DPadUp) && this.lastGamePadState.IsButtonUp(Buttons.DPadUp))
            {
                return -1.0f;
            }
            else if (this.currentGamePadState.IsButtonDown(Buttons.DPadDown) && this.lastGamePadState.IsButtonUp(Buttons.DPadDown))
            {
                return 1.0f;
            }

            return 0f;
        }

        public bool GetJump(bool allowHold)
        {
            if (allowHold)
            {
                return this.currentGamePadState.Triggers.Left > 0.0f;
            }
            return this.currentGamePadState.Triggers.Left > 0.0f && this.lastGamePadState.Triggers.Left == 0.0f;
        }

        public bool GetPrimaryFire(bool allowHold)
        {
            if (allowHold)
            {
                return this.currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed;
            }
            return this.currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed && this.lastGamePadState.Buttons.RightShoulder == ButtonState.Released;
        }

        public bool GetSecondaryFire(bool allowHold)
        {
            if (allowHold)
            {
                return this.currentGamePadState.Triggers.Right > 0.0f;
            }
            return this.currentGamePadState.Triggers.Right > 0.0f && this.lastGamePadState.Triggers.Right == 0.0f;
        }

        public Vector2 GetNozzleDirection(Vector2 playerPosition)
        {
            Vector2 thumbstickVector = this.currentGamePadState.ThumbSticks.Right;
            thumbstickVector.Y *= -1;
            Vector2 normalizedThumbstickVector = Vector2.Normalize(thumbstickVector);

            if (thumbstickVector.Length() < 0.5f)
            {
                normalizedThumbstickVector = this.previousNozzleAngle;
            }
            else
            {
                this.previousNozzleAngle = normalizedThumbstickVector;
            }

            return normalizedThumbstickVector;
        }

        public bool GetWeaponSwitch()
        {
            return this.currentGamePadState.Buttons.A == ButtonState.Pressed && this.lastGamePadState.Buttons.A == ButtonState.Released;
        }

        public bool GetPause()
        {
            if (pauseEnabled)
            {
                return this.currentGamePadState.Buttons.Start == ButtonState.Pressed && this.lastGamePadState.Buttons.Start == ButtonState.Released;
            }
            else
            {
                return false;
            }
        }

        public bool GetMenuSelect()
        {
            return this.currentGamePadState.Buttons.A == ButtonState.Pressed && this.lastGamePadState.Buttons.A == ButtonState.Released;
        }

        public bool GetMenuBack()
        {
            return this.currentGamePadState.Buttons.B == ButtonState.Pressed && this.lastGamePadState.Buttons.B == ButtonState.Released;
        }

        public void EnableTakeDamageFeedback()
        {
            EnableVibration(TAKE_DAMAGE_VIBRATION_INDEX);
        }

        public void EnableJetpackFeedback()
        {
            EnableVibration(JETPACK_VIBRATION_INDEX);
        }

        public void EnableLandFeedback()
        {
            EnableVibration(LAND_VIBRATION_INDEX);
        }

        public void DisableTakeDamageFeedback()
        {
            DisableVibration(TAKE_DAMAGE_VIBRATION_INDEX);
        }

        public void DisableJetpackFeedback()
        {
            DisableVibration(JETPACK_VIBRATION_INDEX);
        }

        public void DisableLandFeedback()
        {
            DisableVibration(LAND_VIBRATION_INDEX);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void EnableVibration(int index)
        {
            this.vibrationSettings[index].Enabled = true;
            SetControllerVibrationToHighestPrioritySetting();
        }

        private void DisableVibration(int index)
        {
            this.vibrationSettings[index].Enabled = false;
            SetControllerVibrationToHighestPrioritySetting();
        }

        private void SetControllerVibrationToHighestPrioritySetting()
        {
            VibrationSetting highestPriorityEnabledVibration = new VibrationSetting(0.0f, 0.0f, true);
            for (int i = 0; i < this.vibrationSettings.Length; i++)
            {
                if (this.vibrationSettings[i].Enabled)
                {
                    highestPriorityEnabledVibration = this.vibrationSettings[i];
                }
            }

            GamePad.SetVibration(this.activePlayerIndex,
                                 highestPriorityEnabledVibration.LowFrequencyVibration,
                                 highestPriorityEnabledVibration.HighFrequencyVibration);
        }

        // Inner Classes ------------------------------------------------------------------------------- Inner Classes

        private struct VibrationSetting
        {
            public float LowFrequencyVibration;
            public float HighFrequencyVibration;
            public bool Enabled;

            public VibrationSetting(float lowFrequencyVibration, float highFrequencyVibration, bool enabled)
            {
                this.LowFrequencyVibration = lowFrequencyVibration;
                this.HighFrequencyVibration = highFrequencyVibration;
                this.Enabled = enabled;
            }
        }
    }
}
