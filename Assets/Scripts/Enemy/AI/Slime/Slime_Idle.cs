using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Idle : SlimeBaseState
{

    // Start is called before the first frame update
    public override void EnterState(SlimeStateController controller)
    {
        //Check if should have aggro on player
        float facingX;
        float awayX;
        float upY;
        float downY;

        facingX = controller.transform.position.x + controller.AggroRangeFacing * controller.transform.localScale.x;
        awayX = controller.transform.position.x - controller.AggroRangeAway * controller.transform.localScale.x;

        upY = controller.transform.position.y + controller.AggroRangeUp;
        downY = controller.transform.position.y - controller.AggroRangeDown;

        if (upY > controller.PlayerTransform.position.y && downY < controller.PlayerTransform.position.y)
        {
            if (controller.transform.localScale.x > 0)
            {
                if (facingX >= controller.PlayerTransform.position.x && awayX <= controller.PlayerTransform.position.x)
                {
                    controller.TransitionToState(controller.WaitingState);
                }
            }
            else
            {
                if (facingX <= controller.PlayerTransform.position.x && awayX >= controller.PlayerTransform.position.x)
                {
                    controller.TransitionToState(controller.WaitingState);
                }
            }
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
        bool falling = true;
        //Under
        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) - new Vector2(0, player.BoxCol.size.y / 2 - player.BoxCol.offset.y - 0.025f), new Vector2(player.BoxCol.size.x - 0.05f, 0.05f), 0, LayerMask.GetMask("Wall"));
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
            player.TransitionToState(player.JumpingState);
        }
    }



    // Update is called once per frame
    public override void Update(SlimeStateController controller)
    {



        //Check if should have aggro on player
        float facingX;
        float awayX;
        float upY;
        float downY;

        facingX = controller.transform.position.x + controller.AggroRangeFacing * controller.transform.localScale.x;
        awayX = controller.transform.position.x - controller.AggroRangeAway * controller.transform.localScale.x;

        upY = controller.transform.position.y + controller.AggroRangeUp;
        downY = controller.transform.position.y - controller.AggroRangeDown;
        
        if(upY > controller.PlayerTransform.position.y && downY < controller.PlayerTransform.position.y) { 
            if(controller.transform.localScale.x > 0)
            {
                if(facingX >= controller.PlayerTransform.position.x && awayX <= controller.PlayerTransform.position.x)
                {
                    controller.JumpCooldown = controller.StartJumpCooldown;
                    controller.TransitionToState(controller.WaitingState);
                }
            }
            else
            {
                if(facingX <= controller.PlayerTransform.position.x && awayX >= controller.PlayerTransform.position.x)
                {
                    controller.JumpCooldown = controller.StartJumpCooldown;
                    controller.TransitionToState(controller.WaitingState);
                }
            }
        }

    }
}
