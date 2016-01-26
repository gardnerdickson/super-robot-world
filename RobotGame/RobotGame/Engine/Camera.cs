using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TiledLib;
using System.Collections.Generic;
using RobotGame.Game;
using RobotGame.Exceptions;

namespace RobotGame.Engine
{
    class Camera
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private int DEFAULT_OFFSET_X = Config.CAMERA_SCREEN_X_OFFSET;
        private int DEFAULT_OFFSET_Y = Config.CAMERA_SCREEN_Y_OFFSET;

        private float TRANSITION_LERP_AMOUNT = 0.1f;
        private float TRANSITION_LERP_INCREMENT = 0.1f;
        private double TRANSITION_LERP_INCREMENT_INTERVAL = 200d;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback CameraTransitionCallback;

        private static Camera instance = null;

        private Vector2 position;

        private int mapWidth;
        private int mapHeight;
        private int screenWidth;
        private int screenHeight;
        private int boundsOffsetX;
        private int boundsOffsetY;

        private Vector2 targetPosition;
        private List<CameraBoundsInfo> lastCameraBoundsInfoList;
        private bool transitioning;
        private float transitionLerpAmount;

        private bool shake;

        // Properties ------------------------------------------------------------------------------------- Properties

        public Vector2 Position
        {
            get { return this.position; }
        }

