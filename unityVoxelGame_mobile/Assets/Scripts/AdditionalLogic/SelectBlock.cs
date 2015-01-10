using UnityEngine;
using System.Collections;

public class SelectBlock : MonoBehaviour {

	private static byte CurBlock;
	public GameObject m_selectBlock;
	private Texture[] blockTextures;


	// Use this for initialization
	void Start ()
	{

		CurBlock=1; // 초기 블록은 stone로..

		blockTextures=new Texture[9];
		blockTextures[0]=Resources.Load("texture/selectBlocks/stone") as Texture;
		blockTextures[1]=Resources.Load("texture/selectBlocks/grass") as Texture;
		blockTextures[2]=Resources.Load("texture/selectBlocks/dirt") as Texture;
		blockTextures[3]=Resources.Load("texture/selectBlocks/rockwall") as Texture;
		blockTextures[4]=Resources.Load("texture/selectBlocks/blackstone") as Texture;
		blockTextures[5]=Resources.Load("texture/selectBlocks/gravle") as Texture;
		blockTextures[6]=Resources.Load("texture/selectBlocks/log") as Texture;
		blockTextures[7]=Resources.Load("texture/selectBlocks/mud") as Texture;
		blockTextures[8]=Resources.Load("texture/selectBlocks/woodbox") as Texture;

		//Init, stone으로 초기화.
		m_selectBlock.renderer.material.SetTexture("_MainTex",blockTextures[0]);


	}
	
	// Update is called once per frame
	void Update ()
	{

		CheckInput (Input.anyKey);
	}

	public void CheckInput(bool isInput)
	{
		if(isInput == true)
		{

			if(Input.GetKey (KeyCode.Alpha1))
			{
				SetCurBlock(1); //
				m_selectBlock.renderer.material.SetTexture("_MainTex",blockTextures[0]);
			}
			else if(Input.GetKey (KeyCode.Alpha2))
			{
				SetCurBlock(2); // 
				m_selectBlock.renderer.material.SetTexture("_MainTex",blockTextures[1]);
			}
			else if(Input.GetKey (KeyCode.Alpha3))
			{
				SetCurBlock(3); // 
				m_selectBlock.renderer.material.SetTexture("_MainTex",blockTextures[2]);
			}
			else if(Input.GetKey (KeyCode.Alpha4))
			{
				SetCurBlock(4); // 
				m_selectBlock.renderer.material.SetTexture("_MainTex",blockTextures[3]);
			}
			else if(Input.GetKey (KeyCode.Alpha5))
			{
				SetCurBlock(5); //
				m_selectBlock.renderer.material.SetTexture("_MainTex",blockTextures[4]);
			}
			else if(Input.GetKey (KeyCode.Alpha6))
			{
				SetCurBlock(6); // 
				m_selectBlock.renderer.material.SetTexture("_MainTex",blockTextures[5]);
			}
			else if(Input.GetKey (KeyCode.Alpha7))
			{
				SetCurBlock(7); // 
				m_selectBlock.renderer.material.SetTexture("_MainTex",blockTextures[6]);
			}
			else if(Input.GetKey (KeyCode.Alpha8))
			{
				SetCurBlock(8); // 
				m_selectBlock.renderer.material.SetTexture("_MainTex",blockTextures[7]);
			}
			else if(Input.GetKey (KeyCode.Alpha9))
			{
				SetCurBlock(9); // 
				m_selectBlock.renderer.material.SetTexture("_MainTex",blockTextures[8]);
			}
		}
	}
	private void SetCurBlock(byte _val)
	{
		CurBlock=_val;
	}
	// ModifyTerrian 컴포넌트에서 해당 정적멤버함수를 써야한다.
	public static byte GetCurBlock()
	{
		return CurBlock;
	}
}
