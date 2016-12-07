using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnCircleManager : MonoBehaviour {

	static List<SpawnCircleManager> circles = new List<SpawnCircleManager>();
	static int activeIndex = 0;

	public static Color Green;
	public static Color Red;

	public Color green;
	public Color red;

	public SpawnCircleManager circleTemplate;

	static SpawnCircleManager template;

	

	public static void Clear()
	{
		for(int i = 0; i < circles.Count; i++)
		{
			circles[i].gameObject.SetActive(false);
		}
		activeIndex = 0;
	}

	public static void Spawn(float radius, Vector2 position, Color color)
	{

		SpawnCircleManager circle;
		if (circles.Count > activeIndex)
		{
			circle = circles[activeIndex];
			circle.gameObject.SetActive(true);
		}
		else
		{
			circle = Instantiate(template);
			circles.Add(circle);
		}
		activeIndex++;
		circle.GetComponent<MeshRenderer>().materials[0].SetColor("_Emission", color);
		circle.transform.position = position;
		circle.transform.localScale = new Vector3(radius, .01f, radius);
	}

	
	// Use this for initialization
	void Start () {
		circles.Add(this);
		template = circleTemplate;
		gameObject.SetActive(false);
		Red = red;
		Green = green;
	}
	void OnDestroy()
	{
		circles.Clear();
		activeIndex = 0;
	}
}
