using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class Leaderboard
{
    public Leaderboard()
    {
        lines = new List<LeaderboardLine>();
    }

    public List<LeaderboardLine> lines;
}

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private string filePath = "Assets/Data/LeaderBoard/HighScores.json";
    [SerializeField]
    private int leaderboardLines = 11;



    Leaderboard leaderboard = new Leaderboard();

    //Temporary?
    GameObject linePrefab;
    [SerializeField]
    GameObject objecto;


    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadLinesFromFile();
        linePrefab = (GameObject) Resources.Load("LeaderboardLinePrefab/Line");
        if(objecto != null)
        {
            ShowRecords(linePrefab, objecto);
        }
        //TODO: Make it so it finds the leaderboard layout object if it exists
    }

    void ShowRecords(GameObject lineContainter, GameObject lineContainer)
    {
        GameObject currentline;
        int numLines = Math.Min(leaderboardLines, leaderboard.lines.Count);

        for (int i = 0; i < numLines; i++)
        {
            currentline = Instantiate(linePrefab);
            currentline.transform.parent = lineContainer.transform;
            currentline.transform.position = new Vector3(currentline.transform.position.x, i * 112.5f, currentline.transform.position.z);
            TextMeshProUGUI[] lines = currentline.GetComponentsInChildren<TextMeshProUGUI>();
            lines[0].text = leaderboard.lines[i].name;
            lines[1].text = Convert.ToString(leaderboard.lines[i].score);
            lines[2].text = Convert.ToString(leaderboard.lines[i].date);
            currentline.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public bool IsHighScore(LeaderboardLine line)
    {
        if(leaderboard.lines.Count < leaderboardLines)
        {
            return true;
        }
        leaderboard.lines.Sort();
        leaderboard.lines.Reverse();
        if(line.CompareTo(leaderboard.lines[leaderboard.lines.Count - 1]) > 0)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    public void AddRecord(LeaderboardLine line)
    {
        leaderboard.lines.Add(line);
        leaderboard.lines.Sort();
        leaderboard.lines.Reverse();
        while (leaderboard.lines.Count > leaderboardLines)
        {
            leaderboard.lines.RemoveAt(leaderboardLines);
        }
        leaderboard.lines.Sort();
        leaderboard.lines.Reverse();
        WriteLinesToFile();
    }

    public void LoadLinesFromFile()
    {
        string text = File.ReadAllText(filePath);
        leaderboard.lines.Clear();
        leaderboard = JsonUtility.FromJson<Leaderboard>(text);
        leaderboard.lines.Sort();
        leaderboard.lines.Reverse();
        //Debug.Log(leaderboard.lines.Count);
    }

    public void WriteLinesToFile()
    {
        int num = Math.Max(leaderboard.lines.Count, leaderboardLines);
        string json = JsonUtility.ToJson(leaderboard);
        var sr = File.CreateText(filePath);
        sr.WriteLine(json);
        sr.Flush();
        sr.Close();
        Debug.Log(json);
    }

    void printLines()
    {
        Debug.Log("lines");
        for(int i = 0; i < leaderboard.lines.Count; i++)
        {
            Debug.Log(leaderboard.lines[i].PrintValues());
        }
    }
}
