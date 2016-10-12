using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkLineRenderer : NetworkBehaviour {
	class SyncListPosition : SyncListStruct<Vector3> {}

	SyncListPosition points = new SyncListPosition ();
	public Vector3 v1;
	public Vector3 v2;

	

	public override void OnStartServer() {
		points.Add(v1);
		points.Add(v2);
	}
	
	public override void OnStartClient() {
		GetComponent<LineRenderer> ().SetPosition (0, points[0]);
		GetComponent<LineRenderer> ().SetPosition (1, points[1]);
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
}
