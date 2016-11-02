﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NodeInfluencer : NetworkBehaviour {

    float influence = 10.0f;

    [Server]
    public override void OnStartServer()
    {

    }
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.down * 1.0f * Time.deltaTime;
	}

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other)
    {
        Node n = other.GetComponent<Node>();
        if(n != null)
        {
            n.echoChamberCoefficient += influence;
        }
    }

    [ServerCallback]
    void OnTriggerExit2D(Collider2D other)
    {
        Node n = other.GetComponent<Node>();
        if (n != null)
        {
            n.echoChamberCoefficient -= influence;
        }
    }
}
