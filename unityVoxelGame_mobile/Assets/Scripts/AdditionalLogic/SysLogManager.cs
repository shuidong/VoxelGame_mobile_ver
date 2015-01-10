using UnityEngine;
using System.Collections;
using System;

public class SysLogManager : MonoBehaviour {

	public GameObject getLable;
	private static UILabel sysLogLable;
	private static ArrayList textArr;
	private static string labelText;
	void Start()
	{
		textArr = new ArrayList();
		labelText = "";
		sysLogLable = getLable.GetComponent("UILabel") as UILabel;

	}

	public static void SetLogMessage(string _str)
	{

		textArr.Add(_str);

		for(int i = textArr.Count; i > 0; i--)
		{
			labelText += textArr[i-1]; 
			labelText += Environment.NewLine; // 개행문자.
		}

		sysLogLable.text = labelText;

		labelText = "";

		if(textArr.Count >= 4)
		{
			textArr.RemoveAt (0);
		}
	}

}
