using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityFrames : MonoBehaviour
{

    float maxIFrames = 0.75f;
    float iFrames;

    bool isInvincible = false;
    public bool IsInvincible
    {
        get { return isInvincible; }
    }


    // Start is called before the first frame update
    void Start()
    {
        iFrames = maxIFrames;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            iFrames -= Time.deltaTime;
        }   
        if(iFrames <= 0)
        {
            isInvincible = false;
        }
    }

    public bool TriggerInvincibility()
    {
        if (!isInvincible)
        {
            isInvincible = true;
            iFrames = maxIFrames;
            return true;
        }
        return false;
    }
}
