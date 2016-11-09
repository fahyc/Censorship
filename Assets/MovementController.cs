using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MovementController : NetworkBehaviour {
	public float moveSpeed;
	public Transform t;

	[SyncVar]
	bool moving = false;
	[SyncVar]
	public Vector3 origin;
	[SyncVar]
	public Vector3 destination;

	public void goTo(Vector3 position)
	{
		moving = true;
		destination = position;
		origin = transform.position;
	}

	// Use this for initialization
	void Start () {
		t = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
		{
			t.Translate((destination - t.position).normalized * moveSpeed * Time.deltaTime);
			if(Vector3.Distance(t.position, destination) < moveSpeed * Time.deltaTime)
			{
				moving = false;
			}
		}

	}
}
