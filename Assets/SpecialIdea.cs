using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class SpecialIdea : Idea {

	public string type;

	public int strength;

	public HashSet<Node> visited;

	public override void Start()
	{
		base.Start();
		//ideaStr += "~" + type;
		if(visited == null)
		{//this allows an instantiate call to copy over the reference of the old hashset.
			visited = new HashSet<Node>();
		}
	}


	[Server]
	public override void CheckLifetime()
	{
		if (Mathf.Abs(time - totalTime) < minTimeToTarget)
		{
			if (dest)
			{
				Node destination = dest.GetComponent<Node>();
				if (visited.Contains(destination))
				{
					destination.receiveIdea(ideaStr);
				}
				else
				{
					visited.Add(destination);
					destination.receiveSpecial(this);
				}

				destination.receiveIdea(ideaStr);
			}
			NetworkServer.Destroy(gameObject);
		}
	}

}
