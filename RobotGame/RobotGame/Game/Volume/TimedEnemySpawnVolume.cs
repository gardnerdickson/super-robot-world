using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using RobotGame.Engine;

namespace RobotGame.Game.Volume
{
    class TimedEnemySpawnVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback EnemySpawnCallback;

        private bool activated;
        private bool enabled;

        private SpawnPoint template;
        private float startDelay;
        private float interval;

        // Properties ------------------------------------------------------------------------------------- Properties

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public TimedEnemySpawnVolume(Rectangle bounds, SpawnPoint template, float startDelay, float interval)
            : base(bounds)
        {
            this.EnemySpawnCallback = new TimerCallback(spawn_enemy);
            this.activated = false;
            this.enabled = true;

            this.startDelay = startDelay;
            this.interval = interval;
            this.template = template;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update()
        {
            if (!this.activated && this.enabled)
            {
                SentientActor player = (SentientActor)Player.PlayerList[0];
                if (CollisionUtil.CheckIntersectionCollision(this.bounds, player.Bounds) != Rectangle.Empty)
                {
                    this.activated = true;

                    TimerManager.GetInstance().RegisterTimer(this.startDelay, this.EnemySpawnCallback, null);
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void spawn_enemy(Object param)
        {
            if (this.enabled)
            {
                ActorDirector.GetInstance().SpawnEnemy(this.template, true);
                TimerManager.GetInstance().RegisterTimer(this.interval, this.EnemySpawnCallback, this.template);
            }
        }
    }
}
