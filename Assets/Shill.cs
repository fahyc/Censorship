using UnityEngine;
using System.Collections;

public class Shill : Spawnable
{
	Node node;

	public Sprite sprite;
	// Use this for initialization
	void Start () {
		//print("Starting shill");
		node = GetComponent<Node>();
		for(int i = 0; i < DummyNode.nodes.Count; i++)
		{
			node.clientLinkTo(DummyNode.nodes[i]);
		}
		float[] strengths = new float[IdeaList.instance.list.Length];
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = sprite;
        //print(node.ideaStrengths);
        print(index);
		strengths[index] = 1;
        node.shill = true;
		node.SetStrengths(strengths);
	}
	

}
