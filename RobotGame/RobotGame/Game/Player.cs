using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

using RobotGame.Engine;
using RobotGame.Game.Enemy;
using RobotGame.Game.Powerup;
using RobotGame.Game.Weapon;
using RobotGame.Game.Audio;
using RobotGame.Game.Volume;


namespace RobotGame.Game
{
    class Player : SentientActor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants
        
        public const int PLAYER_HEALTH_MAX = Config.PLAYER_HEALTH_MAX;
        public const float JETPACK_FUEL_MAX = Config.JETPACK_FUEL_MAX;

        private const float PLAYER_MASS = Config.PLAYER_MASS;
        private const float GRAVITY_FORCE = Config.PLAYER_GRAVITY_FORCE;
        private const float PRIMARY_FIRE_DELAY = Config.PLAYER_PRIMARY_FIRE_DELAY;
        private const float PROJECTILE_BULLET_SPEED = Config.PLAYER_BULLET_SPEED;
        private const float PRIMARY_PROJECTILE_SPAWN_OFFSET = Config.PLAYER_PRIMARY_PROJECTILE_SPAWN_OFFSET;
        private const float SECONDARY_PROJECTILE_SPAWN_OFFSET = Config.PLAYER_SECONDARY_PROJECTILE_SPAWN_OFFSET;
        private const float INPUT_FORCE_MULTIPLIER = Config.INPUT_FORCE_MULTIPLIER;
        private const float PLAYER_JUMP_VELOCITY = Config.PLAYER_JUMP_VELOCITY;
        private const float PLAYER_JUMP_HOLD_FORCE = Config.PLAYER_JUMP_HOLD_FORCE;
        private const float PLAYER_JUMP_HOLD_FORCE_VELOCITY_THRESHOLD = Config.PLAYER_JUMP_HOLD_FORCE_VELOCITY_THRESHOLD;
        private const float MAX_INPUT_FORCE = Config.MAX_INPUT_FORCE;
        private const float JETPACK_FORCE = Config.JETPACK_FORCE;
        private const float MAX_JETPACK_VERTICAL_SPEED = Config.JETPACK_MAX_VERTICAL_SPEED;
        private const float JETPACK_Y_OFFSET = Config.JETPACK_Y_OFFSET;
        private const float JETPACK_PARTICLE_SPAWN_FREQUENCY = Config.JETPACK_PARTICLE_SPAWN_FREQUENCY;
        private const float JETPACK_PARTICLE_ROTATION = Config.JETPACK_PARTICLE_ROTATION;
        private const float JETPACK_PARTICLE_SCALE_START = Config.JETPACK_PARTICLE_SCALE_START;
        private const float JETPACK_PARTICLE_SCALE_END = Config.JETPACK_PARTICLE_SCALE_END;
        private const float AIR_DRAG_MULTIPLIER = Config.AIR_DRAG_FACTOR;

        private const double TAKE_DAMAGE_VIBRATION_DURATION = Config.TAKE_DAMAGE_VIBRATION_DURATION;
        private const double LAND_VIBRATION_DURATION = Config.LAND_VIBRATION_DURATION;
        private const float LAND_VIBRATION_VELOCITY_THRESHOLD = Config.LAND_VIBRATION_VELOCITY_THRESHOLD;

        private const int PROJECTILE_BULLET_DAMAGE = Config.PLAYER_BULLET_DAMAGE;

        private const int ENEMY_COLLISION_DAMAGE = Config.ENEMY_COLLISION_DAMAGE;

        private const float JETPACK_FUEL_DEPLETION_RATE = Config.JETPACK_FUEL_DEPLETION_RATE;
        private const float JETPACK_FUEL_REPLENISH_RATE = Config.JETPACK_FUEL_REPLENISH_RATE;        

        private const float PLAYER_DRAW_DEPTH = Config.PLAYER_DRAW_DEPTH;
        private const float NOZZLE_DRAW_DEPTH = Config.NOZZLE_DRAW_DEPTH;
        private const float JETPACK_FLAME_DRAW_DEPTH = Config.JETPACK_FLAME_DRAW_DEPTH;

        private const double INVINCIBILTY_DURATION = Config.INVINCIBILITY_DURATION;

        private const double DOUBLE_DAMAGE_TEXTURE_FREQUENCY = Config.DOUBLE_DAMAGE_TEXTURE_FLICKER_FREQUENCY;

