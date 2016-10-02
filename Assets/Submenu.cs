using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Submenu : MonoBehaviour {
	public SpawnScript buttons;
	public string tooltips; //use [idea] in the string to dynamically show the name of the idea this button will spawn
	public Spawnable product;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < IdeaList.staticList.Length; i++)
		{
			SpawnScript temp = Instantiate<SpawnScript>(buttons);
			temp.Initiate(tooltips.Replace("[idea]", IdeaList.staticList[i].name), IdeaList.staticList[i].color,product, i);
			temp.transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
