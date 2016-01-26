using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RobotGame.Engine
{
    class TimerManager
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static TimerManager instance = null;
        private List<Timer> timerList;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        private TimerManager()
        {
            timerList = new List<Timer>();
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static TimerManager GetInstance()
        {
            if (instance == null)
            {
                instance = new TimerManager();
            }
            return instance;
        }

        public void RegisterTimer(double time, TimerCallback callback, Object param)
        {
            timerList.Add(new Timer(time, callback, param));
        }

        public bool UnregisterTimer(TimerCallback callback)
        {
            foreach (Timer timer in this.timerList)
            {
                if (Object.ReferenceEquals(timer.Callback, callback))
                {
                    return timerList.Remove(timer);
                }
            }
            return false;
        }

        public bool IsTimerRegistered(TimerCallback callback)
        {
            foreach (Timer timer in this.timerList)
            {
                if (Object.ReferenceEquals(timer.Callback, callback))
                {
                    return true;
                }
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = timerList.Count - 1; i >= 0; i--)
            {
                Timer timer = timerList[i];
                timer.Time -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer.Time < 0f)
                {
                    timer.Callback(timer.Param);
                    timerList.Remove(timer);
                }
            }

#if DEBUG
            //Debug.AddDebugInfo("Number of active timers: " + timerList.Count);
#endif
        }

        public double GetRemainingTime(TimerCallback callback)
        {
            foreach (Timer timer in timerList)
            {
                if (Object.ReferenceEquals(timer.Callback, callback))
                {
                    return timer.Time;
                }
            }
            return -1.0d;
        }

        public void RemoveAllTimers()
        {
            this.timerList.Clear();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        // Inner Classes ------------------------------------------------------------------------------- Inner Classes

        private class Timer
        {
            public double Time;
            public TimerCallback Callback;
            public Object Param;

            public Timer(double time, TimerCallback Callback, Object param)
            {
                this.Time = time;
                this.Callback = Callback;
                this.Param = param;
            }
        }
    }
}
