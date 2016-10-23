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

    public Spawnable lurkerPrefab;

	static Spawnable currentTool;
	static int toolIndex;
	static List<UIItem> toHide = new List<UIItem>();
	static List<RectTransform> focusTakers = new List<RectTransform>();
	//static List<UIItem> 
	Inspect inspector;

    public Inspect inspectCanvas;

    public DummyUnit activeDummy;

    public int startingMoney = 500;
    public int currentMoney = 0;
    private int income = 10;

    public int moneyDiff;
    public bool broke = false;

	void Start()
	{
		if (!isLocalPlayer && !isServer)
		{
			print("Is not local. becoming inactive");
			gameObject.SetActive(false);
		}
	}
    // Use this for initialization
    public override void OnStartLocalPlayer () {
		
		inspector = GameObject.FindGameObjectWithTag("Inspector").GetComponent<Inspect>();
		//infoTextBox = GameObject.FindGameObjectWithTag("")
        currentMoney = startingMoney;
        moneyDiff = income;
		DisableDummy();
        // TODO: spawn based on a list of spawn locations
        SpawnObj(lurkerPrefab, new Vector2(0, 0), 1);
	}

	

   /* // For the host client, disable other players' Canvases
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
	*/
    [Command]
    void CmdSpawnObj(int prefabIndex, Vector2 position, int index)
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
    void SpawnObj(Spawnable prefabObject, Vector3 pos, int index)
    {
		if (!isLocalPlayer)
		{
			//Destroy(gameObject);
			return;
		}
        Spawnable costOfUnit = prefabObject.GetComponent<Spawnable>();
        //Do we have money to spawn this wall? If not, just quit. Also, we should probably display "No money to build" somewhere in the UI.
        if (currentMoney < costOfUnit.initialCost)
        {
            return;
        }
        currentMoney -= costOfUnit.initialCost;
        moneyDiff -= costOfUnit.upkeep;
        // need to find index of prefab to spawn
        int prefabIndex = NetworkManager.singleton.spawnPrefabs.IndexOf(prefabObject.gameObject);

		if(prefabIndex < 0)
		{
			print("Error prefabObject is not valid: " + prefabObject);
		}
        CmdSpawnObj(prefabIndex, pos, index);
    }

    // Update is called once per frame
    [ClientCallback]
	void Update () {
        // only update for the local player
        if (!isLocalPlayer)
			return;
        //infoTextBox.text = text;
        //textImage.enabled = textbg;

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
			//print(currentTool);
			if (currentTool)
			{//spawn whatever is selected
				Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
                SpawnObj(currentTool, position, toolIndex);
				//print("Current tool: " + currentTool);
			}
			else {
				//or if there is nothing to spawn, clear any focus and ui elements, or inspect whatever is below the mouse.
				
				Vector3 mousePos = Input.mousePosition;
				mousePos.Set(mousePos.x, mousePos.y, -Camera.main.transform.position.z);
				Collider2D[] hits = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(mousePos));
				//print("trying clear ui.");
				bool hit = false;
				for (int i = 0; i < hits.Length; i++)
				{//try inspecting something
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
				{//Clear the UI if there is nothing below the mouse.
					//print("clearing UI.");
					Global.text = "";
					textbg = false;
					for(int i = 0; i < toHide.Count; i++)
					{
						toHide[i].Disable();
					}
					toHide.Clear();
					DisableDummy();
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

	public void EnableDummy(DummyUnit dummyPrefab)
	{
        activeDummy = Instantiate(dummyPrefab);
	}

	public void DisableDummy()
	{
		//print("disabling dummy" + activeDummy);
		//print(activeDummy);
		if (activeDummy != null)
		{
			Destroy(activeDummy.gameObject);
		}
	}

	[Client]
	public static void setTool(Spawnable obj, int index)
	{
		toolIndex = index;
		currentTool = obj;
	}
    // Increment a player's income when the day increases.
    [ClientCallback]
    public void addIncome() {
        int count = 0;
        if (currentMoney < Mathf.Abs(moneyDiff) && moneyDiff < 0 && broke == false) {
            print("Disabling all Units due to lack of funds");
            broke = true;
            foreach (Spawnable sp in FindObjectsOfType<Spawnable>()) {
                if (sp.owner == connectionToClient) {
                    count++;
                    sp.disabled = true;

                }
            }
        }
        //If we're not broke, then we can gain or lose income. If we are broke, we don't want to change ANYTHING.
        if(!broke)
            currentMoney += moneyDiff;
        else {
            if(currentMoney > 0) {
                broke = false;
            }
        }
    }
}
