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

        if(Input.GetMouseButtonDown(1)) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.Set(mousePos.x, mousePos.y, -Camera.main.transform.position.z);

            Collider2D hitWall = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(mousePos));

            if (hitWall != null && hitWall.gameObject.GetComponent<WallScript>() != null)
            {
                Destroy(hitWall.gameObject);
            } else
            {
                WallScript temp = (WallScript)Instantiate(wall, Camera.main.ScreenToWorldPoint(mousePos), Quaternion.identity);
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.Set(mousePos.x, mousePos.y, -Camera.main.transform.position.z);
            Collider2D[] hits = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(mousePos));

            if(hits.Length == 0)
            {
                Global.text = "";
            }
            
            //Global.text = "";
        }
    }
}
