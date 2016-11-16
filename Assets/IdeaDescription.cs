using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class IdeaDescription : NetworkBehaviour {

	Text text;

    // Use this for initialization
    public override void OnStartLocalPlayer () {
		text = GetComponent<Text>();

        Global player = Global.getLocalPlayer();

		AbstractIdea idea = IdeaList.instance.list[player.playerIdeaIndex];

		text.text = text.text.Replace("[idea]", idea.name).Replace("[color]", idea.colorHex()).Replace("[description]", idea.description);

		GameObject[] obj = GameObject.FindGameObjectsWithTag("DynamicColor");
		for(int i = 0; i< obj.Length; i++)
		{
			obj[i].GetComponent<Image>().color = idea.color;
		}
	}
}
