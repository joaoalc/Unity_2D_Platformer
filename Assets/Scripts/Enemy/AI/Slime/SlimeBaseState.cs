using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlimeBaseState
{
    public abstract void EnterState(SlimeStateController player);
    public abstract void Update(SlimeStateController player);
    public abstract void FixedUpdate(SlimeStateController player);
    public abstract void OnCollisionEnter2D(SlimeStateController player, Collision2D collision);
    public abstract void OnCollisionExit2D(SlimeStateController player, Collision2D collision);
}
