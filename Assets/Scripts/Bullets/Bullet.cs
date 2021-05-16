using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    virtual public void Shoot(Vector2 position, float maxLifespan, Vector2 speed) {
        Debug.LogError("Lmao no");
    }

    virtual public void Shoot(Vector2 position, float maxLifespan, float gravity, Vector2 initialSpeed)
    {
        Debug.LogError("Lmao no");

    }

    virtual protected void OnTriggerEnter2D(Collider2D col)
    {
        Debug.LogError("Lmao nice OnTriggerEnter2D");
    }

}
