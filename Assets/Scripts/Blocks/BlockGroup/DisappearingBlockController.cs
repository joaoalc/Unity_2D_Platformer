using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingBlockController : MonoBehaviour
{
    [SerializeField]
    int totalNumBlocks = 6;
    [SerializeField]
    BlockStatus[] blocks;
    List<GameObject> blockObjs = new List<GameObject>();

    [SerializeField]
    List<float> times = new List<float>();
    int pointer = 0;
    int counter = 0;
    [SerializeField]
    int times_per_loop = 4;
    [SerializeField]
    int num_loops = 2;

    GameObject player;

    //[SerializeField]
    //float blocks_per_cycle = 3;

    bool runningCoroutine = false;

    [SerializeField]
    float startTime = 200;
    [SerializeField]
    float maxTime = 600;
    float time;
    

    // Start is called before the first frame update
    void Start()
    {
        blocks = transform.GetComponentsInChildren<BlockStatus>();
        player = GameObject.FindGameObjectWithTag("Player");
        for(int i = 0; i < blockObjs.Count; i++)
        {
            blockObjs.Add(blocks[i].gameObject);
        }

        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > maxTime)
        {
            time = maxTime;
        }
        if (!runningCoroutine)
        {
            StartCoroutine(NextTime());
        }
    }

    //TODO: FIX
    protected IEnumerator NextTime()
    {
        runningCoroutine = true;
        yield return new WaitForSeconds(times[counter % times_per_loop] * 200 / time);
        if ((counter + 1) % times_per_loop != 0)
        {
            //TODO: Get jumpable position from the player
            blocks[pointer].ActivateBlock(GeneratePosition());
            pointer++;
        }
        else
        {
            for(int i = 0; i < blocks.Length; i++)
            {
                blocks[i].TickDownHp();
            }
            if(counter + 1 >= times.Count * num_loops)
            {
                pointer = 0;
                counter = -1;
            }
        }
        counter++;
        runningCoroutine = false;
    }

    Vector2 GeneratePosition()
    {

        //consider the nearest block from the player's position
        //Can't be less than 1 tile from the player
        //If it's directly above the player, it can't be less than 2 tiles under and, if it is, it can't be less than .5 tiles away
        //It can't be less than .75 tiles away from another block
        //It has to be in the screen



        Vector2 basePos = FindNearestBlockPos();
        Vector2 newPos = Vector2.zero;
        Vector2 diff;
        bool valid = false;

        int numTries = 0;
        while (!valid)
        {
            valid = true;
            diff = new Vector2(Random.Range(-4f, 4f), Random.Range(-3f, 3f));
            newPos = basePos + diff;
            if (Vector2.Distance(player.transform.position, newPos) < 1)
                valid = false;
            /*else if(player.transform.position.y - newPos.y < -2)
            {
                if((player.transform.position.x - newPos.x) < 0.5f && (player.transform.position.x - newPos.x) > -0.5f)
                    valid = false;
            }*/
            for(int i = 0; i < blockObjs.Count; i++)
            {
                if (blocks[i].isOn && Vector2.Distance(blockObjs[i].transform.position, newPos) < 2f)
                    valid = false;
            }
            if (newPos.x > Camera.main.orthographicSize || newPos.x < -Camera.main.orthographicSize || newPos.y + 20 > (Camera.main.orthographicSize * 9 / 16) - 1 || newPos.y + 20 < -Camera.main.orthographicSize * 9 / 16)
            {
                valid = false;
            }
            numTries++;
            if(numTries >= 25 && !valid)
            {
                diff = new Vector2(Random.Range(-4f, 4f), Random.Range(-3f, 2.5f));
                newPos = new Vector2(player.transform.position.x, player.transform.position.y) + diff;
                valid = true;
                if (newPos.x > Camera.main.orthographicSize || newPos.x < -Camera.main.orthographicSize || newPos.y + 20 > (Camera.main.orthographicSize * 9 / 16) - 1 || newPos.y + 20 < -Camera.main.orthographicSize * 9 / 16)
                {
                    valid = false;
                }
                if(numTries >= 100 && !valid)
                {
                    valid = true;
                    break;
                }
            }
        }
        if(numTries >= 25)
        {
            Debug.Log("Couldn't generate block position that respected the rules, picked one that doesn't respect the ruels.");
        }


        return newPos;

    }

    Vector2 FindNearestBlockPos()
    {
        bool used = false;
        Vector2 minDist = new Vector2(999, 999);
        for(int i = 0; i < blockObjs.Count; i++)
        {
            if(Vector2.Distance(player.transform.position, blockObjs[i].transform.position) < minDist.magnitude && Vector2.Distance(player.transform.position, blockObjs[i].transform.position) < 3.5f)
            {
                minDist = blockObjs[i].transform.position;
                used = true;
            }
        }
        if (!used)
        {
            return new Vector2(player.transform.position.x, player.transform.position.y - 0.5f);
        }
        return minDist;
    }


}
