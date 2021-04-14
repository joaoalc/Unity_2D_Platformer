using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Jump : SlimeBaseState
{
    public override void EnterState(SlimeStateController player)
    {

    }

    public override void FixedUpdate(SlimeStateController player)
    {
        if(player.Direction == "left")
        {
            player.Rigidbody.velocity = Vector3.MoveTowards(new Vector2(player.Rigidbody.velocity.x, player.Rigidbody.velocity.y), new Vector2(-player.jumpSpeed.x, player.Rigidbody.velocity.y), player.AirAcceleration / 50);
        }
        else
        {
            player.Rigidbody.velocity = Vector3.MoveTowards(new Vector2(player.Rigidbody.velocity.x, player.Rigidbody.velocity.y), new Vector2(player.jumpSpeed.x, player.Rigidbody.velocity.y), player.AirAcceleration / 50);
        }
    }

    public override void OnCollisionEnter2D(SlimeStateController controller, Collision2D collision)
    {
        if (controller.Rigidbody.velocity.y <= 0)
        {
            Vector2 pos = controller.Rigidbody.position;
            Vector2 boxSize = controller.BoxCol.size;
            Vector2 offset = controller.BoxCol.offset;

            //Slightly modified y position of colPos; hopefully detection is better
            Vector2 colPos = controller.Rigidbody.position + new Vector2(0, -controller.BoxCol.size.y / 2) + offset + new Vector2(0, 0);
            Vector2 colSize = new Vector2(boxSize.x - 0.05f, 0.05f);

            //TODO: Fix bug: Sliding on ground (state doesn't change to idle) randomly when jumping to the left; usually happens when changing height
            Collider2D[] hits = Physics2D.OverlapBoxAll(colPos, colSize, 0, LayerMask.GetMask("Wall"));
            foreach (Collider2D hit in hits)
            {
                Debug.Log("Bruh");
                controller.TransitionToState(controller.IdleState);
                return;
            }
        }
    }

    public override void OnCollisionExit2D(SlimeStateController player, Collision2D collision)
    {

    }


    // Update is called once per frame
    public override void Update(SlimeStateController controller)
    {
        //Under

        if (controller.Rigidbody.velocity.y <= 0)
        {
            Vector2 pos = controller.Rigidbody.position;
            Vector2 boxSize = controller.BoxCol.size;
            Vector2 offset = controller.BoxCol.offset;

            //Slightly modified y position of colPos; hopefully detection is better
            Vector2 colPos = controller.Rigidbody.position + new Vector2(0, -controller.BoxCol.size.y / 2) + offset + new Vector2(0, 0);
            Vector2 colSize = new Vector2(boxSize.x - 0.05f, 0.05f);

            //TODO: Fix bug: Sliding on ground (state doesn't change to idle) randomly when jumping to the left; usually happens when changing height
            Collider2D[] hits = Physics2D.OverlapBoxAll(colPos, colSize, 0, LayerMask.GetMask("Wall"));
            foreach (Collider2D hit in hits)
            {
                controller.TransitionToState(controller.IdleState);
                return;
            }
        }

        if (controller.Rigidbody.velocity.y < 0)
        {
            controller.Rigidbody.gravityScale = controller.FallGrav;
        }
        else
        {
            controller.Rigidbody.gravityScale = controller.JumpGrav;
        }
    }
}
