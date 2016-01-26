using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using System.Collections.Generic;

namespace RobotGame.Game.Volume
{
    class HealthVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback HealCallback;
        
        private int amount;
        private float interval;

        private bool callbackPending;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public HealthVolume(Rectangle bounds, int amount, float interval)
            : base(bounds)
        {
            this.amount = amount;
            this.interval = interval;

            this.callbackPending = false;

            this.HealCallback += new TimerCallback(heal);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update()
        {
            SentientActor player = (SentientActor)Player.PlayerList[0];
            if (CollisionUtil.CheckIntersectionCollision(player.Bounds, this.bounds) != Rectangle.Empty)
            {
                if (!callbackPending)
                {
                    player.Health = Math.Min(player.Health + amount, Player.PLAYER_HEALTH_MAX);
                    TimerManager.GetInstance().RegisterTimer(interval, heal, player);
                    callbackPending = true;
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        public void heal(Object param)
        {
            callbackPending = false;
            SentientActor player = (Player)param;
            if (CollisionUtil.CheckIntersectionCollision(player.Bounds, this.bounds) != Rectangle.Empty)
            {
                player.Health = Math.Min(player.Health + amount, Player.PLAYER_HEALTH_MAX);
            }
        }
    }
}
