#if WINDOWS


using System;
using System.Xml;
using RobotGame.Game.Input;

namespace RobotGame.Preferences
{
    class XmlPreferences : RobotGamePreferences
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const string SHOW_PREFS_ON_STARTUP = "preferences/showPrefsWindowOnStartup";
        private const string BACK_BUFFER_WIDTH = "preferences/backBufferWidth";
        private const string BACK_BUFFER_HEIGHT = "preferences/backBufferHeight";
        private const string FULL_SCREEN = "preferences/fullScreen";
        private const string INPUT_DEVICE = "preferences/inputDevice";

        // Data Members --------------------------------------------------------------------------------- Data Members

        private XmlDocument prefsXml;
        private string filename;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public XmlPreferences()
        {
            prefsXml = new XmlDocument();
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Load(string filename)
        {
            this.filename = filename;
            prefsXml.Load(filename);
        }

        public bool GetShowPrefsWindowOnStartup()
        {
            XmlNode node = prefsXml.SelectSingleNode(SHOW_PREFS_ON_STARTUP);
            return bool.Parse(node.InnerText);
        }

        public int GetBackBufferWidth()
        {
            XmlNode node = prefsXml.SelectSingleNode(BACK_BUFFER_WIDTH);
            return int.Parse(node.InnerText);
        }

        public int GetBackBufferHeight()
        {
            XmlNode node = prefsXml.SelectSingleNode(BACK_BUFFER_HEIGHT);
            return int.Parse(node.InnerText);
        }

        public bool GetFullScreen()
        {
            XmlNode node = prefsXml.SelectSingleNode(FULL_SCREEN);
            return bool.Parse(node.InnerText);
        }

        public InputDeviceType GetInputDevice()
        {
            XmlNode node = prefsXml.SelectSingleNode(INPUT_DEVICE);
            return (InputDeviceType)Enum.Parse(typeof(InputDeviceType), node.InnerText);
        }

        public void SetShowPrefsWindowOnStartup(bool showPrefsWindowOnStartup)
        {
            XmlNode node = prefsXml.SelectSingleNode(SHOW_PREFS_ON_STARTUP);
            node.InnerText = showPrefsWindowOnStartup.ToString();
        }

        public void SetBackBufferWidth(int backBufferWidth)
        {
            XmlNode node = prefsXml.SelectSingleNode(BACK_BUFFER_WIDTH);
            node.InnerText = backBufferWidth.ToString();
        }

        public void SetBackBufferHeight(int backBufferHeight)
        {
            XmlNode node = prefsXml.SelectSingleNode(BACK_BUFFER_HEIGHT);
            node.InnerText = backBufferHeight.ToString();
        }

        public void SetFullScreen(bool fullScreen)
        {
            XmlNode node = prefsXml.SelectSingleNode(FULL_SCREEN);
            node.InnerText = fullScreen.ToString();
        }

        public void SetInputDevice(InputDeviceType inputDevice)
        {
            XmlNode node = prefsXml.SelectSingleNode(INPUT_DEVICE);
            node.InnerText = Enum.GetName(typeof(InputDeviceType), inputDevice);
        }

        public void Save()
        {
            this.prefsXml.Save(this.filename);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}

#endif

