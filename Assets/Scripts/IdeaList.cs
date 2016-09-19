using UnityEngine;
using System.Collections;
using System;

class IdeaList : MonoBehaviour {

	public AbstractIdea[] list;
}
	
[Serializable]
class AbstractIdea : System.Object {
	public string name;
	public string description;

	[SerializeField]
	public int opposite;

	public int value;
	public Color color;
}