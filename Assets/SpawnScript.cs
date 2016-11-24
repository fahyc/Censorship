using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ExtensionMethods;

public class SpawnScript : MonoBehaviour {
	public Spawnable product;
	public string mouseOver;
	public int index;
	Image img;

	//public EventTrigger trigger; 
	//public Tooltip tooltip;

	public Tooltip tooltip;

	public void Initiate(string tooltip, Color color,Spawnable p, int i)
	{

		product = p;
        img.color = color;
        mouseOver = tooltip;
		index = i;

	}

	// Use this for initialization
	void Awake () {
		img = GetComponent<Image>();
	}
	void Start()
	{
		//		print(GameObject.FindGameObjectWithTag("Tooltip"));
		tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
		print("Getting tooltip " + tooltip + "|");
	}
	
	public void Spawn()
	{

		Global.setTool(product, index);
		//print("spawning");
		//Spawnable temp = Instantiate<Spawnable>(product);
		//temp.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition).xy();
		//temp.index = index;
	}
    public void Spawn(int in_index) {
        print(in_index);
        Global.setTool(product, in_index);
    }
	public void ToolTip()
	{
		print("activating tooltip with " + mouseOver + " tooltip " + tooltip + "|");
		tooltip.Activate(mouseOver);
	}
	public void ToolTipOff()
	{
		tooltip.Deactivate();
	}
}
