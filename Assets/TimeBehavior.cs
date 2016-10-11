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
    public int timer;
    Global[] players;
    // Use this for initialization
    void Start () {
        hourLength = dayLength / 24.0f;
	}
	
	// Update is called once per frame
	void Update () {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");

        if (!isServer)
            return;
        Debug.Log(temp.Length);
        day = Mathf.FloorToInt(Time.time/dayLength);
        if(day != lastday) {
            /*players = new Global[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                players[i] = temp[i].GetComponent<Global>();
                players[i].RpcIncome();
            }*/
        }
        lastday = day;
	}
}
