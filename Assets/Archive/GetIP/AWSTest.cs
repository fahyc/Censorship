using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System;

public class AWSTest : MonoBehaviour {
	public string value;
	public string url;
	public string upload;





	//public string key;
	// Use this for initialization
	void Start () {
		//WWW request = new WWW(url);
		//WWWForm form = new WWWForm();
		//form.AddField("key", key);
		StartCoroutine(Post());

	}
	IEnumerator Post()
	{
		//WWWForm form = new WWWForm();
		//form.AddField("true",upload);
		//form.AddField("test", upload);
		string sendData = "\"test:asdf\"";
		//form.
		WWW w = new WWW(url, Encoding.ASCII.GetBytes(sendData));
		/*foreach(string s in form.headers.Keys)
		{
			print(s + "::" + form.headers[s]);
		}*/
		yield return w;
		if (!string.IsNullOrEmpty(w.error))
		{
			print(w.error);
			print(w.text);
			print(w.data);
			
		}
		else
		{
			print(w.text);
		}
		
	}

}
