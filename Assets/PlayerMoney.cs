using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMoney : MonoBehaviour {
    Text moneyText;
    Global playerstats;

	// Use this for initialization
	void Start () {
        moneyText = GetComponent<Text>();
        playerstats = GetComponentInParent<Global>();
    }

    // Update is called once per frame
    void Update () {
        moneyText.text = "Money: " + playerstats.currentMoney + "(+0)";
	}
    //Because we're breaking down our players' income by days, we don't want to be updating our money all the time.
    void DayElapsed() {

        
    }
}
