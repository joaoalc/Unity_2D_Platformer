using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallState : PlayerBaseState
{
    public string wallDirection = "";
    public override void EnterState(PlayerController_FSM player)
    {
        if(player.Rigidbody.velocity.y >= -0.0001)
        {
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_WALL_UP);
        }
        else
        {
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_WALL_DOWN);
        }

        player.Rigidbody.gravityScale = 0f;
        player.CurrentWallJumpGrace = 0f;
        player.Rigidbody.velocity = new Vector2(0, player.Rigidbody.velocity.y);

    }

    public override void FixedUpdate(PlayerController_FSM player)
    {
        player.Rigidbody.gravityScale = 0f;
        //Sliding while pressing down is faster, when not pressing either down or up slides at another speed, player doesnt slide while pressing up.
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            player.Rigidbody.velocity = Vector2.MoveTowards(new Vector2(0, player.Rigidbody.velocity.y), new Vector2(0, -player.WallSlideSpeed), player.WallAcceleration);
        }
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            player.Rigidbody.velocity = Vector2.MoveTowards(new Vector2(0, player.Rigidbody.velocity.y), new Vector2(0, -player.WallFallSpeed), player.WallAcceleration);
        }
        else
        {
            player.Rigidbody.velocity = Vector2.MoveTowards(new Vector2(0, player.Rigidbody.velocity.y), Vector2.zero, player.WallAcceleration);
        }
    }

    public override void OnCollisionEnter2D(PlayerController_FSM player, Collision2D collision)
    {

    }

    public override void OnCollisionExit2D(PlayerController_FSM player, Collision2D collision)
    {
        //Possible edge case?
        //Under
        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) - new Vector2(-player.BoxCol.offset.x, player.BoxCol.size.y / 2 - player.BoxCol.offset.y - 0.025f), new Vector2(player.BoxCol.size.x - 0.05f, 0.05f), 0, LayerMask.GetMask("Wall"));
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Wall") && player.Rigidbody.velocity.y < 0 && player.DetectGroundOnJumpRemaining <= 0)
            {
                player.TransitionToState(player.MovingState);
            }
        }

        player.TransitionToState(player.JumpingState);
    }

    public override void Update(PlayerController_FSM player)
    {
        if (Input.GetButtonDown("Jump"))
        {
            player.Rigidbody.velocity = new Vector2(-WallDirectionToInt() * player.WallJumpForce.x, player.WallJumpForce.y);
            player.TransitionToState(player.JumpingState);
            return;
        }
        //Under
        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) - new Vector2(-player.BoxCol.offset.x, player.BoxCol.size.y / 2 - player.BoxCol.offset.y - 0.025f), new Vector2(player.BoxCol.size.x - 0.05f, 0.05f), 0, LayerMask.GetMask("Wall"));
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Wall") && player.Rigidbody.velocity.y <= 0 && player.DetectGroundOnJumpRemaining <= 0)
            {
                player.TransitionToState(player.MovingState);
            }
        }
        if (Input.GetAxisRaw("Horizontal") == -WallDirectionToInt())
        {
            player.CurrentWallJumpGrace = player.WallJumpGracePeriod;
            player.TransitionToState(player.JumpingState);
        }

        if (player.Rigidbody.velocity.y >= -0.0001)
        {
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_WALL_UP);
        }
        else
        {
            player.AnimationController.ChangeAnimationState(PlayerAnimationController.PLAYER_WALL_DOWN);
        }
    }

    public int WallDirectionToInt()
    {
        if (wallDirection == "left")
        {
            return -1;
        }
        else if (wallDirection == "right")
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
