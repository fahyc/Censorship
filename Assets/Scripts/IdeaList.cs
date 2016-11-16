using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

class IdeaList : NetworkBehaviour {

    public AbstractIdea[] list;

	[SyncVar]
    public int nodeCount = 0;

    public Dictionary<string, int> ideaDict = new Dictionary<string, int>();

	//public static List<int> Prevalence;
	//public static int[] Prevalence;

	//[SyncVar(hook = "setPrevalence")]
	public SyncListInt Prevalence = new SyncListInt();

	public static IdeaList instance;

    void Awake()
    {
        //Prevalence = new SyncListInt();
        instance = this;
        for (int i = 0; i < list.Length; i++)
        {
            ideaDict.Add(list[i].name, i);
        }
    }

    public override void OnStartServer()
    {
		for (int i = 0; i < list.Length; i++)
		{
			Prevalence.Add(0);
		}
	}

	/*void setPrevalence(int[] obj)
	{
		Prevalence = obj;
	}*/
	public void updateValue(int index, int amt)
	{
		Prevalence[index] += amt;
	}
		/*
		public static void rollForEvent(AbstractIdea idea)
		{
			instance.StartCoroutine(instance.checkEvent(idea));
		}*/
		/*
		IEnumerator checkEvent(AbstractIdea idea)
		{
			int index = instance.ideaDict[idea.name];
			float ratio = instance.list[index].value / (float) nodeCount;

			while (ratio > instance.list[index].getEventThreshold())
			{
				//Debug.Log(idea.name + ": " + ratio);

				if (UnityEngine.Random.value < 0.08)
				{
					idea.triggerEvent();
					break;
				}

				yield return new WaitForSeconds(1);

				ratio = instance.list[index].value / (float) nodeCount;
			}

			idea.stopChecking();
			yield return null;
		}*/
	}

	[Serializable]
public class AbstractIdea : System.Object {
	public string name;
	public string description;
	public int opposite;
	//public float value;
	public Color color;
	//public Event[] events;
	//int eventIndex = 0;
	//bool checkingForEvents = false;
	public string colorHex()
	{
		return ColorToHex(color);
	}
	[Serializable]
    public struct Event
    {
        public string name;
        public string description;
        public float threshold;
    }
	/*
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
	*/
    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
}//