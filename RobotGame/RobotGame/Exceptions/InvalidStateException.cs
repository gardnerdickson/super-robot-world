using System;

namespace RobotGame.Exceptions
{
    class InvalidStateException : RobotGameException
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public InvalidStateException() : base() { }

        public InvalidStateException(string message) : base(message) { }

        public InvalidStateException(string message, Exception inner) : base(message, inner) { }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
