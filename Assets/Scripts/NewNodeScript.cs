using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewNodeScript : MonoBehaviour {

    float[] ideas;
    int numIdeas;
    float connectionRange;
    float deviation;
    int connectionRule;
    int primaryIdea;
    float ideaMin = 0.0f;
    float ideaMax = 25.0f;
    float primaryIdeaMin = 15.0f;
    Collider2D[] nearbyNodes;
    List<Node> connectedNodes;
    NodeGroupScript parentGroup;
    //group objects in charge of spawning initial nodes within group in circular formation, number of nodes spawned in group is up to designer

    /**
     * use spatial connections instead of color connections
     * 
     * blending rules:
     * a = color1
     * b = color2
     * t = number between 0-1 representing point in blend between a and b
     * blend r,g,b values
     * sqrt((1-t)*pow(a, 2) + t * pow(b, 2))
     * 
     * connection rules using color wheel:
     * -complementary = use opposite color
     * -monochromatic = use 3 different values of same color
     * -analogous = use 3 adjacent colors
     * -split complements = use a color and it's two adjacent tertiary colors of its compliment
     * -triadic = use 3 evenly spaced colors
     * -tetradic = use 2 complementary pairs
     * -group node groups based on connection rule to minimize cross network connections or specify connection range
     * 
     * generation rules:
     * -manual initial placement
     * 
     * growth rules:
     * -within a node group once a small group of nodes with colors outside of allowed range is generated separate into new node group
     * -if two or more node groups are converted into color within allowed range combine into larger node group while also combining connections
     * **/

    // Use this for initialization
    void Start () {
        /*
        for(int i = 0; i < 10; i++)
        {
            //generate variation based on primary idea
            if(i == primaryIdea)
            {
                ideas[i] = Random.Range(primaryIdeaMin, ideaMax);
            }
            else
            {
                ideas[i] = Random.Range(ideaMin, ideaMax);
            }
        }
        connectionRule = Random.Range(0, 6);
        connectionRange = Random.Range(1.0f, 5.0f);
        deviation = 25.0f;
        numIdeas = ideas.Length;
        nearbyNodes = Physics2D.OverlapCircleAll(transform.position, connectionRange);
        //generate connections by searching in range and following rule
        switch(connectionRule)
        {
            case 0:
                monochromatic();
                break;
            case 1:
                analogous();
                break;
            case 2:
                splitComplements();
                break;
            case 3:
                triadic();
                break;
            case 4:
                tetradic();
                break;
        }
        */
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
