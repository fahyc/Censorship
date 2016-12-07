using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class VisibilityCheck : NetworkBehaviour {

    public bool visibleToSelf = false;
    public bool visibleToLurkers = true;

    HashSet<VisibilityCheck> connectedEntities = new HashSet<VisibilityCheck>();
    protected HashSet<Spawnable> lurkersWatching = new HashSet<Spawnable>();

    AudioSource soundObj;

    public override void OnStartClient()
    {
        soundObj = GameObject.FindGameObjectWithTag("enemySeenSound").GetComponent<AudioSource>();
        soundObj.gameObject.AddComponent<EnemySeenSound>();
    }

    // For the host client, disable "invisible" objects
    [Client]
    public override void OnSetLocalVisibility(bool vis)
    {
        // disable renderers
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = vis;

            // disable vision for non-authoritative
            if (r.transform.parent != null && !hasAuthority)
            {
                r.enabled = false;
            }
        }
    }

    // make self invisible to new clients
    [Server]
    public override bool OnCheckObserver(NetworkConnection conn)
    {
        return false;
    }

    [TargetRpc]
    public void TargetPlayFoundSound(NetworkConnection target)
    {
        if(soundObj && soundObj.GetComponent<EnemySeenSound>())
            soundObj.GetComponent<EnemySeenSound>().playSound();
        else
            Debug.LogWarning("Sound Object not found to play FoundSound!");
    }

    // We only want the owner client to observe their Canvas
    [Server]
    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool init) {

        if (visibleToSelf)
        {
            Spawnable s = GetComponent<Spawnable>();
            if (s != null)
                observers.Add(s.owner);
        }

            // unless we're initializing, only make viewable to lurkers
        foreach (Spawnable l in lurkersWatching)
        {
            if (l == null) {
                continue;
            }
            if (visibleToLurkers || l.GetComponent<BasicVision>().seesAll)
            {
                observers.Add(l.owner);
            }
        }

        lurkersWatching.Remove(null);

        foreach (VisibilityCheck v in connectedEntities)
        {
            v.lurkersWatching.UnionWith(lurkersWatching);
            v.GetComponent<NetworkIdentity>().RebuildObservers(false);
        }
        return true;
    }

    [Server]
    public virtual void AddLurker(GameObject lurker)
    {
        Spawnable s = lurker.GetComponent<Spawnable>();
        NetworkConnection owner = s.GetComponent<NetworkIdentity>().clientAuthorityOwner;
        if (s != null)
        {
            if (s.GetComponent<BasicVision>().seesAll && (GetComponent<Spawnable>() || GetComponent<Shill>()))
            {
                // don't play sound for ourselves
                if (GetComponent<NetworkIdentity>().clientAuthorityOwner != owner)
                {
                    // now check that the owner can't already see the object
                    bool isSeen = false;
                    foreach (Spawnable l in lurkersWatching)
                    {
                        if (owner == l.GetComponent<NetworkIdentity>().clientAuthorityOwner && l.GetComponent<BasicVision>().seesAll)
                        {
                            isSeen = true;
                            break;
                        }
                    }

                    if (!isSeen && owner != null)
                        ;// TargetPlayFoundSound(owner);
                    else if (owner == null)
                        // Client authority holder = null, problemo
                        Debug.LogWarning("No client authority! Cannot play found sound");
                }
            }
            lurkersWatching.Add(s);
        }
    }

    [Server]
    public virtual void RemoveLurker(GameObject lurker)
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