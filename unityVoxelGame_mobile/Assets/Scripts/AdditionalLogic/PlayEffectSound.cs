using UnityEngine;
using System.Collections;

public class PlayEffectSound : MonoBehaviour {

	public AudioClip pickSound;
	public AudioClip selectSound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetMouseButtonDown(1))
		{
			audio.PlayOneShot(pickSound,1.0f);
		}
		if(   Input.GetKey (KeyCode.Alpha1)
		   || Input.GetKey (KeyCode.Alpha2)
		   || Input.GetKey (KeyCode.Alpha3)
		   || Input.GetKey (KeyCode.Alpha4)
		   || Input.GetKey (KeyCode.Alpha5)
		   || Input.GetKey (KeyCode.Alpha6)
		   || Input.GetKey (KeyCode.Alpha7)
		   || Input.GetKey (KeyCode.Alpha8)
		   || Input.GetKey (KeyCode.Alpha9) )
		{
			audio.PlayOneShot(selectSound,0.2f);
		}


	}
}
