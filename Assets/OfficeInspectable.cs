using UnityEngine;
using System.Collections;

public class NewBehaviourScript : Inspectable {

	public override void DestroySelf()
	{
		GetComponent<OfficeSlot>().spawnOffice(GameObject.FindGameObjectWithTag("Player").GetComponent<Global>().playerIdeaIndex);
	}
}
