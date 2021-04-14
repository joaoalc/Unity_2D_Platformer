using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerController_FSM player);
    public abstract void Update(PlayerController_FSM player);
    public abstract void FixedUpdate(PlayerController_FSM player);
    public abstract void OnCollisionEnter2D(PlayerController_FSM player, Collision2D collision);
    public abstract void OnCollisionExit2D(PlayerController_FSM player, Collision2D collision);
}
