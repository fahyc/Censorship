using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

class IdeaList : MonoBehaviour {

	public AbstractIdea[] list;
	public static AbstractIdea[] staticList;

	public static int nodeCount = 0;

	public Dictionary<string, int> ideaDict = new Dictionary<string, int>();
	public static Dictionary<string, int> staticDict;

	void Awake()
	{
		staticList = list;
		for (int i = 0; i < list.Length; i++)
		{
			ideaDict.Add(list[i].name, i);
		}

		staticDict = ideaDict;
	}
	
	void Update() {
		// TODO: check value of each AbstractIdea and fire an event if high enough
	}
}
	
[Serializable]
class AbstractIdea : System.Object {
	public string name;
	public string description;
	public int opposite;
	public float value;
	public Color color;

	public void updateValue(float amt) {
		value = value + (amt / IdeaList.nodeCount);
	}
}