using UnityEngine;
using System.Collections;

public class HideSelf : MonoBehaviour {
    bool hidden = false;
    public bool sliding = false;
    public string direction = "";
    public float distance = 0;
    public float animDuration = 1;
    Vector3 destination;
    float startTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //if (sliding) {
        //    print("sliderbro");
        //    transform.position = Vector3.Lerp(transform.position, destination, (Time.time - startTime)/animDuration);
        //}

    }

    public void Slide() {
        print("what");
        //sliding = true;
        //if(direction == "down") {
        //    destination = new Vector3(transform.position.x, transform.position.y - distance);
        //    startTime = Time.time;
        //}
        //else {

        //}
    }
}
