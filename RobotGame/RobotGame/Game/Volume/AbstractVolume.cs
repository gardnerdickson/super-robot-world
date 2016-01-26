using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using RobotGame.Engine;

namespace RobotGame.Game.Volume
{
    abstract class AbstractVolume
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        public static List<AbstractVolume> volumeList = new List<AbstractVolume>();

        protected Rectangle bounds;

        // Properties ------------------------------------------------------------------------------------- Properties

        public static List<AbstractVolume> VolumeList
        {
            get { return volumeList; }
        }

        public Rectangle Bounds
        {
            get { return this.bounds; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public AbstractVolume(Rectangle bounds)
        {
            this.bounds = bounds;
            volumeList.Add(this);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public abstract void Update();

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
