using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridAccess : MonoBehaviour {
    public string selectedUnit;
    public GameObject[] Grid;
    List<Transform> activeButtons;
    public GameObject emptySlot;
    public GameObject subMenuPfab;
    GameObject submenu;
	Global gi;
	// Use this for initialization
	void Start () {
        clearButtons(true);
        submenu = Instantiate(subMenuPfab);
        submenu.transform.SetParent(transform);
        //submenu.GetComponent<Submenu>().Disable();
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
					if (!gi)
					{
						gi = Global.getLocalPlayer();
					}
                    Grid[i].GetComponent<Button>().onClick = cc.commands[i].GetComponent<Button>().onClick;
                    Grid[i].GetComponent<Button>().GetComponentInChildren<Text>().text = cc.commands[i].GetComponentInChildren<Text>().text;
                    if (cc.commands[i].name == "button_Shill" || cc.commands[i].name == "button_Wall")
                    {
                        //Put behavior for submenus here.
                        //Grid[i].GetComponentInChildren<Submenu>().Enable();
                        Grid[i].GetComponent<Button>().onClick.AddListener(() => SubmenuCreation());
                    }
                    else if (cc.commands[i].name == "button_Lurker" || cc.commands[i].name == "button_Investigator" || cc.commands[i].name == "button_Hacker")
                    {
                        DummyUnit temp = cc.commands[i].GetComponent<LinkedDummy>().dummy.GetComponent<DummyUnit>();

                        Grid[i].GetComponent<Button>().onClick.AddListener(() => gi.EnableDummy(temp));
                    }
                } else
                {
                    Grid[i].GetComponent<Button>().onClick = emptySlot.GetComponent<Button>().onClick;
                    Grid[i].GetComponent<Button>().GetComponentInChildren<Text>().text = "Empty";
                }
            }
        } 
    }
    void SubmenuCreation() {
        print("woohoo");
    }
}
