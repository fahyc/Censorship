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

	public List<Inspectable> selected = new List<Inspectable>();
	
    public Inspect inspectCanvas;

    public DummyUnit activeDummy;
    public GameObject commandCard;
    [SyncVar]
	public int playerIdeaIndex = 0;//the player's idea.

    public int startingMoney = 500;
    public int currentMoney = 0;
    private int income = 10;

    public int moneyDiff;
    private int upkeep = 0;
    public bool broke = false;

	public SpriteRenderer selectionbox;

	//public RectTransform selector;
	public Vector3 selectStart;
	public float minSelectionDistance = .5f;
	//bool selecting;
	public LineRenderer lineTemplate;
	//public Texture selectTexture;

	//void Start()
	//{
		
	//}
	/*
	[ClientCallback]
	void Start()
	{
		print("localplayer" + isLocalPlayer);
		if (!isLocalPlayer)
		{
			//print("Is not local. becoming inactive");
			//gameObject.SetActive(false);
			if (isServer)
			{
				//print("serverside");
				StartCoroutine(setInactiveSoon());
			}
			else
			{
				//print("clientside");
				gameObject.SetActive(false);
			}
			return;
		}
	}*/
	IEnumerator setInactiveSoon()
	{
		yield return new WaitForSeconds(1);
		gameObject.SetActive(false);
	}
    // Use this for initialization
    public override void OnStartLocalPlayer () {


		inspector = GameObject.FindGameObjectWithTag("Inspector").GetComponent<Inspect>();
		selectionbox = Instantiate(selectionbox);
		selectionbox.gameObject.SetActive(false);
        currentMoney = startingMoney;
        moneyDiff = income - upkeep;
		DisableDummy();
        // TODO: spawn based on a list of spawn locations
        int prefabIndex = NetworkManager.singleton.spawnPrefabs.IndexOf(lurkerPrefab.gameObject);

		if(prefabIndex < 0)
		{
            print("Error prefabObject is not valid: " + lurkerPrefab.gameObject);
		}
        CmdSpawnObj(prefabIndex, transform.position, 1);
        //Center camera on start positions.    
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        commandCard = GameObject.FindGameObjectWithTag("CommandCard");
    }
	
	
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

        // check that we're spawning inside visible FoW zone
        Collider2D[] hits = Physics2D.OverlapPointAll(pos);
        bool hitLurker = false;
        foreach(Collider2D h in hits)
        {
            if (h.GetComponent<BasicVision>() != null && h.GetComponent<NetworkIdentity>().hasAuthority)
                hitLurker = true;
        }

        if (!hitLurker) // we tried to spawn in a disallowed area
        {
            return;
        }

        Spawnable costOfUnit = prefabObject.GetComponent<Spawnable>();
        //Do we have money to spawn this wall? If not, just quit. Also, we should probably display "No money to build" somewhere in the UI.
        if (currentMoney < costOfUnit.initialCost)
        {
            return;
        }
        currentMoney -= costOfUnit.initialCost;
        //moneyDiff -= costOfUnit.upkeep;
        upkeep += costOfUnit.upkeep;
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

		//print("update" + isLocalPlayer);
		//		print(mouseToWorld());
		if (!isLocalPlayer)
		{
			return;
			//gameObject.SetActive(false);
		}
		//infoTextBox.text = text;
		//textImage.enabled = textbg;
		/*
		if (overlappingFocusable())
		{
			return;
		}
		*/


		if (Input.GetKeyDown(KeyCode.K))
		{
			currentMoney += 500;
			Debug.Log("Holla holla get dolla");
		}
		if (IdeaList.instance.Prevalence.Count > 0)
		{
			income = IdeaList.instance.Prevalence[playerIdeaIndex];
		}
		//		print("income: " + income + " upkeep " + upkeep);
		moneyDiff = income - upkeep;


		if (selectStart != Vector3.zero && (mouseToWorld() - selectStart).magnitude > minSelectionDistance)
		{
			DrawSelectBox(mouseToWorld(), selectStart);

		}
		else
		{
			selectionbox.gameObject.SetActive(false);
		}

        if(Input.GetMouseButtonDown(1)) {
			currentTool = null;
            
            DisableDummy();
			SpawnCircleManager.Clear();

			Vector3 pt = mouseToWorld();
			Vector3 averageStart = Vector3.zero;
			int mobilecount = selected.Count;
			for(int i = 0; i< selected.Count; i++)
			{
				if (!selected[i].GetComponent<MovementController>())
				{
					mobilecount--;
					continue;
				}
				averageStart += selected[i].transform.position;
			}
			if (selected.Count > 0){averageStart /= mobilecount;}
			for (int i = 0; i < selected.Count; i++)
			{
				Vector3 dif =  selected[i].transform.position - averageStart;
				selected[i].goTo(dif + pt);
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			selectStart = mouseToWorld(); //Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
		}
		if (Input.GetMouseButtonUp(0)) {
			//if left mouse button
			//print(currentTool);
			//always clear the selection.
			
			
			if ((mouseToWorld() - selectStart).magnitude > minSelectionDistance)
			{
				clearSelected();
				DisableDummy();
				//unity's overlap box uses a center and width system instead of two points, which is how we store the selection box.
				Vector3 center = (mouseToWorld() + selectStart) / 2;
				Vector3 width = mouseToWorld() - selectStart;
				width = new Vector3(Mathf.Abs(width.x), Mathf.Abs(width.y), 0);
				Collider2D[] selectCol = Physics2D.OverlapBoxAll(center, width, 0);
				//print("start: " + center  + "width: " + width + " hit " + selectCol.Length + " objects" );
				for (int i = 0; i < selectCol.Length; i++)
				{
					Inspectable temp = selectCol[i].GetComponent<Inspectable>();
					if (temp && temp.enabled)
					{
						select(temp);
					}
				}
				selectStart = Vector3.zero;
				return;
			}

			selectStart = Vector3.zero;
			if (currentTool)
			{//spawn whatever is selected
				//Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
                SpawnObj(currentTool, activeDummy.transform.position, toolIndex);
				//print("Current tool: " + currentTool);
			}
			else {
				//or if there is nothing to spawn, clear any focus and ui elements, or inspect whatever is below the mouse.
				clearSelected();
				DisableDummy();
				Vector3 mousePos = Input.mousePosition;
				mousePos.Set(mousePos.x, mousePos.y, -Camera.main.transform.position.z);
				Collider2D[] hits = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(mousePos));
				//print("trying clear ui.");
				bool hit = false;
				for (int i = 0; i < hits.Length; i++)
				{//try inspecting something
					Inspectable temp = hits[i].GetComponent<Inspectable>();
					// Make sure we have authority on the object we're looking at
					if (temp && temp.hasAuthority && temp.enabled)
					{
						// print("enabling Inspect");
						inspector.Enable(temp.gameObject);
						select(temp);
						hit = true;
					}
					else if (hits[i].GetComponent<Inspect>())
					{
						hit = true;
					}
				}
				if (!hit && !overlappingFocusable())
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
					clearSelected();
				}
			}

        }


    }

	public void addUpkeep(int amount)
	{
		upkeep += amount;
	}

	void select(Inspectable obj)
	{
		selected.Add(obj);
		obj.select();
	}
	void clearSelected()
	{
		//Zero out the command card.
		print("clearing");
        commandCard.GetComponent<GridAccess>().OnSelectUnit(null);
		SpawnCircleManager.Clear();
		for(int i = 0; i < selected.Count; i++)
		{
			selected[i].deselect();
		}
		selected.Clear();
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

	public Vector3 mouseToWorld()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
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
		DisableDummy();
        Vector3 pos= Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
        activeDummy = (DummyUnit) Instantiate(dummyPrefab, pos, Quaternion.identity);
        print(activeDummy);
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

	
	
	void DrawSelectBox(Vector3 p1, Vector3 p2)
	{
		selectionbox.gameObject.SetActive(true);
		selectionbox.transform.position = p2;
		Vector3 dif = p2 - p1;
		selectionbox.transform.localScale =  new Vector3(-dif.x,dif.y,0);
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
	public static Global getLocalPlayer()
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < temp.Length; i++)
		{
			Global global = temp[i].GetComponent<Global>();
			if (global.isLocalPlayer)
			{
				return global;
			}
		}

        Debug.LogWarning("Local player object not found!");
		return null;
	}
	public Vector2 closestSpawnableLoc(Vector2 position)
	{
		//Vector2 closest = new Vector3(float.MaxValue, float.MaxValue);
		SpawnCircleManager.Clear();
		int closestIndex = 0;
		float closestDist = float.MaxValue;
		for(int i = 0; i < selected.Count; i++)
		{
			if(selected[i].spawnRange == 0)
			{
				continue;
			}
			SpawnCircleManager.Spawn(selected[i].spawnRange * 2, selected[i].transform.position);
			float dist = (position - selected[i].transform.position.xy()).magnitude;
			if (dist < selected[i].spawnRange)
			{
				return position;
			}
			if(dist < closestDist)
			{
				closestDist = dist;
				closestIndex = i;
			}
		}
		if(selected.Count == 0)
		{
			Debug.LogWarning("Warning, no offices selected but we are still running a dummy.");
		}
		return selected[closestIndex].transform.position.xy() + ((position - selected[closestIndex].transform.position.xy()).normalized * selected[closestIndex].spawnRange); 
	}
}
