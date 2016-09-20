using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Global : MonoBehaviour {

    public Text infoTextBox;
    public static string text;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        infoTextBox.text = text;
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Global.text = "";
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Global.text = "";
        }
    }
}
