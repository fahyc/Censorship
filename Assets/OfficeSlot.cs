using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class OfficeSlot : NetworkBehaviour {

	SpriteRenderer r;

	public List<Node> domain = new List<Node>();
	int[] nodeCountList;

	[SyncVar(hook = "onChangeIndex")]
	public int mainIdea;

	public Spawnable office;
	public Spawnable officeInstance;

	// Use this for initialization
	
	public void spawnOffice()
	{
		if (GameObject.FindGameObjectWithTag("Player").GetComponent<Global>().playerIdeaIndex == mainIdea && !officeInstance)
		{
			spawnOffice(GameObject.FindGameObjectWithTag("Player").GetComponent<Global>().playerIdeaIndex);
		}
	}

	public void spawnOffice(int idea)
	{
		Spawnable temp = Instantiate(office);
		temp.transform.position = transform.position;
		temp.index = idea;
		temp.transform.parent = transform;
		officeInstance = temp;
		NetworkServer.Spawn(temp.gameObject);
	}

	public override void OnStartServer() {
		nodeCountList = new int[IdeaList.staticList.Length];
		
	}

	void Start()
	{
		r = GetComponent<SpriteRenderer>();
	}

	public void onChangeIndex(int i)
	{
		if (!r)
		{
			Debug.LogWarning("Getting sprite. This shouldn't happen.");
			r = GetComponent<SpriteRenderer>();
		}
		r.color = IdeaList.staticList[mainIdea].color;
	}

	int index;
	// Update is called once per frame
	[ServerCallback]
	void Update () {
		if (index < domain.Count){
			nodeCountList[domain[index].importantIndex]++;
			index++;
		}
		else
		{
			index = 0;
			int max = nodeCountList[0];
			int maxIndex = 0;
			for(int i = 1; i < nodeCountList.Length; i++)
			{
				if(nodeCountList[i] > max)
				{
					max = nodeCountList[i];
					maxIndex = i;
				}
				//else
				//{
				//	print(nodeCountList[i] + "is smaller than " + max);
				//}
			}
			mainIdea = maxIndex;
			nodeCountList = new int[IdeaList.staticList.Length];
			index = 0;
		}
	}

    // need specialty case for disabling collider
    [Client]
    public override void OnSetLocalVisibility(bool vis)
    {
        // use default behavior
        // GetComponent<VisibilityCheck>().OnSetLocalVisibility(vis);

        // and disable colliders
        foreach (Inspectable i in GetComponentsInChildren<Inspectable>())
        {
            i.enabled = vis;
        }
    }
}