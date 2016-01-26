using System;
using RobotGame.Engine;
using RobotGame.Game.Input;
using RobotGame.Exceptions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using RobotGame.Game.Audio;

namespace RobotGame.Game
{
    class OptionMenu : InteractiveMenu
    {
        protected struct MenuOption
        {
            public string Option;
            public ConfirmationMenu ConfirmationMenu;
            public String SubMessage;

            public MenuOption(string option, String subMessage)
            {
                this.Option = option;
                this.ConfirmationMenu = null;
                this.SubMessage = subMessage;
            }
        }

        // Constants ---------------------------------------------------------------------------------------- Contants

        private const int VERTICAL_SPACING = 5;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private string optionFontAsset;
        private SpriteFont optionFont;

        private string subFontAsset;
        private SpriteFont subFont;

        protected List<MenuOption> options;
        protected int currentOption;
        protected bool confirmationMenuEnabled;

        // Properties ------------------------------------------------------------------------------------- Properties

        public int CurrentOption
        {
            get { return this.currentOption; }
            set { this.currentOption = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public OptionMenu(string textureAsset, InputDevice inputDevice, string messageFontAsset, string optionFontAsset, string subFontAsset, int screenWidth, int screenHeight)
            : base(textureAsset, inputDevice, messageFontAsset, screenWidth, screenHeight)
        {
            this.options = new List<MenuOption>();

            this.optionFontAsset = optionFontAsset;
            this.subFontAsset = subFontAsset;

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            this.confirmationMenuEnabled = false;

            this.currentOption = 0;
        }

        public OptionMenu(string textureAsset, InputDevice inputDevice, string optionFontAsset, int screenWidth, int screenHeight)
            : this(textureAsset, inputDevice, null, optionFontAsset, null, screenWidth, screenHeight)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void AddOption(string option)
        {
            this.options.Add(new MenuOption(option, null));
        }

        public void AddOption(string option, String subMessage)
        {
            this.options.Add(new MenuOption(option, subMessage));
        }

        public void RemoveOptionAt(int index)
        {
            if (index < this.options.Count)
            {
                this.options.RemoveAt(index);
            }
        }

        public void InsertConfirmationMenu(int index, ConfirmationMenu menu)
        {
            MenuOption option = this.options[index];
            option.ConfirmationMenu = menu;
            this.options[index] = option;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            if (this.optionFontAsset != null)
            {
                this.optionFont = contentManager.Load<SpriteFont>(this.optionFontAsset);
            }
            if (this.subFontAsset != null)
            {
                this.subFont = contentManager.Load<SpriteFont>(this.subFontAsset);
            }

            foreach (MenuOption menu in this.options)
            {
                if (menu.ConfirmationMenu != null)
                {
                    menu.ConfirmationMenu.LoadContent(contentManager);
                }
            }
        }

        public override int Update()
        {
            if (confirmationMenuEnabled)
            {
                if (this.options[this.currentOption].ConfirmationMenu.Update() == 0)
                {
                    confirmationMenuEnabled = false;
                    return this.currentOption;
                }
                else if (this.options[this.currentOption].ConfirmationMenu.Update() == 1)
                {
                    confirmationMenuEnabled = false;
                }
            }
            else
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
                    if (this.options[this.currentOption].ConfirmationMenu != null)
                    {
                        SoundManager.PlaySoundEffect(SoundKey.MenuIn);
                        this.confirmationMenuEnabled = true;
                    }
                    else
                    {
                        return this.currentOption;
                    }
                }
            }

            return -1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.confirmationMenuEnabled)
            {
                this.options[this.currentOption].ConfirmationMenu.Draw(spriteBatch);
            }
            else
            {
                DrawMenu(spriteBatch);
            }
        }

        public override void Reset()
        {
            this.currentOption = 0;
            foreach (MenuOption option in options)
            {
                if (option.ConfirmationMenu != null)
                {
                    option.ConfirmationMenu.Reset();
                }
            }
        }
        
        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected void DrawMenu(SpriteBatch spriteBatch)
        {
            int messageFontHeight = 0;
            if (messageFont != null)
            {
                messageFontHeight = this.messageFont.LineSpacing;
            }

            int optionFontHeight = 0;
            if (optionFont != null)
            {
                optionFontHeight = this.optionFont.LineSpacing;
            }

            int totalOptionVerticalSpace = 0;
            if (options != null && options.Count > 0)
            {
                totalOptionVerticalSpace = optionFontHeight * options.Count;
            }

            int offsetY = (int)(this.screenHeight / 2) - (int)((messageFontHeight + totalOptionVerticalSpace) / 2);
            int offsetX;
            if (messages.Count > 0)
            {
                foreach (string message in messages)
                {
                    if (this.alignment == TextAlignment.Center)
                    {
                        offsetX = (int)(this.screenWidth / 2) - (int)(this.messageFont.MeasureString(message).X / 2);
                    }
                    else
                    {
                        offsetX = LEFT_ALIGNMENT_OFFSET_X;
                    }
                    spriteBatch.DrawString(this.messageFont, message, Camera.GetInstance().Position + new Vector2(offsetX, offsetY), Color.White, 0f, Vector2.Zero, this.textScale, SpriteEffects.None, TEXT_DRAW_DEPTH);
                    offsetY += this.messageFont.LineSpacing;
                }
            }

            if (options != null)
            {
                for (int i = 0; i < options.Count; i++)
                {
                    if (this.alignment == TextAlignment.Center)
                    {
                        offsetX = (int)(this.screenWidth / 2) - (int)(this.optionFont.MeasureString(options[i].Option).X / 2);
                    }
                    else
                    {
                        offsetX = LEFT_ALIGNMENT_OFFSET_X;
                    }

                    if (i == currentOption)
                    {
                        spriteBatch.DrawString(this.optionFont, options[i].Option, Camera.GetInstance().Position + new Vector2(offsetX, offsetY), Color.Red, 0f, Vector2.Zero, this.textScale, SpriteEffects.None, TEXT_DRAW_DEPTH);
                        if (options[i].SubMessage != null)
                        {
                            float xPos = ((float)screenWidth / 2f) - (this.subFont.MeasureString(options[i].SubMessage).X / 2) * 0.75f;
                            float yPos = (float)screenHeight - this.subFont.MeasureString(options[i].SubMessage).Y;
                            spriteBatch.DrawString(this.subFont, options[i].SubMessage, Camera.GetInstance().Position + new Vector2(xPos, yPos), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, TEXT_DRAW_DEPTH);
                        }
                    }
                    else
                    {
                        spriteBatch.DrawString(this.optionFont, options[i].Option, Camera.GetInstance().Position + new Vector2(offsetX, offsetY), Color.White, 0f, Vector2.Zero, this.textScale, SpriteEffects.None, TEXT_DRAW_DEPTH);
                    }

                    offsetY += this.optionFont.LineSpacing;
                }
            }
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
