using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using ExtensionMethods;


public class Global : NetworkBehaviour {

    public Text infoTextBox;
    public static string text;
    public Image textImage;
    public static bool textbg = true;

	static Spawnable currentTool;
	static int toolIndex;
	Inspect inspector;

    public Inspect inspectCanvas;

	// Use this for initialization
	void Start () {
		inspector = GameObject.FindGameObjectWithTag("Inspector").GetComponent<Inspect>();
	}

    [Command]
    void CmdSpawnWall(int prefabIndex, Vector3 position, int index)
    {
        // now convert back from index to prefab
        GameObject prefabToSpawn = NetworkManager.singleton.spawnPrefabs[prefabIndex];

        // actually instantiate/initialize the object
        GameObject temp = Instantiate(prefabToSpawn);
        temp.GetComponent<Spawnable>().index = index;
        temp.transform.position = position;

        // and give the client authority over it
        NetworkServer.SpawnWithClientAuthority(temp.gameObject, connectionToClient);
    }

    [Client]
    public void SpawnWall(GameObject prefabObject, Vector3 pos, int index)
    {
        // need to find index of prefab to spawn
        int prefabIndex = NetworkManager.singleton.spawnPrefabs.IndexOf(prefabObject);
        CmdSpawnWall(prefabIndex, pos, index);
    }

    // Update is called once per frame
    [ClientCallback]
	void Update () {
        infoTextBox.text = text;
        textImage.enabled = textbg;

        if(Input.GetMouseButtonDown(1)) {

			currentTool = null;
            //Vector3 mousePos = Input.mousePosition;
            //mousePos.Set(mousePos.x, mousePos.y, -Camera.main.transform.position.z);

            //Collider2D hitWall = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(mousePos));

            /*if (hitWall != null && hitWall.gameObject.GetComponent<WallScript>() != null)
            {
                Destroy(hitWall.gameObject);
            } else
            {
                WallScript temp = (WallScript)Instantiate(wall, Camera.main.ScreenToWorldPoint(mousePos), Quaternion.identity);
            }*/
        }

        if(Input.GetMouseButtonDown(0))
        {
			if (currentTool)
			{
                //print(Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1)));
                //print(Input.mousePosition);

                if (hasAuthority)
                {
                    Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
                    
                    SpawnWall(currentTool.gameObject, position, toolIndex);
                }
                else
                {
                    Debug.LogWarning("No authority to spawn wall");
                }
			}
			else {

				Vector3 mousePos = Input.mousePosition;
				mousePos.Set(mousePos.x, mousePos.y, -Camera.main.transform.position.z);
				Collider2D[] hits = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(mousePos));

				if (hits.Length == 0)
				{
					Global.text = "";
					textbg = false;
					inspector.Disable();
					
				}
				else
				{
					bool hit = false;
					for(int i = 0; i < hits.Length; i++)
					{
						Inspectable temp = hits[i].GetComponent<Inspectable>();
						if (temp)
						{
							print("enabling Inspect");
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

	public static void setTool(Spawnable obj, int index)
	{
		toolIndex = index;
		currentTool = obj;
	}
}
