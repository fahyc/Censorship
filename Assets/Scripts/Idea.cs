using UnityEngine;
using System.Collections;

public class Idea : MonoBehaviour {
	public string ideaStr;
	public Vector3 origin;
	public Node originObj;
	public Vector3 destination;
	float time = 0;
	public float speed;
	public float totalTime;
	Transform t;

	public Node dest;

	public float minTimeToTarget = .1f; //how long should the minimum lifetime of an idea be? Used to fix a memory leak. 
	/*
    string[] ideas = new string[] {
        "Feminism",
        "Mens Rights Movement",
        "Censorship/Privacy violation is a problem",
        "Censorship/Privacy violation is necessary",
        "Conservative",
        "Liberal",
        "Violent Extremism",
        "Pacifism",
        "Xenophobia",
        "Globalism"
    };
	*/
    // Use this for initialization
    void Start () {
		totalTime = (origin - destination).magnitude/speed;
		t = transform;
	}

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        t.position = Vector3.Lerp(origin, destination, time / totalTime);
		if (Mathf.Abs(time-totalTime) < minTimeToTarget)
		{
			dest.reciveIdea(ideaStr);
			Destroy(gameObject);
		}
    }

	/*void OnTriggerEnter2D(Collider2D col)
	{
		print("Collision");
		Node otherNode = col.GetComponent<Node>();
		print(totalTime);
		if (otherNode && (time/totalTime > .5 || totalTime < .2))
		{
			
			return;
		}
		
	}*/

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) {
            Global.text = ideaStr;
        }
    }

}
