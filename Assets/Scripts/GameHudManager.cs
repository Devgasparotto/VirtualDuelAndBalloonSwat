using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHudManager : MonoBehaviour {
    private GameObject drawText;

    private GameObject p1Mark1;
    private GameObject p1Mark2;
    private GameObject p1Mark3;

    private GameObject p2Mark1;
    private GameObject p2Mark2;
    private GameObject p2Mark3;

    private GameObject playerWinsText;
    private GameObject playAgainButton;

    private GameObject matchWinner;

    private GameObject instructions1;
    private GameObject instructions2;

    private int player1Score;
    private int player2Score;

    private bool isGameOver;

    public bool isDrawTime;

    public AudioClip shootSound;
    private AudioSource source;
    public float volLowRange = .5f;
    public float volHighRange = 1.0f;

    public float drawTimeLow = 2;
    public float drawTimeHigh = 6;


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
	public void Start () {
        player1Score = 0;
        player2Score = 0;
        isDrawTime = false;
        isGameOver = false;
        currentReadyState = ReadyState.NotReady;

        drawText = GameObject.Find("DrawIcon");
        
        p1Mark1 = GameObject.Find("P1Mark1");
        p1Mark2 = GameObject.Find("P1Mark2");
        p1Mark3 = GameObject.Find("P1Mark3");

        p2Mark1 = GameObject.Find("P2Mark1");
        p2Mark2 = GameObject.Find("P2Mark2");
        p2Mark3 = GameObject.Find("P2Mark3");

        //TODO: Hide/Show and implement related functionality
        playerWinsText = GameObject.Find("PlayerWinsText");
        playAgainButton = GameObject.Find("PlayAgainButton");

        matchWinner = GameObject.Find("MatchWinner");

        instructions1 = GameObject.Find("Instructions1");
        instructions2 = GameObject.Find("Instructions2");

        SetDrawIconVisible(false);
        SetPlayAgainVisible(false, 0);
        SetNumberOfPlayerMarks(true, 0);
        SetNumberOfPlayerMarks(false, 0);
        SetInstruction1Visible(false);
        SetInstruction2Visible(false);
        DisplayMatchWinner(false, 0);
        
        //Give players 5 seconds to adjust - replace with displaying instructions
        Invoke("DisplayInstructions1", 2);
        Invoke("DisplayInstructions2", 8);
        Invoke("StartGame", 14);
        
        InvokeRepeating("AssessDrawTime", 17, Random.Range(drawTimeLow, drawTimeHigh)); //repeat rate - random time between 3-10 seconds
    }

    private void DisplayInstructions1()
    {
        SetInstruction1Visible(true);
    }

    private void DisplayInstructions2()
    {
        SetInstruction1Visible(false);
        SetInstruction2Visible(true);
    }

    private void StartGame()
    {
        Debug.Log("Starting Game");
        SetInstruction2Visible(false);
        currentReadyState = ReadyState.ReadyOnNextAssessTime;
    }

    //GAME TIMERS
    private void AssessDrawTime()
    {
        if (!isGameOver)
        {
            if (currentReadyState == ReadyState.ReadyOnNextAssessTime)
            {
                Debug.Log("Players ready - will draw on next assess time");
                DisplayMatchWinner(false, 0);
                currentReadyState = ReadyState.ReadyForNextMatch;
            }
            else if (currentReadyState == ReadyState.ReadyForNextMatch)
            {
                //Draw
                if (!isDrawTime) //Condition here to stop repeating
                {
                    Debug.Log("Draw Time!");
                    SetDrawIconVisible(true);
                }
            }
        }
    }

    private void PlayerHasShot()
    {
        //Reset Draw Counter
        currentReadyState = ReadyState.ReadyOnNextAssessTime;
        SetDrawIconVisible(false);
        float vol = volHighRange;
        source.PlayOneShot(shootSound, vol);
    }


    //WIN CONDITIONS

    public void DisplayMatchWinner(bool visible, int winningPlayer)
    {
        matchWinner.SetActive(visible);
        if (visible)
        {
            Text text = matchWinner.GetComponent<Text>();
            text.text = "Player " + winningPlayer.ToString() + " won the match";
        }
    }

    public void Player1WinsMatch()
    {
        PlayerHasShot();
        Debug.Log("Player 1 Wins Match");
        if (player1Score + 1 == 3)
        {
            player1Score += 1;
            SetNumberOfPlayerMarks(true, player1Score);
            Player1WinsGame();
        }
        else
        {
            player1Score += 1;
            SetNumberOfPlayerMarks(true, player1Score);
            DisplayMatchWinner(true, 1);
        }
    }

    public void Player2WinsMatch()
    {
        PlayerHasShot();
        Debug.Log("Player 2 Wins Match");
        if (player2Score + 1 == 3)
        {
            Player2WinsGame();
            player2Score += 1;
            SetNumberOfPlayerMarks(false, player2Score);
        }
        else
        {
            player2Score += 1;
            SetNumberOfPlayerMarks(false, player2Score);
            DisplayMatchWinner(true, 2);
        }
    }

    public void Player1WinsGame()
    {
        isDrawTime = false;
        Debug.Log("Player 1 Wins Game");
        GameOver(1);
    }

    public void Player2WinsGame()
    {
        isDrawTime = false;
        Debug.Log("Player 2 Wins Game");
        GameOver(2);
    }

    private void GameOver(int winningPlayer)
    {
        isGameOver = true;
        SetPlayAgainVisible(true, winningPlayer);
    }

    public void DetectionError()
    {
        Debug.Log("Duel was too close - try again");
        matchWinner.SetActive(true);
        Text text = matchWinner.GetComponent<Text>();
        text.text = "Both guns misfired - Try Again";
        //Reset Draw Counter
        currentReadyState = ReadyState.ReadyOnNextAssessTime;
        SetDrawIconVisible(false);
    }


    //HUD

    public void SetDrawIconVisible(bool visible)
    {
        isDrawTime = visible;
        drawText.SetActive(visible);
    }

    public void SetPlayAgainVisible(bool visible, int winningPlayer)
    {
        playerWinsText.SetActive(visible);
        if (visible)
        {
            Text text = playerWinsText.GetComponent<Text>();
            text.text = "Player " + winningPlayer.ToString() + " Wins!";
        }
        playAgainButton.SetActive(visible);
    }

    public void SetInstruction1Visible(bool visible)
    {
        instructions1.SetActive(visible);
    }

    public void SetInstruction2Visible(bool visible)
    {
        instructions2.SetActive(visible);
    }

    public void SetNumberOfPlayerMarks(bool isPlayer1, int numOfMarks)
    {
        switch(numOfMarks){
            case 3:
                if (isPlayer1)
                {
                    p1Mark1.SetActive(true);
                    p1Mark2.SetActive(true);
                    p1Mark3.SetActive(true);
                }
                else
                {
                    p2Mark1.SetActive(true);
                    p2Mark2.SetActive(true);
                    p2Mark3.SetActive(true);
                }
                break;
            case 2:
                if (isPlayer1)
                {
                    p1Mark1.SetActive(true);
                    p1Mark2.SetActive(true);
                    p1Mark3.SetActive(false);
                }
                else
                {
                    p2Mark1.SetActive(true);
                    p2Mark2.SetActive(true);
                    p1Mark3.SetActive(false);
                }
                break;
            case 1:
                if (isPlayer1)
                {
                    p1Mark1.SetActive(true);
                    p1Mark2.SetActive(false);
                    p1Mark3.SetActive(false);
                }
                else
                {
                    p2Mark1.SetActive(true);
                    p2Mark2.SetActive(false);
                    p2Mark3.SetActive(false);
                }
                break;
            default:
                if (isPlayer1)
                {
                    p1Mark1.SetActive(false);
                    p1Mark2.SetActive(false);
                    p1Mark3.SetActive(false);
                }
                else
                {
                    p2Mark1.SetActive(false);
                    p2Mark2.SetActive(false);
                    p2Mark3.SetActive(false);
                }
                break;
        }
    }



}
