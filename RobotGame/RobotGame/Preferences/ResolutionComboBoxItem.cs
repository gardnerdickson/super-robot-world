using System;

namespace RobotGame.Preferences
{
    class ResolutionComboBoxItem
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private bool selectable;
        private int resolutionX;
        private int resolutionY;
        private String overrideDisplayText;

        // Properties ------------------------------------------------------------------------------------- Properties

        public int ResolutionX
        {
            get { return this.resolutionX; }
        }

        public int ResolutionY
        {
            get { return this.resolutionY; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public ResolutionComboBoxItem(int resolutionX, int resolutionY, bool selectable, String overrideDisplayText)
        {
            this.resolutionX = resolutionX;
            this.resolutionY = resolutionY;
            this.selectable = selectable;
            this.overrideDisplayText = overrideDisplayText;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public bool IsSelectable()
        {
            return selectable;
        }

        public override string ToString()
        {
            return (overrideDisplayText == null) ? resolutionX + " x " + resolutionY : overrideDisplayText;
        }
        
        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        // Inner Classes ------------------------------------------------------------------------------- Inner Classes

    }
}
