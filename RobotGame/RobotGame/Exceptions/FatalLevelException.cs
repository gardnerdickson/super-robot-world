using System;

namespace RobotGame.Exceptions
{
    class FatalLevelException : LevelException
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public FatalLevelException() : base() { }

        public FatalLevelException(String message) : base(message) { }

        public FatalLevelException(String message, Exception inner) : base(message, inner) { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
