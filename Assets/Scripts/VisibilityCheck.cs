using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class VisibilityCheck : NetworkBehaviour {

    public List<Spawnable> lurkersWatching;

    // For the host client, disable other players' Canvases
    [Client]
    public override void OnSetLocalVisibility(bool vis)
    {
        foreach(Renderer r in GetComponents<Renderer>())
        {
            r.enabled = false;
        }
    }

    // make self invisible to new clients
    [Server]
    public override bool OnCheckObserver(NetworkConnection conn)
    {
        return false;
    }

    // We only want the owner client to observe their Canvas
    [Server]
    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool init) {
        bool changed = false;
        if (init)
        {
            Spawnable me = GetComponent<Spawnable>();
            if (me != null)
            {
                observers.Add(me.owner);
                changed = true;
            }
            // observers.Add(connectionToClient);
        }
        else {
            foreach (Spawnable l in lurkersWatching)
            {
                changed = true;
                observers.Add(l.owner);
            }
        }

        return changed;
    }


}
