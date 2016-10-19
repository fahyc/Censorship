using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using ExtensionMethods;
using System.Collections.Generic;

public class Node : NetworkBehaviour {
	public Node[] linksSeed;//this should be set before the node's start function is called. 
	public List<Node> links;
	public List<LineRenderer> linkObj;
    public float[] ideaStrengths;
    public float spawnMultiplier = 0.005f;
    string mostImportantIdea;
    //The index of the most important idea in this node's opinion
	[SyncVar(hook="onChangeIndex")]
    int importantIndex = -1;
    int secondImportantIndex;
    int thirdImportantIndex;



	
	public SpecialIdea specialIdeaPrefab;
    //Gets 
    public float echoChamberCoefficient=1.0f;
    //This gets added every time a Node hears about something that is its most important idea.
    public float echoChamberStepIncrease=0.5f;
    public float echoChamberStepDecrease = 0.1f;
    AbstractIdea[] ideasList;

    public bool stubborn = false;

	public bool shill = false;

	public float spawnChance = .01f;//the chances an idea will spawn every frame.

	public LineRenderer line;
	public Idea ideaObj;
    //The base amount of influence that any one opinion will have.
    public float baseInfluence = 0.05f;
	
	SpriteRenderer sprite;

	[Client]
	void Start()
	{
        //print("client node");
		sprite = GetComponent<SpriteRenderer>();
	}

	public void onChangeIndex(int i)
	{
		sprite.color = IdeaList.staticList[i].color;
	}

	public int index;
	
    // Use this for initialization
    public override void OnStartServer () {
		//Get a reference to the global game object that keeps track of the ideological climate.
		ideasList = IdeaList.staticList;// GameObject.Find("EventSystem").GetComponent<IdeaList>().list;
		links = new List<Node>(linksSeed);

        //ideaStrengths = new float[ideasList.Length];
		if(ideaStrengths == null)
		{
			ideaStrengths = new float[ideasList.Length];
			Debug.LogWarning("Error! ideaStrengths is null");
		}

		linkObj = new List<LineRenderer>();
		for(int i = 0; i < links.Count; i++)
		{
            if (links[i] == null)
            {
                Debug.LogWarning("Link " + i + " is null");
                continue;
            }
			LineRenderer link = links[i].LinkedTo(this);
			if(link == null)
			{
				link = GameObject.Instantiate<LineRenderer>(line);
                // make the link visible to everyone that can see this node
                GetComponent<VisibilityCheck>().AddConnection(link.gameObject);
                links[i].GetComponent<VisibilityCheck>().AddConnection(link.gameObject);
				link.GetComponent<NetworkLineRenderer>().setPoints(transform.position, links[i].transform.position);
				NetworkServer.Spawn(link.gameObject);
			}
			linkObj.Add(link);
		}
	}

	[Client]
	public void clientLinkTo(Node other)
	{
		//print("visibility: " + GetComponent<VisibilityCheck>() + " other's " + other.GetComponent<VisibilityCheck>());
		//print("using clientlinkto with node: " + other);
		CmdlinkTo(other.GetComponent<NetworkIdentity>().netId);
		//print(other + " goes to netid: " + other.GetComponent<NetworkIdentity>().netId);
	}

    [Command]
	public void CmdlinkTo(NetworkInstanceId id)
	{
		//print("visibility: " + GetComponent<VisibilityCheck>());
		Node other = NetworkServer.FindLocalObject(id).GetComponent<Node>();
		linkTo(other);
		
	}

	[Server]
	public void linkTo(Node other)
	{
		//print("Linking with node: " + other);
		other.links.Add(this);
		LineRenderer link = GameObject.Instantiate<LineRenderer>(line);
		//print("Link: " + link.gameObject + " visibilitycheck " + GetComponent<VisibilityCheck>());
        GetComponent<VisibilityCheck>().AddConnection(link.gameObject);
        other.GetComponent<VisibilityCheck>().AddConnection(link.gameObject);
		link.GetComponent<NetworkLineRenderer>().setPoints(transform.position, other.transform.position);
		NetworkServer.Spawn(link.gameObject);
		linkObj.Add(link);
		other.linkObj.Add(link);
		links.Add(other);

        GetComponent<NetworkIdentity>().RebuildObservers(true);
	}


