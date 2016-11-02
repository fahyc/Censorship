using UnityEngine;
using System.Collections;
using ExtensionMethods;
using UnityEngine.Networking;

public class DummyUnit : MonoBehaviour {

    Transform t;

    void Start()
    {
        t = transform;
    }

    void Update() {
        t.position = Camera.main.ScreenToWorldPoint(Input.mousePosition.append(Camera.main.transform.position.z * -1));

        Collider2D[] hits = Physics2D.OverlapPointAll(t.position);
        bool hitLurker = false;
        foreach(Collider2D h in hits)
        {
            if (h.GetComponent<Lurker>() != null && h.GetComponent<NetworkIdentity>().hasAuthority)
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
