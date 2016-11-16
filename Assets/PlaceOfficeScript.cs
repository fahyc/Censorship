using UnityEngine;
using System.Collections;

public class PlaceOfficeScript : MonoBehaviour {

	public Global g;
	public void click()
	{
		g = Global.getLocalPlayer();
		for (int i = 0; i < g.selected.Count; i++)
		{
			OfficeSlot temp = g.selected[i].GetComponent<OfficeSlot>();
			if (temp)
			{
				print("spawning office");
				temp.spawnOffice();
			}
			else
			{
				print(temp + " is not an office slot ");
			}
		}
	}
}
