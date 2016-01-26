#if WINDOWS

namespace RobotGame.Preferences
{
    partial class PreferencesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.okButton = new System.Windows.Forms.Button();
            this.fullScreenCheckbox = new System.Windows.Forms.CheckBox();
            this.showPrefsOnStartupCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.inputDeviceLabel = new System.Windows.Forms.Label();
            this.inputDeviceComboBox = new System.Windows.Forms.ComboBox();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.resolutionComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(1, 73);
            this.okButton.Name = "okButton";
            this.okButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.SavePreferences);
            // 
            // fullScreenCheckbox
            // 
            this.fullScreenCheckbox.AutoSize = true;
            this.fullScreenCheckbox.Location = new System.Drawing.Point(222, 12);
            this.fullScreenCheckbox.Name = "fullScreenCheckbox";
            this.fullScreenCheckbox.Size = new System.Drawing.Size(79, 17);
            this.fullScreenCheckbox.TabIndex = 3;
            this.fullScreenCheckbox.Text = "Full Screen";
            this.fullScreenCheckbox.UseVisualStyleBackColor = true;
            this.fullScreenCheckbox.CheckedChanged += new System.EventHandler(this.OnFullScreenChange);
            // 
            // showPrefsOnStartupCheckBox
            // 
            this.showPrefsOnStartupCheckBox.AutoSize = true;
            this.showPrefsOnStartupCheckBox.Location = new System.Drawing.Point(12, 102);
            this.showPrefsOnStartupCheckBox.Name = "showPrefsOnStartupCheckBox";
            this.showPrefsOnStartupCheckBox.Size = new System.Drawing.Size(249, 17);
            this.showPrefsOnStartupCheckBox.TabIndex = 4;
            this.showPrefsOnStartupCheckBox.Text = "Always show this window when the game starts";
            this.showPrefsOnStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(83, 73);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // inputDeviceLabel
            // 
            this.inputDeviceLabel.AutoSize = true;
            this.inputDeviceLabel.Location = new System.Drawing.Point(9, 40);
            this.inputDeviceLabel.Name = "inputDeviceLabel";
            this.inputDeviceLabel.Size = new System.Drawing.Size(68, 13);
            this.inputDeviceLabel.TabIndex = 6;
            this.inputDeviceLabel.Text = "Input Device";
            // 
            // inputDeviceComboBox
            // 
            this.inputDeviceComboBox.FormattingEnabled = true;
            this.inputDeviceComboBox.Location = new System.Drawing.Point(83, 37);
            this.inputDeviceComboBox.Name = "inputDeviceComboBox";
            this.inputDeviceComboBox.Size = new System.Drawing.Size(121, 21);
            this.inputDeviceComboBox.TabIndex = 7;
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(20, 13);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(57, 13);
            this.resolutionLabel.TabIndex = 8;
            this.resolutionLabel.Text = "Resolution";
            // 
            // resolutionComboBox
            // 
            this.resolutionComboBox.FormattingEnabled = true;
            this.resolutionComboBox.Location = new System.Drawing.Point(83, 10);
            this.resolutionComboBox.Name = "resolutionComboBox";
            this.resolutionComboBox.Size = new System.Drawing.Size(121, 21);
            this.resolutionComboBox.TabIndex = 9;
            this.resolutionComboBox.SelectedIndexChanged += new System.EventHandler(this.OnResolutionSelect);
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(313, 131);
            this.ControlBox = false;
            this.Controls.Add(this.resolutionComboBox);
            this.Controls.Add(this.resolutionLabel);
            this.Controls.Add(this.inputDeviceComboBox);
            this.Controls.Add(this.inputDeviceLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.showPrefsOnStartupCheckBox);
            this.Controls.Add(this.fullScreenCheckbox);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "PreferencesForm";
            this.Text = "Robot Game - Preferences";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckBox fullScreenCheckbox;
        private System.Windows.Forms.CheckBox showPrefsOnStartupCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label inputDeviceLabel;
        private System.Windows.Forms.ComboBox inputDeviceComboBox;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.ComboBox resolutionComboBox;
    }
}

#endif