        private Vector2 JETPACK_FLAME_OFFSET = new Vector2(-11f, 35f);

        // Data Members --------------------------------------------------------------------------------- Data Members
        
        private TimerCallback InvincibleCallback;
        private TimerCallback TakeDamageVibrationCallback;
        private TimerCallback LandVibrationCallback;
        private TimerCallback JetpackVibrationCallback;
        private TimerCallback DoubleDamageCallback;
        private TimerCallback PlayerDeathJingleCallback;

        private static List<GameActor> playerList = new List<GameActor>();

        private int lives;
        private int points;

        private AbstractWeapon primaryWeapon;
        private WeaponInventory secondaryWeaponInventory;
        private bool primaryFireActionPending;

        private Sprite nozzleSprite;
        private float nozzleRotation;

        private Sprite jetpackFlame;

        private ParticleEmitter jetPackParticleEmitter;
        private bool jetPackOn;
        private bool jetPackEnabled;
        private float jetPackFuel;


        private bool invincible;

        private bool showDoubleDamage;

        private Vector2 lastPosition;

        private Vector2 jetpackFlameOffset;

        private bool disableInput;
        
        // Properties ------------------------------------------------------------------------------------- Properties

        public static List<GameActor> PlayerList
        {
            get { return playerList; }
        }

        public int Lives
        {
            get { return this.lives; }
            set { this.lives = value; }
        }

        public int Points
        {
            get { return this.points; }
            set { this.points = value; }
        }

        public bool JetPackEnabled
        {
            get { return this.jetPackEnabled; }
            set
            {
                this.jetPackEnabled = value;
                if (value)
                {
                    this.sprite = new Sprite(SpriteKey.PlayerJetpack);
                }
                else
                {
                    this.sprite = new Sprite(SpriteKey.Player);
                }
            }
        }

        public float JetPackFuel
        {
            get { return this.jetPackFuel; }
            set { this.jetPackFuel = value; }
        }

        public bool Invincible
        {
            get { return this.invincible; }
            set { this.invincible = value; }
        }

        public bool DoubleDamageEnabled
        {
            get { return this.showDoubleDamage; }
            set
            {
                if (value)
                {
                    StartDoubleDamageTextureFlicker();
                    ((BulletFactory)this.primaryWeapon.ProjectileFactory).Damage *= 2;
                }
                else
                {
                    StopDoubleDamageTextureFlicker();
                    ((BulletFactory)this.primaryWeapon.ProjectileFactory).Damage /= 2;
                }
                this.showDoubleDamage = value;
            }
        }

        public AbstractWeapon PrimaryWeapon
        {
            get { return this.primaryWeapon; }
        }

        public WeaponInventory SecondaryWeaponInventory
        {
            get { return this.secondaryWeaponInventory; }
            set { this.secondaryWeaponInventory = value; }
        }

