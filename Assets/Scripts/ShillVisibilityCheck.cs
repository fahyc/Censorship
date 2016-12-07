using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ShillVisibilityCheck : VisibilityCheck {

    public Sprite visibleSprite;
    public Sprite hiddenSprite;

    [Client]
    public override void OnStartClient()
    {
        // show the "hidden" sprite by default for non-owner players
        GetComponent<SpriteRenderer>().sprite = hiddenSprite;
    }

    [Client]
    public override void OnStartAuthority()
    {
        // for the owner, set the visible sprite
        GetComponent<SpriteRenderer>().sprite = visibleSprite;
    }

    [TargetRpc]
    public void TargetSetVisibility(NetworkConnection target, bool visible)
    {
        // never change the sprite for the owner player
        if (hasAuthority)
            return;

        if (!visible)
            GetComponent<SpriteRenderer>().sprite = hiddenSprite;
        else
            GetComponent<SpriteRenderer>().sprite = visibleSprite;
    }

    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool init)
    {
        // use same visibility code as normal
        bool result = base.OnRebuildObservers(observers, init);

        // now update sprite based on owner of lurkers watching
        foreach (Spawnable s in lurkersWatching)
        {
            // For any single lurker that can see it, set it visible
            if (s.GetComponent<BasicVision>().seesAll)
            {
                TargetSetVisibility(s.GetComponent<Spawnable>().owner, true);
                break;
            }
            TargetSetVisibility(s.GetComponent<Spawnable>().owner, false);
        }

        return result;
    }

    /*
    [Server]
    public override void AddLurker(GameObject lurker)
    {
        base.AddLurker(lurker);
    }

    [Server]
    public override void RemoveLurker(GameObject lurker)
    {
        base.RemoveLurker(lurker);
        if (lurker.GetComponent<BasicVision>().seesAll)
        {
            TargetSetVisibility(lurker.GetComponent<Spawnable>().owner, true);
        }
    }
    */

    // all connections can see shills
    [Server]
    public override bool OnCheckObserver(NetworkConnection conn)
    {
        return true;
    }
}
