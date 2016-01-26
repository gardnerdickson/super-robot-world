using System;
using Microsoft.Xna.Framework;
using System.Text;

namespace RobotGame.Game
{
    class PlayTimer
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private double totalPlayTime;

        // Properties ------------------------------------------------------------------------------------- Properties

        public TimeSpan TotalPlayTime
        {
            get { return TimeSpan.FromMilliseconds(this.totalPlayTime); }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public PlayTimer()
            : this(0d)
        { }

        public PlayTimer(double startTime)
        {
            this.totalPlayTime = startTime;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Update(GameTime gameTime)
        {
            this.totalPlayTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public String GetPrettyString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            TimeSpan time = TotalPlayTime;

            if (time.Hours > 0)
            {
                if (time.Hours < 10)
                {
                    stringBuilder.Append("0");
                }
                stringBuilder.AppendFormat("{0}:", time.Hours);
            }
            else
            {
                stringBuilder.Append("00:");
            }

            if (time.Minutes > 0)
            {
                if (time.Minutes < 10)
                {
                    stringBuilder.AppendFormat("0");
                }
                stringBuilder.AppendFormat("{0}:", time.Minutes);
            }
            else
            {
                stringBuilder.Append("00:");
            }

            if (time.Seconds > 0)
            {
                if (time.Seconds < 10)
                {
                    stringBuilder.Append("0");
                }
                stringBuilder.AppendFormat("{0}.", time.Seconds);
            }
            else
            {
                stringBuilder.Append("00.");
            }

            if (time.Milliseconds > 0)
            {
                if (time.Milliseconds < 100)
                {
                    stringBuilder.Append("0");
                }
                if (time.Milliseconds < 10)
                {
                    stringBuilder.Append("0");
                }
                stringBuilder.Append(time.Milliseconds);
            }
            else
            {
                stringBuilder.Append("000");
            }

            return stringBuilder.ToString();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
