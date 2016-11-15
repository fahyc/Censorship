using UnityEngine;
using System.Collections;
using ExtensionMethods;
using UnityEngine.Networking;

public class DummyUnit : MonoBehaviour {

    Transform t;

	Global g;

    void Start()
    {
        t = transform;
		g = Global.getLocalPlayer();
    }

    void Update() {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));


		t.position = g.closestSpawnableLoc(position);
		Collider2D[] hits = Physics2D.OverlapPointAll(t.position);
        bool hitLurker = false;
        foreach(Collider2D h in hits)
        {
            if (h.GetComponent<BasicVision>() != null && h.GetComponent<NetworkIdentity>().hasAuthority)
                hitLurker = true;
        }
        SpriteRenderer[] renders = GetComponentsInChildren<SpriteRenderer>();

        if (hitLurker)
        {
            foreach (SpriteRenderer r in renders)
            {
                Color newCol = Color.green;
                newCol.a = r.color.a;
                r.color = newCol;
            }
        }
        else
        {
            foreach (SpriteRenderer r in renders)
            {
                Color newCol = Color.red;
                newCol.a = r.color.a;
                r.color = newCol;
            }
        }
	}
}
