using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using System;
public class GameEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void EndGame(GameObject player, float time)
    {

        GameObject scoreControllerObj = GetHighScoreObject();
        if (scoreControllerObj == null)
        {
            scoreControllerObj = CreateHighScoreController();
        }

        ScoreController scoreController = scoreControllerObj.GetComponent<ScoreController>();
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        int secondsSinceEpoch = (int)t.TotalSeconds;
        LeaderboardLine newScore = new LeaderboardLine(System.Environment.MachineName, time, secondsSinceEpoch);

        //Show results
        GameObject.FindGameObjectWithTag("EndUI").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("TimeText2").GetComponent<TextMeshProUGUI>().text = time.ToString() + " seconds.";

        if (scoreController.IsHighScore(newScore))
        {
            scoreController.AddRecord(newScore);
            //Show that it's a high score
            GameObject.Find("HighScoreText").GetComponent<TextMeshProUGUI>().text = "New High Score!";
            Debug.Log("High score!");
        }

        //player.SetActive(false);
    }


    private static GameObject GetHighScoreObject()
    {
        return GameObject.Find("ScoreController");
    }

    private static GameObject CreateHighScoreController()
    {
        return Instantiate((GameObject)Resources.Load("LeaderboardLinePrefab/ScoreController"));
    }
}
