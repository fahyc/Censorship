using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class BasicVision : NetworkBehaviour {

    public bool seesAll = false;
    CircleCollider2D visionCollider;

    public override void OnStartClient()
    {
        foreach (SpriteRenderer r in GetComponentsInChildren<SpriteRenderer>())
        {
            if (r.transform.parent != null)
                r.enabled = false;
        }
    }

    public override void OnStartAuthority()
    {
        foreach (SpriteRenderer r in GetComponentsInChildren<SpriteRenderer>())
        {
            if (r.transform.parent != null)
                r.enabled = true;
        }
    }

    // For the host client, disable other players' Canvases
    [Client]
    public override void OnSetLocalVisibility(bool vis)
    {
        // disable rendering
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.transform.parent == null)
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
        CircleCollider2D c = null;
        foreach (CircleCollider2D col in GetComponentsInChildren<CircleCollider2D>())
        {
            if (col.gameObject.transform.parent != null)
            {
                // find a circle collider in the child, not parent
                c = col;
                break;
            }
        }

        if (c == null)
            return; // no child circle collider found

        visionCollider = c;
        
        float r = c.radius;
        Collider2D[] visibleNodes = Physics2D.OverlapCircleAll(transform.position, r);

        foreach (Collider2D other in visibleNodes)
        {
            // no need to check self
            if (other.gameObject == gameObject)
                continue;
            ViewObject(other, true);
        }
    }

    // When we overlap a visCheck object, add this lurker to the list of objs that can see it
    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other)
    {
        // don't collide with self
        if (other.gameObject == gameObject)
            return;
        ViewObject(other, true);
    }

    [ServerCallback]
    void OnTriggerExit2D(Collider2D other)
    {
        // make sure it's actually out of our radius
        if (!visionCollider.IsTouching(other))//Null references!
            ViewObject(other, false);
    }

    [Server]
    // add this lurker to list of objects that can see object
    public void ViewObject(Collider2D other, bool add)
    {
        VisibilityCheck vis = other.GetComponent<VisibilityCheck>();
        if (vis != null)
        {
            if (add)
            {
                vis.AddLurker(gameObject);
                /*
                if(!vis.visibleToLurkers && !vis.hasAuthority)
                {
                    soundObj.Play();
                }
                */
            } else {
                vis.RemoveLurker(gameObject);
            }
            vis.GetComponent<NetworkIdentity>().RebuildObservers(false);
        }
    }
}
