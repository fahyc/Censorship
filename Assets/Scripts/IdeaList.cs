﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

class IdeaList : MonoBehaviour {

    public AbstractIdea[] list;
    public static AbstractIdea[] staticList;

    public static int nodeCount = 0;

    public Dictionary<string, int> ideaDict = new Dictionary<string, int>();
    public static Dictionary<string, int> staticDict;

    static IdeaList instance;

    void Awake()
    {
        instance = this;
        staticList = list;
        for (int i = 0; i < list.Length; i++)
        {
            ideaDict.Add(list[i].name, i);
        }

        staticDict = ideaDict;
    }

    public static void rollForEvent(AbstractIdea idea)
    {
        instance.StartCoroutine(instance.checkEvent(idea));
    }

    IEnumerator checkEvent(AbstractIdea idea)
    {
        int index = staticDict[idea.name];
        float ratio = staticList[index].value / (float) nodeCount;

        while (ratio > staticList[index].getEventThreshold())
        {
            Debug.Log(idea.name + ": " + ratio);

            if (UnityEngine.Random.value < 0.08)
            {
                idea.triggerEvent();
                break;
            }

            yield return new WaitForSeconds(1);

            ratio = staticList[index].value / (float) nodeCount;
        }

        idea.stopChecking();
        yield return null;
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
    bool checkingForEvents = false;

    [Serializable]
    public struct Event
    {
        public string name;
        public string description;
        public float threshold;
    }

    public void updateValue(int amt) {
        value += amt;

        if (eventIndex < events.Length && !checkingForEvents)
        {
            checkingForEvents = true;
            IdeaList.rollForEvent(this);
        }
	}

    public void stopChecking()
    {
        checkingForEvents = false;
    }

    public void triggerEvent()
    {
        // trigger event and display description
        Global.text = "<size=16><b><color=#" + ColorToHex(color) + ">" + events[eventIndex].name + "</color></b></size>" + ": " + events[eventIndex].description + "\n\n";
        Global.textbg = true;
        eventIndex++;
    }

    public float getEventThreshold()
    {
        return events[eventIndex].threshold;
    }

    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
}