using UnityEngine;
using System.Collections;

public class Hacker : Spawnable {
    public float attackRadius;
    public float attackInterval;
    //Let's... just one shot kill for now? I guess?
    public float damage;
    public LayerMask targetable;
    GameObject[] enemiesNear;
    int num_inRange;
    int oldLength;
    bool ableToFire=true;
    float timeSinceLastShot;
    void Start() {
        enemiesNear = new GameObject[50];
    }
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, attackRadius, 1 << LayerMask.NameToLayer("Targetable"));

            if (collisions != null && collisions.Length != oldLength) {
                num_inRange = 0;
                foreach (Collider2D col in collisions) {
                    Spawnable m = col.gameObject.GetComponent<Spawnable>();
                    if (m != null) {
                        //print(m.owner == this.owner);
                        if(m.owner != this.owner) {
                            enemiesNear[num_inRange] = col.gameObject;
                            num_inRange++;
                        }
                        //print(m.owner);
                    }
                }

                //print(enemiesNear.Length);
            }

            oldLength = collisions.Length;
            //print(num_inRange);
            if (num_inRange > 0 && ableToFire) {
                //fireShot();
                //print("Should fire laser");
                fireShot();
            } else if (!ableToFire) {
                if(Time.time-timeSinceLastShot >= attackInterval) {
                    ableToFire = true;
                }
            }
        }

    }

    void fireShot() {
        print("imma firin mah laser");
        //Pick a random enemy and shoot at it.
        int targetIndex = Mathf.FloorToInt(Random.Range(0, num_inRange));
        GameObject target = enemiesNear[targetIndex];
        LineRenderer laser = gameObject.AddComponent<LineRenderer>();
        laser.SetWidth(0.025f, 0.025f);
        laser.SetColors(Color.green, Color.green);
        laser.SetVertexCount(2);
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, target.transform.position);
        ableToFire = false;
        timeSinceLastShot = Time.time;
        Destroy(target);
        Destroy(laser, 1.0f);
        //Destroy(laser);

        //yield return new /*WaitForSeconds*/(1.0f);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
