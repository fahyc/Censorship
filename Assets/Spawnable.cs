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
    public Color disabledColor = Color.gray;




    //public override void OnStartLocalPlayer() {
    //    owningPlayer = LayerMask.NameToLayer("PlayerOwned");
    //    print("Starting on a local player");
    //    //base.OnStartLocalPlayer();
    //}

    //public override void OnStartServer() {
    //    owningPlayer = LayerMask.NameToLayer("EnemyOwned");
    //    base.OnStartServer();
    //}

    [ClientCallback]
    void Update() {
        if (disabled) {
            GetComponent<SpriteRenderer>().color = disabledColor;
        }
    }
	void OnDestroy()
	{
		Global.getLocalPlayer().addUpkeep(-upkeep);
	}
}
