using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LeaderboardLine: IComparable
{
    [SerializeField]
    public int nameLength = 3;
    public string name;
    public float score;
    public int date;
    public LeaderboardLine(string name, float score, int date)
    {
        if (name.Length == 3)
        {
            this.name = name;
        }
        else
        {
            this.name = name.Substring(0, 3);
        }
        this.score = score;
        this.date = date;
    }
    /*
    public static bool operator >(LeaderboardLine thisLine, LeaderboardLine otherLine)
    {
        if(thisLine.score == otherLine.score)
        {
            return thisLine.date < otherLine.date;
        }
        return thisLine.score > otherLine.score;
    }

    public static bool operator <(LeaderboardLine thisLine, LeaderboardLine otherLine)
    {
        if (thisLine.score == otherLine.score)
        {
            return thisLine.date > otherLine.date;
        }
        return thisLine.score < otherLine.score;
    }*/

    public String PrintValues(bool shorthand)
    {
        if (!shorthand)
        {
            return "Name: " + name + " Score: " + score + " Date: " + date;
        }
        else
        {
            return name + " - " + score + " - " + date;
        }
    }

    public String PrintValues()
    {
        return "Name: " + name + " Score: " + score + " Date: " + date;
    }

    public int CompareTo(object obj)
    {
        if(object.ReferenceEquals(obj.GetType(), GetType()))
        {
            LeaderboardLine line = (LeaderboardLine) obj;
            if (line.score == score)
            {
                return date - line.date;
            }
            if (score > line.score)
            {
                return (int)(score - line.score + 1);
            }
            return (int)(score - line.score) - 1;
        }
        else
        {
            throw new NotImplementedException("Cannot compare LeaderboardLine with non LeaderboardLine");
        }
    }
}