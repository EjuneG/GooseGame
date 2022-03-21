using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance = null;
    void Awake()
    {
        if(Instance == null) {
            Instance = this;
        }else if(Instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {
            return;
        }


        s.source.Play();
    }

    public void StopPlay(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {
            return;
        }

        if (s.source.isPlaying) {
            s.source.Stop();
        }
    }

    public void StopBGM() {
        Sound[] bgms = Array.FindAll(sounds, sound => (sound.name.Contains("bgm") || sound.name.Contains("BGM")));
        
        foreach (Sound bgm in bgms) {
            if (bgm.source.isPlaying) {
                bgm.source.Stop();
            }
        }
    }

    public bool isPlaying(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            return false;
        }

        if (s.source.isPlaying) {
            return true;
        } else {
            return false;
        }
    }
    
}
