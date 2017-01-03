using UnityEngine;
using System.Collections;

public class ShootTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if (c != null)
        {
            Debug.Log(c.gameObject.transform.parent.name.ToString());
        }
    }
}
