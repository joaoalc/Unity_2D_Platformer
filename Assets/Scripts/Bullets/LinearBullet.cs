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
}
