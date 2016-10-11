using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Lurker : MonoBehaviour {

	// When we overlap a visCheck object, add this lurker to the list of objs that can see it
    void OnTriggerEnter2D(Collider2D other)
    {
        Spawnable spawn = GetComponent<Spawnable>();
        VisibilityCheck vis = other.GetComponent<VisibilityCheck>();
        if (vis != null)
        {
            vis.lurkersWatching.Add(spawn);
            vis.GetComponent<NetworkIdentity>().RebuildObservers(false);
        }
    }
}
