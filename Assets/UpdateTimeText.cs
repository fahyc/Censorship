using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateTimeText : MonoBehaviour {
    Text dayRef;
    TimeBehavior timeKeeper;
    int lastday;
	// Use this for initialization
	void Start () {
        dayRef = GetComponent<Text>();
        timeKeeper = GameObject.FindGameObjectWithTag("Time").GetComponent<TimeBehavior>();
        lastday = 0;
	}
	
	// Update is called once per frame
	void Update () {
        dayRef.text = "DAY: " + timeKeeper.day;
        if (timeKeeper.day != lastday)
        {
            GetComponentInParent<Global>().addIncome();
        }
        lastday = timeKeeper.day;
	}
}
