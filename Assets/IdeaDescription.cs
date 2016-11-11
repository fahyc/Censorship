using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IdeaDescription : MonoBehaviour {

	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();

		AbstractIdea idea = IdeaList.staticList[Global.getLocalPlayer().playerIdeaIndex];
		text.text = text.text.Replace("[idea]", idea.name).Replace("[color]", idea.colorHex()).Replace("[description]", idea.description);

		GameObject[] obj = GameObject.FindGameObjectsWithTag("DynamicColor");
		for(int i = 0; i< obj.Length; i++)
		{
			obj[i].GetComponent<Image>().color = idea.color;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
