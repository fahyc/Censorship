using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Inspectable : NetworkBehaviour {

    [Command]
    void CmdDestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    
    [Client]
    public void DestroySelf()
    {
        if (hasAuthority)
            CmdDestroySelf();
    }
}
