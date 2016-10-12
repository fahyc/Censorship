using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class VisibilityCheck : NetworkBehaviour {

    public bool startVisibleToSelf = false;

    HashSet<VisibilityCheck> connectedEntities = new HashSet<VisibilityCheck>();
    HashSet<Spawnable> lurkersWatching = new HashSet<Spawnable>();

    // For the host client, disable other players' Canvases
    [Client]
    public override void OnSetLocalVisibility(bool vis)
    {
        // gameObject.SetActive(vis);
        foreach (Renderer r in GetComponents<Renderer>())
        {
            r.enabled = vis;
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

        if (startVisibleToSelf)
        {
            observers.Add(connectionToClient);
        }

        // unless we're initializing, only make viewable to lurkers
        foreach (Spawnable l in lurkersWatching)
        {
            observers.Add(l.owner);
        }

        foreach (VisibilityCheck v in connectedEntities)
        {
            v.lurkersWatching.UnionWith(lurkersWatching);
            v.GetComponent<NetworkIdentity>().RebuildObservers(false);
        }
        return true;
    }

    [Server]
    public void AddLurker(GameObject lurker)
    {
        Spawnable s = lurker.GetComponent<Spawnable>();
        if (s != null)
            lurkersWatching.Add(s);
    }

    [Server]
    public void RemoveLurker(GameObject lurker)
    {
        Spawnable s = lurker.GetComponent<Spawnable>();
        if (s != null)
            lurkersWatching.Remove(s);
    }

    [Server]
    public void AddConnection(GameObject conn)
    {
        VisibilityCheck v = conn.GetComponent<VisibilityCheck>();
        if (v != null)
            connectedEntities.Add(v);
    }

}
