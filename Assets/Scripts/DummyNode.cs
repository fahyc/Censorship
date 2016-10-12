﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (DummyUnit))]
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

    void Update()
    {
		clearLines();
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
	
	public void clearLines()
	{
		for (int i = 0; i < lines.Count; i++)
		{
			Destroy(lines[i].gameObject);
		}
		lines.Clear();
		nodes.Clear();
	}

	void OnDestroy()
	{
		clearLines();
	}
}