	// Update is called once per frame
    [ServerCallback]
	public virtual void Update () {
        float localmax = -1;
        float secondmax = -1;
        float thirdmax = -1;

        int oldIndex = importantIndex;
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

        if (oldIndex != importantIndex)
        {
            IdeaList.staticList[importantIndex].updateValue(1);
            if (oldIndex >= 0)
                IdeaList.staticList[oldIndex].updateValue(-1);
        }

        //Determine the probability of sending an idea based on the importance to this individual Node.
        if (importantIndex < 0 || ideaStrengths.Length <= 0)
        {
            return;
        }

        float sumImportances = ideaStrengths[importantIndex];// +ideaStrengths[secondImportantIndex]+ideaStrengths[thirdImportantIndex];
        //We can get the percent probability of sending an idea by normalizing to the sum of the three ideas and then making intervals based on those.
        float primaryIdeaInterval = ideaStrengths[importantIndex] / sumImportances;
        //float secondaryIdeaInterval = primaryIdeaInterval + (ideaStrengths[secondImportantIndex] / sumImportances);
        //float tertiaryIdeaInterval = secondaryIdeaInterval + (ideaStrengths[thirdImportantIndex] / sumImportances);
        //Pick an idea based on these intervals.
        float roll = Random.value;
        //This is the variable that decides which idea we send.
        int chosenIdeaIndex = -1;
        if(roll < primaryIdeaInterval)
        {
            chosenIdeaIndex = importantIndex;
        }
        /*else if (primaryIdeaInterval <= roll  && roll < secondaryIdeaInterval)
        {
            chosenIdeaIndex = secondImportantIndex;
        } 
        else if (secondaryIdeaInterval <= roll)
        {
            chosenIdeaIndex = thirdImportantIndex;
        }*/
        //Debug, use this when you just want it to spout off its most important idea only.
        chosenIdeaIndex = importantIndex;
		//spawn chance is affected by how strongly the node believes in its opinion.
		//print("about to change color");
		//RpcChangeColor(importantIndex);
        if (Random.value < spawnChance+ideaStrengths[chosenIdeaIndex]*spawnMultiplier)
		{

            if (links.Count > 0)
			{
                if(shill == true && GetComponent<Shill>().disabled) {
                    print("Disabled shill node will not spawn stuff");
                } else {
                    sendIdea(ideasList[chosenIdeaIndex].name, links[Random.Range(0, links.Count)], chosenIdeaIndex);

                }
            }
			else
			{
				//print("orphaned node");
			}
		}
	}
	

	public void SetStrengths(float[] strengths)
	{
		if (isServer)
		{
			ideaStrengths = strengths;
		}
		else
		{
			CmdSetStrengths(strengths);
		}
	}

	[Command]
	public void CmdSetStrengths(float[] strengths)
	{
		ideaStrengths = strengths;
	}

	[ClientRpc]
	public void RpcChangeColor(int index)
	{
		///print(index);
		sprite.color = IdeaList.staticList[index].color;
	}

    [Server]
	void sendIdea(string idea, Node dest, int idx,Idea obj = null)
	{
		Idea temp;
		if (obj!= null)
		{
			temp = obj;
		}
		else
		{
			temp = Instantiate<Idea>(ideaObj);
		}
		temp.ideaStr = idea;
		temp.origin = transform.position;
		temp.destination = dest.transform.position;
		temp.transform.position = transform.position;
		temp.dest = dest.gameObject;
		temp.originObj = this;
        temp.index = idx;
        NetworkServer.Spawn(temp.gameObject);
	}


	public void receiveSpecial(SpecialIdea special)
	{
		receiveIdea(special.ideaStr);
		if(special.strength <= 1)
		{
			return;
		}
		for(int i = 0; i < links.Count; i++)
		{
			SpecialIdea obj = Instantiate(special);
			obj.visited = special.visited;
			obj.strength -= 1;
			sendIdea(special.ideaStr, links[i], IdeaList.staticDict[special.ideaStr], obj);
		}
	}

