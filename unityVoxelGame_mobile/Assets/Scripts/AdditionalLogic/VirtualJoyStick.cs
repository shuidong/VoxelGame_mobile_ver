using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class VirtualJoyStick : MonoBehaviour {

	[SerializeField]
	private Camera uiCamera;
	[SerializeField]
	private Camera mainCamera;
	[SerializeField]
	private Transform playerTransform;
	[SerializeField]
	private UISprite touchPad;
	[SerializeField] // Create block & delete 메소드를 얻어오기 위한 스크립트 변수.
	private ModifyTerrain modifyTerrian;
	[SerializeField]
	private AudioClip blockPickSound;
	[SerializeField]
	private AudioClip blockSelectSound;


	private float playerSpeed;

	//----- detail option list -----------//
	public bool invertY = false;
	
	public float sensitivityX = 10F;
	public float sensitivityY = 9F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -85F;
	public float maximumY = 85F;
	
	float rotationX = 0F;
	float rotationY = 0F;
	
	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	
	
	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0F;
	
	public float framesOfSmoothing = 5;
	
	private Quaternion originalRotation;
	//-------list end--------------------//

	private Vector3 worldPoint;
	private Vector2 touchPos;

	public RaycastHit hit;
	public Ray r;

	private float touchMovingX;
	private float touchMovingY;

	private bool movingCharacterFlag = false;

	public void Start()
	{
		playerSpeed = 10.0f;
		originalRotation = mainCamera.transform.localRotation;
		modifyTerrian = GameObject.Find("World").GetComponent<ModifyTerrain>();
	}

	public void Update()
	{
		foreach (Touch touch in Input.touches)
		{
			r = uiCamera.ScreenPointToRay(touch.position);

			if(Physics.Raycast(r, out hit, Mathf.Infinity, 1<<5))
			{
				if(hit.transform.gameObject.name == "spr_touchPad")
				{
					touchMovingX = touch.deltaPosition.x;
					touchMovingY = touch.deltaPosition.y;

					TouchRotation();
					SysLogManager.SetLogMessage("Touch Pos (X,Y) : " + touch.position.x +","+touch.position.y);
					SysLogManager.SetLogMessage("Hit Pos (X,Y) : " + hit.transform.position.x +","+hit.transform.position.y);
					SysLogManager.SetLogMessage("Cur Touch ID : " +UICamera.currentTouchID);
					SysLogManager.SetLogMessage("All Touches : "+ Input.touchCount);
				}
			}
		}
		// test code by jjw
		if(movingCharacterFlag == true)
			MovingCharacter();
	}
	public void TouchRotation()
	{
		rotAverageX = 0f;
		rotAverageY = 0f;
		
		float invertFlag = 1f;
		
		if( invertY )
		{
			invertFlag = -1f;
		}

		rotationX += touchMovingX * sensitivityX * Time.timeScale;
		rotationY += touchMovingY * sensitivityY * invertFlag * Time.timeScale;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
		
		rotArrayX.Add(rotationX);
		rotArrayY.Add(rotationY);
		
		if (rotArrayX.Count >= framesOfSmoothing)
		{
			rotArrayX.RemoveAt(0);
		}
		for(int i = 0; i < rotArrayX.Count; i++)
		{
			rotAverageX += rotArrayX[i];
		}
		rotAverageX /= rotArrayX.Count;
		rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

		if (rotArrayY.Count >= framesOfSmoothing)
		{
			rotArrayY.RemoveAt(0);
		}
		for(int j = 0; j < rotArrayY.Count; j++)
		{
			rotAverageY += rotArrayY[j];
		}
		rotAverageY /= rotArrayY.Count;
		
		Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);

		mainCamera.transform.localRotation = originalRotation * xQuaternion * yQuaternion;	
	}
	// TouchRotation 메소드를 돕는 Help메소드입니다.
	public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}

	public void MoveCharacterFlagOn()
	{
		movingCharacterFlag = true;
	}
	public void MoveCharacterFlagOFF()
	{
		movingCharacterFlag = false;
	}
	public void MovingCharacter()
	{
		Vector3 moveDirection = mainCamera.transform.forward;
		
		playerTransform.position += moveDirection * playerSpeed * Time.deltaTime;
	}

	public void DeleteBlock()
	{
		modifyTerrian.ReplaceBlockCenter(0);
		audio.PlayOneShot(blockPickSound, 1.0f);
		modifyTerrian.StartLoadChunks();
	}

	public void CreateBlock()
	{
		modifyTerrian.AddBlockCenter(SelectBlock.GetCurBlock());
		audio.PlayOneShot(blockPickSound, 1.0f);
		modifyTerrian.StartLoadChunks();
	}
}




