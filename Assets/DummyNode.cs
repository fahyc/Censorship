using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System.Collections.Generic;

public class DummyNode : MonoBehaviour {
	Transform t;
	public float range;
	public static List<Node> nodes = new List<Node>();
	List<LineRenderer> lines = new List<LineRenderer>();
	public LineRenderer link;
	
	// Use this for initialization
	void Start () {
		t = transform;
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < lines.Count; i++)
		{
			Destroy(lines[i].gameObject);
		}
		lines.Clear();
		nodes.Clear();
		t.position = Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
		Collider2D[] col = Physics2D.OverlapCircleAll(t.position, range);
		for(int i = 0; i < col.Length; i++)
		{
			Node temp = col[i].GetComponent<Node>();
			if (temp)
			{
				nodes.Add(temp);
				LineRenderer line = Instantiate(link);
				line.SetPosition(0, t.position);
				line.SetPosition(1, temp.transform.position);
				lines.Add(line);
			}
		}
	}
}
