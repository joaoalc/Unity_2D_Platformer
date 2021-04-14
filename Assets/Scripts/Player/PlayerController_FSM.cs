using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_FSM : MonoBehaviour
{
    
    public Vector2 jumpForce = new Vector2(0, 1500);

    private BoxCollider2D boxCol;
    public BoxCollider2D BoxCol
    {
        get { return boxCol; }
    }

    [SerializeField] float acceleration = 0.1f;

    public float Acceleration
    {
        get { return acceleration; }
    }

    [SerializeField] private float airAcceleration;
    public float AirAcceleration
    {
        get { return airAcceleration; }
    }

    [SerializeField] private float wallAcceleration;

    public float WallAcceleration
    {
        get { return wallAcceleration; }
    }

    [SerializeField] float jumpGrav = 2f;
    public float JumpGrav
    {
        get { return jumpGrav; }
    }

    [SerializeField] float moveSpeed = 2.5f;
    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    [SerializeField] float noLongerJumpingUpGrav = 3.5f;
    public float NoLongerJumpingUpGrav
    {
        get { return noLongerJumpingUpGrav; }
    }

    [SerializeField] float fallGrav = 2.5f;
    public float FallGrav
    {
        get { return fallGrav; }
    }

    [SerializeField] float wallFallSpeed = 1f;

    public float WallFallSpeed
    {
        get { return wallFallSpeed; }
    }

    [SerializeField] float wallSlideSpeed = 1.5f;

    public float WallSlideSpeed
    {
        get { return wallSlideSpeed; }
    }

    private PlayerBaseState currentState;

    public PlayerBaseState CurrentState
    {
        get { return currentState;  }
    }

    private Rigidbody2D rigidbody;
    public Rigidbody2D Rigidbody
    {
        get { return rigidbody; }
    }

    //Wall jump related 
    #region 
    //Time given to the player to jump off of a wall after pressing the opposite direction
    [SerializeField] private float wallJumpGracePeriod = 0.1f;
    public float WallJumpGracePeriod
    {
        get { return wallJumpGracePeriod; }
    }
    private float currentWallJumpGrace = 0f;

    public float CurrentWallJumpGrace
    {
        get { return currentWallJumpGrace; }
        set { currentWallJumpGrace = value; }
    }
    
    [SerializeField] private Vector2 wallJumpForce;

    public Vector2 WallJumpForce
    {
        get { return wallJumpForce; }
    }
    #endregion
    
    [SerializeField] float detectGroundOnJump = 0.05f;

    public float DetectGroundOnJump
    {
        get { return detectGroundOnJump; }
    }

    float detectGroundOnJumpRemaining = 0;

    public float DetectGroundOnJumpRemaining
    {
        get { return detectGroundOnJumpRemaining; }
        set { detectGroundOnJumpRemaining = value; }
    }

    public readonly PlayerMovingState MovingState = new PlayerMovingState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerWallState WallState = new PlayerWallState();


    #region 
    PlayerAnimationController animationController;

    public PlayerAnimationController AnimationController
    {
        get { return animationController; }
    }

    #endregion

    private void Awake()
    {
        animationController = GetComponent<PlayerAnimationController>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        boxCol = gameObject.GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionToState(MovingState);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Rigidbody.gravityScale);
        //Debug.Log(currentState.ToString());
        if (CurrentWallJumpGrace > 0)
        {
            CurrentWallJumpGrace -= Time.deltaTime;
        }
        else
        {
            CurrentWallJumpGrace = 0;
        }
        
        currentState.Update(this);

        if (detectGroundOnJumpRemaining > 0)
        {
            detectGroundOnJumpRemaining -= Time.deltaTime;
        }
        else
        {
            detectGroundOnJumpRemaining = 0;
        }
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate(this);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(this, collision);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        currentState.OnCollisionExit2D(this, collision);
    }

    public void TransitionToState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

}
