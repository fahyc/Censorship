using UnityEngine;
using System.Collections;
using ExtensionMethods;
using UnityEngine.UI;


public class Tooltip : MonoBehaviour {
	Transform t;
	Image img;
	Text text;

	public Vector2 offset;
	// Use this for initialization


	// Update is called once per frame
	void Start() {
		t = transform;
		text = GetComponentInChildren<Text>();
		img = GetComponent<Image>();
		img.enabled = false;
		text.enabled = false;
	}

	void Update()
	{

		t.position = Input.mousePosition.xy() + offset;
	}

	public void Activate(string input)
	{
		//gameObject.SetActive(true);
		//print("Activating");
		img.enabled = true;
		text.enabled = true;
		text.text = input;
	}

	public void Deactivate()
	{
		//print("Deactivating");
		img.enabled = false;
		text.enabled = false;
	}

}
