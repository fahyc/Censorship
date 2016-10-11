using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateTimeText : MonoBehaviour {
    Text dayRef;
    TimeBehavior timeKeeper;
	// Use this for initialization
	void Start () {
        dayRef = GetComponent<Text>();
        timeKeeper = GameObject.FindGameObjectWithTag("Time").GetComponent<TimeBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
        dayRef.text = "DAY: " + timeKeeper.day;
	}
}
