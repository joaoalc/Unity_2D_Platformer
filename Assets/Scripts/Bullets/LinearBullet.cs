using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : Bullet
{
    public Vector2 speed = Vector2.zero;
    public float lifespan = 0;

    bool active;

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
                active = false;
            }
        }
    }

    override
    public void Shoot(Vector2 position, float maxLifespan, Vector2 speed)
    {
        if (!active) { 
            transform.position = position;
            this.speed = speed;
            active = true;
            this.lifespan = maxLifespan;
        }
    }

    override
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if(this.speed.x > 0)
            {
                if(col.GetComponent<Rigidbody2D>().velocity.y < 0) { 
                    col.GetComponent<Rigidbody2D>().velocity = new Vector2(col.GetComponent<Rigidbody2D>().velocity.x, 0);
                }
                col.GetComponent<Rigidbody2D>().AddForce(new Vector2(350, 200));
            }
            else
            {
                if (col.GetComponent<Rigidbody2D>().velocity.y < 0)
                {
                    col.GetComponent<Rigidbody2D>().velocity = new Vector2(col.GetComponent<Rigidbody2D>().velocity.x, 0);
                }
                col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-350, 200));
            }
        }
    }

}
