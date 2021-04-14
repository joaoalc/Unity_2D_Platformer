using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimationController : MonoBehaviour
{


    Animator animator;

    public const string PLAYER_IDLE = "Base Layer.Player_Idle";
    public const string PLAYER_RUN = "Base Layer.Player_Run";
    public const string PLAYER_JUMPING_UP = "Base Layer.Player_Jump_Up";
    public const string PLAYER_JUMPING_DOWN = "Base Layer.Player_Jump_Down";
    public const string PLAYER_WALL = "Base Layer.Player_Wall";

    private string currentState;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

}
