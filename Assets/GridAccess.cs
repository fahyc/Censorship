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
            if (g.GetComponent<Button>() != null)
            {
                g.GetComponent<Button>().onClick = emptySlot.GetComponent<Button>().onClick;
                g.GetComponent<Button>().GetComponentInChildren<Text>().text = "Empty";
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
            clearButtons(false);
            for (int i = 0; i < Grid.Length; i++)
            {
                //print(Grid[i].GetComponent<Button>().onClick);
                if (cc.commands[i] != null)
                {
                    Grid[i].GetComponent<Button>().onClick = cc.commands[i].GetComponent<Button>().onClick;
                    Grid[i].GetComponent<Button>().GetComponentInChildren<Text>().text = cc.commands[i].GetComponentInChildren<Text>().text;

                } else
                {
                    Grid[i].GetComponent<Button>().onClick = emptySlot.GetComponent<Button>().onClick;
                    Grid[i].GetComponent<Button>().GetComponentInChildren<Text>().text = "Empty";
                }
            }
        } 
    }
}
