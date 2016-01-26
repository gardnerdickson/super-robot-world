using System;

namespace RobotGame.Exceptions
{
    class RobotGameException : Exception
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public RobotGameException() { }

        public RobotGameException(string message) : base(message) { }

        public RobotGameException(string message, Exception inner) : base(message, inner) { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
