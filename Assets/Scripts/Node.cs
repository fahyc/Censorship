﻿using UnityEngine;
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
    
    //Gets 
    public float echoChamberCoefficient=1.0f;
    //This gets added every time a Node hears about something that is its most important idea.
    public float echoChamberStepIncrease=0.5f;
    public float echoChamberStepDecrease = 0.1f;
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
			// print(links[i]);
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
        float localmax = -1;
        float secondmax = -1;
        float thirdmax = -1;
        // find most important ideas to this node.
        for (int i = 0; i < ideaStrengths.Length; i++)
        {
            if (ideaStrengths[i] > localmax)
            {
                localmax = ideaStrengths[i];
                mostImportantIdea = ideasList[i].name;
                importantIndex = i;
            }
            if (ideaStrengths[i] > secondmax && ideaStrengths[i] < localmax)
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
        //Determine the probability of sending an idea based on the importance to this individual Node.
        float sumImportances = ideaStrengths[importantIndex]+ideaStrengths[secondImportantIndex]+ideaStrengths[thirdImportantIndex];
        //We can get the percent probability of sending an idea by normalizing to the sum of the three ideas and then making intervals based on those.
        float primaryIdeaInterval = ideaStrengths[importantIndex] / sumImportances;
        float secondaryIdeaInterval = primaryIdeaInterval + (ideaStrengths[secondImportantIndex] / sumImportances);
        float tertiaryIdeaInterval = secondaryIdeaInterval + (ideaStrengths[thirdImportantIndex] / sumImportances);
        //Pick an idea based on these intervals.
        float roll = Random.value;
        //This is the variable that decides which idea we send.
        int chosenIdeaIndex = -1;
        if(roll < primaryIdeaInterval)
        {
            chosenIdeaIndex = importantIndex;
        }
        else if (primaryIdeaInterval <= roll  && roll < secondaryIdeaInterval)
        {
            chosenIdeaIndex = secondImportantIndex;
        } 
        else if (secondaryIdeaInterval <= roll)
        {
            chosenIdeaIndex = thirdImportantIndex;
        }
        //Debug, use this when you just want it to spout off its most important idea only.
        chosenIdeaIndex = importantIndex;
        //spawn chance is affected by how strongly the node believes in its opinion.
        if (Random.value < spawnChance+ideaStrengths[chosenIdeaIndex]*spawnMultiplier)
		{

            if (links.Length > 0)
			{
				sendIdea(ideasList[chosenIdeaIndex].name, links[Random.Range(0, links.Length)], chosenIdeaIndex);
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
            echoChamberCoefficient += echoChamberStepIncrease;

        }
        //This portion happens if the node receives an idea that is conflicting with its most important idea.
        else if(ideasList[ideasList[importantIndex].opposite].name == ideaStr)
        {
            // baseinfluence / (1 + the strength of the node's most important idea) 
            //ideaStrengths[ideasList[importantIndex].opposite] += baseInfluence*(1-ideaStrengths[importantIndex]);
            //Tweak to system: subtract if i hold diametrically opposed viewpoint.
            ideaStrengths[ideasList[importantIndex].opposite] -= baseInfluence*(ideaStrengths[importantIndex]);
            ideaStrengths[ideasList[importantIndex].opposite] = Mathf.Clamp(ideaStrengths[ideasList[importantIndex].opposite], 0.0f, 1.0f);
            //Additionally, if this idea conflicts with the most important idea for a Node it will start to care about other issues less
            for (int t = 0; t < ideaStrengths.Length; t++)
            {
                if (t == importantIndex)
                {
                    continue;
                }
                ideaStrengths[t] -= baseInfluence;
                Mathf.Clamp(ideaStrengths[t], 0.0f, 1.0f);
            }
            //Decrease our echoChamber
            echoChamberCoefficient -= echoChamberStepDecrease;
            if(echoChamberCoefficient < 0)
            {
                echoChamberCoefficient = 0;
            }
            

        }
        // If it's just a normal idea being sent towards a node, it'll increase by an amount influenced by how much the current node has been hearing about its favorite idea.
        else
        {
            //No real good way to do this atm and I don't wanna mess with how we're sending info between nodes so just do a linear search
            for(int x=0; x<ideasList.Length; x++)
            {
                if(ideaStr == ideasList[x].name)
                {
                    ideaStrengths[x] += baseInfluence*echoChamberCoefficient;
                    ideaStrengths[x] = Mathf.Clamp(ideaStrengths[x], 0.0f, 1.0f);
                    echoChamberCoefficient -= echoChamberStepDecrease;
                    if(echoChamberCoefficient < 0)
                    {
                        echoChamberCoefficient = 0;
                    }
                    break;
                }
            }
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
