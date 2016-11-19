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
	public string sendurl;
	public string geturl;
	public string upload;





	//public string key;
	// Use this for initialization
	void Start () {
		//WWW request = new WWW(url);
		//WWWForm form = new WWWForm();
		//form.AddField("key", key);
		//StartCoroutine(Post(upload));

	}



	public void SendUpdate(string uploadJSON)
	{
		StartCoroutine(Post(sendurl,uploadJSON));
	}

	public void RequestUpdate(IPListener listener)
	{
		StartCoroutine(Post(geturl, upload, listener));
	}

	IEnumerator Post(string url, string send,IPListener callback = null)
	{
		//WWWForm form = new WWWForm();
		//form.AddField("true",upload);
		//form.AddField("test", upload);
		//string sendData = "\"test:asdf\"";
		//form.
		WWW w = new WWW(url, Encoding.ASCII.GetBytes(send));
		/*foreach(string s in form.headers.Keys)
		{
			print(s + "::" + form.headers[s]);
		}*/
		yield return w;
		if (!string.IsNullOrEmpty(w.error))
		{
			print(w.error);
			print(w.text);
		//	print(w.data);
			
		}
		else
		{
			if(callback != null)
			{
				callback.getData(w.text);
			}
			print(w.text);
		}
		
	}

}
