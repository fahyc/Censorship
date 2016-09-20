using UnityEngine;
using System.Collections;
using System;

class IdeaList : MonoBehaviour {

	public AbstractIdea[] list;
	public static int nodeCount = 0;

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