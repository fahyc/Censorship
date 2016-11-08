using UnityEngine;
using System.Collections;

public class CommandCard : MonoBehaviour {
    public GameObject[] commands;
    public GameObject[] initialCommands;
	// Use this for initialization
	void Start () {
        
        print(initialCommands[0]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetReferences() {
        print("RESET TO : " + initialCommands[0]);
        commands = initialCommands;
    }
}
