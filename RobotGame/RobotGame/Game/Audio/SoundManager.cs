using System;
using System.Collections.Generic;
using RobotGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace RobotGame.Game.Audio
{
    public enum SoundKey
    {
        PrimaryFire = 0,
        SecondaryFire,
        FireNoAmmo,
        HomingMissileSeekMode,
        EnemyFire,

        ProjectileExplosion1,
        ProjectileExplosion2,

        EnemyTakeDamage1,
        EnemyTakeDamage2,
        EnemyTakeDamage3,
        EnemyDeath,

        PlayerLand,
        PlayerTakeDamage1,
        PlayerTakeDamage2,
        PlayerTakeDamage3,
        PlayerTakeDamage4,
        PlayerTakeDamage5,
        PlayerDeath,
        PlayerDeathJingle,

        JetpackOn,
        AcquirePowerup,

        MenuToggle,
        MenuStartGame,
        MenuIn,
        MenuOut,

        LevelEnd,
        GameEndRumble,
        GameEndSong,

        None,
    }

    public enum SongKey
    {
        Level_1_1_Intro,
        Level_1_1_Loop,
        Level_1_2_Intro,
        Level_1_2_Loop,
        Level_1_3_Intro,
        Level_1_3_Loop,

        Level_2_1_Intro,
        Level_2_1_Loop,
        Level_2_2_Intro,
        Level_2_2_Loop,
        Level_2_3_Intro,
        Level_2_3_Loop,

        Level_3_1_Intro,
        Level_3_1_Loop,
        Level_3_2_Intro,
        Level_3_2_Loop,
        Level_3_3_Intro,
        Level_3_3_Loop,

        CoreBossMusicIntro,
        CoreBossMusicLoop,

        None,
    }


    public struct LevelSongInfo
    {
        public SongKey Intro;
        public SongKey Loop;

        public LevelSongInfo(SongKey intro, SongKey loop)
        {
            this.Intro = intro;
            this.Loop = loop;
        }
    }

    static class SoundManager
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private static string PRIMARY_FIRE_ASSET = Config.PRIMARY_FIRE_SE;
        private static string SECONDARY_FIRE_ASSET = Config.SECONDARY_FIRE_SE;
        private static string FIRE_NO_AMMO_ASSET = Config.FIRE_NO_AMMO_SE;
        private static string HOMING_MISSILE_SEEK_MODE_ASSET = Config.HOMING_MISSILE_SEEK_MODE_SE;
        private static string ENEMY_PRIMARY_FIRE_ASSET = Config.ENEMY_PRIMARY_FIRE_SE;

        private static string PROJECTILE_EXPLOSION1_ASSET = Config.PROJECTILE_EXPLODE1_SE;
        private static string PROJECTILE_EXPLOSION2_ASSET = Config.PROJECTILE_EXPLODE2_SE;

        private static string ENEMY_TAKE_DAMAGE1_ASSET = Config.ENEMY_TAKE_DAMAGE1_SE;
        private static string ENEMY_TAKE_DAMAGE2_ASSET = Config.ENEMY_TAKE_DAMAGE2_SE;
        private static string ENEMY_TAKE_DAMAGE3_ASSET = Config.ENEMY_TAKE_DAMAGE3_SE;
        private static string ENEMY_DEATH_ASSET = Config.ENEMY_DEATH_SE;

        private static string PLAYER_LAND_ASSET = Config.PLAYER_LAND_SE;
        private static string JETPACK_ON_ASSET = Config.JETPACK_SE_LOOP;

        private static string PLAYER_TAKE_DAMAGE1_ASSET = Config.PLAYER_TAKE_DAMAGE1_SE;
        private static string PLAYER_TAKE_DAMAGE2_ASSET = Config.PLAYER_TAKE_DAMAGE2_SE;
        private static string PLAYER_TAKE_DAMAGE3_ASSET = Config.PLAYER_TAKE_DAMAGE3_SE;
        private static string PLAYER_TAKE_DAMAGE4_ASSET = Config.PLAYER_TAKE_DAMAGE4_SE;
        private static string PLAYER_TAKE_DAMAGE5_ASSET = Config.PLAYER_TAKE_DAMAGE5_SE;
        private static string PLAYER_DEATH_ASSET = Config.PLAYER_DEATH_SE;
        private static string PLAYER_DEATH_JINGLE_ASSET = Config.PLAYER_DEATH_JINGLE_SE;

        private static string ACQUIRE_POWERUP_ASSET = Config.ACQUIRE_POWERUP_SE;

        private static string LEVEL_END_ASSET = Config.LEVEL_END_SE;
        private static string GAME_END_RUMBLE_ASSET = Config.GAME_END_RUMBLE_SE;
        private static string GAME_END_SONG = Config.GAME_END_SONG;

        private static string MENU_TOGGLE_ASSET = Config.MENU_TOGGLE_SE;
        private static string MENU_START_GAME_ASSET = Config.MENU_START_GAME_SE;
        private static string MENU_IN_ASSET = Config.MENU_IN_SE;
        private static string MENU_OUT_ASSET = Config.MENU_OUT_SE;

        private static string LEVEL_1_1_SONG_INTRO = Config.LEVEL_1_1_SONG_INTRO;
        private static string LEVEL_1_1_SONG_LOOP = Config.LEVEL_1_1_SONG_LOOP;
        private static string LEVEL_1_2_SONG_INTRO = Config.LEVEL_1_2_SONG_INTRO;
        private static string LEVEL_1_2_SONG_LOOP = Config.LEVEL_1_2_SONG_LOOP;
        private static string LEVEL_1_3_SONG_INTRO = Config.LEVEL_1_3_SONG_INTRO;
        private static string LEVEL_1_3_SONG_LOOP = Config.LEVEL_1_3_SONG_LOOP;
        private static string LEVEL_2_1_SONG_INTRO = Config.LEVEL_2_1_SONG_INTRO;
        private static string LEVEL_2_1_SONG_LOOP = Config.LEVEL_2_1_SONG_LOOP;
        private static string LEVEL_2_2_SONG_INTRO = Config.LEVEL_2_2_SONG_INTRO;
        private static string LEVEL_2_2_SONG_LOOP = Config.LEVEL_2_2_SONG_LOOP;
        private static string LEVEL_2_3_SONG_INTRO = Config.LEVEL_2_3_SONG_INTRO;
        private static string LEVEL_2_3_SONG_LOOP = Config.LEVEL_2_3_SONG_LOOP;
        private static string LEVEL_3_1_SONG_INTRO = Config.LEVEL_3_1_SONG_INTRO;
        private static string LEVEL_3_1_SONG_LOOP = Config.LEVEL_3_1_SONG_LOOP;
        private static string LEVEL_3_2_SONG_INTRO = Config.LEVEL_3_2_SONG_INTRO;
        private static string LEVEL_3_2_SONG_LOOP = Config.LEVEL_3_2_SONG_LOOP;
        private static string LEVEL_3_3_SONG_INTRO = Config.LEVEL_3_3_SONG_INTRO;
        private static string LEVEL_3_3_SONG_LOOP = Config.LEVEL_3_3_SONG_LOOP;
        private static string CORE_BOSS_MUSIC_INTRO = Config.BOSS_MUSIC_INTRO;
        private static string CORE_BOSS_MUSIC_LOOP = Config.BOSS_MUSIC_LOOP;

        private static int SOUND_ASSETS_SIZE = 29;
        private static int SONG_ASSETS_SIZE = 20;


        // Data Members --------------------------------------------------------------------------------- Data Members

        private static string[] soundAssets;
        private static string[] songAssets;

        private static float soundEffectPitch = 0.0f;
        private static float soundEffectVolume = Config.SOUND_EFFECT_VOLUME;
        private static float musicVolume = Config.MUSIC_VOLUME;

        private static SongKey pendingLevelSong = SongKey.None;

        private static bool musicFade = false;
        private static bool restoreVolume = false;
        private static float musicFadeInitialVolume;
        private static float musicFadeLifeTime;
        private static float musicFadeTimeLeft;
        private static int musicFadeFrameCount = 0;

        // Properties ------------------------------------------------------------------------------------- Properties

        public static float SoundEffectPitch
        {
            get { return soundEffectPitch; }
            set { soundEffectPitch = value; }
        }

        public static float SoundEffectVolume
        {
            get { return soundEffectVolume; }
            set
            {
                soundEffectVolume = value;
                SoundEngine.ChangeSoundEffectVolume(value);
            }
        }

        public static float MusicVolume
        {
            get { return musicVolume; }
            set
            {
                musicVolume = value;
                SoundEngine.ChangeMusicVolume(value);
            }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        static SoundManager()
        {
            soundAssets = new string[SOUND_ASSETS_SIZE];

            soundAssets[(int)SoundKey.PrimaryFire] = PRIMARY_FIRE_ASSET;
            soundAssets[(int)SoundKey.SecondaryFire] = SECONDARY_FIRE_ASSET;
            soundAssets[(int)SoundKey.FireNoAmmo] = FIRE_NO_AMMO_ASSET;
            soundAssets[(int)SoundKey.HomingMissileSeekMode] = HOMING_MISSILE_SEEK_MODE_ASSET;
            soundAssets[(int)SoundKey.EnemyFire] = ENEMY_PRIMARY_FIRE_ASSET;
            soundAssets[(int)SoundKey.ProjectileExplosion1] = PROJECTILE_EXPLOSION1_ASSET;
            soundAssets[(int)SoundKey.ProjectileExplosion2] = PROJECTILE_EXPLOSION2_ASSET;
            soundAssets[(int)SoundKey.EnemyTakeDamage1] = ENEMY_TAKE_DAMAGE1_ASSET;
            soundAssets[(int)SoundKey.EnemyTakeDamage2] = ENEMY_TAKE_DAMAGE2_ASSET;
            soundAssets[(int)SoundKey.EnemyTakeDamage3] = ENEMY_TAKE_DAMAGE3_ASSET;
            soundAssets[(int)SoundKey.EnemyDeath] = ENEMY_DEATH_ASSET;
            soundAssets[(int)SoundKey.PlayerLand] = PLAYER_LAND_ASSET;
            soundAssets[(int)SoundKey.PlayerTakeDamage1] = PLAYER_TAKE_DAMAGE1_ASSET;
            soundAssets[(int)SoundKey.PlayerTakeDamage2] = PLAYER_TAKE_DAMAGE2_ASSET;
            soundAssets[(int)SoundKey.PlayerTakeDamage3] = PLAYER_TAKE_DAMAGE3_ASSET;
            soundAssets[(int)SoundKey.PlayerTakeDamage4] = PLAYER_TAKE_DAMAGE4_ASSET;
            soundAssets[(int)SoundKey.PlayerTakeDamage5] = PLAYER_TAKE_DAMAGE5_ASSET;
            soundAssets[(int)SoundKey.PlayerDeath] = PLAYER_DEATH_ASSET;
            soundAssets[(int)SoundKey.PlayerDeathJingle] = PLAYER_DEATH_JINGLE_ASSET;
            soundAssets[(int)SoundKey.JetpackOn] = JETPACK_ON_ASSET;
            soundAssets[(int)SoundKey.AcquirePowerup] = ACQUIRE_POWERUP_ASSET;
            soundAssets[(int)SoundKey.LevelEnd] = LEVEL_END_ASSET;
            soundAssets[(int)SoundKey.GameEndRumble] = GAME_END_RUMBLE_ASSET;
            soundAssets[(int)SoundKey.GameEndSong] = GAME_END_SONG;
            soundAssets[(int)SoundKey.MenuToggle] = MENU_TOGGLE_ASSET;
            soundAssets[(int)SoundKey.MenuStartGame] = MENU_START_GAME_ASSET;
            soundAssets[(int)SoundKey.MenuIn] = MENU_IN_ASSET;
            soundAssets[(int)SoundKey.MenuOut] = MENU_OUT_ASSET;

            songAssets = new string[SONG_ASSETS_SIZE];

            songAssets[(int)SongKey.Level_1_1_Intro] = LEVEL_1_1_SONG_INTRO;
            songAssets[(int)SongKey.Level_1_1_Loop] = LEVEL_1_1_SONG_LOOP;
            songAssets[(int)SongKey.Level_1_2_Intro] = LEVEL_1_2_SONG_INTRO;
            songAssets[(int)SongKey.Level_1_2_Loop] = LEVEL_1_2_SONG_LOOP;
            songAssets[(int)SongKey.Level_1_3_Intro] = LEVEL_1_3_SONG_INTRO;
            songAssets[(int)SongKey.Level_1_3_Loop] = LEVEL_1_3_SONG_LOOP;

            songAssets[(int)SongKey.Level_2_1_Intro] = LEVEL_2_1_SONG_INTRO;
            songAssets[(int)SongKey.Level_2_1_Loop] = LEVEL_2_1_SONG_LOOP;
            songAssets[(int)SongKey.Level_2_2_Intro] = LEVEL_2_2_SONG_INTRO;
            songAssets[(int)SongKey.Level_2_2_Loop] = LEVEL_2_2_SONG_LOOP;
            songAssets[(int)SongKey.Level_2_3_Intro] = LEVEL_2_3_SONG_INTRO;
            songAssets[(int)SongKey.Level_2_3_Loop] = LEVEL_2_3_SONG_LOOP;

            songAssets[(int)SongKey.Level_3_1_Intro] = LEVEL_3_1_SONG_INTRO;
            songAssets[(int)SongKey.Level_3_1_Loop] = LEVEL_3_1_SONG_LOOP;
            songAssets[(int)SongKey.Level_3_2_Intro] = LEVEL_3_2_SONG_INTRO;
            songAssets[(int)SongKey.Level_3_2_Loop] = LEVEL_3_2_SONG_LOOP;
            songAssets[(int)SongKey.Level_3_3_Intro] = LEVEL_3_3_SONG_INTRO;
            songAssets[(int)SongKey.Level_3_3_Loop] = LEVEL_3_3_SONG_LOOP;

            songAssets[(int)SongKey.CoreBossMusicIntro] = CORE_BOSS_MUSIC_INTRO;
            songAssets[(int)SongKey.CoreBossMusicLoop] = CORE_BOSS_MUSIC_LOOP;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static void Init(RobotGameContentManager contentManager)
        {
            SoundEngine.Init(contentManager);
        }

        public static void Reset()
        {
            StopMusic();
            UnloadSoundEffects();
            SoundEngine.RemoveAllSoundEffects();
        }

        public static void PreLoadAllSoundEffects(params SoundKey[] exclusions)
        {
            for (int i = 0; i < SOUND_ASSETS_SIZE; i++)
            {
                SoundKey soundKey = (SoundKey)i;

                if (Array.IndexOf(exclusions, soundKey) == -1)
                {
                    SoundEngine.LoadSoundEffect(soundKey.ToString(), soundAssets[i]);
                }
            }
        }

        public static void LoadSoundEffect(SoundKey soundKey)
        {
            SoundEngine.LoadSoundEffect(soundKey.ToString(), soundAssets[(int)soundKey]);
        }
        
        public static void PreLoadSongs(params SongKey[] songKeys)
        {
            foreach (SongKey songKey in songKeys)
            {
                if (songAssets[(int)songKey] != null)
                {
                    SoundEngine.LoadSong(songKey.ToString(), songAssets[(int)songKey]);
                }
            }
        }

        public static void UnloadSoundEffects()
        {
            SoundEngine.StopAllSoundEffects();
            SoundEngine.DisposeStoppedSoundEffects();
        }

        public static void UnloadSongs()
        {
            SoundEngine.StopAndDisposeAllSongs();
        }

        public static void Update(GameTime gameTime)
        {
            SoundEngine.DisposeStoppedSoundEffects();

            if (pendingLevelSong != SongKey.None && SoundEngine.MediaPlayerState == MediaState.Stopped)
            {
                SoundEngine.PlaySong(songAssets[(int)pendingLevelSong]);
                pendingLevelSong = SongKey.None;
            }

            // Fade out the music
            if (musicFade)
            {
                musicFadeTimeLeft -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (musicFadeFrameCount > 10)
                {
                    musicFadeFrameCount = 0;

                    float fadePercentage = (musicFadeLifeTime - musicFadeTimeLeft) / musicFadeLifeTime;

                    MusicVolume = MathHelper.Lerp(musicFadeInitialVolume, 0.0f, fadePercentage);

                    if (musicVolume <= 0.0f)
                    {
                        StopMusic();
                        restoreVolume = true;
                        musicFade = false;
                    }
                }
                else
                {
                    musicFadeFrameCount++;
                }
            }

            if (restoreVolume)
            {
                if (SoundEngine.MediaPlayerState == MediaState.Stopped)
                {
                    MusicVolume = musicFadeInitialVolume;
                    restoreVolume = false;
                }
            }

#if DEBUG
            Debug.AddDebugInfo("Sound Engine music volume: " + musicVolume);
#endif
        }

        public static void PlaySoundEffect(SoundKey soundEffect)
        {
            PlaySoundEffect(soundEffect, soundEffectVolume, soundEffectPitch);
        }

        public static void PlaySoundEffect(SoundKey soundEffect, float volume, float pitch)
        {
            if (soundEffect == SoundKey.None)
            {
                return;
            }

            if (!SoundEngine.PlaySoundEffect(soundEffect.ToString(), volume, pitch))
            {
                SoundEngine.LoadSoundEffect(soundEffect.ToString(), soundAssets[(int)soundEffect]);
                SoundEngine.PlaySoundEffect(soundEffect.ToString(), volume, pitch);
            }
        }

        public static void PlayAndLoopSoundEffect(SoundKey soundEffect)
        {
            if (soundEffect == SoundKey.None)
            {
                return;
            }

            if (!SoundEngine.PlaySoundEffect(soundEffect.ToString(), soundEffectVolume, soundEffectPitch, true))
            {
                SoundEngine.LoadSoundEffect(soundEffect.ToString(), soundAssets[(int)soundEffect]);
                SoundEngine.PlaySoundEffect(soundEffect.ToString(), soundEffectVolume, soundEffectPitch, true);
            }
        }

        public static void StopSoundEffect(SoundKey soundKey)
        {
            List<SoundEffectInstance> instanceList = SoundEngine.GetSoundEffectInstances(soundKey.ToString());
            if (instanceList != null)
            {
                foreach (SoundEffectInstance instance in instanceList)
                {
                    if (!instance.IsDisposed)
                    {
                        instance.Stop();
                    }
                }
            }
        }
        
        public static void StopAllLoopingSoundEffects()
        {
            SoundEngine.StopAllLoopingSoundEffects();
        }

        public static void PlayRandomSoundEffect(params SoundKey[] soundEffects)
        {
            SoundKey soundEffect = soundEffects[new Random().Next(0, soundEffects.Length)];
            PlaySoundEffect(soundEffect);
        }

        public static void ChangeSoundEffectInstanceVolume(SoundKey soundKey, float volume)
        {
            List<SoundEffectInstance> instanceList = SoundEngine.GetSoundEffectInstances(soundKey.ToString());
            if (instanceList != null)
            {
                foreach (SoundEffectInstance instance in instanceList)
                {
                    if (!instance.IsDisposed)
                    {
                        instance.Volume = volume;
                    }
                }
            }
        }
        
        public static void StartLevelMusic(LevelSongInfo songInfo)
        {
            SoundEngine.StopSong();

            if (songAssets[(int)songInfo.Intro] != null)
            {
                SoundEngine.PlaySong(songInfo.ToString());

                if (songAssets[(int)songInfo.Loop] != null)
                {
                    pendingLevelSong = songInfo.Loop;
                }
                else
                {
                    pendingLevelSong = SongKey.None;
                }
            }
            else if (songAssets[(int)songInfo.Loop] != null)
            {
                SoundEngine.PlaySong(songInfo.Loop.ToString());
                SoundEngine.LoopMusic = true;

                pendingLevelSong = SongKey.None;
            }
        }

        public static void StopMusic()
        {
            SoundEngine.StopSong();
        }

        public static bool MusicFadeOut(float time)
        {
            if (musicFade)
            {
                return false; 
            }

            musicFade = true;
            musicFadeTimeLeft = time;
            musicFadeLifeTime = time;
            musicFadeInitialVolume = musicVolume;

            return true;
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
