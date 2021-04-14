using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeWaiting : SlimeBaseState
{
    public override void EnterState(SlimeStateController player)
    {
        if (player.JumpCooldown < 0)
        {
            Debug.Log(player.Rigidbody.velocity);
            JumpTorwardsPlayer(player);
        }
    }

    public override void FixedUpdate(SlimeStateController controller)
    {
        controller.Rigidbody.velocity = Vector3.MoveTowards(new Vector2(controller.Rigidbody.velocity.x, controller.Rigidbody.velocity.y), new Vector2(0, controller.Rigidbody.velocity.y), controller.Acceleration / 50);
    }

    public override void OnCollisionEnter2D(SlimeStateController player, Collision2D collision)
    {

    }

    public override void OnCollisionExit2D(SlimeStateController player, Collision2D collision)
    {
        bool falling = false;
        Vector2 pos = player.Rigidbody.position;
        Vector2 boxSize = player.BoxCol.size;
        Vector2 offset = player.BoxCol.offset;

        //Slightly modified y position of colPos; hopefully detection is better
        Vector2 colPos = player.Rigidbody.position + new Vector2(0, -player.BoxCol.size.y / 2) + offset + new Vector2(0, 0);
        Vector2 colSize = new Vector2(boxSize.x - 0.05f, 0.05f);

        //TODO: Fix bug: Sliding on ground (state doesn't change to idle) randomly when jumping to the left; usually happens when changing height
        Collider2D[] hits = Physics2D.OverlapBoxAll(colPos, colSize, 0, LayerMask.GetMask("Wall")); 
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.layer == LayerMask.GetMask("Wall"))
            {
                falling = false;
            }
        }
        //Nothing under player: Jump
        if (falling)
        {
            player.JumpCooldown = player.MaxJumpCooldown;
            player.TransitionToState(player.JumpingState);
        }
    }

    public override void Update(SlimeStateController player)
    {
        /*if (!NearPlayer(player)) {
            player.TransitionToState(player.IdleState);
            return;
        }*/
        if(player.JumpCooldown < 0)
        {
            JumpTorwardsPlayer(player);
        }


        //Check ground collision
        bool falling = false;
        Vector2 pos = player.Rigidbody.position;
        Vector2 boxSize = player.BoxCol.size;
        Vector2 offset = player.BoxCol.offset;

        //Slightly modified y position of colPos; hopefully detection is better
        Vector2 colPos = player.Rigidbody.position + new Vector2(0, -player.BoxCol.size.y / 2) + offset + new Vector2(0, 0);
        Vector2 colSize = new Vector2(boxSize.x - 0.05f, 0.05f);

        //TODO: Fix bug: Sliding on ground (state doesn't change to idle) randomly when jumping to the left; usually happens when changing height
        Collider2D[] hits = Physics2D.OverlapBoxAll(colPos, colSize, 0, LayerMask.GetMask("Wall"));
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.layer == LayerMask.GetMask("Wall"))
            {
                falling = false;
            } 
        }
        //Nothing under player: Jump
        if (falling)
        {
            player.JumpCooldown = player.MaxJumpCooldown;
            player.TransitionToState(player.JumpingState);
        }

    }

    public void JumpTorwardsPlayer(SlimeStateController player)
    {
        if (player.PlayerTransform.position.x > player.Rigidbody.position.x)
        {
            player.Rigidbody.velocity = player.jumpSpeed;
            player.Direction = "right";
        }
        else
        {
            player.Rigidbody.velocity = new Vector2(-player.jumpSpeed.x, player.jumpSpeed.y);
            player.Direction = "left";
        }
        player.JumpCooldown = player.MaxJumpCooldown;
        player.TransitionToState(player.JumpingState);
    }

}
