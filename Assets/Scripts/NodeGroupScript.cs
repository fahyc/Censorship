using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class NodeGroupScript : NetworkBehaviour {

    int maxNodes = 15;
    int minNodes = 5;
    int numNodes;
    public Node referenceNode;
    List<Node> nodes = new List<Node>();
    //make group connections pre-determined (the same everytime level is loaded)
    //connections within a group will be procedural but connections between groups will not
    //for group connections make a variable number of connections and get list of groups within certain range and distribute variable number of connections amongst them

	// Use this for initialization
    [Server]
    public override void OnStartServer() {
        numNodes = Random.Range(minNodes, maxNodes);
        //spawn nodes in circular formation
        //calculate radius based on number of nodes
        float radius = (numNodes * (referenceNode.GetComponent<SpriteRenderer>().sprite.rect.width / 300)) / 2;
        int angle = 360 / numNodes;
        //create surrounding nodes
        for(int i = 0; i < numNodes; i++)
        {
            /*
            Vector3 pos = RandomCircle(transform.position, 5.0f);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, transform.position - pos);
            Instantiate(referenceNode, pos, Quaternion.identity);
            */
            int a = i * angle;

            Vector3 pos = RandomCircle(transform.position, radius, a);
            nodes.Add((Node)Instantiate(referenceNode, pos, Quaternion.identity));
            NetworkServer.Spawn(nodes[i].gameObject);
            nodes[i].index = i;
        }
        //create central node
        nodes.Add((Node)Instantiate(referenceNode, transform.position, Quaternion.identity));
        NetworkServer.Spawn(nodes[nodes.Count - 1].gameObject);
        nodes[nodes.Count-1].index = nodes.Count-1;
        for(int i = 0; i < nodes.Count; i++)
        {
            //get connections
            List<Node> connections = nodes[i].links;
            //check connections
            
            int connectionType = Random.Range(0, 5);
            switch(connectionType)
            {
                case 0:

                    break;
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
            }
            if(i != nodes.Count-1)
            {
                nodes[i].linkTo(nodes[nodes.Count - 1]);
            }
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

    void central()
    {
        //central = connection to center
    }

    void analogous()
    {
        //analogous = 2 connections, first node clockwise and counterclockwise
    }

    void splitComplements()
    {
        //split complements = 2 connections, first node 150 degrees clockwise and counterclockwise
    }

    void triadic()
    {
        //triadic = 2 connections, first node 120 degrees clockwise and counterclockwise
    }

    void tetradic()
    {
        //tetradic = "square connection", 90 degrees clockwise and cunterclockwise between all 4 nodes
    }
}
