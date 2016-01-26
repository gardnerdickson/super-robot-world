using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;
using RobotGame.Game.Audio;

namespace RobotGame.Game.Volume
{
    class LevelEndVolume : AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants
        
        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public LevelEndVolume(Rectangle bounds)
            : base(bounds)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update()
        {
            GameActor player = Player.PlayerList[0];

            if (CollisionUtil.CheckIntersectionCollision(player.Bounds, this.bounds) != Rectangle.Empty)
            {
                RobotGame.RobotGameExecutionState = ExecutionState.LevelEnd;
                SoundManager.StopMusic();
                SoundManager.StopAllLoopingSoundEffects();
                SoundManager.PlaySoundEffect(SoundKey.LevelEnd);
                RobotGame.RobotGameInputDevice.DisableJetpackFeedback();
                RobotGame.RobotGameInputDevice.DisableLandFeedback();
                RobotGame.RobotGameInputDevice.DisableTakeDamageFeedback();
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
