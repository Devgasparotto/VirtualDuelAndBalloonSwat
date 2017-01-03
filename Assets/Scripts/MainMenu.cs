using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadHowToPlayMenu()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void LoadDuel()
    {
        SceneManager.LoadScene("DuelScene");
    }

    public void LoadBalloonSwat()
    {
        SceneManager.LoadScene("BalloonSwat");
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