        public bool DisableInput
        {
            set { this.disableInput = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public Player(Vector2 position, PhysicsMode physicsMode)
            : base(position, Vector2.Zero, physicsMode)
        {
            this.sprite = new Sprite(SpriteKey.Player);
            this.nozzleSprite = new Sprite(SpriteKey.Nozzle);
            this.jetpackFlame = new Sprite(SpriteKey.PlayerJetpackFlame);
            this.drawDepth = PLAYER_DRAW_DEPTH;
            this.mass = PLAYER_MASS;
            this.jetPackFuel = 0;
            this.jetPackEnabled = false;
            this.jetPackParticleEmitter = new ParticleEmitter(JETPACK_PARTICLE_SPAWN_FREQUENCY, new Vector2(this.position.X, this.position.Y + JETPACK_Y_OFFSET),
                                                              Vector2.Zero, 0f, 0f, Vector2.Zero, SpriteKey.JetpackExhaust, JETPACK_PARTICLE_ROTATION, 220, 0, JETPACK_PARTICLE_SCALE_START, JETPACK_PARTICLE_SCALE_END);

            this.invincible = false;

            ProjectileFactory bulletFactory = new BulletFactory(PROJECTILE_BULLET_DAMAGE, SpriteKey.Bullet, SpriteKey.BulletCollisionParticle, null, ProjectileSource.Player);
            SimpleFireLogic simpleFireLogic = new SimpleFireLogic();
            this.primaryWeapon = new ProjectileLauncher(bulletFactory, new SimpleDelayLogic(PRIMARY_FIRE_DELAY), simpleFireLogic, PROJECTILE_BULLET_SPEED, SoundKey.PrimaryFire, SoundKey.FireNoAmmo);
            this.primaryFireActionPending = false;

            this.InvincibleCallback = new TimerCallback(invincibility_disable);
            this.TakeDamageVibrationCallback = new TimerCallback(take_damage_vibration_disable);
            this.LandVibrationCallback = new TimerCallback(land_vibration_disable);
            this.JetpackVibrationCallback = new TimerCallback(jetpack_vibration_disable);
            this.DoubleDamageCallback = new TimerCallback(toggle_show_double_damage);
            this.PlayerDeathJingleCallback = new TimerCallback(player_death_jingle_callback);

            playerList.Add(this);
            
            this.physicsMode = PhysicsMode.Gravity;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void TakeDamage(int damage)
        {
            if (damage > 0)
            {
                if (!this.invincible)
                {
                    // RUMBLE!!!!
                    RobotGame.RobotGameInputDevice.EnableTakeDamageFeedback();
                    TimerManager.GetInstance().RegisterTimer(TAKE_DAMAGE_VIBRATION_DURATION, this.TakeDamageVibrationCallback, null);
                    
                    this.health -= damage;
                    if (this.health <= 0)
                    {
                        RobotGame.GameState.PlayerDeaths++;

                        Explode(SpriteKey.PlayerExplosion, SpriteSheetFactory.PLAYER_EXPLOSION_TILES_X, SpriteSheetFactory.PLAYER_EXPLOSION_TILES_Y);

                        SoundManager.StopMusic();
                        SoundManager.PlaySoundEffect(SoundKey.PlayerDeath);
                        SoundManager.StopSoundEffect(SoundKey.JetpackOn);
                        TimerManager.GetInstance().RegisterTimer(500, this.PlayerDeathJingleCallback, null);

                        ExecutionState newState = ExecutionState.Died;

                        Actor.ActorList.Remove(this);
                        this.visible = false;
                        this.invincible = true;
                        RobotGame.RobotGameInputDevice.DisableJetpackFeedback();
                        this.jetPackParticleEmitter.Stop();


                        if (RobotGame.GameMode == GameMode.NinteenEightyFive)
                        {
                            this.lives--;
                            if (this.lives < 0)
                            {
                                this.Remove();
                                newState = ExecutionState.GameOver;
                            }
                        }
                        
                        RobotGame.RobotGameExecutionState = newState;
                    }
                    else
                    {
                        InvincibilityEnable();
                        SoundManager.PlayRandomSoundEffect(SoundKey.PlayerTakeDamage1, SoundKey.PlayerTakeDamage2, SoundKey.PlayerTakeDamage3, SoundKey.PlayerTakeDamage4, SoundKey.PlayerTakeDamage5);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.lastPosition = this.position;

            Vector2 inputForce = Vector2.Zero;
            
            // Handle user movement input
            Vector2 movementForce = Vector2.Zero;
            Vector2 jetpackForce = Vector2.Zero;
            Vector2 jumpForce = Vector2.Zero;

            if (!this.disableInput)
            {
                ProcessMovementInput(ref movementForce, ref jumpForce, ref jetpackForce);
            }
            
            inputForce += movementForce + jetpackForce + jumpForce;
            
            if (this.force.Length() > 0)
            {
                DisableJetpack();
            }

            this.force.Y += inputForce.Y + GRAVITY_FORCE;

            float acceleration = CalculateAccelertion(inputForce);
            this.velocity.X += acceleration;

            this.velocity.Y += this.force.Y / this.mass;
            this.velocity.X += this.force.X / this.mass;
            
            // Clamp the velocity
            if (this.jetPackOn)
            {
                this.velocity.Y = MathHelper.Clamp(this.velocity.Y, -MAX_JETPACK_VERTICAL_SPEED, 10f);
            }
            else
            {
                this.velocity.Y = MathHelper.Clamp(this.velocity.Y, -MAX_VERTICAL_SPEED, MAX_VERTICAL_SPEED);
            }

            // Filter out small movements
            if (Math.Abs(this.velocity.X) < 0.5f)
            {
                this.velocity.X = 0.0f;
            }

            this.Move(this.velocity);

            // Check for collisions with other actors
            CheckActorCollisions();

            // Make sure the player doesn't go off the map
            CheckMapBoundsAndCorrect();

            // Correct player's position based on map collisions
            EnvironmentState previousEnvironmentState = this.environmentState;
            float velocityWhenHittingGround = Math.Abs(this.velocity.Y);

            HandleMapCollisions();

            HandleMoverCollisions();

            if (previousEnvironmentState == EnvironmentState.InAir && this.environmentState == EnvironmentState.OnGround)
            {
                // Vibrate device if velocity was high enough when hitting the ground
                if (Math.Abs(velocityWhenHittingGround) >= LAND_VIBRATION_VELOCITY_THRESHOLD)
                {
                    RobotGame.RobotGameInputDevice.EnableLandFeedback();
                    TimerManager.GetInstance().RegisterTimer(LAND_VIBRATION_DURATION, this.LandVibrationCallback, null);

                    SoundManager.PlaySoundEffect(SoundKey.PlayerLand);
                }
            }

            // Handle weapon related input
            if (!this.disableInput)
            {
                ProcessWeaponInput();
            }
            
            HandleJetPackLogic();

#if DEBUG
            //Engine.Debug.AddDebugInfo("Player points: " + this.points, Color.DarkOrange);
            //Engine.Debug.AddDebugInfo("Player lives: " + this.lives);
            //Engine.Debug.AddDebugInfo("Player position: " + this.position, Color.CornflowerBlue);
            //Engine.Debug.AddDebugInfo("Player ammo: " + this.secondaryWeaponInventory.Ammo, Color.CornflowerBlue);
            //Engine.Debug.AddDebugInfo("Player velocity: " + this.velocity, Color.CornflowerBlue);
            //Engine.Debug.AddDebugInfo("Player environment state: " + this.environmentState, Color.CornflowerBlue);
            //Engine.Debug.AddDebugInfo("Player visible: " + this.visible, Color.CornflowerBlue);
            //Engine.Debug.AddDebugInfo("Player invincible: " + this.invincible, Color.CornflowerBlue);
            //Engine.Debug.AddDebugInfo("Primary fire action pending: " + this.primaryFireActionPending, Color.IndianRed);
            //Engine.Debug.AddDebugInfo("Double damage: " + this.showDoubleDamage);
#endif

            // Kill horizontal momentum if we hit a wall
            if (this.lastPosition.X == this.position.X)
            {
                this.velocity.X = 0.0f;
            }
            this.lastPosition = this.position;

            // Stop the sprite animation if we are not moving horizontally
            if (this.velocity.X == 0.0f || this.environmentState == EnvironmentState.InAir)
            {
                this.sprite.Animate = false;
            }
            else
            {
                this.sprite.Animate = true;
            }

            if (this.jetPackOn)
            {
                this.jetpackFlame.Update(gameTime);
            }

            DirectionOrientateHorizontal();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (this.visible)
            {
                Color tint = Color.White;
                if (this.showDoubleDamage)
                {
                    tint = Color.Red;
                }
                
                this.nozzleSprite.Draw(spriteBatch, this.position, this.nozzleRotation, NOZZLE_DRAW_DEPTH, SpriteEffects.None, tint);

                if (this.jetPackOn)
                {
                    if (this.flip == SpriteEffects.FlipHorizontally)
                    {
                        jetpackFlameOffset = new Vector2(JETPACK_FLAME_OFFSET.X * -1f, JETPACK_FLAME_OFFSET.Y);
                    }
                    else
                    {
                        jetpackFlameOffset = JETPACK_FLAME_OFFSET;
                    }
                    this.jetpackFlame.Draw(spriteBatch, this.position + jetpackFlameOffset, 0f, JETPACK_FLAME_DRAW_DEPTH, SpriteEffects.None, Color.White);
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void DirectionOrientateHorizontal()
        {
            Vector2 nozzleDirection = RobotGame.RobotGameInputDevice.GetNozzleDirection(this.position);
            if (nozzleDirection.X < 0f && this.velocity.X < 0f)
            {
                this.flip = SpriteEffects.FlipHorizontally;
                this.sprite.ReverseFrameIteration = false;
            }
            else if (nozzleDirection.X < 0f && this.velocity.X > 0f)
            {
                this.flip = SpriteEffects.FlipHorizontally;
                this.sprite.ReverseFrameIteration = true;
            }
            else if (nozzleDirection.X > 0f && this.velocity.X < 0f)
            {
                this.flip = SpriteEffects.None;
                this.sprite.ReverseFrameIteration = true;
            }
            else if (nozzleDirection.X > 0f && this.velocity.X > 0f)
            {
                this.flip = SpriteEffects.None;
                this.sprite.ReverseFrameIteration = false;
            }
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void invincibility_disable(Object param)
        {
            this.invincible = false;
            this.Flicker = false;
        }

        private void take_damage_vibration_disable(Object param)
        {
            RobotGame.RobotGameInputDevice.DisableTakeDamageFeedback();
        }

        private void land_vibration_disable(Object param)
        {
            RobotGame.RobotGameInputDevice.DisableLandFeedback();
        }

        private void jetpack_vibration_disable(Object param)
        {
            RobotGame.RobotGameInputDevice.DisableJetpackFeedback();
        }

        private void toggle_show_double_damage(Object param)
        {
            this.showDoubleDamage = !this.showDoubleDamage;
            TimerManager.GetInstance().RegisterTimer(DOUBLE_DAMAGE_TEXTURE_FREQUENCY, this.DoubleDamageCallback, null);
        }

        private void player_death_jingle_callback(Object param)
        {
            SoundManager.PlaySoundEffect(SoundKey.PlayerDeathJingle);
        }

        private void InvincibilityEnable()
        {
            this.Flicker = true;
            this.invincible = true;
            TimerManager.GetInstance().RegisterTimer(INVINCIBILTY_DURATION, this.InvincibleCallback, null);
        }

        private void ProcessMovementInput(ref Vector2 movementForce, ref Vector2 jumpForce, ref Vector2 jetpackForce)
        {
            //Vector2 inputForce = Vector2.Zero;
            Vector2 direction = Vector2.Zero;
            
            // Process movement related input
            float horizontalMovement = RobotGame.RobotGameInputDevice.GetHorizontalMove();
            movementForce.X += horizontalMovement * INPUT_FORCE_MULTIPLIER;
            

            if (this.flip == SpriteEffects.FlipHorizontally)
            {
                this.jetPackParticleEmitter.Position = new Vector2(this.position.X + JETPACK_FLAME_OFFSET.X * -1, this.position.Y + JETPACK_FLAME_OFFSET.Y + 5f);
            }
            else
            {
                this.jetPackParticleEmitter.Position = new Vector2(this.position.X + JETPACK_FLAME_OFFSET.X, this.position.Y + JETPACK_FLAME_OFFSET.Y + 5f);
            }

            if (this.jetPackOn)
            {
                if (RobotGame.RobotGameInputDevice.GetJump(true) && this.jetPackFuel > 0f)
                {
                    jetpackForce += new Vector2(0f, -258.75f);

                    if (!this.jetPackParticleEmitter.IsEnabled)
                    {
                        this.jetPackParticleEmitter.Start();
                        RobotGame.RobotGameInputDevice.EnableJetpackFeedback();
                        SoundManager.PlayAndLoopSoundEffect(SoundKey.JetpackOn);
                    }
                }
                else
                {
                    DisableJetpack();
                }
            }
            else
            {
                // Process jumping related input
                if (RobotGame.RobotGameInputDevice.GetJump(false))
                {
                    if (environmentState == EnvironmentState.OnGround)
                    {
                        jumpForce += new Vector2(0f, -1800f);

                        this.environmentState = EnvironmentState.InAir;
                    }
                    else
                    {
                        if (this.JetPackEnabled)
                        {
                            // Activate jet pack
                            jetpackForce += new Vector2(0f, -258.75f);
                            this.jetPackOn = true;
                        }
                    }
                }
                else if (RobotGame.RobotGameInputDevice.GetJump(true))
                {
                    if (environmentState == EnvironmentState.InAir && this.velocity.Y < PLAYER_JUMP_HOLD_FORCE_VELOCITY_THRESHOLD)
                    {
                        jumpForce.Y += -112.5f;
                    }
                }
            }
        }

        private void ProcessWeaponInput()
        {
            // Process weapon related input
            Vector2 nozzleDirection = RobotGame.RobotGameInputDevice.GetNozzleDirection(this.position);
            this.nozzleRotation = (float)Math.Atan2(nozzleDirection.Y, nozzleDirection.X);

            if (RobotGame.RobotGameInputDevice.GetPrimaryFire(false) || this.primaryFireActionPending)
            {
                this.primaryWeapon.Position = this.position + (nozzleDirection * PRIMARY_PROJECTILE_SPAWN_OFFSET);
                this.primaryWeapon.Direction = nozzleDirection;

                int ammoUsed = this.primaryWeapon.TryFire(1);
                this.primaryFireActionPending = !(ammoUsed > 0);
            }

            if (RobotGame.RobotGameInputDevice.GetSecondaryFire(false))
            {
                this.secondaryWeaponInventory.FireSelectedWeapon(this.position + (nozzleDirection * SECONDARY_PROJECTILE_SPAWN_OFFSET), nozzleDirection);
                this.primaryFireActionPending = false;
            }

            if (RobotGame.RobotGameInputDevice.GetWeaponSwitch())
            {
                this.secondaryWeaponInventory.CycleSelectedWeapon();
            }
        }

        private float CalculateAccelertion(Vector2 inputForce)
        {
            float acceleration;
            float C_drag = 0.99f;
            float C_rr = 30 * C_drag;

            float F_traction = inputForce.X;
            float F_rollingresistence = -C_rr * this.velocity.X;
            float F_drag = -C_drag * this.velocity.X * Math.Abs(this.velocity.X);
            float F_total = F_traction + F_rollingresistence + F_drag;
            acceleration = F_total / this.mass;

            if (this.environmentState == EnvironmentState.InAir)
            {
                acceleration *= AIR_DRAG_MULTIPLIER;
            }

            return acceleration;
        }

        private void CheckActorCollisions()
        {
            // Enemy collisions
            for (int i = 0; i < AbstractEnemy.EnemyList.Count; i++)
            {
                AbstractEnemy enemy = (AbstractEnemy)AbstractEnemy.EnemyList[i];
                if (!enemy.Dead)
                {
                    if (CollisionUtil.CheckPerPixelCollision(this, enemy))
                    {
                        this.TakeDamage(ENEMY_COLLISION_DAMAGE);
                    }
                }
            }

            // Projectile collisions
            for (int i = 0; i < Projectile.EnemyProjectileList.Count; i++)
            {
                Projectile projectile = (Projectile)Projectile.EnemyProjectileList[i];
                if (!projectile.Dead)
                {
                    if (CollisionUtil.CheckIntersectionCollision(this.Bounds, projectile.Bounds) != Rectangle.Empty)
                    {
                        if (!this.invincible)
                        {
                            this.TakeDamage(projectile.Damage);
                            projectile.Collide(this);
                        }
                    }
                }
            }

            // Powerup collisions
            for (int i = 0; i < AbstractPowerup.PowerupList.Count; i++)
            {
                AbstractPowerup powerup = (AbstractPowerup)AbstractPowerup.PowerupList[i];
                if (!powerup.Dead)
                {
                    if (CollisionUtil.CheckIntersectionCollision(this.Bounds, powerup.Bounds) != Rectangle.Empty)
                    {
                        powerup.Collide(this);

                        SoundManager.PlaySoundEffect(SoundKey.AcquirePowerup);
                    }
                }
            }
        }

        private void HandleJetPackLogic()
        {
            if (this.jetPackEnabled)
            {
                if (this.jetPackOn)
                {
                    this.jetPackFuel -= JETPACK_FUEL_DEPLETION_RATE;
                    if (this.jetPackFuel < 0)
                    {
                        this.jetPackFuel = 0;
                    }
                }

                if (this.environmentState == EnvironmentState.OnGround)
                {
                    if (this.jetPackFuel < JETPACK_FUEL_MAX)
                    {
                        this.jetPackFuel += JETPACK_FUEL_REPLENISH_RATE;
                        if (this.jetPackFuel >= JETPACK_FUEL_MAX)
                        {
                            this.jetPackFuel = JETPACK_FUEL_MAX;
                        }
                    }
                }
            }
        }

        private void DisableJetpack()
        {
            this.jetPackOn = false;
            this.jetPackParticleEmitter.Stop();
            RobotGame.RobotGameInputDevice.DisableJetpackFeedback();
            SoundManager.StopSoundEffect(SoundKey.JetpackOn);
        }

        private void StartDoubleDamageTextureFlicker()
        {
            TimerManager.GetInstance().RegisterTimer(DOUBLE_DAMAGE_TEXTURE_FREQUENCY, this.DoubleDamageCallback, null);
            this.showDoubleDamage = true;
        }

        private void StopDoubleDamageTextureFlicker()
        {
            TimerManager.GetInstance().UnregisterTimer(this.DoubleDamageCallback);
            this.showDoubleDamage = false;
        }
        
    } // End of class
}
