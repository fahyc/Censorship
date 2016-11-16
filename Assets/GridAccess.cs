using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class GridAccess : UIItem {
    public string selectedUnit;
    public GameObject[] Grid;
    public GameObject[] submenu;
    List<Transform> activeButtons;
    public GameObject emptySlot;
    public GameObject subMenuPfab;
    public Shill sref;
    public blockScript wref;
    public GameObject buttons;
    public UnityEvent[] AdditionalOnClicks;
    public bool inSubMenu = false;
    Global gi;
    // Use this for initialization
    void Start() {
        clearButtons(true);
        submenu = new GameObject[12];
        //submenu = Instantiate(subMenuPfab);
        //submenu.transform.SetParent(transform);
        //submenu.GetComponent<Submenu>().Disable();
    }
    void clearButtons(bool noneSelected) {
        foreach (GameObject g in Grid) {
            if (g.GetComponent<Button>() != null) {
                //g.GetComponent<Button>().onClick.RemoveAllListeners();
                g.GetComponent<Button>().onClick = emptySlot.GetComponent<Button>().onClick;
                g.GetComponent<Button>().GetComponentInChildren<Text>().text = "Empty";
                
            }
        }
    }
    public void toggleButtons(bool enabled) {
        foreach(GameObject g in Grid) {
            g.SetActive(enabled);
        }
    }
    // Update is called once per frame
    void Update() {
        
    }

    public void OnSelectUnit(GameObject newUnit) {
        if (newUnit == null) {
            clearButtons(true);
            
            return;
        } 
        CommandCard cc = newUnit.GetComponent<CommandCard>();

        if (cc != null) {
            clearButtons(false);
            for (int i = 0; i < Grid.Length; i++) {
                //print(Grid[i].GetComponent<Button>().onClick);
                if (cc.commands[i] != null) {
                    if (!gi) {
                        gi = Global.getLocalPlayer();
                    }
                    Grid[i].GetComponent<Button>().onClick = cc.commands[i].GetComponent<Button>().onClick;
                    Grid[i].GetComponent<Button>().GetComponentInChildren<Text>().text = cc.commands[i].GetComponentInChildren<Text>().text;
                    if (cc.commands[i].name == "button_Shill" || cc.commands[i].name == "button_Wall") {
                        //Put behavior for submenus here.
                        //Grid[i].GetComponentInChildren<Submenu>().Enable();
                        DummyUnit temp1 = cc.commands[i].GetComponent<LinkedDummy>().dummy.GetComponent<DummyUnit>();
                        //Grid[i].AddComponent<SpawnScript>();
                        Grid[i].GetComponent<Button>().onClick.AddListener(() => SubmenuCreation(temp1));
                    }
                    else if (cc.commands[i].name == "button_Lurker" || cc.commands[i].name == "button_Investigator" || cc.commands[i].name == "button_Hacker") {
                        DummyUnit temp = cc.commands[i].GetComponent<LinkedDummy>().dummy.GetComponent<DummyUnit>();

                        Grid[i].GetComponent<Button>().onClick.AddListener(() => gi.EnableDummy(temp));
                    }
                }
                else {
                    Grid[i].GetComponent<Button>().onClick = emptySlot.GetComponent<Button>().onClick;
                    Grid[i].GetComponent<Button>().GetComponentInChildren<Text>().text = "Empty";
                }
            }
        }
    }
    void SubmenuCreation(DummyUnit inDummy) {
        inSubMenu = true;
        print("woohoo");
        clearButtons(true);
        for (int i = 0; i < IdeaList.instance.list.Length; i++) {
            GameObject temp = Instantiate(buttons);
            if (inDummy.name == "DummyNode")
                temp.GetComponent<SpawnScript>().Initiate("blah blah", IdeaList.instance.list[i].color, sref, i);
            else if (inDummy.name == "DummyWall")
                temp.GetComponent<SpawnScript>().Initiate("blah blah", IdeaList.instance.list[i].color, wref, i);
            temp.GetComponent<Button>().onClick.AddListener(() => gi.EnableDummy(inDummy));
            submenu[i] = temp;
            temp.transform.SetParent(Grid[i].transform, false);
            //onClick = AdditionalOnClicks[0].;
            //if (AdditionalOnClicks.Length > 0) {
            //    temp.GetComponent<Button>().onClick.AddListener(() => { AdditionalOnClicks[0].Invoke(); });
            //}
        }

        ////Submenu s = Instantiate<Submenu>(sm);
        //for (int z = 0; z < IdeaList.instance.list.Length; z++) {
        //    Grid[z].GetComponent<Image>().color = IdeaList.instance.list[z].color;
        //    Grid[z].GetComponent<Button>().onClick.AddListener(() => gi.EnableDummy(inDummy));
        //    //print(temp);
        //    if (inDummy.name == "DummyNode") {
        //        Grid[z].GetComponent<SpawnScript>().Initiate("Spawn a shill", IdeaList.instance.list[z].color, sref, z);
        //        SpawnScript temp = Grid[z].GetComponent<SpawnScript>();

        //        Button tempb = Grid[z].GetComponent<Button>();
        //        int what = z;
        //        Spawnable t = Instantiate(sref);
        //        tempb.onClick.AddListener(() => Global.setTool(t, what));
        //        //Grid[i].GetComponent<Button>().onClick.AddListener(() => temp.Spawn());
        //        //print(sref);
        //    } else if (inDummy.name == "DummyWall") {
        //        int index = z;
        //        Grid[z].GetComponent<SpawnScript>().Initiate("Spawn a wall", IdeaList.instance.list[z].color, wref, index);
        //    }
        //    //Grid[i].
        //}
        //for(int j=0; j < IdeaList.instance.list.Length; j++) {
        //    SpawnScript d = temp[j];
        //    Grid[j].GetComponent<Button>().onClick.AddListener(() => d.Spawn());
        //}
    }
}
