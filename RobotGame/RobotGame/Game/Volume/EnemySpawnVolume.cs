using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using System.Collections.Generic;

namespace RobotGame.Game.Volume
{
    class EnemySpawnVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected bool activated;
        protected List<SpawnPoint> enemySpawnPoints;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public EnemySpawnVolume(Rectangle bounds, List<SpawnPoint> enemySpawnPoints)
            : base(bounds)
        {
            this.enemySpawnPoints = enemySpawnPoints;
            this.activated = false;
        }
        
        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update()
        {
            if (!this.activated)
            {
                SentientActor player = (SentientActor)Player.PlayerList[0];
                if (CollisionUtil.CheckIntersectionCollision(player.Bounds, this.bounds) != Rectangle.Empty)
                {
                    this.activated = true;
                    foreach (SpawnPoint enemySpawnPoint in this.enemySpawnPoints)
                    {
                        ActorDirector.GetInstance().SpawnEnemy(enemySpawnPoint);
                    }
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
