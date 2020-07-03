using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager instance;
    public Sound[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.loop = s.loop;
        s.source.Play();
    }

    public void Play(string name, float pitchVar)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.pitch = s.pitch + UnityEngine.Random.Range(-pitchVar, pitchVar);
        s.source.loop = s.loop;
        s.source.Play();
    }

    public void SetVolume(string name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        StartCoroutine(AdjustVolume(s, value, 1.5f));
    }

    private IEnumerator AdjustVolume(Sound s, float targetVolume, float fadeDuration)
    {
        float startVolume = s.source.volume;
        float delta = targetVolume - startVolume;

        while(Mathf.Abs(s.source.volume - targetVolume) > 0.02f)
        {
            s.source.volume += (0.1f / fadeDuration) * delta;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void PlayPitched(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.pitch = pitch;
        s.source.loop = s.loop;
        s.source.Play();
    }

    public void PlayPitched(string name, float pitch, float pitchVar)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.pitch = pitch + UnityEngine.Random.Range(-pitchVar, pitchVar);
        s.source.loop = s.loop;
        s.source.Play();
    }

    internal static AudioManager GetInstance()
    {
        return instance;
    }
}