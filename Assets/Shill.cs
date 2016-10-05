using UnityEngine;
using System.Collections;

public class Shill : Spawnable
{
	Node node;
	// Use this for initialization
	void Start () {
		node = GetComponent<Node>();
		for(int i = 0; i < DummyNode.nodes.Count; i++)
		{
			node.linkTo(DummyNode.nodes[i]);
		}

		node.ideaStrengths[index] = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
