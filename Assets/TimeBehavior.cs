using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TimeBehavior : NetworkBehaviour {
    [SyncVar]
    public int day;
    [SyncVar]
    public int hr;

    private int lastday = 0;

    public float dayLength=1.0f;
    float hourLength;
    float startTime;
    //Global[] players;
    // Use this for initialization
    void Start () {
        hourLength = dayLength / 24.0f;
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        if (!isServer)
            return;
        day = Mathf.FloorToInt((Time.time-startTime)/dayLength);

	}
}
