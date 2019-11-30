using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static MusicManager _instance;

    public AudioSource[] AudioSource=new AudioSource[13];

    public AudioClip[] musicSource;
    private static GameObject gamePlayAudio;

    //public static MusicManager getInstance()
    //{
    //    if (_instance == null)
    //    {
    //        GameObject temp = MusicManager.gamePlayAudio;
    //        _instance = temp.GetComponentInChildren<MusicManager>();
    //    }
    //    return _instance;
    //}
    private void Awake()
    {
        //DontDestroyOnLoad(this);
    }
    void Start()
    {
        AudioSource[0].clip = musicSource[0];
        AudioSource[0].loop = true;
        AudioSource[0].Play();
    }
    public void PlayMusicloop(int MusicNum)
    {
        AudioSource[MusicNum].clip = musicSource[MusicNum];
        AudioSource[MusicNum].loop = true;
        AudioSource[MusicNum].Play();
    }
    public void PlayMusicOnce(int MusicNum)
    {
        AudioSource[MusicNum].clip = musicSource[MusicNum];
        AudioSource[MusicNum].Play();
    }
    public void StopMusic(int MusicNum)
    {
        AudioSource[MusicNum].Stop();
    }
}
