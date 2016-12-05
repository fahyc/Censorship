using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Office : Spawnable {
	public OfficeSlot slot;

	[SyncVar]
	float defenses;

	public ProgressBar bar;

	public Vector2 barOffset;

	public float captureSpeed = .2f;

    public AudioSource soundObj, soundObj2;
    bool built = false;

	[SyncVar]
	NetworkInstanceId slotId;
	

	// Use this for initialization
	void Start () {
        soundObj = GameObject.FindGameObjectWithTag("officeLostSound").GetComponent<AudioSource>();
        soundObj2 = GameObject.FindGameObjectWithTag("officeEstablishedSound").GetComponent<AudioSource>();
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
		if (isServer)
		{
			if (slot.mainIdea != index)
			{
				defenses -= captureSpeed * Time.deltaTime;
				if (defenses <= 0)
				{
                    soundObj.Play();
                    NetworkServer.Destroy(gameObject);
				}
			}
			else
			{
				defenses += captureSpeed * Time.deltaTime;
				defenses = Mathf.Clamp01(defenses);
			}
			//return;

			if (slotId != NetworkInstanceId.Invalid)
			{
				slotId = slot.GetComponent<NetworkIdentity>().netId;
			}
		}
        if(isClient) {
            if(hasAuthority)
            {
                if (defenses <= 0)
                {
                    soundObj.Play();
                }
                if (defenses >= 1 && !built)
                {
                    soundObj2.Play();
                    built = true;
                }
            }
        }
		bar.SetFill(defenses);
	}

	[ServerCallback]
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (slot)
		{
			slot.RpcSetVisible(true);
		}
		//NetworkServer.FindLocalObject(slotId).GetComponent<OfficeSlot>().setVisible(true);
	}
}
