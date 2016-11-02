using UnityEngine;
using System.Collections;

public class UIItem : MonoBehaviour {
	public bool blocksClicks = true;
	public bool hideOnClick = true;
	
	void Start()
	{
		Enable();
	}

	public virtual void Disable()
	{
		print("disabling");
		gameObject.SetActive(false);
	}

	public virtual void Enable()
	{
		if (hideOnClick)
		{
			Global.addUIItem(this);
		}
		if (blocksClicks)
		{
			// print(this);
			Global.addFocusTaker(GetComponent<RectTransform>());
		}

	}
}
