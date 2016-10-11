using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TimeBehavior : NetworkBehaviour {
    [SyncVar]
    public int day;
    [SyncVar]
    public int hr;

    public float dayLength;
    public int timer;
    Global[] players;
    // Use this for initialization
    void Start () {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");
        for (int i=0; i<temp.Length; i++) {
            players[i] = temp[i].GetComponent<Global>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
            return;
        day = (int)(Time.time/dayLength);
                
	}
}
