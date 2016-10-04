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
    
    // just disable stuff that is "invisible" to the host
    [Client]
    public override void OnSetLocalVisibility(bool vis)
    {
        // disable rendering first
        foreach (Renderer r in GetComponents<Renderer>())
        {
            r.enabled = vis;
        }

        // disable collision for clicking, etc
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = vis;
        }
    }

    // don't let new players see stuff
    [Server]
    public override bool OnCheckObserver(NetworkConnection conn)
    {
        return false;
    }

    [Server]
    // when rebuilding observers, only include the owner
    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
    {
        if (initialize)
        {
            // add the client authority owner, and update if not already in observer set
            observers.Add(owner);
            return true;
        }
        return false;
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
