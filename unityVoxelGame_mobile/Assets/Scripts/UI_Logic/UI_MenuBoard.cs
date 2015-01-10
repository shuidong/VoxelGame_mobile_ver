using UnityEngine;
using System.Collections;

public class UI_MenuBoard : MonoBehaviour {

	// 메뉴 오브젝트 Get.
	public GameObject menuObject;
	// 홀짝 판별 카운팅변수.
	private int count;
	// 현재 메뉴창이 ON 인지 OFF인지, LockMouse.cs 에서 참조할수있는 static 변수.
	public static bool isProcessMenu;
	// Use this for initialization
	void Start () {
		// 초기 진입시 메뉴 및 하위옵션 전부 OFF.
		IsOnMenu(false);
		// 홀/짝 계산 편의로 인해 2로 시작.
		count=2;
		//메뉴는 꺼져있다. (초기값).
		isProcessMenu = false;
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F10))
		{	
			//메뉴창을 보여주는것인지 확인.
			if(count%2==1) //홀수. OFF
			{
				IsOnMenu(false);
				Screen.lockCursor=true;
				count++;
			}
			else if(count%2==0) // 짝수. ON
			{
				IsOnMenu(true);
				Screen.lockCursor=false;
				count++;
			}

		}
	}

	void IsOnMenu(bool _b)
	{
		menuObject.SetActive(_b);
		isProcessMenu = _b;
	}
}
