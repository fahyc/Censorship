using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class DummyUnit : MonoBehaviour {

    Transform t;

    void Start()
    {
        t = transform;
    }

	void Update () {
		t.position = Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));
	}
}
