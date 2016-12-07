using UnityEngine;
using System.Collections;

public class GroupFire : MonoBehaviour {

	public Global g;
	public void click()
	{
		g = Global.getLocalPlayer();
		for (int i = 0; i < g.selected.Count; i++)
		{
			Inspectable inspecting = g.selected[i].GetComponent<Inspectable>();
			if (!inspecting || inspecting.firable == false)
			{
				continue;
			}
			//Global.getLocalPlayer().moneyDiff += inspecting.GetComponent<Spawnable>().upkeep;
			inspecting.DestroySelf();
		}
	}
}
