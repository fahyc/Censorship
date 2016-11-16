using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class NetworkLineRenderer : NetworkBehaviour {
	class SyncListPosition : SyncListStruct<Vector3> {}

	SyncListPosition points = new SyncListPosition ();
	public Vector3 v1;
	public Vector3 v2;
    [SyncVar]
    public Color lineColor;
	public override void OnStartServer() {
		points.Add(v1);
		points.Add(v2);
        
	}
	/*
    [Server]
    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
    {
        if (initialize) {
            // visible to all players
            foreach (NetworkConnection conn in NetworkServer.connections)
            {
                observers.Add(conn);
            }
            return true;
        }
        return false;
    }

    [Server]
    public override bool OnCheckObserver(NetworkConnection newObserver)
    {
        return true;
    }*/

    public override void OnStartClient() {
		GetComponent<LineRenderer> ().SetPosition (0, points[0]);
		GetComponent<LineRenderer> ().SetPosition (1, points[1]);
        GetComponent<LineRenderer>().SetColors(lineColor, lineColor);
        
        //VisibilityCheck v = GetComponent<VisibilityCheck>();

        Vector2 p0 = points[0];
        Vector2 p1 = points[1];
        List<RaycastHit2D> hits = new List<RaycastHit2D>(Physics2D.LinecastAll(p0, p1));

        // the inverse direction of the cast
        Vector2 backwardDir = - (p1 - p0).normalized;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.fraction == 0)
            {
                // completely inside a collider, we're good
                hits.Clear();
                break;
            }
        }

        // sort the hits by distance from p0
        hits.Sort((v1, v2) => (v1.point - p0).sqrMagnitude.CompareTo((v2.point - p0).sqrMagnitude));

        int outPoint = -1, inPoint = -1;

        for (int i=0; i<hits.Count; i++)
        {
            RaycastHit2D hit = hits[i];
            // if this is an out hit
            if (hit.normal.Equals(backwardDir))
            {
                // we want the furthest out point
                if (outPoint < i)
                    outPoint = i;
            }
            // or if it's an inward hit
            else
            {
                if (inPoint < 1)
                    inPoint = i;
            }
        }

        if (outPoint >= 0 && inPoint >= 0)
        {
            // this case there must be a gap
            if (outPoint < inPoint)
            {

            }
        }
	}


	public void setPoints(Vector3 pt1, Vector3 pt2) {
		//Should only be called on the server, but can be called before it knows it is on the server. (Before being spawned.)
		//points = new SyncListPosition();
		
		v1 = pt1;
		v2 = pt2;
//		points.Add(pt1);
	//	points.Add(pt2);
		GetComponent<LineRenderer> ().SetPosition (0, pt1);
		GetComponent<LineRenderer> ().SetPosition (1, pt2);
	}

    public void setColor(Color c)
    {
        lineColor = c;
    }

    public IEnumerator networkLaser()
    {
        NetworkServer.Spawn(gameObject);
        yield return new WaitForSeconds(1.0f);
        NetworkServer.Destroy(gameObject);
        // Destroy(l);

    }
}
