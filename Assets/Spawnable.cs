using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Spawnable : NetworkBehaviour {
    [SyncVar]
	public int index;// = -1;

    public NetworkConnection owner;

    public int initialCost;
    public int upkeep;
    public bool disabled = false;

    [Server]
    void Update() {
        if (disabled) {
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}