        public bool Shake
        {
            set { this.shake = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        private Camera() { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static Camera GetInstance()
        {
            if (instance == null)
            {
                instance = new Camera();
            }
            return instance;
        }

        public void Init(Vector2 initialPosition, int mapWidth, int mapHeight, int screenWidth, int screenHeight)
        {
            this.position = initialPosition;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            this.transitioning = false;
            this.shake = false;

            this.lastCameraBoundsInfoList = new List<CameraBoundsInfo>();

            this.transitionLerpAmount = TRANSITION_LERP_AMOUNT;

            this.CameraTransitionCallback = new TimerCallback(camera_transition_callback);
        }

        public Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(-(int)this.position.X, -(int)this.position.Y, 0);
        }

        public void Update(GameActor player)
        {
            List<CameraBoundsInfo> cameraBoundsInfoList = Level.GetInstance().GetCameraBounds(player.Position);

            if (cameraBoundsInfoList.Count > 2)
            {
                throw new FatalLevelException("Player should never intersect with more than 2 camera bounds at once.");
            }

            if (StartTransition(cameraBoundsInfoList))
            {
                this.transitioning = true;
                if (!TimerManager.GetInstance().IsTimerRegistered(this.CameraTransitionCallback))
                {
                    TimerManager.GetInstance().RegisterTimer(TRANSITION_LERP_INCREMENT_INTERVAL, this.CameraTransitionCallback, null);
                }
            }

            this.boundsOffsetX = DEFAULT_OFFSET_X;
            this.boundsOffsetY = DEFAULT_OFFSET_Y;

            int bestPriority = 99;
            foreach (CameraBoundsInfo cameraBoundsInfo in cameraBoundsInfoList)
            {
                if (cameraBoundsInfo.OffsetPriority < bestPriority)
                {
                    bestPriority = cameraBoundsInfo.OffsetPriority;
                    this.boundsOffsetX = cameraBoundsInfo.OffsetX;
                    this.boundsOffsetY = cameraBoundsInfo.OffsetY;
                }
            }
            
            Rectangle effectiveCameraBounds = GetEffectiveBounds(cameraBoundsInfoList, player.Position);
            
            if (effectiveCameraBounds != Rectangle.Empty)
            {
                if (transitioning)
                {
                    this.targetPosition = CalculateCameraPosition(player.Position, effectiveCameraBounds);
                    this.position = Vector2.Lerp(this.position, this.targetPosition + player.Velocity, this.transitionLerpAmount);


                    Vector2 distance = this.targetPosition - this.position;
                    if (distance.Length() < 2)
                    {
                        transitioning = false;
                        this.position = CalculateCameraPosition(player.Position, effectiveCameraBounds);
                        this.transitionLerpAmount = TRANSITION_LERP_AMOUNT;
                    }
                }
                else
                {
                    this.position = CalculateCameraPosition(player.Position, effectiveCameraBounds);
                }
            }

            if (this.shake)
            {
                Random random = new Random();
                int x = random.Next(-5, 6);
                int y = random.Next(-5, 6);

                this.position += new Vector2(x, y);
            }
            
#if DEBUG
            //Debug.AddDebugInfo("Camera position: " + this.position, Color.Yellow);
            //Debug.AddDebugInfo("Camera offset x: " + this.boundsOffsetX, Color.Yellow);
            //Debug.AddDebugInfo("Camera offset y: " + this.boundsOffsetY, Color.Yellow);
#endif

            this.position = new Vector2((int)Math.Round(this.position.X), (int)Math.Round(this.position.Y));

            this.lastCameraBoundsInfoList = cameraBoundsInfoList;
        }

        public bool IsActorOnScreen(Actor actor)
        {
            Rectangle cameraBounds = new Rectangle((int)this.position.X, (int)this.position.Y,
                                                   this.screenWidth, this.screenHeight);

            if (CollisionUtil.CheckIntersectionCollision(cameraBounds, actor.Bounds) != Rectangle.Empty)
            {
                return true;
            }
            return false;
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void camera_transition_callback(Object param)
        {
            if (transitioning)
            {
                this.transitionLerpAmount += TRANSITION_LERP_INCREMENT;
                if (this.transitionLerpAmount >= 1)
                {
                    this.transitionLerpAmount = 1;
                }
                TimerManager.GetInstance().RegisterTimer(TRANSITION_LERP_INCREMENT_INTERVAL, this.CameraTransitionCallback, null);
            }
        }

        private Vector2 CalculateCameraPosition(Vector2 playerPosition, Rectangle cameraBounds)
        {
            Vector2 cameraMin;
            Vector2 cameraMax;
            Vector2 cameraPosition;

            // Lock the camera's position on the playerPosition parameter
            cameraPosition = new Vector2(playerPosition.X - (this.screenWidth / 2),
                                         playerPosition.Y - (this.screenHeight / 2));
            cameraPosition += new Vector2(this.boundsOffsetX, this.boundsOffsetY);

            // Clamp the camera to the camera bounds area that we are in
            cameraMin = new Vector2(cameraBounds.Left, cameraBounds.Top);
            cameraMax = new Vector2(cameraBounds.Right - this.screenWidth, cameraBounds.Bottom - this.screenHeight);
            cameraPosition = Vector2.Clamp(cameraPosition, cameraMin, cameraMax);

            // Clamp the camera's position so it doesn't show the void outside the map
            cameraMin = new Vector2(Level.GetInstance().TileWidth, Level.GetInstance().TileHeight);
            cameraMax = new Vector2(mapWidth - this.screenWidth - Level.GetInstance().TileWidth, mapHeight - this.screenHeight);
            cameraPosition = Vector2.Clamp(cameraPosition, cameraMin, cameraMax);

            return cameraPosition;
        }

        private Rectangle GetEffectiveBounds(List<CameraBoundsInfo> cameraBoundsInfoList, Vector2 playerPosition)
        {
            int cameraTop = -99;
            int cameraBottom = -99;
            int cameraLeft = -99;
            int cameraRight = -99;

            foreach (CameraBoundsInfo cameraBoundsInfo in cameraBoundsInfoList)
            {
                if (cameraTop == -99 || cameraBoundsInfo.Bounds.Top < cameraTop)
                {
                    cameraTop = cameraBoundsInfo.Bounds.Top;
                }
                if (cameraBottom == -99 || cameraBoundsInfo.Bounds.Bottom > cameraBottom)
                {
                    cameraBottom = cameraBoundsInfo.Bounds.Bottom;
                }
                if (cameraLeft == -99 || cameraBoundsInfo.Bounds.Left < cameraLeft)
                {
                    cameraLeft = cameraBoundsInfo.Bounds.Left;
                }
                if (cameraRight == -99 || cameraBoundsInfo.Bounds.Right > cameraRight)
                {
                    cameraRight = cameraBoundsInfo.Bounds.Right;
                }
            }
            
            if (cameraTop == -99 || cameraBottom == -99 || cameraLeft == -99 || cameraRight == -99)
            {
                return Rectangle.Empty;
            }

            return new Rectangle(cameraLeft, cameraTop, cameraRight - cameraLeft, cameraBottom - cameraTop);
        }

        private bool StartTransition(List<CameraBoundsInfo> cameraBoundsInfoList)
        {
            if (this.lastCameraBoundsInfoList.Count > 0 && !cameraBoundsInfoList.SequenceEqual<CameraBoundsInfo>(this.lastCameraBoundsInfoList))
            {
                foreach (CameraBoundsInfo cameraBoundsInfo in this.lastCameraBoundsInfoList)
                {
                    if (cameraBoundsInfo.Transition)
                    {
                        return true;
                    }
                }
                foreach (CameraBoundsInfo cameraBoundsInfo in cameraBoundsInfoList)
                {
                    if (cameraBoundsInfo.Transition)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
