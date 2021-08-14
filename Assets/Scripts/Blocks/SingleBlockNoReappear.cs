using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SingleBlockNoReappear : BlockStatus
{

    [SerializeField]
    protected float timeBetweenTicks = 1.25f;

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
                Destroy(gameObject);
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

}
