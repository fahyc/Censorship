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
            Vector3 mousePos = Input.mousePosition;
            mousePos.Set(mousePos.x, mousePos.y, -Camera.main.transform.position.z);
            WallScript temp = (WallScript)Instantiate(wall, Camera.main.ScreenToWorldPoint(mousePos), Quaternion.identity);
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
