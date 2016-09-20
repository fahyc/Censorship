using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System.Collections.Generic;

public class Node : MonoBehaviour {
	public Node[] links;//this should be set before the node's start function is called. 
	public LineRenderer[] linkObj;
	public float[] ideaStrengths;
    //How much agreeing with an idea influences the spawn rate. Note that this
    public float spawnMultiplier = 0.005f;
    string mostImportantIdea;
    //The index of the most important idea in this node's opinion
    int importantIndex;
    int secondImportantIndex;
    int thirdImportantIndex;


    AbstractIdea[] ideasList;


	public float spawnChance = .01f;//the chances an idea will spawn every frame.

	public LineRenderer line;
	public Idea ideaObj;
    //The base amount of influence that any one opinion will have.
    public float baseInfluence = 0.05f;

    // Use this for initialization
    void Start () {
		//Get a reference to the global game object that keeps track of the ideological climate.
		ideasList = IdeaList.staticList;// GameObject.Find("EventSystem").GetComponent<IdeaList>().list;

        //ideaStrengths = new float[ideasList.Length];
		if(ideaStrengths == null)
		{
			ideaStrengths = new float[ideasList.Length];
			print("Error! ideaStrengths is null");
		}
        else
        {
            for (int i=0; i<ideaStrengths.Length; i++)
            {
                IdeaList.staticList[i].updateValue(ideaStrengths[i]);
            }
        }


        linkObj = new LineRenderer[links.Length];
		//ideas = global.getIdeas()
		//ideaStrengths = new int[ideas.Length];
		for(int i = 0; i < links.Length; i++)
		{
			print(links[i]);
			LineRenderer link = links[i].LinkedTo(this);
			if(link == null)
			{
				link = GameObject.Instantiate<LineRenderer>(line);
				link.SetPosition(0, transform.position);
				link.SetPosition(1, links[i].transform.position);
			}
			linkObj[i] = link;
		}

	}
	

	public void linkTo(Node other)
	{
		//Node local = nodes[Random.Range(0, nodes.Length)];
		other.links = other.links.slowAdded(this);
		LineRenderer link = GameObject.Instantiate<LineRenderer>(line);
		link.SetPosition(0, transform.position);
		link.SetPosition(1, other.transform.position);
		links = links.slowAdded(other);
	}


	// Update is called once per frame
	void Update () { //THIS SHOULD BE IMPROVED ON ONCE MORE COMPLEX NODE AI IS ADDED.
        // find most important idea to this node.

        //spawn chance is affected by how strongly the node believes in its opinion.
        if (Random.value < spawnChance+ideaStrengths[importantIndex]*spawnMultiplier)
		{
            float localmax = -1;
            float secondmax = -1;
            float thirdmax = -1;
            for (int i = 0; i < ideaStrengths.Length; i++)
            {
                if (ideaStrengths[i] > localmax)
                {
                    localmax = ideaStrengths[i];
                    mostImportantIdea = ideasList[i].name;
                    importantIndex = i;
                }
                if(ideaStrengths[i] > secondmax && ideaStrengths[i] < localmax)
                {
                    secondmax = ideaStrengths[i];
                    secondImportantIndex = i;
                }
                if (ideaStrengths[i] > thirdmax && ideaStrengths[i] < secondmax)
                {
                    thirdmax = ideaStrengths[i];
                    thirdImportantIndex = i;
                }
            }
            if (links.Length > 0)
			{
				sendIdea(mostImportantIdea, links[Random.Range(0, links.Length)], importantIndex);
			}
			else
			{
				//print("orphaned node");
			}
		}
	}

	void sendIdea(string idea, Node dest, int idx)
	{
		Idea temp = GameObject.Instantiate<Idea>(ideaObj);
		temp.ideaStr = idea;
		temp.origin = transform.position;
		temp.destination = dest.transform.position;
		temp.transform.position = transform.position;
		temp.dest = dest;
		temp.originObj = this;
        temp.index = idx;
	}

	public void reciveIdea(string ideaStr)
	{
        float prevVal = ideaStrengths[importantIndex];

        if (ideasList[importantIndex].name == ideaStr)
        {
            ideaStrengths[importantIndex] += baseInfluence * 2;
            ideaStrengths[importantIndex] = Mathf.Clamp(ideaStrengths[importantIndex], 0.0f, 1.0f);

        }
        //This portion happens if the node receives an idea that is conflicting with its most important idea.
        else if(ideasList[ideasList[importantIndex].opposite].name == ideaStr)
        {
            // baseinfluence / (1 + the strength of the node's most important idea) 
            ideaStrengths[ideasList[importantIndex].opposite] += baseInfluence*(1-ideaStrengths[importantIndex]);
        }
        // If it's just a normal idea being sent towards a node, it'll a
        else
        {
            
        }
        //ADD CODE TO CHANGE VALUES AND SUCH HERE!
        //print("recieved: " + ideaStr);
        //Destroy(gameObject);

        if (prevVal != ideaStrengths[importantIndex])
            IdeaList.staticList[importantIndex].updateValue(ideaStrengths[importantIndex] - prevVal);
	}


	public LineRenderer LinkedTo(Node node)
	{
		//returns the lineRenderer that links this Node with node if it exists. Used for making one way connections two way.
		for(int i = 0; i < links.Length; i++)
		{
			if(links[i] == node)
			{
				if(linkObj.Length > i)
				{
					return linkObj[i];
				}
				else
				{
					return null;
				}
			}
		}
		print("Error, one way connection detected between " + node + ", and " + this);
		return null;
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Global.text = "";
            Global.text += "<size=16><b><color=#" + ColorToHex(ideasList[importantIndex].color) + ">" + ideasList[importantIndex].name + "</color></b></size>" + ": " + ideasList[importantIndex].description + "\n\n";
            Global.text += "<size=16><b><color=#" + ColorToHex(ideasList[secondImportantIndex].color) + ">" + ideasList[secondImportantIndex].name + "</color></b></size>" + ": " + ideasList[secondImportantIndex].description + "\n\n";
            Global.text += "<size=16><b><color=#" + ColorToHex(ideasList[thirdImportantIndex].color) + ">" + ideasList[thirdImportantIndex].name + "</color></b></size>" + ": " + ideasList[thirdImportantIndex].description + "\n\n";
        }
    }

    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
}
