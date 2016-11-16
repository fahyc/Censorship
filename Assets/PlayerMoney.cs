using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMoney : MonoBehaviour {
    Text moneyText;
    public Global playerstats;

	// Use this for initialization
	void Start () {
        moneyText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
		if (!playerstats) {
            if (Global.isReady())
                playerstats = Global.getLocalPlayer();

			return;
		}
		moneyText.text = "Money: " + playerstats.currentMoney + "(" + playerstats.moneyDiff + ")";
	}

}
