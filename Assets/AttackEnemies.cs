using UnityEngine;
using System.Collections;

public class AttackEnemies : MonoBehaviour {
    public float attackRadius;
    public float attackInterval;
    //Let's... just one shot kill for now? I guess?
    public float damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Physics.CheckSphere(transform.position, attackRadius))
        {
            print("Enemy detected. Proceed to fire");
        }


	}
}
