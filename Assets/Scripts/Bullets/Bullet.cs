using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{

    protected InvincibilityFrames playerIFrames;

    [SerializeField] protected Vector2 knockbackForce;

    // Start is called before the first frame update
    protected void Start()
    {
        playerIFrames = GameObject.FindGameObjectWithTag("Player").GetComponent<InvincibilityFrames>();
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
