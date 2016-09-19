using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
	public Node[] links;//this should be set before the node's start function is called. 
	public LineRenderer[] linkObj;
	public string[] ideas;
	public int[] ideaStrengths;

	public float spawnChance = .01f;//the chances an idea will spawn every frame.

	public LineRenderer line;
	public Idea ideaObj;

	// Use this for initialization
	void Start () {
		linkObj = new LineRenderer[links.Length];
		//ideas = global.getIdeas()
		//ideaStrengths = new int[ideas.Length];
		for(int i = 0; i < links.Length; i++)
		{
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
	
	// Update is called once per frame
	void Update () { //THIS SHOULD BE IMPROVED ON ONCE MORE COMPLEX NODE AI IS ADDED.
		if(Random.value < spawnChance)
		{
			if (links.Length > 0)
			{
				sendIdea("asdf", links[Random.Range(0, links.Length)]);
			}
			else
			{
				//print("orphaned node");
			}
		}
	}

	void sendIdea(string idea, Node dest)
	{
		Idea temp = GameObject.Instantiate<Idea>(ideaObj);
		temp.ideaStr = idea;
		temp.origin = transform.position;
		temp.destination = dest.transform.position;
		temp.transform.position = transform.position;

	}

	public void reciveIdea(string ideaStr)
	{
		//ADD CODE TO CHANGE VALUES AND SUCH HERE!
		//print("recieved: " + ideaStr);
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
            for(int i = 0; i < ideas.Length; i++)
            {
                Global.text += ideas[i] + "\n";
            }
        }
    }
}
