﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using ExtensionMethods;

public class Inspect : UIItem {
/*=======
public class Inspect : NetworkBehaviour {
>>>>>>> 6ed8a1d38f5753f2a22eddfb294a2bf5a841f7fb
=======
public class Inspect : MonoBehaviour {
>>>>>>> dc24bfaa1e662120245bf730f32a1e33eda8beb5*/
	public Vector2 offset;

	GameObject inspecting;

	Image img;
	Text text;
	BoxCollider2D col;

	// Use this for initialization
	void Start () {
		img = GetComponentInChildren<Image>();
		text = GetComponentInChildren<Text>();
		col = GetComponentInParent<BoxCollider2D>();
		Disable();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Enable(GameObject target)
	{
		base.Enable();
		inspecting = target;
		img.enabled = true;
		text.enabled = true;
		col.enabled = true;
		transform.position = target.transform.position.xy() + offset;
	}
	
	//[Client]
	public override void Disable()
	{
		img.enabled = false;
		text.enabled = false;
		col.enabled = false;
	}

	public void FireTarget()
	{
        // print("Destroying: " + inspecting);
        //Refund our player's income. Should we return some of the principle investment or not?
        GameObject.FindGameObjectWithTag("Player").GetComponent<Global>().moneyDiff += inspecting.GetComponent<Spawnable>().upkeep;
        inspecting.GetComponent<Inspectable>().DestroySelf();
		Disable();
	}
}
