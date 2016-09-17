using UnityEngine;
using System.Collections;

public class LineDraw : MonoBehaviour {

	public float start;
	public float end;
	public float thickness;

	public void Draw(Vector3 start, Vector3 end)
	{
		float len = (start - end).magnitude;
		float angle = Mathf.Atan2(start.y - end.y, start.x - end.x) * Mathf.Rad2Deg - 90;
		print(angle);
		transform.eulerAngles = new Vector3(0, 0, angle);
		transform.localScale = new Vector3(len, thickness, 1);

	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
