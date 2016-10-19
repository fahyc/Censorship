using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateTimeText : MonoBehaviour {
    Text dayRef;
    public TimeBehavior timeKeeper;
	Global global;
    int lastday;
	// Use this for initialization
	void Start () {
        dayRef = GetComponent<Text>();
    //    timeKeeper = GameObject.FindGameObjectWithTag("Player").GetComponent<TimeBehavior>();
        lastday = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!timeKeeper)
		{
			GameObject temp = GameObject.FindGameObjectWithTag("Player");
			if (!temp)
			{
				//print("none found.");
				return;
			}
			timeKeeper = temp.GetComponent<TimeBehavior>(); 
		}
		if (!global)
		{
			global = timeKeeper.GetComponent<Global>();
		}
        dayRef.text = "DAY: " + timeKeeper.day;
        if (timeKeeper.day != lastday)
        {
            global.addIncome();
        }
        lastday = timeKeeper.day;
	}
}
