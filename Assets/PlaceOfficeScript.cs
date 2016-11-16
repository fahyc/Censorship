using UnityEngine;
using System.Collections;

public class PlaceOfficeScript : MonoBehaviour {

	public Global g;
	void Start()
	{
		g = Global.getLocalPlayer();
	}
	void click()
	{
		for(int i = 0; i < g.selected.Count; i++)
		{
			OfficeSlot temp = g.selected[i].GetComponent<OfficeSlot>();
			if (temp)
			{
				temp.spawnOffice();
			}
		}
	}
}
