﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class VisibilityCheck : NetworkBehaviour {

    public bool startVisibleToSelf = false;
    public bool visibleToLurkers = true;

    HashSet<VisibilityCheck> connectedEntities = new HashSet<VisibilityCheck>();
    HashSet<Spawnable> lurkersWatching = new HashSet<Spawnable>();

    /*
	[Server]
	void Awake()
	{
		print("awake");
	}*/

    public override void OnStartServer()
    {
        // We need to initialize visibility for lurkers
        CircleCollider2D c = GetComponent<CircleCollider2D>();

        if (c != null) {
            Collider2D[] visibleNodes = Physics2D.OverlapCircleAll(transform.position, c.radius);

            foreach (Collider2D other in visibleNodes)
            {
                Lurker l = other.GetComponent<Lurker>();
                if (l != null)
                {
                    l.ViewObject(c, true);
                    lurkersWatching.Add(l.GetComponent<Spawnable>());
                }
            }
        }
    }

    // For the host client, disable "invisible" objects
    [Client]
    public override void OnSetLocalVisibility(bool vis)
    {
        // disable renderers
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
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

        if (startVisibleToSelf && init)
        {
            Spawnable s = GetComponent<Spawnable>();
            if (s != null)
                observers.Add(s.owner);
        }

        if (visibleToLurkers)
        {
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