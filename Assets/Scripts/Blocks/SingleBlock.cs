using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBlock : BlockStatus
{

    [SerializeField]
    protected float timeBetweenTicks = 1.25f;
    //TODO: Change time to respawn back
    protected float timeToRespawn = 5f;

    // Update is called once per frame
    protected override void Update()
    {
        if (!runningCoroutine)
        {
            if (blockHp > 0)
            {
                StartCoroutine(ExampleCoroutine());
            }
            else
            {
                StartCoroutine(RespawnCoroutine());
            }
        }
    }

    protected IEnumerator ExampleCoroutine()
    {
        runningCoroutine = true;
        yield return new WaitForSeconds(timeBetweenTicks);
        TickDownHp();
        runningCoroutine = false;
    }

    protected IEnumerator RespawnCoroutine()
    {
        runningCoroutine = true;
        yield return new WaitForSeconds(timeToRespawn);
        ActivateBlock(transform.position);
        runningCoroutine = false;
    }
}
