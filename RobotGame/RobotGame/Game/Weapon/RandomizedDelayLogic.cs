using System;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    class RandomizedDelayLogic : DelayLogic
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private float delay;
        private int randomRange;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public RandomizedDelayLogic(float delay, int randomRange)
        {
            this.delay = delay;
            this.randomRange = randomRange;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public void Delay(TimerCallback timerCallback)
        {
            int randomNumber = new Random().Next(-this.randomRange, this.randomRange + 1);
            TimerManager.GetInstance().RegisterTimer(this.delay + randomNumber, timerCallback, null);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
