using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class NodeGroupScript : NetworkBehaviour {

    int maxNodes = 15;
    int minNodes = 5;
    int numNodes;
    int maxNodeConnections = 2;
    float ideaSpawnRateMin = 0.01f;
    float ideaSpawnRateMax = 0.1f;
    public Node referenceNode;
    List<Node> nodes = new List<Node>();
    List<NodeGroupScript> connectedGroups = new List<NodeGroupScript>();
    //make group connections pre-determined (the same everytime level is loaded)
    //connections within a group will be procedural but connections between groups will not
    //for group connections make a variable number of connections and get list of groups within certain range and distribute variable number of connections amongst them

	// Use this for initialization
    [Server]
    public override void OnStartServer() {
        numNodes = Random.Range(minNodes, maxNodes);
        //spawn nodes in circular formation
        //calculate radius based on number of nodes
        float radius = (numNodes * (referenceNode.GetComponent<SpriteRenderer>().sprite.rect.width / 200)) / 2;
        int angle = 360 / numNodes;
        //create surrounding nodes
        for(int i = 0; i < numNodes; i++)
        {
            int a = i * angle;
            //maybe vary the radius
            float rad = Random.Range(radius * 0.5f, radius * 1.25f);
            Vector3 pos = RandomCircle(transform.position, rad, a);
            Node n = (Node)Instantiate(referenceNode, pos, Quaternion.identity);
            n.spawnChance = Random.Range(ideaSpawnRateMin, ideaSpawnRateMax);
            nodes.Add(n);
            NetworkServer.Spawn(nodes[i].gameObject);
            nodes[i].index = i;
        }
        //create central node
        Node central = (Node)Instantiate(referenceNode, transform.position, Quaternion.identity);
        central.spawnChance = Random.Range(ideaSpawnRateMin, ideaSpawnRateMax);
        nodes.Add(central);
        NetworkServer.Spawn(nodes[nodes.Count - 1].gameObject);
        nodes[nodes.Count-1].index = nodes.Count-1;
        for(int i = 0; i < nodes.Count; i++)
        {
            //get connections
            List<Node> connections = nodes[i].links;
            //check connections
            //TODO: fix max connections check to check connections of node its connecting to
            if (connections.Count < maxNodeConnections && i != nodes.Count - 1)
            {
                int connectionType = Random.Range(0, 5);
                switch (connectionType)
                {
                    case 0:
                        //central = connection to center
                        if (!connections.Contains(nodes[nodes.Count - 1]))
                        {
                            nodes[i].linkTo(nodes[nodes.Count - 1]);
                        }
                        break;
                    case 1:
                        //analogous = 2 connections, first node clockwise and counterclockwise
                        int clockwise = i + 1;
                        int counterclockwise = i - 1;
                        if (clockwise == nodes.Count - 1)
                        {
                            clockwise = 0;
                        }
                        if (counterclockwise < 0)
                        {
                            counterclockwise = nodes.Count - 2;
                        }
                        if (!connections.Contains(nodes[clockwise]))
                        {
                            nodes[i].linkTo(nodes[clockwise]);
                        }
                        if (!connections.Contains(nodes[counterclockwise]))
                        {
                            nodes[i].linkTo(nodes[counterclockwise]);
                        }
                        break;
                    case 2:
                        //split complements = 2 connections, first node 150 degrees clockwise and counterclockwise
                        int nodeOne = nodes[i].index * angle + 150;
                        int nodeTwo = nodes[i].index * angle - 150;
                        if (nodeOne > 360)
                        {
                            nodeOne -= 360;
                        }
                        if (nodeTwo < 0)
                        {
                            nodeTwo = 360 + nodeTwo;
                        }
                        int indexOne = nodeOne / angle;
                        int indexTwo = nodeTwo / angle;
                        if (!connections.Contains(nodes[indexOne]))
                        {
                            nodes[i].linkTo(nodes[indexOne]);
                        }
                        if (!connections.Contains(nodes[indexTwo]))
                        {
                            nodes[i].linkTo(nodes[indexTwo]);
                        }
                        break;
                    case 3:
                        //triadic = 2 connections, first node 120 degrees clockwise and counterclockwise
                        nodeOne = nodes[i].index * angle + 120;
                        nodeTwo = nodes[i].index * angle - 120;
                        if (nodeOne > 360)
                        {
                            nodeOne -= 360;
                        }
                        if (nodeTwo < 0)
                        {
                            nodeTwo = 360 + nodeTwo;
                        }
                        indexOne = nodeOne / angle;
                        indexTwo = nodeTwo / angle;
                        if (!connections.Contains(nodes[indexOne]))
                        {
                            nodes[i].linkTo(nodes[indexOne]);
                        }
                        if (!connections.Contains(nodes[indexTwo]))
                        {
                            nodes[i].linkTo(nodes[indexTwo]);
                        }
                        break;
                    case 4:
                        //tetradic = "square connection", 90 degrees clockwise and cunterclockwise between all 4 nodes
                        nodeOne = nodes[i].index * angle + 90;
                        nodeTwo = nodes[i].index * angle - 90;
                        if (nodeOne > 360)
                        {
                            nodeOne -= 360;
                        }
                        if (nodeTwo < 0)
                        {
                            nodeTwo = 360 + nodeTwo;
                        }
                        indexOne = nodeOne / angle;
                        indexTwo = nodeTwo / angle;
                        if (!connections.Contains(nodes[indexOne]))
                        {
                            nodes[i].linkTo(nodes[indexOne]);
                        }
                        if (!connections.Contains(nodes[indexTwo]))
                        {
                            nodes[i].linkTo(nodes[indexTwo]);
                        }
                        int nodeThree = nodeOne + 90;
                        if (nodeThree > 360)
                        {
                            nodeThree -= 360;
                        }
                        int indexThree = nodeThree / angle;
                        List<Node> nodeTwoConnections = nodes[indexTwo].links;
                        if (!nodeTwoConnections.Contains(nodes[indexThree]))
                        {
                            nodes[indexTwo].linkTo(nodes[indexThree]);
                        }
                        break;
                }
            }
            //separate connection logic for central node
            if (i == nodes.Count - 1 && connections.Count < maxNodeConnections)
            {
                //randomize?
                nodes[i].linkTo(nodes[Random.Range(0, nodes.Count - 1)]);
            }
        }
        //create connections between groups
        float maxConnectionDist = radius * 20;
        GameObject[] groups = GameObject.FindGameObjectsWithTag("Group");
        //choose connection types
        //maybe calculate paths to each group and connect if path to group does not exist for now connect with every group
        //calculate angle to nearby groups and choose corresponding node to connect with
        
        for (int i = 0; i < groups.Length; i++)
        {
            NodeGroupScript other = groups[i].GetComponent<NodeGroupScript>();
            if (this != other && !other.connectedGroups.Contains(this) && Vector3.Distance(this.transform.position, other.transform.position) < maxConnectionDist)
            {
                if(other.numNodes != 0)
                {
                    //print("connect");
                    List<Node> otherNodes = other.nodes;
                    int connectIndex = nodes.Count - 1;
                    int otherConnectIndex = otherNodes.Count - 1;
                    float minDist = maxConnectionDist;
                    //int otherAngle = 360 / other.numNodes;

                    //find closest node
                    for (int j = 0; j < nodes.Count - 1; j++)
                    {
                        float dist = Vector3.Distance(nodes[j].transform.position, other.transform.position);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            connectIndex = j;
                        }
                    }

                    minDist = maxConnectionDist;
                    for (int j = 0; j < otherNodes.Count - 1; j++)
                    {
                        float dist = Vector3.Distance(otherNodes[j].transform.position, this.transform.position);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            otherConnectIndex = j;
                        }
                    }
                    nodes[connectIndex].linkTo(otherNodes[otherConnectIndex]);
                    connectedGroups.Add(other);
                }
                
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    Vector3 RandomCircle(Vector3 center, float radius, int a)
    {
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}
