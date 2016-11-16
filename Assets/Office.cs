using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Office : Spawnable {
	public OfficeSlot slot;

	float defenses;

	public ProgressBar bar;

	public Vector2 barOffset;

	public float captureSpeed = .2f;

	[SyncVar]
	NetworkInstanceId slotId;
	

	// Use this for initialization
	void Start () {
		//slot = GetComponentInParent<OfficeSlot>();
		defenses = 0;
		bar = Instantiate(bar);
		bar.transform.SetParent(transform);
		bar.transform.localPosition = barOffset;
		GetComponent<SpriteRenderer>().color = IdeaList.instance.list[index].color;
		bar.SetColor(IdeaList.instance.list[index].color);
	}

	// Update is called once per frame
	void Update() {
		if (!isLocalPlayer)
		{
			return;
		}
		if (slotId!=NetworkInstanceId.Invalid)
		{
			slotId = slot.GetComponent<NetworkIdentity>().netId;
		}
		if (slot.mainIdea != index)
		{
			defenses -= captureSpeed * Time.deltaTime;
			if(defenses <= 0)
			{
				NetworkServer.Destroy(gameObject);
			}
		}
		else
		{
			defenses += captureSpeed * Time.deltaTime;
			defenses = Mathf.Clamp01(defenses);
		}
		bar.SetFill(defenses);
	}
	[ServerCallback]
	protected virtual void OnDestroy()
	{
		base.OnDestroy();
		slot.RpcSetVisible(true);
		//NetworkServer.FindLocalObject(slotId).GetComponent<OfficeSlot>().setVisible(true);
	}
}
