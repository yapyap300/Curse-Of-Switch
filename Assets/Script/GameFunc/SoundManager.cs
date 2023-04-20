using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioClip[] bgmClip;
    [SerializeField] AudioClip[] sfxClips;
    Dictionary<string, AudioClip> soundDic;
    AudioSource sfxPlayer;
    AudioSource bgmPlayer;

    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            soundDic = new Dictionary<string, AudioClip>();
            PlayerSetUp();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PlayerSetUp()
    {
        GameObject sfx = new("SFX");
        sfx.transform.SetParent(transform);        
        sfxPlayer = sfx.AddComponent<AudioSource>();
        sfxPlayer.playOnAwake = false;
        sfxPlayer.volume = 0.5f;

        GameObject bgm = new("BGM");
        bgm.transform.SetParent(transform);
        bgmPlayer = bgm.AddComponent<AudioSource>(); 
        bgmPlayer.loop= true;
        bgmPlayer.volume = 0.3f; 

        foreach(AudioClip clip in sfxClips)
        {
            soundDic.Add(clip.name, clip);
        }
    }

    public void PlaySfx(string name)
    {
        if(soundDic.ContainsKey(name) == false)        
            return;
        
        sfxPlayer.PlayOneShot(soundDic[name]);
    }

    public void PlayBGM(int index)
    {
        if(bgmPlayer.isPlaying == true)
            bgmPlayer.Stop();
        bgmPlayer.clip = bgmClip[index];
        bgmPlayer.Play();
    }
}
