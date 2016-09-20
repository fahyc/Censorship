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
}
	
[Serializable]
class AbstractIdea : System.Object {
	public string name;
	public string description;
	public int opposite;
	public float value;
	public Color color;
    public Event[] events;
    int eventIndex = 0;

    [Serializable]
    public struct Event
    {
        public string name;
        public string description;
        public float threshold;
    }

    public void updateValue(float amt) {
		value = value + (amt / IdeaList.nodeCount);

        Debug.Log("update value for " + name + " : " + value);

        if (eventIndex < events.Length && value > events[eventIndex].threshold)
        {
            // trigger event and display description
            Global.text = events[eventIndex].description;
            eventIndex++;
        }
	}
}