using UnityEngine;
using System.Collections;

public class ManualListener : MonoBehaviour,IPListener {
	public AWSTest aws;

	[System.Serializable]
	public class SendData{
		public string ip;
	}
	public void getData(string str)
	{
		print(str);
	}
	public void request()
	{
		aws.RequestUpdate(this);
	}
	public void send()
	{
		SendData t = new SendData();
		t.ip = Network.player.ipAddress;
		aws.SendUpdate(JsonUtility.ToJson(t));
	}
}
