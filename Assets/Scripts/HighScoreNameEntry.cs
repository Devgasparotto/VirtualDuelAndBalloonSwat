using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HighScoreNameEntry : MonoBehaviour {

    public void InputNameEntry()
    {
        GameObject newPosition = GameObject.Find("NewPosition");
        Text text = newPosition.GetComponent<Text>();
        int position = Int32.Parse(text.text.ToString());
        
        GameObject inputName = GameObject.Find("InputName");
        text = inputName.GetComponent<Text>();
        string name = text.text.ToString();

        GameObject newScore = GameObject.Find("Score");
        text = newScore.GetComponent<Text>();
        int score = Int32.Parse(text.text.ToString());
        
        LeaderboardManager LBM = new LeaderboardManager();
        LBM.ResetLeaderboard();
        LBM.ReadLeaderboard();
        LBM.UpdateLeaderboard(position, name, score);

        inputName.SetActive(false);

        GameObject nameInputField = GameObject.Find("NameInputField");
        nameInputField.SetActive(false);

        GameObject nameInputButton = GameObject.Find("NameInputButton");
        nameInputButton.SetActive(false);

        UpdateLeaderboardDisplay(LBM);
    }

    void UpdateLeaderboardDisplay(LeaderboardManager lbManager)
    {
        GameObject position1;
        GameObject position2;
        GameObject position3;
        GameObject position4;
        GameObject position5;
    
        GameObject name1;
        GameObject name2;
        GameObject name3;
        GameObject name4;
        GameObject name5;

        GameObject score1;
        GameObject score2;
        GameObject score3;
        GameObject score4;
        GameObject score5;
        
        position1 = GameObject.Find("Position1");
        position2 = GameObject.Find("Position2");
        position3 = GameObject.Find("Position3");
        position4 = GameObject.Find("Position4");
        position5 = GameObject.Find("Position5");

        name1 = GameObject.Find("Name1");
        name2 = GameObject.Find("Name2");
        name3 = GameObject.Find("Name3");
        name4 = GameObject.Find("Name4");
        name5 = GameObject.Find("Name5");

        score1 = GameObject.Find("Score1");
        score2 = GameObject.Find("Score2");
        score3 = GameObject.Find("Score3");
        score4 = GameObject.Find("Score4");
        score5 = GameObject.Find("Score5");
        
        SetTextForGameObject(position1, lbManager.leaderboardList[0].position.ToString());
        SetTextForGameObject(position2, lbManager.leaderboardList[1].position.ToString());
        SetTextForGameObject(position3, lbManager.leaderboardList[2].position.ToString());
        SetTextForGameObject(position4, lbManager.leaderboardList[3].position.ToString());
        SetTextForGameObject(position5, lbManager.leaderboardList[4].position.ToString());

        SetTextForGameObject(name1, lbManager.leaderboardList[0].name.ToString());
        SetTextForGameObject(name2, lbManager.leaderboardList[1].name.ToString());
        SetTextForGameObject(name3, lbManager.leaderboardList[2].name.ToString());
        SetTextForGameObject(name4, lbManager.leaderboardList[3].name.ToString());
        SetTextForGameObject(name5, lbManager.leaderboardList[4].name.ToString());

        SetTextForGameObject(score1, lbManager.leaderboardList[0].score.ToString());
        SetTextForGameObject(score2, lbManager.leaderboardList[1].score.ToString());
        SetTextForGameObject(score3, lbManager.leaderboardList[2].score.ToString());
        SetTextForGameObject(score4, lbManager.leaderboardList[3].score.ToString());
        SetTextForGameObject(score5, lbManager.leaderboardList[4].score.ToString());
    }

    void SetTextForGameObject(GameObject gameObj, string textToDisplay)
    {
        Text text = gameObj.GetComponent<Text>();
        text.text = textToDisplay;
    }
}
