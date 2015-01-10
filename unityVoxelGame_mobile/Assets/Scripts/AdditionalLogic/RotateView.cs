using UnityEngine;
using System.Collections;

public class RotateView : MonoBehaviour {

	public GameObject _player;
	private float speed;
	// Update is called once per frame
	void Start()
	{
		speed=5.0f;
	}
	void Update () {
	
		_player.transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.World);
		_player.transform.Rotate(Vector3.left * Time.deltaTime * speed, Space.World);
	}
}
