using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : Bullet
{
    public Vector2 speed = Vector2.zero;
    public float lifespan = 0;

    public SpriteRenderer spr;
    public Collider2D col;

    bool active = false;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        spr.enabled = false;
        col.enabled = false;
        if(knockbackForce == null || knockbackForce == Vector2.zero) { 
            knockbackForce = new Vector2(350, 200);
        }
        base.Start();
    }

    public bool getActive()
    {
        return active;
    }

    private void Update()
    {
        if (active) {
            transform.position += new Vector3(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0);
            lifespan -= Time.deltaTime;
            if(lifespan < 0)
            {
                Deactivate();
            }
        }
    }

    override
    public void Shoot(Vector2 position, float maxLifespan, Vector2 speed)
    {
        if (!active) { 
            transform.position = position;
            this.speed = speed;
            Activate();
            this.lifespan = maxLifespan;
        }
    }

    public void Activate()
    {
        active = true;
        spr.enabled = true;
        col.enabled = true;
    }


    public void Deactivate()
    {
        active = false;
        spr.enabled = false;
        col.enabled = false;
    }

    override
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            setPlayerIFrames(col.gameObject);
            if (playerIFrames.TriggerInvincibility())
            {
                if (this.speed.x > 0)
                {
                    if (col.GetComponent<Rigidbody2D>().velocity.y < 0)
                    {
                        col.GetComponent<Rigidbody2D>().velocity = new Vector2(col.GetComponent<Rigidbody2D>().velocity.x, 0);
                    }
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockbackForce.x, knockbackForce.y));
                }
                else
                {
                    if (col.GetComponent<Rigidbody2D>().velocity.y < 0)
                    {
                        col.GetComponent<Rigidbody2D>().velocity = new Vector2(col.GetComponent<Rigidbody2D>().velocity.x, 0);
                    }
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-knockbackForce.x, knockbackForce.y));
                }
            }
            Deactivate();
        }
    }

}
