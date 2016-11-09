using UnityEngine;
using System.Collections;

public class CommandCard : MonoBehaviour {
    public GameObject[] commands;
    public GameObject[] initialCommands;
	// Use this for initialization
	void Start () {
        for(int i=0; i < commands.Length; i++)
        {
            if(commands[i] == null)
            {
                //commands[i] = Resources.Load("Prefabs/button_Empty");
            }
        }    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetReferences() {
        print("RESET TO : " + initialCommands[0]);
        commands = initialCommands;
    }
}
