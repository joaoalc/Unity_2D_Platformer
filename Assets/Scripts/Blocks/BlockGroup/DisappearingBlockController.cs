using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    //[SerializeField]
    float speedMultiplier = 1f;
    [SerializeField]
    float startTime = 200;
    [SerializeField]
    float maxTime = 600;
    float time;
    //From settings
    int difficulty;


    #region blockBounds
    public static readonly float minX = -8;
    public static readonly float maxX = 8;
    public static readonly float minY = -26.5f;
    public static readonly float maxY = -18.5f;
    #endregion

    #region UI
    Slider slider; //TODO: Give tag and search through tag
    TextMeshProUGUI timeText; //TODO: Give tag and search through tag
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.FindGameObjectWithTag("TimeSlider").GetComponent<Slider>();
        timeText = GameObject.FindGameObjectWithTag("TimeText").GetComponent<TextMeshProUGUI>();

        difficulty = PlayerPrefs.GetInt(DifficultySelect.difficulty);
        speedMultiplier = 0.63333f + 0.06666f * (float) difficulty;
        maxTime *= 0.95f + 0.016666f * (float)difficulty;


        blocks = transform.GetComponentsInChildren<BlockStatus>();
        player = GameObject.FindGameObjectWithTag("Player");
        for(int i = 0; i < blocks.Length; i++)
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
        yield return new WaitForSeconds(times[counter % times_per_loop] * startTime / (speedMultiplier * time));
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
        timeText.text = (time - startTime).ToString();
        slider.value = (time - startTime) / (maxTime - startTime);

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
            float height = 3.5f;
            float width = 5.5f;
            float angle = Random.Range(0f, 2f * Mathf.PI);
            float lambda = Random.Range(0f, 1f);
            Vector2 offset = new Vector2(0f, 0.20f);
            diff = new Vector2(Mathf.Cos(angle) * width * lambda / 2, Mathf.Sin(angle) * height * lambda / 2);

            if(diff.y < height / 2.5f)
            {
                diff = new Vector2(diff.x, diff.y * 2);
            }
            if(diff.x < width / 3f)
            {
                diff = new Vector2(diff.x * 2.75f, diff.y);
            }
            

            //diff check
            if(Mathf.Abs(diff.x) > width - 0.5f - diff.y / 2)
            {
                valid = false;
                numTries++;
                continue;
            }

            //diff = new Vector2(Random.Range(-4f, 4f), Random.Range(-3f, 3f));
            newPos = basePos + diff + offset;
            
            if (Vector2.Distance(player.transform.position, newPos) < 1f)
                valid = false;
            else if(player.transform.position.y - newPos.y < 2 && player.transform.position.y - newPos.y > 0)
            {
                if((player.transform.position.x - newPos.x) < 0.5f && (player.transform.position.x - newPos.x) > -0.5f)
                    valid = false;
            }
            for(int i = 0; i < blockObjs.Count; i++)
            {
                if (blocks[i].IsOn() && Vector2.Distance(blockObjs[i].transform.position, newPos) < 1.5f && blocks[i].blockHp >= 2 || blocks[i].IsOn() && Vector2.Distance(blockObjs[i].transform.position, newPos) < 0.75f)
                    valid = false;
            }
            if (!IsInBounds(newPos))
            {
                valid = false;
            }
            numTries++;
            if(numTries >= 25 && !valid)
            {
                diff = new Vector2(Random.Range(-3.5f, 3.5f), Random.Range(-3f, 2.5f));
                newPos = new Vector2(player.transform.position.x, player.transform.position.y) + diff;
                valid = true;
                if (!IsInBounds(newPos))
                {
                    Vector2 tempPos = NewBlockPos(newPos);
                    if (tempPos != newPos)
                    {
                        newPos = tempPos;
                    }
                    else
                    {
                        valid = false;
                    }
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
            Debug.Log(" Num tries:" + numTries + "Couldn't generate block position that respected the rules, picked one that doesn't respect the rules.");
        }
        //Debug.Log("Num tries: " + numTries);

        return newPos;

    }

    public bool IsInBounds(Vector2 blockPos)
    {
        if ((blockPos.x < maxX && blockPos.x > minX) && (blockPos.y < maxY && blockPos.y > minY))
        {
            return true;
        }
        return false;
    }

    Vector2 FindNearestBlockPos()
    {/*
        bool used = false;
        Vector2 minDist = new Vector2(999, 999);
        for(int i = 0; i < blockObjs.Count; i++)
        {
            if(Vector2.Distance(player.transform.position, blockObjs[i].transform.position) < minDist.magnitude && Vector2.Distance(player.transform.position, blockObjs[i].transform.position) < 3.5f)
            {
                if (player.transform.position.y > blockObjs[i].transform.position.y + 2)
                {
                    minDist = blockObjs[i].transform.position;
                    used = true;
                }
            }
        }
        if (!used)
        {*/
            return new Vector2(player.transform.position.x, player.transform.position.y - 0.5f);
        //}
        //return minDist;
    }

    Vector2 NewBlockPos(Vector2 oldBlockPos)
    {
        if(oldBlockPos.y > maxY)
        {
            return new Vector2(oldBlockPos.x, Random.Range(minY, maxY));
        }
        return oldBlockPos;
    }

}
