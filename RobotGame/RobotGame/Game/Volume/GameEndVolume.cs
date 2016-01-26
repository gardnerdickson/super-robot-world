using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Volume
{
    class GameEndVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members
        
        private bool activated;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public GameEndVolume(Rectangle bounds)
            : base(bounds)
        {
            this.activated = false;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update()
        {
            if (!this.activated)
            {
                GameActor player = Player.PlayerList[0];
                if (CollisionUtil.CheckIntersectionCollision(this.bounds, player.Bounds) != Rectangle.Empty)
                {
                    this.activated = true;
                    RobotGame.RobotGameExecutionState = ExecutionState.GameEnd;
                    
                    ((Player)player).Invincible = true;
                    ((Player)player).DisableInput = true;

                    SoundManager.SoundEffectVolume = 0.1f;
                    
                    SoundManager.PlaySoundEffect(SoundKey.GameEndSong, 0.9f, 0.0f);
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

    }
}
