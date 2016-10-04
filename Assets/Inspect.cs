using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using ExtensionMethods;
<<<<<<< HEAD
public class Inspect : UIItem {
=======
public class Inspect : NetworkBehaviour {
>>>>>>> 6ed8a1d38f5753f2a22eddfb294a2bf5a841f7fb
	public Vector2 offset;

	GameObject inspecting;

	Image img;
	Text text;
	BoxCollider2D col;

	// Use this for initialization
    [ClientCallback]
	void Start () {
		img = GetComponentInChildren<Image>();
		text = GetComponentInChildren<Text>();
		col = GetComponentInParent<BoxCollider2D>();
		Disable();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [Client]
	public void Enable(GameObject target)
	{
		base.Enable();
		inspecting = target;
		img.enabled = true;
		text.enabled = true;
		col.enabled = true;
		transform.position = target.transform.position.xy() + offset;
	}

<<<<<<< HEAD
	public override void Disable()
=======
    [Client]
	public void Disable()
>>>>>>> 6ed8a1d38f5753f2a22eddfb294a2bf5a841f7fb
	{
		img.enabled = false;
		text.enabled = false;
		col.enabled = false;
	}

    [Client]
	public void FireTarget()
	{
		print("Destroying: " + inspecting);
        inspecting.GetComponent<Inspectable>().DestroySelf();
		Disable();
	}
}
