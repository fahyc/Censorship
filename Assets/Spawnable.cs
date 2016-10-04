using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Spawnable : NetworkBehaviour {
    [SyncVar]
	public int index;// = -1;

    public NetworkConnection owner;
}
