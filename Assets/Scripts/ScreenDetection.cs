using UnityEngine;

using System.Collections;


public class ScreenDetection : MonoBehaviour
{
    public bool selectable = false;

    private bool canReload = false;

    public GameObject gameManager;
    private ScoreGameHudManager gameHudManager;

    void Start()
    {
        canReload = false;
        Invoke("DelayedStartSetup", 0.1f);
    }

    void DelayedStartSetup()
    {
        gameHudManager = (ScoreGameHudManager)gameManager.GetComponent(typeof(ScoreGameHudManager));
        canReload = false;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.transform.parent.parent.name.ToString() == "HandModels")
        {
            canReload = true;
        }
    }

    void OnTriggerExit(Collider c){

        if (canReload)
        {
            gameHudManager.Reload(); //Error here
            canReload = false;
        }
    }
}