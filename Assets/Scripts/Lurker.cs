using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Lurker : NetworkBehaviour {

    public bool seesAll = false;

    // For the host client, disable other players' Canvases
    [Client]
    public override void OnSetLocalVisibility(bool vis)
    {
        // disable rendering
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = vis;
        }

        // and colliders
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = vis;
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
    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool init)
    {
        if (init)
        {
            // add the owner as able to view the lurker
            observers.Add(GetComponent<Spawnable>().owner);
            return true;
        }
        return false;
    }

    // get initial nodes
    public override void OnStartServer()
    {
        // Just do a simple circle overlap check for colliders when spawned
        float r = GetComponent<CircleCollider2D>().radius;
        Collider2D[] visibleNodes = Physics2D.OverlapCircleAll(transform.position, r);

        foreach (Collider2D other in visibleNodes)
        {
            ViewObject(other, true);
        }
    }

    // When we overlap a visCheck object, add this lurker to the list of objs that can see it
    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other)
    {
        ViewObject(other, true);
    }

    [ServerCallback]
    void OnTriggerExit2D(Collider2D other)
    {
        ViewObject(other, false);
    }

    // add this lurker to list of objects that can see object
    public void ViewObject(Collider2D other, bool add)
    {
        VisibilityCheck vis = other.GetComponent<VisibilityCheck>();
        if (vis != null)
        {
            if (add)
                vis.AddLurker(gameObject);
            else
                vis.RemoveLurker(gameObject);
            vis.GetComponent<NetworkIdentity>().RebuildObservers(false);
        }
    }
}
