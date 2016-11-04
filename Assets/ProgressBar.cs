using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour {
	Image bar;
	Color color;
	// Use this for initialization
	void Start () {
		foreach(Image img in GetComponentsInChildren<Image>())
		{
			if(img.type == Image.Type.Filled)
			{
				bar = img;
				bar.color = color;
			}
		}

	}
	
	public void SetFill(float amt)
	{
		bar.fillAmount = amt;
	}
	public void SetColor(Color color)
	{
		if (bar)
		{
			bar.color = color;
		}
		else
		{
			this.color = color;
		}
	}

}
