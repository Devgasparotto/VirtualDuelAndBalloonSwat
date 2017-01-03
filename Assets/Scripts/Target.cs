using UnityEngine;

using System.Collections;


public class Target : MonoBehaviour
{
    private bool acceptShot = false;

    public float minXRange = 3;
    public float maxXRange = 6;

    public float minYRange = 4;
    public float maxYRange = 6;

    public float minStartingX = -3;
    public float maxStartingX = 3;

    public float minStartingY = -3;
    public float maxStartingY = 3;

    public float minSpeed = -3f;
    public float maxSpeed = 3f;

    public float minSize = 0.5f;
    public float maxSize = 1f;

    public float size;
    private Vector3 defaultScale;

    public float movementXSpeed;
    public float movementYSpeed;
    public Vector3 position;
    public float[] movementXRange = new float[2];
    public float[] movementYRange = new float[2];
    public Vector3 dormantPosition;

    public int targetId;

    public bool frozen;

    public GameObject gameManager;
    private ScoreGameHudManager gameHudManager;

    public float barrelY;
    public float barrelLeftX;
    public float barrelRightX;
    public float barrelTolerance;
    private GameObject barrel1;

    void Start(){
        SetDefaultScale();
        barrel1 = GameObject.Find("Barrel1");
        frozen = true;
        dormantPosition.x = -10;
        dormantPosition.y = -10;
        dormantPosition.z = -10;
        this.transform.position = dormantPosition;
        Invoke("DelayedStartSetup", 0.5f);
        InvokeRepeating("CheckAmmo", 1, 0.1f);
        InvokeRepeating("UpdatePosition", 0, 0.01f);
        InvokeRepeating("Expire", 0, 7f);
    }

    void DelayedStartSetup()
    {
        gameHudManager = (ScoreGameHudManager)gameManager.GetComponent(typeof(ScoreGameHudManager));
    }

    void SetDefaultScale()
    {
        defaultScale = this.transform.localScale;
    }

    public void ReleaseTarget()
    {
        frozen = false;
        
        size = Random.Range(minSize, maxSize);
        this.transform.localScale = defaultScale;
        Vector3 newScale = defaultScale;
        newScale.x = size * newScale.x;
        newScale.y = size * newScale.x;
        newScale.z = size * newScale.x;
        this.transform.localScale = newScale;

        movementXSpeed = Random.Range(minSpeed, maxSpeed)*0.03f;
        movementYSpeed = Random.Range(minSpeed, maxSpeed) * 0.03f;

        Vector3 currentPosition = new Vector3();
        currentPosition.x = Random.Range(minStartingX, maxStartingX);
        currentPosition.y = Random.Range(minStartingY, maxStartingY);
        currentPosition.z = 3.3f;
        position = currentPosition;
        this.transform.position = currentPosition;

        movementXRange[0] = currentPosition.x - 4f;//Random.Range(minXRange, maxXRange);
        movementXRange[1] = currentPosition.x + 4f;//Random.Range(minXRange, maxXRange);

        movementYRange[0] = currentPosition.y - 1.5f;
        movementYRange[1] = currentPosition.y + 2f;
    }

    void OnTriggerEnter(Collider c)
    {
        if (acceptShot)
        {
            //Debug.Log("Shot fired!");
            gameHudManager.PlayerHasShot(targetId);
            acceptShot = false;
            frozen = true;
            SetToDormantPosition();
        }
    }

    public void Expire()
    {
        if (!frozen)
        {
            //Debug.Log("Target Expired");
            acceptShot = false;
            frozen = true;
            SetToDormantPosition();
        }
    }

    public void SetToDormantPosition()
    {
        this.transform.position = dormantPosition;
    }


    //Ammo Handling
    private void CheckAmmo()
    {
        if (!frozen)
        {
            if (gameHudManager.ammoCount < 1)
            {
                acceptShot = false;
            }
            else
            {
                acceptShot = true;
            }
        }
    }


    //MOVEMENT
    private void UpdatePosition()
    {
        if (!frozen)
        {
            UpdateXPosition();
            UpdateYPosition();
        }
    }

    private void UpdateXPosition(){
        Vector3 currentPosition = this.transform.position;
        if (currentPosition.x <= movementXRange[0]) //too far left
        {
            movementXSpeed = movementXSpeed * -1;
        }
        else if (currentPosition.x >= movementXRange[1]) //too far right
        {
            movementXSpeed = movementXSpeed * -1;
        }

        currentPosition.x += movementXSpeed;
        this.transform.position = currentPosition;
    }

    private void UpdateYPosition()
    {
        Vector3 currentPosition = this.transform.position;
        if (currentPosition.y <= movementYRange[0]) //too far down
        {
            movementYSpeed = movementYSpeed * -1;
        }
        else if (currentPosition.y >= movementYRange[1]) //too far up
        {
            movementYSpeed = movementYSpeed * -1;
        }

        currentPosition.y += movementYSpeed;
        this.transform.position = currentPosition;
    }

    private bool InBarrelRange()
    {
        Vector3 currentPosition = this.transform.position;
        if (currentPosition.x > (barrelLeftX - barrelTolerance) && currentPosition.x < (barrelRightX + barrelTolerance) && currentPosition.y < (barrelY + barrelTolerance)) //if behind barrel
        {
            return true;
        }
        return false;
    }
}