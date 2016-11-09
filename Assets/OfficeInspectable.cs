using UnityEngine;
using System.Collections;

public class NewBehaviourScript : Inspectable {

	public override void DestroySelf()
	{
		GetComponent<OfficeSlot>().spawnOffice(Global.getLocalPlayer().playerIdeaIndex);
	}
}
