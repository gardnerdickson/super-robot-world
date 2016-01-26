using System;
using Microsoft.Xna.Framework.Content;

namespace RobotGame.Engine
{
    public class RobotGameContentManager : ContentManager
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public RobotGameContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public RobotGameContentManager(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public T Load<T>(string assetName, bool useCache)
        {
            if (useCache)
            {
                return this.Load<T>(assetName);
            }
            return base.ReadAsset<T>(assetName, null);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
