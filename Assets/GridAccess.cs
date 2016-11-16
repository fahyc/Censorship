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
    public string tooltips = "[idea]";
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

                // g.GetComponent<Button>().GetComponentInChildren<Text>().text = "Empty";
                g.GetComponent<Button>().onClick.RemoveAllListeners();
                assignButton(g, emptySlot);
                
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
    public void clearOutSubMenu() {
        print("Executing now");
        foreach (GameObject s in submenu) {
            if (s != null) {
                Destroy(s);
            }
        }
        inSubMenu = false;
        
    }
    public void OnSelectUnit(GameObject newUnit) {
        if (newUnit == null) {
            clearButtons(true);
            //inSubMenu = false;
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

                    // set the image component of the grid
                    assignButton(Grid[i], cc.commands[i]);

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
                    assignButton(Grid[i], emptySlot);
                }
            }
        }
    }

    void assignButton (GameObject dst, GameObject src)
    {
        // get the image component of the grid
        Image dstImg = null;
        foreach (Image i in dst.GetComponentsInChildren<Image>())
        {
            if (i.transform.parent != null)
            {
                dstImg = i;
            }
        }

        // and of the button we want
        Image srcImg = null;
        foreach (Image i in src.GetComponentsInChildren<Image>())
        {
            if (i.transform.parent != null)
            {
                srcImg = i;
            }
        }

        // set them appropriately
        if (dstImg != null && srcImg != null)
        {
            dstImg.sprite = srcImg.sprite;
            dstImg.color = srcImg.color;
            dstImg.preserveAspect = srcImg.preserveAspect;

            // dstImg.rectTransform.position = srcImg.rectTransform.position;
            dstImg.rectTransform.pivot = srcImg.rectTransform.pivot;
            dstImg.rectTransform.localScale = srcImg.rectTransform.localScale;
        }

        dst.GetComponent<Image>().color = src.GetComponent<Image>().color;
    }

    void SubmenuCreation(DummyUnit inDummy) {
        print("woohoo");
        clearButtons(true);
        for (int i = 0; i < IdeaList.instance.list.Length; i++) {
            GameObject temp = Instantiate(buttons);
            if (inDummy.name == "DummyNode")
                temp.GetComponent<SpawnScript>().Initiate(tooltips.Replace("[idea]", IdeaList.instance.list[i].name), IdeaList.instance.list[i].color, sref, i);
            else if (inDummy.name == "DummyWall")
                temp.GetComponent<SpawnScript>().Initiate(tooltips.Replace("[idea]", IdeaList.instance.list[i].name), IdeaList.instance.list[i].color, wref, i);
            temp.GetComponent<Button>().onClick.AddListener(() => gi.EnableDummy(inDummy));

            submenu[i] = temp;


            temp.transform.SetParent(Grid[i].transform, false);

            temp.GetComponent<RectTransform>().rect.Set(15, 15, 30, 30);

            //onClick = AdditionalOnClicks[0].;
            //if (AdditionalOnClicks.Length > 0) {
            //    temp.GetComponent<Button>().onClick.AddListener(() => { AdditionalOnClicks[0].Invoke(); });
            //}
        }
        print("Set insubmenu to true");
        inSubMenu = true;

        ////Submenu s = Instantiate<Submenu>(sm);
        //for (int z = 0; z < IdeaList.staticList.Length; z++) {
        //    Grid[z].GetComponent<Image>().color = IdeaList.staticList[z].color;
        //    Grid[z].GetComponent<Button>().onClick.AddListener(() => gi.EnableDummy(inDummy));
        //    //print(temp);
        //    if (inDummy.name == "DummyNode") {
        //        Grid[z].GetComponent<SpawnScript>().Initiate("Spawn a shill", IdeaList.staticList[z].color, sref, z);
        //        SpawnScript temp = Grid[z].GetComponent<SpawnScript>();

        //        Button tempb = Grid[z].GetComponent<Button>();
        //        int what = z;
        //        Spawnable t = Instantiate(sref);
        //        tempb.onClick.AddListener(() => Global.setTool(t, what));
        //        //Grid[i].GetComponent<Button>().onClick.AddListener(() => temp.Spawn());
        //        //print(sref);
        //    } else if (inDummy.name == "DummyWall") {
        //        int index = z;
        //        Grid[z].GetComponent<SpawnScript>().Initiate("Spawn a wall", IdeaList.staticList[z].color, wref, index);
        //    }
        //    //Grid[i].
        //}
        //for(int j=0; j < IdeaList.staticList.Length; j++) {
        //    SpawnScript d = temp[j];
        //    Grid[j].GetComponent<Button>().onClick.AddListener(() => d.Spawn());
        //}
    }
}
