using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialColliderScript : MonoBehaviour {

    public TutorialScript tut;
    public Sprite investigatorSprite;
    public Sprite hackerSprite;
    public Sprite botnetSprite;
    public Sprite firewallSprite;

	// Use this for initialization
	void Start () {
        this.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        
        switch(tut.promptNum)
        {
            case 1:
                List<Inspectable> lst = Global.getLocalPlayer().selected;
                if(lst.Count > 0)
                {
                    for(int i = 0; i < lst.Count; i++)
                    {
                        if(lst[i].gameObject.GetComponent<OfficeSlot>() != null)
                        {
                            tut.promptNum++;
                            tut.openPrompt();
                        }
                    }
                } 
                break;
            case 2:
                if (other.gameObject.GetComponent<Office>() != null)
                {
                    tut.promptNum++;
                    tut.openPrompt();
                }
                break;
            case 3:
                if (other.gameObject.tag == "Lurker")
                {
                    tut.promptNum++;
                    tut.openPrompt();
                    this.GetComponent<SpriteRenderer>().enabled = true;
                    this.GetComponent<RectTransform>().position = new Vector3(.5f, -.3f, 0);
                }
                break;
            case 4:
                if (other.gameObject.GetComponent<Shill>() != null)
                {
                    tut.promptNum++;
                    tut.openPrompt();
                    this.GetComponent<RectTransform>().position = new Vector3(.49f, 1.59f, 0);
                }
                break;
            case 5:
                if (other.gameObject.GetComponent<blockScript>() != null)
                {
                    tut.promptNum++;
                    tut.openPrompt();
                    this.GetComponent<RectTransform>().position = new Vector3(-2f, 0.5f, 0);
                }
                break;
            case 6:
                if (other.gameObject.GetComponent<SpriteRenderer>().sprite == investigatorSprite)
                {
                    tut.promptNum++;
                    tut.openPrompt();
                    this.GetComponent<RectTransform>().position = new Vector3(-.29f, 0.29f, 0);
                }
                break;
            case 7:
                if (other.gameObject.GetComponent<SpriteRenderer>().sprite == hackerSprite)
                {
                    tut.promptNum++;
                    tut.openPrompt();
                    this.GetComponent<RectTransform>().position = new Vector3(-1.19f, 0.39f, 0);
                }
                break;
            case 8:
                if (other.gameObject.GetComponent<SpriteRenderer>().sprite == botnetSprite)
                {
                    tut.promptNum++;
                    tut.openPrompt();
                    this.GetComponent<RectTransform>().position = new Vector3(-1.19f, 3.06f, 0);
                }
                break;
            case 9:
                if (other.gameObject.GetComponent<SpriteRenderer>().sprite == firewallSprite)
                {
                    tut.promptNum++;
                    tut.openPrompt();
                    this.GetComponent<SpriteRenderer>().enabled = false;
                    this.GetComponent<Collider2D>().enabled = false;
                }
                break;
        }
        
        
    }
}
