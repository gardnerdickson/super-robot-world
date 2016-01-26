using System;
using System.Collections.Generic;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    public interface DelayLogic
    {
        void Delay(TimerCallback timerCallback);
    }
}
