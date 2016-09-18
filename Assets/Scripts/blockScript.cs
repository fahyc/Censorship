using UnityEngine;
using System.Collections;

public class blockScript : MonoBehaviour {
	public string sendsOut;//what idea this sends 
	public Idea ideaTemplate;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		Idea idea = col.GetComponent<Idea>();
		if (idea && idea.ideaStr != sendsOut)
		{
			Vector3 dest = idea.origin;
			Destroy(idea.gameObject);
			Idea temp = GameObject.Instantiate<Idea>(ideaTemplate);
			temp.ideaStr = sendsOut;
			temp.origin = transform.position;
			temp.destination = dest;
			temp.transform.position = transform.position;
		}

	}
}
