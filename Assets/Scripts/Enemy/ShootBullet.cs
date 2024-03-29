﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    [SerializeField]
    float lifespan = 4;
    [SerializeField]
    float time_to_shoot = 5;
    [SerializeField]
    bool flipped = false;

    bool first_shot = true;
    bool running_coroutine = false;

    List<Bullet> bullets = new List<Bullet>();

    [SerializeField]
    Vector2 bulletSpeed = new Vector2(5, 0);


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).CompareTag("BulletHolder"))
            {
                for(int j = 0; j < transform.parent.GetChild(i).childCount; j++)
                {
                    bullets.Add(transform.parent.GetChild(i).GetChild(j).gameObject.GetComponent<Bullet>());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!running_coroutine) {
            StartCoroutine(NextTime());
        }
    }

    protected IEnumerator NextTime()
    {
        running_coroutine = true;
        if(first_shot == true) {
            yield return new WaitForSeconds(time_to_shoot);
            first_shot = false;
        }
        else
        {
            yield return new WaitForSeconds(time_to_shoot);
        }
        if (!flipped) {
            bullets[0].Shoot(transform.position, lifespan, bulletSpeed);
        }
        else
        {
            bullets[0].Shoot(transform.position, lifespan, new Vector2(-bulletSpeed.x, bulletSpeed.y));
        }
        //bullets[0].transform.localPosition = Vector3.zero;
        //bullets[0].GetComponent<Rigidbody2D>().velocity = new Vector2(5, 0);
        running_coroutine = false;
    }

    public void Flip()
    {
        flipped = !flipped;
        transform.localScale = new Vector3(-1, 1);
    }

}
