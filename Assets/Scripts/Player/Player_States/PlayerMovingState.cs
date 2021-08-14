using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{

    #region inputs
    bool jumping = false;
    #endregion
    public override void EnterState(PlayerController_FSM player)
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            player.AudioController.ChangeAudioState(PlayerAudioController.PLAYER_RUN);
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_RUN);
        }
        else
        {
            player.AudioController.ChangeAudioState(PlayerAudioController.PLAYER_IDLE);
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_IDLE);
        }
    }

    public override void FixedUpdate(PlayerController_FSM player)
    {
       
        //Player jumps:
        if (jumping)
        {
            jumping = false;
            player.Rigidbody.velocity = new Vector2(player.Rigidbody.velocity.x, player.jumpForce.y);
            if (Input.GetAxisRaw("Horizontal") != 1)
            {
                //To the left
                Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) - new Vector2(player.BoxCol.size.x / 2 - player.BoxCol.offset.x - 0.025f, -player.BoxCol.offset.y), new Vector2(0.05f, player.BoxCol.size.y - 0.05f), 0, LayerMask.GetMask("Wall"));
                foreach (Collider2D hit in hits)
                {
                    player.WallState.wallDirection = "left";
                    player.TransitionToState(player.WallState);
                    return;
                }
            }

            else if (Input.GetAxisRaw("Horizontal") != -1)
            {
                //To the Right
                Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) + new Vector2(player.BoxCol.size.x / 2 + player.BoxCol.offset.x - 0.025f, -player.BoxCol.offset.y), new Vector2(0.05f, player.BoxCol.size.y - 0.05f), 0, LayerMask.GetMask("Wall"));
                foreach (Collider2D hit in hits)
                {
                    player.WallState.wallDirection = "right";
                    player.TransitionToState(player.WallState);
                    return;
                }
            }
            player.TransitionToState(player.JumpingState);
            return;
        }
        else
        {
            player.Rigidbody.velocity = Vector3.MoveTowards(new Vector2(player.Rigidbody.velocity.x, player.Rigidbody.velocity.y), new Vector2(player.MoveSpeed * Input.GetAxisRaw("Horizontal"), player.Rigidbody.velocity.y), player.Acceleration / 50);
        }
    }

    public override void OnCollisionEnter2D(PlayerController_FSM player, Collision2D collision)
    {

    }

    public override void OnCollisionExit2D(PlayerController_FSM player, Collision2D collision)
    {
        bool falling = true;
        //Under
        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) - new Vector2(0, player.BoxCol.size.y / 2 - player.BoxCol.offset.y - 0.025f), new Vector2(player.BoxCol.size.x - 0.05f, 0.05f), 0, LayerMask.GetMask("Wall"));
        
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                falling = false;
            }
        }
        //Nothing under player: Jump
        if (falling)
        {
            player.TransitionToState(player.JumpingState);
        }
    }

    public override void Update(PlayerController_FSM player)
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }
        if(Input.GetAxis("Horizontal") != 0)
        {
            player.AudioController.ChangeAudioState(PlayerAudioController.PLAYER_RUN);
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_RUN);
        }
        else
        {
            player.AudioController.ChangeAudioState(PlayerAudioController.PLAYER_IDLE);
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_IDLE);
        }

        
    }
}
