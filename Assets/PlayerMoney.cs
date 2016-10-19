using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMoney : MonoBehaviour {
    Text moneyText;
    public Global playerstats;

	// Use this for initialization
	void Start () {
        moneyText = GetComponent<Text>();
		//playerstats = GameObject.FindGameObjectWithTag("Player").GetComponent<Global>();//GetComponentInParent<Global>();
    }

    // Update is called once per frame
    void Update () {
		if (!playerstats) {
			GameObject temp = GameObject.FindGameObjectWithTag("Player");
			if (!temp)
			{
				print("none found.");
				return;
			}
			playerstats = temp.GetComponent<Global>();
		}
		moneyText.text = "Money: " + playerstats.currentMoney + "(" + playerstats.moneyDiff + ")";
	}
    //Because we're breaking down our players' income by days, we don't want to be updating our money all the time.
    void DayElapsed() {

        
    }
}
