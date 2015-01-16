using UnityEngine;
using System.Collections;

public class FPSCalculator : MonoBehaviour {
	[SerializeField]
	private UILabel lblFps;
	private float accum   = 0f; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	public  float frequency = 0.5F; // The update frequency of the fps
	public int nbDecimal = 1; // How many decimal do you want to display

	private string sFPS = ""; // The fps formatted into a string.
	
	void Start()
	{
		StartCoroutine( FPS() );
	}
	
	void Update()
	{
		accum += Time.timeScale/ Time.deltaTime;
		++frames;

		lblFps.text = "[ffff00] FPS:"+sFPS+"[ffff00]";
	}
	
	IEnumerator FPS()
	{
		// Infinite loop executed every "frenquency" secondes.
		while( true )
		{
			// Update the FPS
			float fps = accum/frames;
			sFPS = fps.ToString( "f" + Mathf.Clamp( nbDecimal, 0, 10 ));
			
			accum = 0.0F;
			frames = 0;
			
			yield return new WaitForSeconds( frequency );
		}
	}
}
