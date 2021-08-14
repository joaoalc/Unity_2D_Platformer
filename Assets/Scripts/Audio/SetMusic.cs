using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class SetMusic : MonoBehaviour
{
    AudioClip audioClip;

    [SerializeField]
    static readonly string musicLocation = "Music/Main_Theme.wav";

    // Start is called before the first frame update
    void Awake()
    {
        AudioSource music = GetComponent<AudioSource>();
        float vol = PlayerPrefs.GetFloat("MusicPref");
        if(vol == null)
        {
            vol = 0.5f;
        }
        music.volume = vol;

        
        if (File.Exists(Path.Combine(Application.streamingAssetsPath, musicLocation))){
            StartCoroutine(GetAudio());
            Debug.Log("File exists");
        }
        else
        {
            Debug.Log("File does not exist");
        }
    }

    IEnumerator GetAudio()
    {
        string url = Path.Combine(Application.streamingAssetsPath, musicLocation);
        using (var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET))
        {
            uwr.downloadHandler = new DownloadHandlerAudioClip(url, AudioType.WAV);
            yield return uwr.SendWebRequest();
            audioClip = DownloadHandlerAudioClip.GetContent(uwr);
            PlayAudioFile();
        }
    }

    private void PlayAudioFile()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        audioSource.loop = true;
    }
}
