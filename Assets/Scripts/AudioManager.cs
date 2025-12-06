using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager main;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource loopSfxSource;

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
    }

    public Sound[] sounds;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                sfxSource.PlayOneShot(s.clip, s.volume);
                return;
            }
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayMusic(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                musicSource.clip = s.clip;
                musicSource.loop = true;
                musicSource.Play();
                return;
            }
        }
    }

    public void PlayLoopSFX(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                loopSfxSource.clip = s.clip;
                loopSfxSource.volume = s.volume;

                if (!loopSfxSource.isPlaying)
                    loopSfxSource.Play();

                return;
            }
        }
    }

    public void StopLoopSFX()
    {
        if (loopSfxSource.isPlaying)
            loopSfxSource.Stop();
    }
}
