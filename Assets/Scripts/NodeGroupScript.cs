using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeGroupScript : MonoBehaviour {

    int maxNodes = 15;
    int minNodes = 5;
    int numNodes;
    public NewNodeScript referenceNode;
    List<Node> nodes;
    //make group connections pre-determined (the same everytime level is loaded)
    //connections within a group will be procedural but connections between groups will not
    //for group connections make a variable number of connections and get list of groups within certain range and distribute variable number of connections amongst them

	// Use this for initialization
	void Start () {
        numNodes = Random.Range(minNodes, maxNodes);
        //spawn nodes in circular formation
        //calculate radius based on number of nodes
        float radius = (numNodes * (referenceNode.GetComponent<SpriteRenderer>().sprite.rect.width / 600)) / 2;

        for(int i = 0; i < numNodes; i++)
        {
            /*
            Vector3 pos = RandomCircle(transform.position, 5.0f);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, transform.position - pos);
            Instantiate(referenceNode, pos, Quaternion.identity);
            */
            int a = i * 30;
            int dist = a / 360;
            float newRadius = radius / dist;

            Vector3 pos = RandomCircle(transform.position, 0.5f, a);
            Instantiate(referenceNode, pos, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    Vector3 RandomCircle(Vector3 center, float radius, int a)
    {
        /*
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
        */
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}
