using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderBehavior : MonoBehaviour {
    public int associatedScore;
    public Image fill;
    public AbstractIdea ideaTracker;
    Slider tracker;
    public float sliderMax=0.25f;
	// Use this for initialization
	public IEnumerator Start () {

        tracker = GetComponentInParent<Slider>();
        Text txtRef = GetComponentInChildren<Text>();

        yield return new WaitUntil (() => IdeaList.isReady());

        ideaTracker = IdeaList.instance.list[associatedScore];
        tracker.maxValue = sliderMax;
        fill.color = ideaTracker.color;
        txtRef.text = ideaTracker.name;
        txtRef.alignment = TextAnchor.MiddleCenter;
        txtRef.color = Color.white;
        txtRef.fontSize = 9;
	}
	
	// Update is called once per frame
	void Update () {
        //print("running " + IdeaList.nodeCount);
        // Debug.LogFormat("IdeaList instance is {0}", IdeaList.instance);
        if (IdeaList.isReady())
        {
            if (IdeaList.instance.nodeCount > 0)
            {
                tracker.value = (float)IdeaList.instance.Prevalence[associatedScore] / IdeaList.instance.nodeCount;
                fill.fillAmount = tracker.value;
                //print((float)IdeaList.instance.Prevalence[associatedScore] / IdeaList.instance.nodeCount);
            }
        }
	}
}
