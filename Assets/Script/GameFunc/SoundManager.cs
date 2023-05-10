using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    [Header("#===BGM")]
    [SerializeField] AudioClip[] bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    [Header("#===SFX")]
    [SerializeField] AudioClip[] sfxClips;
    public float sfxVolume;
    [SerializeField] int channelCount;
    int channelIndex;

    Dictionary<string, AudioClip> soundDic;
    AudioSource[] sfxPlayer;
    AudioSource[] deadPlayer;

    public static SoundManager Instance
    {
        get
        {            
            return instance;
        }
    }
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
        sfxPlayer = new AudioSource[channelCount];
        for(int index = 0; index < sfxPlayer.Length; index++)
        {
            sfxPlayer[index] = sfx.AddComponent<AudioSource>();
            sfxPlayer[index].playOnAwake = false;
            sfxPlayer[index].volume = sfxVolume;
        }

        GameObject dead = new("DeadSFX");
        dead.transform.SetParent(transform);
        deadPlayer = new AudioSource[12];
        for (int index = 0; index < deadPlayer.Length; index++)
        {
            deadPlayer[index] = dead.AddComponent<AudioSource>();
            deadPlayer[index].clip = sfxClips[0];
            deadPlayer[index].playOnAwake = false;
            deadPlayer[index].volume = 0.2f;
        }

        GameObject bgm = new("BGM");
        bgm.transform.SetParent(transform);
        bgmPlayer = bgm.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop= true;
        bgmPlayer.volume = bgmVolume;

        foreach(AudioClip clip in sfxClips)
        {
            soundDic.Add(clip.name, clip);
        }
    }

    public void PlaySfx(string name)
    {
        for(int index = 0; index < sfxPlayer.Length;index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayer.Length;

            if (sfxPlayer[loopIndex].isPlaying)
                continue;
            channelIndex = loopIndex;
            sfxPlayer[loopIndex].clip = soundDic[name];
            sfxPlayer[loopIndex].Play();
            break;
        }
    }

    public void PlayDeadSfx()
    {
        for (int index = 0; index < sfxPlayer.Length; index++)
        {
            int loopIndex = (index + channelIndex) % deadPlayer.Length;

            if (deadPlayer[loopIndex].isPlaying)
                continue;
            channelIndex = loopIndex;
            deadPlayer[loopIndex].Play();
            break;
        }
    }
    public void PlayBGM(int index)
    {
        if(bgmPlayer.isPlaying == true)
            bgmPlayer.Stop();
        bgmPlayer.clip = bgmClip[index];
        bgmPlayer.Play();
    }
}
