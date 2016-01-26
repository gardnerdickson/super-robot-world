using System;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    class SequentialDelayLogic : DelayLogic
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private int index = 0;
        private double[] delays;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public SequentialDelayLogic(double[] delays)
        {
            this.delays = delays;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Delay(TimerCallback timerCallback)
        {
            if (index >= delays.Length)
            {
                index = 0;
            }

            TimerManager.GetInstance().RegisterTimer(delays[index++], timerCallback, null);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
