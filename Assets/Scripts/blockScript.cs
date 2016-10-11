using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class blockScript : Spawnable {
    [SyncVar]
	public string sendsOut;//what idea this sends 
	public Idea ideaTemplate;
	

	// Use this for initialization
	void Start () {
		if (index != -1) {
			GetComponent<SpriteRenderer>().color = IdeaList.staticList[index].color;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [ServerCallback]
	void OnTriggerEnter2D(Collider2D col)
	{
		Idea idea = col.GetComponent<Idea>();
		if (idea && idea.ideaStr != sendsOut && (index < 0 || idea.index == index)) 
		{
			//Vector3 dest = idea.origin;
			NetworkServer.Destroy(idea.gameObject);
			//Idea temp = GameObject.Instantiate<Idea>(ideaTemplate);
			//temp.ideaStr = sendsOut;
			//temp.origin = transform.position;
			//temp.destination = dest;
			//temp.transform.position = transform.position;
			//temp.dest = idea.originObj;
		}

	}
}
