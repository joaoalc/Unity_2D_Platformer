﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStatus : MonoBehaviour
{

    public bool isOn;

    protected SpriteRenderer spr;
    protected BoxCollider2D col;

    // Start is called before the first frame update
    [SerializeField]
    protected int maxBlockHp = 2;
    protected int blockHp;

    [SerializeField]
    List<Color> colors;


    protected GameObject player;
    protected Rigidbody2D playerRb;
    protected void Start()
    {
        blockHp = maxBlockHp;
        col = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    protected bool runningCoroutine = false;

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public int TickDownHp()
    {
        blockHp--;
        if (blockHp < 0)
        {
            //Debug.Log("HP is already zero!");
            blockHp = 0;
            return 0;
        }
        if (blockHp == 0)
        {
            DestroyBlock();
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = colors[blockHp - 1];
        }
        return blockHp;
    }

    public bool ActivateBlock(Vector2 newPosition)
    {
        transform.position = newPosition;
        if (col.enabled || spr.enabled)
        {
            return false;
        }

        Collider2D[] arr = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0);
        for(int i = 0; i < arr.Length; i++)
        {
            if (arr[i].gameObject.CompareTag("Player"))
            {
                player.transform.position = new Vector3(player.transform.position.x, transform.position.y + 0.5f + ((BoxCollider2D) arr[i]).size.y / 2f, player.transform.position.z);
                playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
            }
        }

        col.enabled = true;
        spr.enabled = true;
        blockHp = maxBlockHp;
        gameObject.GetComponent<SpriteRenderer>().color = colors[blockHp - 1];
        return true;
    }

    public void DestroyBlock()
    {
        col.enabled = false;
        spr.enabled = false;
    } 
}