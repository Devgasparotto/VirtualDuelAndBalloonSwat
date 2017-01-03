using UnityEngine;

using System.Collections;


public class CubeInteraction : MonoBehaviour
{

    public Color c;
    public static Color selectedColor;
    public bool selectable = false;
    private bool acceptShot = false;

    public GameObject gameManager;
    private GameHudManager gameHudManager;

    void Start()
    {
        //Determine if its ok to accept trigger
        InvokeRepeating("CheckIfDrawTime", 3, 0.1f); //TODO: Verify this doesn't have any implications
    }

    void Update()
    {
        
    }

    private void CheckIfDrawTime()
    {
        gameHudManager = (GameHudManager)gameManager.GetComponent(typeof(GameHudManager));
        if (gameHudManager.isDrawTime)
        {
            acceptShot = true;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (acceptShot)
        {
            Debug.Log("Shot fired!");
            string parentName = DetermineHandByTransformParentName(c.gameObject.transform.parent.parent.name.ToString());
            
            if (parentName == "R")
            {
                //Player 1 Wins
                gameHudManager.Player2WinsMatch();
            }
            else if (parentName == "L")
            {
                //Player 2 wins
                gameHudManager.Player1WinsMatch();
            }
            else
            {
                //error
                gameHudManager.DetectionError();
            }

            acceptShot = false;
        }
    }

    //Returns R for Right hand object, returns L for Left Hand Object
    private string DetermineHandByTransformParentName(string parentName)
    {
        parentName = parentName.Replace("RigidRoundHand_", "");
        parentName = parentName.Replace("(Clone)", "");
        Debug.Log(parentName);
        return parentName; //should return R or L
    }
}