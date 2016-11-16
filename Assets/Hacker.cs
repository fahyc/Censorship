using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

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
    public LineRenderer line;

    LineRenderer laser;

    void Start() {
        enemiesNear = new GameObject[50];
    }
    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        if (!isLocalPlayer) {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, attackRadius, 1 << LayerMask.NameToLayer("Targetable"));

            if (collisions != null) {
                num_inRange = 0;
                foreach (Collider2D col in collisions) {
                    Spawnable m = col.gameObject.GetComponent<Spawnable>();
                    if (m != null) {
                        //print(m.owner == this.owner);
                        if(m.owner != this.owner && m.GetComponent<NetworkIdentity>().observers.Contains(this.owner)) {
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

    [Server]
    void fireShot() {
        //Pick a random enemy and shoot at it.
        int targetIndex = Mathf.FloorToInt(Random.Range(0, num_inRange));
        GameObject target = enemiesNear[targetIndex];

        laser = GameObject.Instantiate<LineRenderer>(line);
        GetComponent<VisibilityCheck>().AddConnection(laser.gameObject);
        laser.SetColors(Color.green, Color.green);
        laser.GetComponent<NetworkLineRenderer>().setPoints(transform.position, target.transform.position);
        laser.GetComponent<NetworkLineRenderer>().setColor(Color.green);


        StartCoroutine(laser.GetComponent<NetworkLineRenderer>().networkLaser());
        ableToFire = false;
        timeSinceLastShot = Time.time;

        NetworkServer.Destroy(target);
        //Destroy(laser, 1.0f);
        //Destroy(laser);

        //yield return new /*WaitForSeconds*/(1.0f);
    }

	protected override void OnDestroy()
    {
		base.OnDestroy();
        if (laser != null)
            NetworkServer.Destroy(laser.gameObject);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
