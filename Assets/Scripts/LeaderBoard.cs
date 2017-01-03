using UnityEngine;
using System.Collections;

public class LeaderBoard : MonoBehaviour {
    public int position;
    public string name;
    public int score;

    public LeaderBoard(int newPosition, string newName, int newScore)
    {
        position = newPosition;
        name = newName;
        score = newScore;
    }
}
