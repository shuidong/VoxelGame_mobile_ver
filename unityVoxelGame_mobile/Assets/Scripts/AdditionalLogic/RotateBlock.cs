using UnityEngine;
using System.Collections;

public class RotateBlock : MonoBehaviour {

	private float rotSpeed;

	private Vector3 _vec;
	void Start()
	{
		rotSpeed=50.0f;
	}
	void Update ()
	{
		transform.Rotate(Vector3.right * Time.deltaTime * rotSpeed);
		transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed, Space.Self);

	}
}
