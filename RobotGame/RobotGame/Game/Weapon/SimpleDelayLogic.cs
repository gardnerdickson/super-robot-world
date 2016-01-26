using System;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    class SimpleDelayLogic : DelayLogic
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private float delay;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public SimpleDelayLogic(float delay)
        {
            this.delay = delay;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Delay(TimerCallback timerCallback)
        {
            TimerManager.GetInstance().RegisterTimer(this.delay, timerCallback, null);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
