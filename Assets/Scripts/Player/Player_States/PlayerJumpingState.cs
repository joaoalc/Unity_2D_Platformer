using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{

    bool firstUpdate = false;

    bool jumping = false;
    public override void EnterState(PlayerController_FSM player)
    {
        player.AudioController.ChangeAudioState(PlayerAudioController.PLAYER_JUMPING);
        if (player.Rigidbody.velocity.y > 0) { 
        player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_JUMPING_UP);
        }
        else
        {
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_JUMPING_DOWN);
        }
        player.DetectGroundOnJumpRemaining = player.DetectGroundOnJump;
        firstUpdate = true;
    }

    public override void FixedUpdate(PlayerController_FSM player)
    {
        if (firstUpdate)
        {
            player.Rigidbody.gravityScale = player.JumpGrav;
            firstUpdate = false;
        }
        player.Rigidbody.velocity = Vector2.MoveTowards(player.Rigidbody.velocity, new Vector2(player.MoveSpeed * Input.GetAxisRaw("Horizontal"), player.Rigidbody.velocity.y), player.AirAcceleration / 50);

        //Under

        if (player.Rigidbody.velocity.y <= 0 && player.DetectGroundOnJumpRemaining <= 0)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) - new Vector2(-player.BoxCol.offset.x, player.BoxCol.size.y / 2 - player.BoxCol.offset.y - 0.025f), new Vector2(player.BoxCol.size.x - 0.05f, 0.05f), 0, LayerMask.GetMask("Wall"));
            foreach (Collider2D hit in hits)
            {
                player.TransitionToState(player.MovingState);
            }
        }

        //If walljumping
        if (jumping && player.CurrentWallJumpGrace > 0)
        {
            player.Rigidbody.velocity = new Vector2(-player.WallState.WallDirectionToInt() * player.WallJumpForce.x, player.WallJumpForce.y);
            player.TransitionToState(player.JumpingState);
        }


        if (player.DetectGroundOnJumpRemaining <= 0)
        {
            Collider2D[] hits;

            if (Input.GetAxisRaw("Horizontal") != 1)
            {
                //To the left
                hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) - new Vector2(player.BoxCol.size.x / 2 - player.BoxCol.offset.x - 0.025f, -player.BoxCol.offset.y), new Vector2(0.05f, player.BoxCol.size.y - 0.05f), 0, LayerMask.GetMask("Wall"));
                foreach (Collider2D hit in hits)
                {
                    player.WallState.wallDirection = "left";
                    player.TransitionToState(player.WallState);
                }
            }

            if (Input.GetAxisRaw("Horizontal") != -1)
            {
                //To the Right
                hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) + new Vector2(player.BoxCol.size.x / 2 + player.BoxCol.offset.x - 0.025f, -player.BoxCol.offset.y), new Vector2(0.05f, player.BoxCol.size.y - 0.05f), 0, LayerMask.GetMask("Wall"));
                foreach (Collider2D hit in hits)
                {
                    player.WallState.wallDirection = "right";
                    player.TransitionToState(player.WallState);
                }
            }
        }



        //Debug.Log(Input.GetAxis("Vertical"));
        if (player.Rigidbody.velocity.y >= 0 && Input.GetButton("Jump"))
        {
            player.Rigidbody.gravityScale = player.JumpGrav;
        }

        else if (player.Rigidbody.velocity.y >= 0 && !Input.GetButton("Jump"))
        {
            player.Rigidbody.gravityScale = player.NoLongerJumpingUpGrav;
        }
        //Change gravity when going down
        //Also check for walls
        else if (player.Rigidbody.velocity.y < 0)
        {
            player.Rigidbody.gravityScale = player.FallGrav;
        }

    }

    public override void OnCollisionEnter2D(PlayerController_FSM player, Collision2D collision)
    {
        //Under
        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) - new Vector2(-player.BoxCol.offset.x, player.BoxCol.size.y / 2 - player.BoxCol.offset.y - 0.025f), new Vector2(player.BoxCol.size.x - 0.05f, 0.05f), 0, LayerMask.GetMask("Wall"));
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == collision.gameObject)
            {
                player.TransitionToState(player.MovingState);
            }
        }

        if (Input.GetAxisRaw("Horizontal") != 1)
        {
            //To the left
            hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) - new Vector2(player.BoxCol.size.x / 2 - player.BoxCol.offset.x - 0.025f, -player.BoxCol.offset.y), new Vector2(0.05f, player.BoxCol.size.y - 0.05f), 0, LayerMask.GetMask("Wall"));
            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == collision.gameObject)
                {
                    player.WallState.wallDirection = "left";
                    player.TransitionToState(player.WallState);
                }
            }
        }

        if (Input.GetAxisRaw("Horizontal") != -1)
        {
            //To the Right
            hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) + new Vector2(player.BoxCol.size.x / 2 + player.BoxCol.offset.x - 0.025f, -player.BoxCol.offset.y), new Vector2(0.05f, player.BoxCol.size.y - 0.05f), 0, LayerMask.GetMask("Wall"));
            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == collision.gameObject)
                {
                    player.WallState.wallDirection = "right";
                    player.TransitionToState(player.WallState);
                }
            }
        }
    }

    public override void OnCollisionExit2D(PlayerController_FSM player, Collision2D collision)
    {

    }

    public override void Update(PlayerController_FSM player)
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }

        if (player.Rigidbody.velocity.y > 0)
        {
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_JUMPING_UP);
        }
        else
        {
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_JUMPING_DOWN);
        }
    }
}

