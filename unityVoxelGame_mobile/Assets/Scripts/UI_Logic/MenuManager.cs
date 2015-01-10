using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	private GameObject tmpObject;
	private SaveAndLoad saveManager;
	private UnityThreading.ActionThread saveThread;

	public void SaveGame()
	{
		tmpObject = GameObject.Find ("SaveAndLoadManager");
		
		if(tmpObject != null )
		{
			saveManager = tmpObject.GetComponent("SaveAndLoad") as SaveAndLoad;
			saveThread = UnityThreadHelper.CreateThread((System.Action)saveManager.SaveGame);
		}
	}

	private GameObject getManager;
	private SaveAndLoad loadManager;
	private UnityThreading.ActionThread loadThread;

	public void LoadGame()
	{
		getManager = GameObject.Find ("SaveAndLoadManager");
		
		if(getManager != null )
		{
			loadManager = getManager.GetComponent("SaveAndLoad") as SaveAndLoad;
			loadThread = UnityThreadHelper.CreateThread((System.Action)loadManager.LoadGame);
		}
	}

	public void NewGame()
	{
		Application.LoadLevel("LoadingNextScene");
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void Settings()
	{
		//nothing.
	}
}
