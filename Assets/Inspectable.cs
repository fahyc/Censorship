using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Inspectable : NetworkBehaviour {

    public override void OnStartAuthority()
    {
        Debug.Log(GetComponent<NetworkIdentity>().clientAuthorityOwner);
    }

    [Command]
    void CmdDestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    
    [Client]
    public void DestroySelf()
    {
        Debug.Log(GetComponent<NetworkIdentity>().clientAuthorityOwner);
        CmdDestroySelf();
    }
}
