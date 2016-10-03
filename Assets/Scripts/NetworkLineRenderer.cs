using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkLineRenderer : NetworkBehaviour {
	class SyncListPosition : SyncListStruct<Vector3> {}

	SyncListPosition points = new SyncListPosition ();

	public override void OnStartServer() {
		points.Add (new Vector3 ());
		points.Add (new Vector3 ());
	}

	public override void OnStartClient() {
		GetComponent<LineRenderer> ().SetPosition (0, points[0]);
		GetComponent<LineRenderer> ().SetPosition (1, points[1]);
	}

	[Server]
	public void setPoints(Vector3 pt1, Vector3 pt2) {
		points [0] = pt1;
		points [1] = pt2;
		GetComponent<LineRenderer> ().SetPosition (0, pt1);
		GetComponent<LineRenderer> ().SetPosition (1, pt2);
	}
}
