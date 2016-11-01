using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {
	public Vector3 target;
	public float moveSpeed;
	public Transform t;
	bool moving = false;

	public void goTo(Vector3 position)
	{
		moving = true;
		target = position;
	}

	// Use this for initialization
	void Start () {
		t = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
		{
			t.Translate((target - t.position).normalized * moveSpeed * Time.deltaTime);
			if(Vector3.Distance(t.position, target) < moveSpeed * Time.deltaTime)
			{
				moving = false;
			}
		}

	}
}
