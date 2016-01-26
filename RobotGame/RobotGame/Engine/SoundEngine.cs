using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using RobotGame.Exceptions;

namespace RobotGame.Engine
{
    static class SoundEngine
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        public const float MUSIC_VOLUME = Config.MUSIC_VOLUME;
        public const float SOUND_EFFECT_VOLUME = Config.SOUND_EFFECT_VOLUME;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static RobotGameContentManager content;

        private static Dictionary<string, Song> songs = new Dictionary<string, Song>();

        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, List<SoundEffectInstance>> soundEffectInstances = new Dictionary<string, List<SoundEffectInstance>>();

        // Properties ------------------------------------------------------------------------------------- Properties

        public static bool LoopMusic
        {
            get { return MediaPlayer.IsRepeating; }
            set { MediaPlayer.IsRepeating = value; }
        }

        public static MediaState MediaPlayerState
        {
            get { return MediaPlayer.State; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public static void Init(RobotGameContentManager contentManager)
        {
            content = contentManager;
            MediaPlayer.Volume = MUSIC_VOLUME;
        }

        public static void RemoveAllSoundEffects()
        {
            soundEffects.Clear();
            soundEffectInstances.Clear();
        }

        public static void DisposeStoppedSoundEffects()
        {
            foreach (KeyValuePair<string, List<SoundEffectInstance>> pair in soundEffectInstances)
            {
                List<SoundEffectInstance> instanceList = pair.Value;
                for (int i = instanceList.Count - 1; i >= 0; i--)
                {
                    SoundEffectInstance instance = instanceList[i];
                    if (!instance.IsDisposed && instance.State == SoundState.Stopped)
                    {
                        instance.Dispose();
                        instanceList.Remove(instance);
                    }
                }
            }
        }

        public static void StopAndDisposeAllSongs()
        {
            MediaPlayer.Stop();

            while (songs.Count > 0)
            {
                KeyValuePair<string, Song> pair = songs.Last();
                songs.Remove(pair.Key);
                if (!pair.Value.IsDisposed)
                {
                    pair.Value.Dispose();
                }
            }
        }

        public static bool LoadSoundEffect(string soundEffectKey, string assetPath)
        {
            if (soundEffects.ContainsKey(soundEffectKey))
            {
                return false;
            }

            soundEffects.Add(soundEffectKey, content.Load<SoundEffect>(assetPath));
            return true;
        }

        public static bool LoadSong(string songKey, string assetPath)
        {
            if (songs.ContainsKey(songKey))
            {
                return false;
            }

            songs.Add(songKey, content.Load<Song>(assetPath, false));
            return true;
        }

        public static bool PlaySoundEffect(string soundEffectKey, float volume, float pitch)
        {
            return PlaySoundEffect(soundEffectKey, volume, pitch, 0.0f, false);
        }

        public static bool PlaySoundEffect(string soundEffectKey, float volume, float pitch, bool loop)
        {
            return PlaySoundEffect(soundEffectKey, volume, pitch, 0.0f, loop);
        }

        public static bool PlaySoundEffect(string soundEffectKey, float volume, float pitch, float pan, bool loop)
        {
            SoundEffect soundEffect;
            if (!soundEffects.TryGetValue(soundEffectKey, out soundEffect))
            {
                return false;
            }

            SoundEffectInstance instance = soundEffect.CreateInstance();
            instance.Volume = volume;
            instance.Pitch = pitch;
            instance.Pan = pan;
            instance.IsLooped = loop;

            if (!soundEffectInstances.ContainsKey(soundEffectKey))
            {
                soundEffectInstances.Add(soundEffectKey, new List<SoundEffectInstance>());
            }
            soundEffectInstances[soundEffectKey].Add(instance);

            instance.Play();

            return true;
        }

        public static List<SoundEffectInstance> GetSoundEffectInstances(string soundKey)
        {
            List<SoundEffectInstance> instanceList;
            if (!soundEffectInstances.TryGetValue(soundKey, out instanceList))
            {
                return null;
            }
            return instanceList;
        }

        public static void ChangeSoundEffectVolume(float volume)
        {
            foreach (KeyValuePair<string, List<SoundEffectInstance>> pair in soundEffectInstances)
            {
                foreach (SoundEffectInstance instance in pair.Value)
                {
                    if (!instance.IsDisposed)
                    {
                        instance.Volume = volume;
                    }
                }
            }
        }

        public static void ChangeMusicVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public static void StopAllSoundEffects()
        {
            foreach (KeyValuePair<string, List<SoundEffectInstance>> pair in soundEffectInstances)
            {
                foreach (SoundEffectInstance instance in pair.Value)
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
            foreach (KeyValuePair<string, List<SoundEffectInstance>> pair in soundEffectInstances)
            {
                foreach (SoundEffectInstance instance in pair.Value)
                {
                    if (!instance.IsDisposed && instance.IsLooped)
                    {
                        instance.Stop();
                    }
                }
            }
        }


        public static void PlaySong(string songKey)
        {
            MediaPlayer.Play(songs[songKey]);
        }

        public static void StopSong()
        {
            MediaPlayer.Stop();
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
