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
			node.clientLinkTo(DummyNode.nodes[i]);
		}
		float[] strengths = new float[IdeaList.staticList.Length];

		//print(node.ideaStrengths);
		strengths[index] = 1;
        node.shill = true;
		node.SetStrengths(strengths);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
