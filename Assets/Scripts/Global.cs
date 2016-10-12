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


	static DummyNode dummy;

    // Use this for initialization
    public override void OnStartLocalPlayer() {
        inspector = GameObject.FindGameObjectWithTag("Inspector").GetComponent<Inspect>();
        dummy = GameObject.FindGameObjectWithTag("Dummy").GetComponent<DummyNode>();
        DisableDummy();

        // TODO: spawn based on a list of spawn locations
        SpawnObj(lurkerPrefab, new Vector2(0, 0), 0);
    }

    [Command]
    void CmdSpawnObj(int prefabIndex, Vector2 position, int index)
    {
        // now convert back from index to prefab
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
    public void SpawnObj(Spawnable prefabObject, Vector3 pos, int index)
    {
        // need to find index of prefab to spawn
        int prefabIndex = NetworkManager.singleton.spawnPrefabs.IndexOf(prefabObject.gameObject);
        CmdSpawnObj(prefabIndex, pos, index);
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
        }

        if(Input.GetMouseButtonDown(0))
        {
			if (currentTool)
			{/*
				print("Spawning with index: " + toolIndex);
				Spawnable temp = Instantiate<Spawnable>(currentTool);
				temp.index = toolIndex;
				temp.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
            */
                //print(Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1)));
                //print(Input.mousePosition);

                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
                
                SpawnObj(currentTool, position, toolIndex);
			}
			else {

				Vector3 mousePos = Input.mousePosition;
				mousePos.Set(mousePos.x, mousePos.y, -Camera.main.transform.position.z);
				Collider2D[] hits = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(mousePos));

				if (hits.Length == 0)
				{
					Global.text = "";
					textbg = false;
					//inspector.Disable();
					for(int i = 0; i < toHide.Count; i++)
					{
						toHide[i].Disable();
					}
					toHide.Clear();
				}
				else
				{
					bool hit = false;
					for(int i = 0; i < hits.Length; i++)
					{
						Inspectable temp = hits[i].GetComponent<Inspectable>();
                        // Make sure we have authority on the boject we're looking at
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

	public static void EnableDummy()
	{
		dummy.gameObject.SetActive(true);
	}


	public static void DisableDummy()
	{
		dummy.gameObject.SetActive(false);
	}

	[Client]
	public static void setTool(Spawnable obj, int index)
	{
		toolIndex = index;
		currentTool = obj;
	}
}
