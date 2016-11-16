using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class IdeaDescription : MonoBehaviour {

	Text text;

    // Use this for initialization
    public IEnumerator Start () {
		text = GetComponent<Text>();

        yield return new WaitUntil (() => Global.isReady());   // wait until global is ready to initialize
        yield return new WaitUntil (() => IdeaList.isReady());   // wait until global is ready to initialize

		AbstractIdea idea = IdeaList.instance.list[Global.localPlayer.playerIdeaIndex];

		text.text = text.text.Replace("[idea]", idea.name).Replace("[color]", idea.colorHex()).Replace("[description]", idea.description);

		GameObject[] obj = GameObject.FindGameObjectsWithTag("DynamicColor");
		for(int i = 0; i< obj.Length; i++)
		{
			obj[i].GetComponent<Image>().color = idea.color;
		}
	}
}