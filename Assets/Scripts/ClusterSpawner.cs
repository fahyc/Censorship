using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ClusterSpawner : NetworkBehaviour {
	public Node node;
	public int quantity;
	public float radius;
	public float spaceBetween;
	public ClusterSpawner[] outsideConnections;

	public bool stubborn = false;

	Node[] nodes;
	public int maxTries = 5;

	public string mainIdea;//the idea the nodes in this cluster will follow.
	public float mainStrength = .5f;//the average strength nodes in this cluster will follow the idea at. 

	List<Node> waitingLinks = new List<Node>();

	// Use this for initialization
	public override void OnStartServer () {
		nodes = new Node[quantity];
        IdeaList.nodeCount += quantity;
		for(int i = 0; i < quantity; i++)
		{
			
			Vector2 point = Vector2.zero;
			
			for(int tries = 0; tries < maxTries; tries++)
			{
				Vector2 attempt = Random.insideUnitCircle * radius + transform.position.xy();
				Collider2D[] col = Physics2D.OverlapCircleAll(attempt, spaceBetween);
				if(col.Length <= 0)
				{
					point = attempt;
					break;
				}
			}
			if (point != Vector2.zero)
			{
                Node spawn = Instantiate(node);
				spawn.transform.position = point;
				nodes[i] = spawn;
                Node[] seed = new Node[i];
                for (int k=0; k < i; k++)
                {
                    seed[k] = nodes[k];
                }
				spawn.linksSeed = seed;
				float[] ideaStrengths = new float[IdeaList.staticList.Length];
				for(int j = 0; j < ideaStrengths.Length; j++)
				{
					ideaStrengths[j] = Random.Range(0f, 1f);
				}
				ideaStrengths[IdeaList.staticDict[mainIdea]] = Mathf.Max( mainStrength, ideaStrengths[IdeaList.staticDict[mainIdea]]);
				//ideaStrengths.print();
				spawn.stubborn = stubborn;
				spawn.ideaStrengths = ideaStrengths;

				NetworkServer.Spawn(spawn.gameObject);
                Debug.Log("Network spawning a node");
			}
		}

		for(int i = 0; i < outsideConnections.Length; i++)
		{
            if (outsideConnections[i] != null)
                outsideConnections[i].makeConnection(nodes[Random.Range(0, nodes.Length)]);
            else
            {
                Debug.LogWarning("connection " + i + " is null");
            }
		}

	}
		
    [ServerCallback]
	void Update() {
        for (int i = 0; i < waitingLinks.Count; i++)
        {
            nodes[Random.Range(0, nodes.Length)].linkTo(waitingLinks[i]);
        }
        waitingLinks = null;
        NetworkServer.Destroy(gameObject);
	}

	[Server]
	public void makeConnection(Node node)
	{
		waitingLinks.Add(node);
	}
	
	// Update is called once per frame
	

	/*
	This might be needed:
	static bool Intersects(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
	{
		//taken from http://forum.unity3d.com/threads/line-intersection.17384/
		Vector2 a = p2 - p1;
		Vector2 b = p3 - p4;
		Vector2 c = p1 - p3;

		float alphaNumerator = b.y * c.x - b.x * c.y;
		float alphaDenominator = a.y * b.x - a.x * b.y;
		float betaNumerator = a.x * c.y - a.y * c.x;
		float betaDenominator = a.y * b.x - a.x * b.y;

		bool doIntersect = true;

		if (alphaDenominator == 0 || betaDenominator == 0)
		{
			doIntersect = false;
		}
		else {

			if (alphaDenominator > 0)
			{
				if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
				{
					doIntersect = false;

				}
			}
			else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
			{
				doIntersect = false;
			}

			if (doIntersect && betaDenominator > 0) {
				if (betaNumerator < 0 || betaNumerator > betaDenominator)
				{
					doIntersect = false;
				}
			} else if (betaNumerator > 0 || betaNumerator < betaDenominator)
			{
				doIntersect = false;
			}
		}

		return doIntersect;
	}
	*/
}
