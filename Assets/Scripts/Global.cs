using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Global : MonoBehaviour {

    public Text infoTextBox;
    public static string text;
    public WallScript wall;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        infoTextBox.text = text;
        if(Input.GetMouseButtonDown(0)) {
            WallScript temp = (WallScript)Instantiate(wall, new Vector2(Input.mousePosition.x, Input.mousePosition.y), Quaternion.identity);
            print(Input.mousePosition);
        }
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
