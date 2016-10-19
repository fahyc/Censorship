using UnityEngine;
using System.Collections;

public class GlobalInterface : MonoBehaviour {

	Global global;
	

	public void EnableDummy(DummyUnit dummy)
	{
		global.EnableDummy(dummy);
	}
	/*
	public void SetTool(Spawnable obj, int index)
	{
		global.SetT
	}
	*/
	// Update is called once per frame
	void Update () {
		if (!global)
		{
			GameObject temp = GameObject.FindGameObjectWithTag("Player");
			if (temp)
			{
				global = temp.GetComponent<Global>();
			}
		}
	}
}
