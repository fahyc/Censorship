using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ExtensionMethods;
public class Inspect : MonoBehaviour {
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
		inspecting = target;
		img.enabled = true;
		text.enabled = true;
		col.enabled = true;
		transform.position = target.transform.position.xy() + offset;
	}

	public void Disable()
	{
		img.enabled = false;
		text.enabled = false;
		col.enabled = false;
	}

	public void FireTarget()
	{
		print("Destroying: " + inspecting);
		Destroy(inspecting);
		Disable();
	}
}
