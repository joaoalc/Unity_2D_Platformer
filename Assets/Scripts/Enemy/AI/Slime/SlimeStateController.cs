using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateController : MonoBehaviour
{

    #region Possible states
    private SlimeBaseState currentState;
    public SlimeBaseState CurrentState
    {
        get { return currentState; }
    }

    public readonly Slime_Idle IdleState = new Slime_Idle();
    public readonly Slime_Jump JumpingState = new Slime_Jump();
    public readonly SlimeWaiting WaitingState = new SlimeWaiting();

    #endregion

    #region Jumping values

    private float jumpCooldown;

    public float JumpCooldown
    {
        get { return jumpCooldown; }
        set { jumpCooldown = value; }
    }
    [SerializeField]
    private float startJumpCooldown = 1;
    public float StartJumpCooldown
    {
        get { return startJumpCooldown; }
        set { startJumpCooldown = value; }
    }
    [SerializeField]
    private float maxJumpCooldown = 3;

    public float MaxJumpCooldown
    {
        get { return maxJumpCooldown; }
    }

    public Vector2 jumpSpeed = new Vector2(2, 8.5f);

    [SerializeField] float airAcceleration = 20f;

    public float AirAcceleration
    {
        get { return airAcceleration; }
    }

    [SerializeField] float acceleration = 20f;

    public float Acceleration
    {
        get { return acceleration; }
    }

    private string direction;

    public string Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    #endregion


    //public Vector2 SmallJumpForce = new Vector2(4, 5.5f);

    //public Vector2 BigJumpForce = new Vector2(2, 8.5f);

    private BoxCollider2D boxCol;
    public BoxCollider2D BoxCol
    {
        get { return boxCol; }
    }

    [SerializeField] float jumpGrav = 2f;
    public float JumpGrav
    {
        get { return jumpGrav; }
    }

    [SerializeField] float fallGrav = 2.5f;
    public float FallGrav
    {
        get { return fallGrav; }
    }

    //Enemy AI stuff + player transform
    #region 

    //Hit detection in the 4 ways

    [SerializeField] float aggroRangeFacing = 4;
    public float AggroRangeFacing
    {
        get { return aggroRangeFacing; }
    }

    [SerializeField] float aggroRangeAway = 3;
    public float AggroRangeAway
    {
        get { return aggroRangeAway; }
    }

    [SerializeField] float aggroRangeUp = 4;
    public float AggroRangeUp
    {
        get { return aggroRangeUp; }
    }

    [SerializeField] float aggroRangeDown = 1f;
    public float AggroRangeDown
    {
        get { return aggroRangeDown; }
    }

    Transform playerTransform;
    public Transform PlayerTransform
    {
        get { return playerTransform; }
    }

    #endregion

    Rigidbody2D rigidbody;

    public Rigidbody2D Rigidbody
    {
        get { return rigidbody; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        jumpCooldown = startJumpCooldown;
        currentState = IdleState;
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Rigidbody.AddForce(new Vector2(300f, 175f));
        }*/
        jumpCooldown -= Time.deltaTime;
        currentState.Update(this);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate(this);
    }

    public void TransitionToState(SlimeBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public bool NearPlayer(SlimeStateController controller)
    {
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
                    return true;
                }
            }
            else
            {
                if (facingX <= controller.PlayerTransform.position.x && awayX >= controller.PlayerTransform.position.x)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
