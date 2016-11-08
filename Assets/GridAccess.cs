using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridAccess : MonoBehaviour {
    public string selectedUnit;
    public GameObject[] Grid;
    List<Transform> activeButtons;
    public GameObject emptySlot;
	// Use this for initialization
	void Start () {
        clearButtons(true);

	}
	void clearButtons(bool noneSelected) {
        foreach(GameObject g in Grid) {
            Destroy(g);
        }
        if (noneSelected) {
            for (int i = 0; i < Grid.Length; i++) {
                Grid[i] = Instantiate(emptySlot);
                Grid[i].transform.parent = GameObject.FindGameObjectWithTag("CommandCard").transform;

            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
    public void OnSelectUnit(GameObject newUnit) {
        if(newUnit == null) {
            clearButtons(true);
            return;
        }
        CommandCard cc = newUnit.GetComponent<CommandCard>();
        if (cc != null) {
            print(cc.commands.Length);
            clearButtons(false);
            Grid = cc.commands;
            for(int i=0; i < cc.commands.Length; i++) {
                if (cc.commands[i] != null) {
                    print(cc.commands[i]);
                    Grid[i] = Instantiate(cc.commands[i]);
                    Grid[i].transform.parent = GameObject.FindGameObjectWithTag("CommandCard").transform;
                } else {
                    Grid[i] = Instantiate(emptySlot);
                    Grid[i].transform.parent = GameObject.FindGameObjectWithTag("CommandCard").transform;
                }
            }
            cc.ResetReferences();
        } 
    }
}
