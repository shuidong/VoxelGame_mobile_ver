using UnityEngine;
using System.Collections;

public class UnityChanMoving : MonoBehaviour {

	private Animator animator;
	void Start()
	{

		animator = gameObject.GetComponent<Animator>();

		animator.Play("WAIT01");
	}
}
