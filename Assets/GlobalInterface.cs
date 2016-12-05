using UnityEngine;
using System.Collections;

public class GlobalInterface : MonoBehaviour {

	Global global = null;

	public void EnableDummy(DummyUnit dummy)
	{
        print(dummy);
        if (global != null)
            global.EnableDummy(dummy);
	}

    public void EnableDummy(DummyUnit dummy, Color c)
    {
        if (global != null)
            global.EnableDummy(dummy, c);
    }
	/*
	public void SetTool(Spawnable obj, int index)
	{
		global.SetT
	}
	*/
	// Update is called once per frame
	void Update () {
		if (!global && Global.isReady())
		{
            global = Global.getLocalPlayer();
		}
	}
}
