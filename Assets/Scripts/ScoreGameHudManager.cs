using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreGameHudManager : MonoBehaviour
{
    private GameObject timer;
    private GameObject scoreText;
    private GameObject bullet1;
    private GameObject bullet2;
    private GameObject bullet3;
    private GameObject bullet4;
    private GameObject playAgainButton;
    private GameObject menuButton;
    private bool isGameOver;
    private int score;

    private GameObject highScoreText;
    private GameObject nameInputField;
    private GameObject nameInputButton;

    private GameObject position1;
    private GameObject position2;
    private GameObject position3;
    private GameObject position4;
    private GameObject position5;
    
    private GameObject name1;
    private GameObject name2;
    private GameObject name3;
    private GameObject name4;
    private GameObject name5;

    private GameObject score1;
    private GameObject score2;
    private GameObject score3;
    private GameObject score4;
    private GameObject score5;

    private List<Target> targetList = new List<Target>();

    public int ammoCount;

    public int gameSeconds;

    private int currentPosition;

    private AudioSource source;
    public AudioClip shootSound;
    
    public float volLowRange = .5f;
    public float volHighRange = 1.0f;

    public int maxAmmoCount = 4;

    private LeaderboardManager lbManager;

    private enum ReadyState
    {
        NotReady,
        ReadyOnNextAssessTime,
        ReadyForNextMatch
    }
    private ReadyState currentReadyState;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    public void Start()
    {
        
        score = 0;
        bullet1 = GameObject.Find("Bullet1");
        bullet2 = GameObject.Find("Bullet2");
        bullet3 = GameObject.Find("Bullet3");
        bullet4 = GameObject.Find("Bullet4");

        playAgainButton = GameObject.Find("PlayAgainButton");
        menuButton = GameObject.Find("MenuButton");
        timer = GameObject.Find("Timer");
        scoreText = GameObject.Find("Score");
        
        isGameOver = false;
        ammoCount = maxAmmoCount;
        InvokeRepeating("SpawnTarget", 2, 0.7f);
        InvokeRepeating("TimerCountdown", 0, 1);

        SetPlayAgainVisible(false);
        SetMenuButtonVisible(false);
        InitializeLeaderBoard();
        CreateListOfTargets();
    }

    private void InitializeLeaderBoard()
    {
        currentPosition = 0;
        lbManager = new LeaderboardManager();
        highScoreText = GameObject.Find("HighScoreText");

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

        nameInputButton = GameObject.Find("NameInputButton");
        nameInputField = GameObject.Find("NameInputField");

        SetLeaderboardVisible(false);
    }

    private void TimerCountdown()
    {
        if (gameSeconds > 0)
        {
            gameSeconds--;
            UpdateTimerDisplay();
        }
        else if(!isGameOver)
        {
            GameOver();
        }
    }

    private void SpawnTarget()
    {
        //Debug.Log("Attempting to spawn target");
        if (!isGameOver)
        {
            Target target = GetNextAvailableTarget();
            if (target != null)
            {
                target.ReleaseTarget();
                //Debug.Log("Target spawned");
            }
        }
    }

    private void CreateListOfTargets()
    {
        GameObject targetPool = GameObject.Find("TargetBalloons");
        foreach (Transform child in targetPool.transform)
        {
            Target targ = child.GetComponent<Target>();
            targetList.Add(targ);
        }
    }

    private Target GetNextAvailableTarget()
    {
        Target availableTarget = null;
        foreach (Target targ in targetList)
        {
            if (targ.frozen)
            {
                availableTarget = targ;
                return availableTarget;
            }
        }

        return availableTarget;
    }


    public void PlayerHasShot(int targetId)
    {
        if (!isGameOver)
        {
            float vol = volHighRange;
            source.PlayOneShot(shootSound, vol);
            if (ammoCount > 0)
            {
                ammoCount--;
                SetAmmoDisplay();
            }
            score++;
            UpdateScoreDisplay();
        }
    }

    public void Reload()
    {
        if (!isGameOver)
        {
            ammoCount = maxAmmoCount;
            SetAmmoDisplay();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        ResetTargets();
        SetPlayAgainVisible(true);
        SetMenuButtonVisible(true);
        currentPosition = lbManager.IsNewHighScore(score);
        if (currentPosition > 0)
        {
            GameObject newPosition = GameObject.Find("NewPosition");
            Text text = newPosition.GetComponent<Text>();
            text.text = currentPosition.ToString();
            
            DisplayHighScore(currentPosition);
        }
    }

    private void ResetTargets()
    {
        foreach (Target targ in targetList)
        {
            targ.Expire();
        }
    }

    //HUD

    private void DisplayHighScore(int position)
    {
        UpdateHighScoreText(position);
        SetLeaderboardVisible(true);
    }

    private void UpdateHighScoreText(int position)
    {
        Text text = highScoreText.GetComponent<Text>();
        text.text = "Congratulations, your score of " + score.ToString() + " earned you spot " + position.ToString() + " on the leaderboards!";

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

    private void SetTextForGameObject(GameObject gameObj, string textToDisplay)
    {
        Text text = gameObj.GetComponent<Text>();
        text.text = textToDisplay;
    }

    private void SetLeaderboardVisible(bool visible)
    {
        highScoreText.SetActive(visible);
        nameInputField.SetActive(visible);
        nameInputButton.SetActive(visible);
        
        position1.SetActive(visible);
        position2.SetActive(visible);
        position3.SetActive(visible);
        position4.SetActive(visible);
        position5.SetActive(visible);

        name1.SetActive(visible);
        name2.SetActive(visible);
        name3.SetActive(visible);
        name4.SetActive(visible);
        name5.SetActive(visible);

        score1.SetActive(visible);
        score2.SetActive(visible);
        score3.SetActive(visible);
        score4.SetActive(visible);
        score5.SetActive(visible);
    }

    public void EnterHighScoreName()
    {
        GameObject inputName = GameObject.Find("InputName");
        Text text = inputName.GetComponent<Text>();
        string inputNameString = text.ToString();
        lbManager.UpdateLeaderboard(currentPosition, inputNameString, score);
        lbManager.ReadLeaderboard();
        DisplayHighScore(currentPosition);
    }

    private void UpdateTimerDisplay()
    {
        Text text = timer.GetComponent<Text>();
        text.text = gameSeconds.ToString();
    }

    private void UpdateScoreDisplay()
    {
        Text text = scoreText.GetComponent<Text>();
        text.text = score.ToString();
    }

    public void SetPlayAgainVisible(bool visible)
    {
        playAgainButton.SetActive(visible);
    }

    public void SetMenuButtonVisible(bool visible)
    {
        menuButton.SetActive(visible);
    }

    public void SetAmmoDisplay()
    {
        switch (ammoCount)
        {
            case 4:
                bullet1.SetActive(true);
                bullet2.SetActive(true);
                bullet3.SetActive(true);
                bullet4.SetActive(true);
                break;
            case 3:
                bullet1.SetActive(true);
                bullet2.SetActive(true);
                bullet3.SetActive(true);
                bullet4.SetActive(false);
                break;
            case 2:
                bullet1.SetActive(true);
                bullet2.SetActive(true);
                bullet3.SetActive(false);
                bullet4.SetActive(false);
                break;
            case 1:
                bullet1.SetActive(true);
                bullet2.SetActive(false);
                bullet3.SetActive(false);
                bullet4.SetActive(false);
                break;
            default:
                bullet1.SetActive(false);
                bullet2.SetActive(false);
                bullet3.SetActive(false);
                bullet4.SetActive(false);
                break;
        }
        
    }



}
