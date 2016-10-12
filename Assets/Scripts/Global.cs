using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine.EventSystems;

public class Global : NetworkBehaviour {

    public Text infoTextBox;
    public static string text;
    public Image textImage;
    public static bool textbg = true;

	static Spawnable currentTool;
	static int toolIndex;
	static List<UIItem> toHide = new List<UIItem>();
	static List<RectTransform> focusTakers = new List<RectTransform>();
	//static List<UIItem> 
	Inspect inspector;

    public Inspect inspectCanvas;


	static DummyNode dummy;

    public int startingMoney = 500;
    public int currentMoney = 0;
    private int income = 10;

    public int moneyDiff;

    //


    // Use this for initialization
    public override void OnStartLocalPlayer () {
		inspector = GameObject.FindGameObjectWithTag("Inspector").GetComponent<Inspect>();
		dummy = GameObject.FindGameObjectWithTag("Dummy").GetComponent<DummyNode>();

        currentMoney = startingMoney;
        moneyDiff = income;
		DisableDummy();
	}

    // For the host client, disable other players' Canvases
    [Client]
    public override void OnSetLocalVisibility(bool vis)
    {
        gameObject.SetActive(vis);
    }

    // make self invisible to new clients
    [Server]
    public override bool OnCheckObserver(NetworkConnection conn)
    {
        return false;
    }

    // We only want the owner client to observe their Canvas
    [Server]
    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool init) {
        if (init)
        {
            observers.Add(connectionToClient);
            return true;
        }
        return false;
    }

    [Command]
    void CmdSpawnWall(int prefabIndex, Vector3 position, int index)
    {
		// now convert back from index to prefab
		//print(prefabIndex);
        GameObject prefabToSpawn = NetworkManager.singleton.spawnPrefabs[prefabIndex];

        // actually instantiate/initialize the object
        GameObject temp = Instantiate(prefabToSpawn);
        temp.GetComponent<Spawnable>().index = index;
        temp.GetComponent<Spawnable>().owner = connectionToClient;
        temp.transform.position = position;

        // and give the client authority over it
        NetworkServer.SpawnWithClientAuthority(temp, connectionToClient);
    }

    [Client]
    public void SpawnWall(GameObject prefabObject, Vector3 pos, int index)
    {
        Spawnable costOfUnit = prefabObject.GetComponent<Spawnable>();
        //Do we have money to spawn this wall? If not, just quit. Also, we should probably display "No money to build" somewhere in the UI.
        if (currentMoney < costOfUnit.initialCost)
        {
            return;
        }
        currentMoney -= costOfUnit.initialCost;
        moneyDiff -= costOfUnit.upkeep;
        // need to find index of prefab to spawn
        int prefabIndex = NetworkManager.singleton.spawnPrefabs.IndexOf(prefabObject);

		if(prefabIndex < 0)
		{
			print("Error prefabObject is not valid: " + prefabObject);
		}
        CmdSpawnWall(prefabIndex, pos, index);
    }

    // Update is called once per frame
    [ClientCallback]
	void Update () {

        // only update for the local player
        if (!isLocalPlayer)
            return;

        infoTextBox.text = text;
        textImage.enabled = textbg;


		if (overlappingFocusable())
		{
			return;
		}

        if(Input.GetMouseButtonDown(1)) {
			currentTool = null;
			DisableDummy();
		}

        if(Input.GetMouseButtonDown(0))
        {//if left mouse button
			if (currentTool)
			{//spawn whatever is selected
				Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
                SpawnWall(currentTool.gameObject, position, toolIndex);
			}
			else {
				//or if there is nothing to spawn, clear any focus and ui elements, or inspect whatever is below the mouse.
				Vector3 mousePos = Input.mousePosition;
				mousePos.Set(mousePos.x, mousePos.y, -Camera.main.transform.position.z);
				Collider2D[] hits = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(mousePos));

				if (hits.Length == 0)
				{//Clear the UI if there is nothing below the mouse.
					Global.text = "";
					textbg = false;
					for(int i = 0; i < toHide.Count; i++)
					{
						toHide[i].Disable();
					}
					toHide.Clear();
					DisableDummy();
				}
				else
				{//otherwise try inspecting somehting. 
					bool hit = false;
					for(int i = 0; i < hits.Length; i++)
					{
						Inspectable temp = hits[i].GetComponent<Inspectable>();
                        // Make sure we have authority on the object we're looking at
						if (temp && temp.hasAuthority)
						{
							// print("enabling Inspect");
							inspector.Enable(temp.gameObject);
							hit = true;
						}
						else if (hits[i].GetComponent<Inspect>())
						{
							hit = true;
						}
					}
					if (!hit)
					{
						inspector.Disable();
					}
				}
			}
            //Global.text = "";

        }


        if (Input.GetKeyDown(KeyCode.K)) {
            currentMoney += 500;
            Debug.Log("Holla holla get dolla");
        }
    }
	bool overlappingFocusable()
	{
		for(int i = 0; i < focusTakers.Count; i++)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(focusTakers[i], Input.mousePosition)){
				return true;
			}
		}
		return false;
	}

	public static void addUIItem(UIItem item)
	{
		toHide.Add(item);
	}

	public static void addFocusTaker(RectTransform item)
	{
		focusTakers.Add(item);
	}
	public static void removeFocusTaker(RectTransform item)
	{
		focusTakers.Remove(item);
	}

	public void EnableDummy()
	{
		dummy.gameObject.SetActive(true);
	}


	public void DisableDummy()
	{
		dummy.gameObject.SetActive(false);
	}

	[Client]
	public static void setTool(Spawnable obj, int index)
	{
		toolIndex = index;
		currentTool = obj;
	}
    // Increment a player's income when the day increases.
    [Client]
    public void addIncome() {
        currentMoney += moneyDiff;
    }
}
