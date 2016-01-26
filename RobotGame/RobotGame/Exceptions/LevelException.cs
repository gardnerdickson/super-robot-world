using System;

namespace RobotGame.Exceptions
{
    class LevelException : RobotGameException
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public LevelException() : base() { }

        public LevelException(string message) : base(message) { }

        public LevelException(string message, Exception inner) : base(message, inner) { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
