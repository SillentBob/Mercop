using Mercop.Core;
using UnityEngine;

namespace Mercop.Audio
{
    //TODO!! sources pooling
    public class AudioPlayer : Singleton<AudioPlayer>
    {
        [SerializeField] private bool enabled = true;
        [SerializeField] private AudioClip music;
        [SerializeField] private AudioClip engine;
        [SerializeField] private AudioClip uiClick;
        private AudioSource musicSource;
        private AudioSource engineSource;
        private AudioSource uiClickSource;

        public void Play(Sound sound)
        {
            if (!enabled)
            {
                return;
            }
            switch (sound)
            {
                case Sound.Music:
                    if (musicSource == null)
                    {
                        musicSource = gameObject.AddComponent<AudioSource>();
                        musicSource.clip = music;
                        musicSource.loop = true;
                        musicSource.volume = 0.5f;
                    }

                    musicSource.Play();
                    break;
                case Sound.Engine:
                    if (engineSource == null)
                    {
                        engineSource = gameObject.AddComponent<AudioSource>();
                        engineSource.clip = engine;
                        engineSource.loop = true;
                        engineSource.pitch = 0;
                        engineSource.volume = 0.5f;
                    }

                    engineSource.Play();
                    break;
                case Sound.UiClick:
                    if (uiClickSource == null)
                    {
                        uiClickSource = gameObject.AddComponent<AudioSource>();
                        uiClickSource.clip = uiClick;
                        uiClickSource.loop = false;
                    }

                    uiClickSource.Play();
                    break;
            }
        }

        public void Stop(Sound sound)
        {
            AudioSource audioSource = GetSourceByKind(sound);
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        public void ChangePitch(Sound sound, float pitch)
        {
            AudioSource audioSource = GetSourceByKind(sound);
            if (audioSource != null)
            {
                audioSource.pitch = pitch;
            }
        }

        private AudioSource GetSourceByKind(Sound sound)
        {
            switch (sound)
            {
                case Sound.Music:
                    return musicSource;
                case Sound.Engine:
                    return engineSource;
                case Sound.UiClick:
                    return uiClickSource;
            }

            return null;
        }

        public enum Sound
        {
            Music,
            Engine,
            UiClick
        }
    }
}