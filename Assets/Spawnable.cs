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

    AudioSource sound;

	
	void Start()
	{
        sound = GameObject.FindGameObjectWithTag("unitLostSound").GetComponent<AudioSource>();
        //GetComponent<SpriteRenderer>().color = IdeaList.instance.list[index].color;
    }


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
	protected virtual void OnDestroy()
	{
		//if (Global.isReady())
//		print("Destroying " + this);
		

        if (isClient && hasAuthority)
        {
			Global global = Global.getLocalPlayer();
			if (global)
			{
				global.addUpkeep(-upkeep);
			}
			try
            {
                sound = GameObject.FindGameObjectWithTag("unitLostSound").GetComponent<AudioSource>();
            } catch
            {

            }
            if(sound != null)
            {
                sound.Play();
            }
            
        }
    }
}
