using UnityEngine;
using System.Collections;

public class NodeGroupScript : MonoBehaviour {

    int maxNodes = 15;
    int minNodes = 5;
    int numNodes;
    Node referenceNode;
    //make group connections pre-determined (the same everytime level is loaded)
    //connections within a group will be procedural but connections between groups will not
    //for group connections make a variable number of connections and get list of groups within certain range and distribute variable number of connections amongst them

	// Use this for initialization
	void Start () {
        numNodes = Random.Range(minNodes, maxNodes);
        //spawn nodes in circular formation
        //calculate radius based on number of nodes
        float radius = (numNodes * referenceNode.GetComponent<SpriteRenderer>().sprite.rect.width) / 2;

        for(int i = 0; i < numNodes; i++)
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
