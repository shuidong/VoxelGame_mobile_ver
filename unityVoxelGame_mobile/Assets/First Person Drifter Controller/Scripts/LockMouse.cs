// by @torahhorse

using UnityEngine;
using System.Collections;

public class LockMouse : MonoBehaviour
{	
	void Start()
	{
		LockCursor(false);
	}

    void Update()
    {
		// UI 메뉴창이 OFF 일때만, MouseCursor를 On/Off 할 수 있다.
		if(UI_MenuBoard.isProcessMenu == false )
		{
			// lock when mouse is clicked
			if( Input.GetMouseButtonDown(0) && Time.timeScale > 0.0f )
			{
				LockCursor(true);
			}
			
			// unlock when escape is hit
			if  ( Input.GetKeyDown(KeyCode.Escape) )
			{
				LockCursor(!Screen.lockCursor);
			}
		}
    	
    }
    
    public void LockCursor(bool lockCursor)
    {
    	Screen.lockCursor = lockCursor;
    }
}