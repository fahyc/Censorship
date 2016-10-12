﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Submenu : UIItem {
	public SpawnScript buttons;
	public string tooltips; //use [idea] in the string to dynamically show the name of the idea this button will spawn
	public Spawnable product;

	public UnityAction onClick;

	public UnityEvent[] AdditionalOnClicks;

	

	// Use this for initialization
	void Start () {
		for(int i = 0; i < IdeaList.staticList.Length; i++)
		{
			SpawnScript temp = Instantiate<SpawnScript>(buttons);
			temp.Initiate(tooltips.Replace("[idea]", IdeaList.staticList[i].name), IdeaList.staticList[i].color,product, i);
			temp.transform.parent = transform;
			//onClick = AdditionalOnClicks[0].;
			temp.GetComponent<Button>().onClick.AddListener(() => { AdditionalOnClicks[0].Invoke(); });
		}
	}


	public override void Enable()
	{
		base.Enable();
		gameObject.SetActive(true);
	}
	// Update is called once per frame
	void Update () {
	
	}
}