using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaboleBullet : Bullet
{

    float lifespan = 0;

    bool active;

    public Rigidbody2D rb2d;
    public Collider2D col;
    public SpriteRenderer spr;

    //TODO: Move attribute to special class in player that contains bullet stats
    [SerializeField]
    float gravity = 0;

    public bool getActive()
    {
        return active;
    }

    public void Deactivate()
    {
        active = false;
        spr.enabled = false;
        col.enabled = false;
        rb2d.gravityScale = 0;
        rb2d.velocity = Vector2.zero;
    }

    public void Activate()
    {
        active = true;
        spr.enabled = true;
        col.enabled = true;

        //TODO: Move attribute to special class in player that contains bullet stats
        rb2d.gravityScale = gravity;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<BoxCollider2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();

        if (knockbackForce == null || knockbackForce == Vector2.zero)
        {
            knockbackForce = new Vector2(350, 200);
        }
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            //transform.position += new Vector3(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0);
            lifespan -= Time.deltaTime;
            if (lifespan < 0)
            {
                Deactivate();
            }
        }
    }
    override
    public void Shoot(Vector2 position, float maxLifespan, Vector2 initialSpeed)
    {
        if (!active)
        {
            transform.position = position;
            Activate();
            this.lifespan = maxLifespan;
            rb2d.velocity = initialSpeed;
        }
    }

    override
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (playerIFrames.TriggerInvincibility())
            {
                if (rb2d.velocity.y > 0)
                {
                    if (col.GetComponent<Rigidbody2D>().velocity.y < 0)
                    {
                        col.GetComponent<Rigidbody2D>().velocity = new Vector2(col.GetComponent<Rigidbody2D>().velocity.x, 0);
                    }
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-knockbackForce.x, knockbackForce.y));
                    playerIFrames.TriggerInvincibility();
                    Deactivate();
                }
                else
                {
                    if (col.GetComponent<Rigidbody2D>().velocity.y < 0)
                    {
                        col.GetComponent<Rigidbody2D>().velocity = new Vector2(col.GetComponent<Rigidbody2D>().velocity.x, 0);
                    }
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockbackForce.x, knockbackForce.y));
                    playerIFrames.TriggerInvincibility();
                    Deactivate();
                }
            }
        }
    }

    /*
    override
    public void Shoot(Vector2 position, float maxLifespan, float gravity, Vector2 initialSpeed) {
        if (!active)
        {
            transform.position = position;
            Activate();
            this.lifespan = maxLifespan;
            rb2d.gravityScale = gravity;
            rb2d.velocity = initialSpeed;
        }
    }*/

}
