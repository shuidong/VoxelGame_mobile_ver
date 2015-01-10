using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WWWtest : MonoBehaviour {


	private string url = "192.168.123.124/php/phptest00.php";
	private byte[] arr = new byte[5];

	void Start()
	{
		for(int i=0; i < 5;i++)
		{
			arr[i]=(byte)i;
		}
	}

	IEnumerator StartQueryWWW() // 간단히 www 클래스 이용 웹페잊 ㅣ접속.
	{
		WWW www = new WWW(url);
		yield return www;
		Debug.Log("Query is OK. ");
	}

	IEnumerator GetData() { //웹페이지 텍스트 긁어오는 예제.
		gameObject.guiText.text = "Loading...";
		WWW www = new WWW("192.168.123.124/php/phptest00.php"); //GET data is sent via the URL
		
		while(!www.isDone && string.IsNullOrEmpty(www.error)) {
			gameObject.guiText.text = "Loading... " + www.progress.ToString("0%"); //Show progress
			yield return null;
		}
		
		if(string.IsNullOrEmpty(www.error)) gameObject.guiText.text = www.text;
		else Debug.LogWarning(www.error);
	}

	IEnumerator UpPostData() // POST 방식으로 웹페이지에 데이터전송.
	{

		WWWForm wwwForm = new WWWForm();

		for(int i=0; i< 5; i++)
		{
			wwwForm.AddField(""+i,(int)arr[i]);
		}

		WWW www=new WWW("192.168.123.124/php/phpGetPOST.php",wwwForm);

		yield return www;

		Debug.Log ("Work Done..");
		Debug.Log (www.error);


		// For call Webpage by wwwform //
		/*
		Dictionary<string,string> dic = new Dictionary<string, string>();
		
		dic.Add ( "user_srl", "1" );
		
		dic.Add ( "item_id", "10001");
		
		WWWForm form = new WWWForm();
		
		foreach(KeyValuePair<string,string> post_arg in dic)
			
		{
			
			form.AddField(post_arg.Key, post_arg.Value);
			
		}
		
		WWW www = new WWW("192.168.123.124/php/phpGetPOST.php", form );

		yield return www;*/
	}



	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F10))
		{
			StartCoroutine(UpPostData());
		}
	}
}
