using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    AudioSource source;

    [SerializeField]
    AudioClip PLAYER_JUMP;
    [SerializeField]
    AudioClip PLAYER_RUNNING;
    [SerializeField]
    AudioClip PLAYER_WALLJUMP;
    [SerializeField]
    AudioClip PLAYER_HIT_WALL;
    [SerializeField]
    AudioClip PLAYER_HIT_FLOOR;

    public const string PLAYER_IDLE = "Base Layer.Player_Idle";
    public const string PLAYER_RUN = "Base Layer.Player_Run";
    public const string PLAYER_JUMPING = "Base Layer.Player_Jump";
    public const string PLAYER_WALL = "Base Layer.Player_Wall";

    private string currentState;

    float volSFX;

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        volSFX = PlayerPrefs.GetFloat("SoundEffectsPref");
        if(volSFX == null)
        {
            volSFX = 0.5f;
        }
        source.volume = volSFX;
    }
    /*
    float time = 0;
    private void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = Random.Range(0.1f, 5f);
            source.PlayOneShot(PLAYER_RUNNING);
        }
    }*/
    public void ChangeAudioState(string newState)
    {
        //if (currentState == newState) return;

        PlaySFX(newState, currentState);

        currentState = newState;
    }


    //TODO: change this to when actions are done and not when state is changed
    void PlaySFX(string newstate, string oldState)
    {
        switch (newstate)
        {
            case PLAYER_JUMPING:
                switch (oldState)
                {
                    case PLAYER_IDLE:
                        source.PlayOneShot(PLAYER_JUMP);
                        break;
                    case PLAYER_RUN:
                        source.PlayOneShot(PLAYER_JUMP);
                        break;
                    case PLAYER_WALL:
                        source.PlayOneShot(PLAYER_WALLJUMP);
                        break;
                }
                break;
            case PLAYER_WALL:
                switch (oldState)
                {
                    case PLAYER_IDLE:
                    case PLAYER_JUMPING:
                        source.PlayOneShot(PLAYER_HIT_WALL, volSFX);
                        break;
                }
                break;
            case PLAYER_RUN:
                if(oldState != newstate)
                {
                    source.clip = PLAYER_RUNNING;
                    source.Play();
                }
                break;
            default:
                break;
        }
        if(newstate != PLAYER_RUN)
        {
            source.Stop();
        }
    }

}
