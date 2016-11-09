using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Submenu : UIItem {
	public SpawnScript buttons;
	public string tooltips; //use [idea] in the string to dynamically show the name of the idea this button will spawn
	public Spawnable product;

	public UnityAction onClick;

	public UnityEvent[] AdditionalOnClicks;

	

	// Use this for initialization
	void Start () {
        print("Starting submenu");
		for(int i = 0; i < IdeaList.staticList.Length; i++)
		{
			SpawnScript temp = Instantiate<SpawnScript>(buttons);
			temp.Initiate(tooltips.Replace("[idea]", IdeaList.staticList[i].name), IdeaList.staticList[i].color,product, i);
            temp.transform.SetParent(transform, false);
			//onClick = AdditionalOnClicks[0].;
			if (AdditionalOnClicks.Length > 0)
			{
				temp.GetComponent<Button>().onClick.AddListener(() => { AdditionalOnClicks[0].Invoke(); });
			}
		}
	}


	public override void Enable()
	{
		print("enabling");
		base.Enable();
		gameObject.SetActive(true);
	}
	// Update is called once per frame
	void Update () {
	    
	}
}
