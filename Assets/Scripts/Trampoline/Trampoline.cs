using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    int hp = 1;

    [SerializeField]
    Vector2 force = new Vector2(0, 2200);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hp--;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(force);
            if(hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