    [Server]
    public void receiveIdea(string ideaStr)
    {
		if (shill)
		{
			return;
		}
        if (ideasList[importantIndex].name == ideaStr)
        {
            ideaStrengths[importantIndex] += baseInfluence * 2;
            ideaStrengths[importantIndex] = Mathf.Clamp(ideaStrengths[importantIndex], 0.0f, 1.0f);
            if(!stubborn)
                echoChamberCoefficient += echoChamberStepIncrease;

        }
        //This portion happens if the node receives an idea that is conflicting with its most important idea.
        else if(ideasList[ideasList[importantIndex].opposite].name == ideaStr)
        {
			// baseinfluence / (1 + the strength of the node's most important idea) 
			//ideaStrengths[ideasList[importantIndex].opposite] += baseInfluence*(1-ideaStrengths[importantIndex]);
			//Tweak to system: subtract if i hold diametrically opposed viewpoint.
			ideaStrengths[ideasList[importantIndex].opposite] = ideaStrengths[ideasList[importantIndex].opposite] - baseInfluence * ideaStrengths[importantIndex] * echoChamberCoefficient;//,ideaStrengths[importantIndex]/2f);
            ideaStrengths[ideasList[importantIndex].opposite] = Mathf.Clamp(ideaStrengths[ideasList[importantIndex].opposite], 0.0f, 1.0f);
            //Additionally, if this idea conflicts with the most important idea for a Node it will start to care about other issues less
            for (int t = 0; t < ideaStrengths.Length; t++)
            {
                if (t == importantIndex)
                {
                    continue;
                }
                ideaStrengths[t] -= baseInfluence;
                ideaStrengths[t] = Mathf.Clamp(ideaStrengths[t], 0.0f, 1.0f);
            }
            //Decrease our echoChamber
            if (!stubborn)
            {
                echoChamberCoefficient -= echoChamberStepDecrease;
                if (echoChamberCoefficient < 0)
                {
                    echoChamberCoefficient = 0;
                }
            }

        }
        // If it's just a normal idea being sent towards a node, it'll increase by an amount influenced by how much the current node has been hearing about its favorite idea.
        else
        {
            //No real good way to do this atm and I don't wanna mess with how we're sending info between nodes so just do a linear search
            for(int x=0; x<ideasList.Length; x++)
            {
                if (ideaStr == ideasList[x].name)
                {
                    ideaStrengths[x] += baseInfluence * Mathf.Max(echoChamberCoefficient, 1);
                    ideaStrengths[x] = Mathf.Clamp(ideaStrengths[x], 0.0f, 1.0f);
                    ideaStrengths[importantIndex] -= baseInfluence * Mathf.Max(echoChamberCoefficient, 1);
                    ideaStrengths[importantIndex] = Mathf.Clamp(ideaStrengths[importantIndex], 0f, 1f);
                    if (!stubborn) { 
                        echoChamberCoefficient -= echoChamberStepDecrease;
                        if (echoChamberCoefficient < 0)
                        {
                            echoChamberCoefficient = 0;
                        }
                    }
                    break;
                }
            }
        }
	}

    [Server]
	public LineRenderer LinkedTo(Node node)
	{
		//returns the lineRenderer that links this Node with node if it exists. Used for making one way connections two way.
		for(int i = 0; i < links.Count; i++)
		{
			if(links[i] == node)
			{
				if(linkObj.Count > i)
				{
					return linkObj[i];
				}
				else
				{
					return null;
				}
			}
		}
		// Debug.LogWarning("Error, one way connection detected between " + node + ", and " + this);
		return null;
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Global.textbg = true;
            Global.text = "";
            Global.text += "<size=16><b><color=#" + ColorToHex(ideasList[importantIndex].color) + ">" + ideasList[importantIndex].name + "</color></b></size>" + ": " + ideasList[importantIndex].description + "\n\n";
            if (ideaStrengths[secondImportantIndex] > 0)
                Global.text += "<size=16><b><color=#" + ColorToHex(ideasList[secondImportantIndex].color) + ">" + ideasList[secondImportantIndex].name + "</color></b></size>" + ": " + ideasList[secondImportantIndex].description + "\n\n";
            if (ideaStrengths[thirdImportantIndex] > 0)
                Global.text += "<size=16><b><color=#" + ColorToHex(ideasList[thirdImportantIndex].color) + ">" + ideasList[thirdImportantIndex].name + "</color></b></size>" + ": " + ideasList[thirdImportantIndex].description + "\n\n";
        }
    }

	[Server]
	void OnDestroy()
	{
		for(int i = 0; i < links.Count; i++)
		{
			if (i < linkObj.Count && linkObj[i])
			{
				Destroy(linkObj[i].gameObject);
			}
			links[i].links.Remove(this);
		}
	}

    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
}
