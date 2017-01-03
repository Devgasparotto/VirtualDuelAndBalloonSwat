using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LeaderboardManager : MonoBehaviour {
    public List<LeaderBoard> leaderboardList = new List<LeaderBoard>();

    public string path = "Duel/Duel/Assets/Saves/Leaderboard.json";

    public LeaderboardManager()
    {
        leaderboardList.Add(CreateLeaderboardInstance(1, "---", 0));
        leaderboardList.Add(CreateLeaderboardInstance(2, "---", 0));
        leaderboardList.Add(CreateLeaderboardInstance(3, "---", 0));
        leaderboardList.Add(CreateLeaderboardInstance(4, "---", 0));
        leaderboardList.Add(CreateLeaderboardInstance(5, "---", 0));

        #if UNITY_STANDALONE_WIN
            path = "Duel/Duel/Assets/Saves/Leaderboard.json";
        #endif
        //for unity editor: path = "Assets/Saves/Leaderboard.json"
    }

    public void ResetLeaderboard()
    {
        foreach (LeaderBoard lB in leaderboardList)
        {
            lB.name = "---";
            lB.score = 0;
        }
    }

    private LeaderBoard CreateLeaderboardInstance(int position, string name, int score)
    {
        LeaderBoard leaderBoard = new LeaderBoard(position, name, score);
        return leaderBoard;
    }

    public void ReadLeaderboard()
    {
        
        using (StreamReader r = new StreamReader(path))
        {
            string jsonText = r.ReadToEnd();
            string[] jsonEntries = jsonText.Split('}');

            for (int i = 0; i < jsonEntries.Length - 1; i++)
            {
                if(i > 0){
                    //Remove first character - ","
                    jsonEntries[i] = jsonEntries[i].Substring(1, jsonEntries[i].Length - 1);
                }
                //Add previously removed "}" to end
                jsonEntries[i] += "}";
            }

            for (int i = 0; i < leaderboardList.Count; i++)
            {
                JsonUtility.FromJsonOverwrite(jsonEntries[i], leaderboardList[i]);
            }
            
        }
    }

    private void SaveItemInfo()
    {
        string fullJSON = "";
        foreach (LeaderBoard leaderboardEntry in leaderboardList)
        {
            fullJSON += JsonUtility.ToJson(leaderboardEntry) + ",";
        }
        fullJSON = fullJSON.Remove(fullJSON.Length - 1);

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(fullJSON);
            }
        }
    }


    public int IsNewHighScore(int score){
        int currentPosition = 0;
        ReadLeaderboard();
        
        for (int i = 0; i < leaderboardList.Count; i++)
        {
            if (score > leaderboardList[i].score)
            {
                currentPosition = i + 1;
                break;
            }
        }
        return currentPosition;
    }

    public void UpdateLeaderboard(int position, string name, int score)
    {
        //Push other scores down
        for (int i = leaderboardList.Count-1; i >= position; i--)
        {
            leaderboardList[i].name = leaderboardList[i - 1].name;
            leaderboardList[i].score = leaderboardList[i - 1].score;
        }
        
        leaderboardList[position - 1] = CreateLeaderboardInstance(position, name, score);
        SaveItemInfo();
    }
}
