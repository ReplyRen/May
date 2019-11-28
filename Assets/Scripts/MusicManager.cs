using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static MusicManager _instance;

    public AudioSource BGMSource;
    public AudioSource AudioSource;

    public AudioClip[] musicSource;
    private static GameObject gamePlayAudio;

    public static MusicManager getInstance()
    {
        if (_instance == null)
        {
            GameObject temp = MusicManager.gamePlayAudio;
            _instance = temp.GetComponentInChildren<MusicManager>();
        }
        return _instance;
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
        BGMSource = gameObject.AddComponent<AudioSource>();
        AudioSource = gameObject.AddComponent<AudioSource>();
    }
    void Start()
    {
        BGMSource.clip = musicSource[0];
        BGMSource.loop = true;
        BGMSource.Play();
    }

    public void PlayMusicOnce(int MusicNum)
    {
        AudioSource.clip = musicSource[MusicNum];
        AudioSource.Play();
    }
}
