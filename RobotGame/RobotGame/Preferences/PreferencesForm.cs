#if WINDOWS

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RobotGame.Game.Input;

namespace RobotGame.Preferences
{
    public partial class PreferencesForm : Form
    {
        private RobotGamePreferences preferences;
        private int previousResolutionIndex;

        private ResolutionComboBoxItem[] resolutionComboBoxItems = new ResolutionComboBoxItem[]
        {
            new ResolutionComboBoxItem(0, 0, false, "--- 4:3 ---"),
            new ResolutionComboBoxItem(800, 600, true, null),
            new ResolutionComboBoxItem(1024, 768, true, null),
            new ResolutionComboBoxItem(1280, 1024, true, null),
            new ResolutionComboBoxItem(1152, 864, true, null),
            new ResolutionComboBoxItem(1280, 960, true, null),
            new ResolutionComboBoxItem(1400, 1050, true, null),
            new ResolutionComboBoxItem(1600, 1200, true, null),
            new ResolutionComboBoxItem(0, 0, false, "--- 16:9 ---"),
            new ResolutionComboBoxItem(1280, 720, true, null),
            new ResolutionComboBoxItem(1365, 768, true, null),
            new ResolutionComboBoxItem(1600, 900, true, null),
            new ResolutionComboBoxItem(1920, 1080, true, null),
            new ResolutionComboBoxItem(0, 0, false, "--- 16:10 ---"),
            new ResolutionComboBoxItem(1440, 900, true, null),
            new ResolutionComboBoxItem(1680, 1050, true, null),
            new ResolutionComboBoxItem(1920, 1200, true, null),
        };


        public PreferencesForm(RobotGamePreferences preferences)
        {
            InitializeComponent();

            // Center the buttons
            this.okButton.Left = (this.ClientSize.Width / 2) - this.okButton.Width;
            this.cancelButton.Left = this.ClientSize.Width / 2;

            // Make sure the user can't type in the combo boxes
            this.resolutionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.inputDeviceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;            

            this.preferences = preferences;

            LoadPreferences(preferences);
        }

        private void LoadPreferences(RobotGamePreferences preferences)
        {
            // Load the current resolution value
            foreach (ResolutionComboBoxItem item in resolutionComboBoxItems)
            {
                this.resolutionComboBox.Items.Add(item);
                if (item.ResolutionX == preferences.GetBackBufferWidth() && item.ResolutionY == preferences.GetBackBufferHeight())
                {
                    this.resolutionComboBox.SelectedItem = item;
                }
            }

            // Load the input device combo box
            IEnumerable<InputDeviceType> inputDeviceTypes = Enum.GetValues(typeof(InputDeviceType)).Cast<InputDeviceType>();
            foreach (InputDeviceType inputDevice in inputDeviceTypes)
            {
                this.inputDeviceComboBox.Items.Add(inputDevice);
            }
            this.inputDeviceComboBox.SelectedItem = preferences.GetInputDevice();

            // Load the full screen value
            this.fullScreenCheckbox.Checked = preferences.GetFullScreen();

            // Load the show on startup value
            this.showPrefsOnStartupCheckBox.Checked = preferences.GetShowPrefsWindowOnStartup();

            this.previousResolutionIndex = this.resolutionComboBox.SelectedIndex;
        }


        private void SavePreferences(object sender, EventArgs e)
        {
            // Save the resolution value from the form
            ResolutionComboBoxItem comboBoxItem = (ResolutionComboBoxItem)this.resolutionComboBox.SelectedItem;
            this.preferences.SetBackBufferWidth(comboBoxItem.ResolutionX);
            this.preferences.SetBackBufferHeight(comboBoxItem.ResolutionY);

            // Save the input device value from the form
            InputDeviceType inputDevice = (InputDeviceType)this.inputDeviceComboBox.SelectedItem;
            this.preferences.SetInputDevice(inputDevice);

            // Save the full screen value from the form
            bool fullScreen = this.fullScreenCheckbox.Checked;
            this.preferences.SetFullScreen(fullScreen);

            // Save the show on startup value from the form
            bool showOnStartup = this.showPrefsOnStartupCheckBox.Checked;
            this.preferences.SetShowPrefsWindowOnStartup(showOnStartup);

            this.DialogResult = DialogResult.OK;
            this.Dispose(true);
        }

        private void OnResolutionSelect(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            object selectedItem = comboBox.SelectedItem;

            if (!(selectedItem != null && selectedItem is ResolutionComboBoxItem && ((ResolutionComboBoxItem)selectedItem).IsSelectable()))
            {
                comboBox.SelectedIndex = this.previousResolutionIndex;
            }
            else
            {
                this.previousResolutionIndex = comboBox.SelectedIndex;
            }
        }

        private void OnFullScreenChange(object sender, EventArgs e)
        {
            CheckBox fullScreenCheckBox = sender as CheckBox;

            this.resolutionComboBox.Enabled = !fullScreenCheckBox.Checked;
            this.resolutionLabel.Enabled = !fullScreenCheckBox.Checked;
        }
    }
}
#endif