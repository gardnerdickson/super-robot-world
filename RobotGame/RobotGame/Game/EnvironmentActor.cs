using System;
using RobotGame.Engine;
using Microsoft.Xna.Framework;

namespace RobotGame.Game
{
    class EnvironmentActor : Actor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float DRAW_DEPTH = Config.ENVIRONEMNT_ACTOR_DRAW_DEPTH;

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public EnvironmentActor(Vector2 position, SpriteKey spriteKey, float rotation)
            : base(position)
        {
            this.position = position;
            this.sprite = new Sprite(spriteKey);
            this.rotation = rotation;

            this.drawDepth = DRAW_DEPTH;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
