using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Chunk : MonoBehaviour {
	
	
	public GameObject worldGO;
	private World world;
	
	private List<Vector3> newVertices = new List<Vector3>();
	private List<int> newTriangles = new List<int>();
	private List<Vector2> newUV = new List<Vector2>();
	
	// 텍스처 이미지는 왼쪽 하단이 (0,0) 으로 시작한다. 
	private float tUnit = 0.0625f;  // 맵 텍스처 이미지를 어느단위로 읽어야하는지.
	
	private Vector2 tStone = new Vector2 (1, 15); // 1
	private Vector2 tGrass = new Vector2 (3, 15); // 2
	private Vector2 tGrassTop = new Vector2 (0, 15); // 초기 맵 chunk mesh 생성시 상단면의 texture는 grassTop을 이용.
	private Vector2 tDirt = new Vector2 (2, 14); // 3
	private Vector2 tRockwall = new Vector2(0,14); //4
	private Vector2 tBlackStone = new Vector2(1,14); //5
	private Vector2 tGravle = new Vector2(3,14); //6
	private Vector2 tLog = new Vector2(5,14); //7
	private Vector2 tMud = new Vector2(2,15); //8
	private Vector2 tWoodBox = new Vector2(4,15); //9
	
	
	private Mesh mesh;
	private MeshCollider col;
	
	private int faceCount;
	
	public int chunkSize;
	
	public int chunkX;
	public int chunkY;
	public int chunkZ;
	public bool update;
	
	
	void LateUpdate () {
		if(update){
			GenerateMesh();
			update=false;
		}
	}
	// Use this for initialization
	void Start () { 
		
		world=worldGO.GetComponent("World") as World;
		mesh = GetComponent<MeshFilter> ().mesh;
		col = GetComponent<MeshCollider> ();
		
		GenerateMesh();
	}

	public void GenerateMesh()
	{
		for (int x=0; x<chunkSize; x++)
		{
			for (int y=0; y<chunkSize; y++)
			{
				for (int z=0; z<chunkSize; z++)
				{
					//This code will run for every block in the chunk
					if(Block(x,y,z)!=0)
					{
						if(Block(x,y+1,z)==0){
							//Block above is air
							CubeTop(x,y,z,Block(x,y,z));
						}
						
						if(Block(x,y-1,z)==0){
							//Block below is air
							CubeBot(x,y,z,Block(x,y,z));

						}
						
						if(Block(x+1,y,z)==0){
							//Block east is air
							CubeEast(x,y,z,Block(x,y,z));

						}
						
						if(Block(x-1,y,z)==0){
							//Block west is air
							CubeWest(x,y,z,Block(x,y,z));

						}
						
						if(Block(x,y,z+1)==0){
							//Block north is air
							CubeNorth(x,y,z,Block(x,y,z));

						}
						
						if(Block(x,y,z-1)==0){
							//Block south is air
							CubeSouth(x,y,z,Block(x,y,z));

						}

					}


				}
			}
		}
		UpdateMesh ();

	}
	
	byte Block(int x, int y, int z){
		return world.Block(x+chunkX,y+chunkY,z+chunkZ);
	}
	
	void CubeTop (int x, int y, int z, byte block) {
		
		newVertices.Add(new Vector3 (x,  y,  z + 1));
		newVertices.Add(new Vector3 (x + 1, y,  z + 1));
		newVertices.Add(new Vector3 (x + 1, y,  z ));
		newVertices.Add(new Vector3 (x,  y,  z ));
		
		Vector2 texturePos=new Vector2(0,0);
		
		if(Block(x,y,z)==1){
			texturePos=tStone;
		} 
		else if(Block(x,y,z)==2){
			texturePos=tGrassTop;
		}
		else if(Block(x,y,z)==3)
		{
			texturePos=tDirt;
		}
		else if(Block(x,y,z)==4)
		{
			texturePos=tRockwall;
		}
		else if(Block (x,y,z)==5)
		{
			texturePos=tBlackStone;
		}
		else if(Block (x,y,z)==6)
		{
			texturePos=tGravle;
		}
		else if(Block (x,y,z)==7)
		{
			texturePos=tLog;
		}
		else if(Block (x,y,z)==8)
		{
			texturePos=tMud;
		}
		else if(Block (x,y,z)==9)
		{
			texturePos=tWoodBox;
		}
		
		
		
		Cube (texturePos);
		
	}
	
	void CubeNorth (int x, int y, int z, byte block) {
		
		newVertices.Add(new Vector3 (x + 1, y-1, z + 1));
		newVertices.Add(new Vector3 (x + 1, y, z + 1));
		newVertices.Add(new Vector3 (x, y, z + 1));
		newVertices.Add(new Vector3 (x, y-1, z + 1));
		
		Vector2 texturePos=new Vector2(0,0);
		
		if(Block(x,y,z)==1){
			texturePos=tStone;
		} 
		else if(Block(x,y,z)==2){
			texturePos=tGrass;
		}
		else if(Block(x,y,z)==3)
		{
			texturePos=tDirt;
		}
		else if(Block(x,y,z)==4)
		{
			texturePos=tRockwall;
		}
		else if(Block (x,y,z)==5)
		{
			texturePos=tBlackStone;
		}
		else if(Block (x,y,z)==6)
		{
			texturePos=tGravle;
		}
		else if(Block (x,y,z)==7)
		{
			texturePos=tLog;
		}
		else if(Block (x,y,z)==8)
		{
			texturePos=tMud;
		}
		else if(Block (x,y,z)==9)
		{
			texturePos=tWoodBox;
		}
		Cube (texturePos);
		
	}
	
	void CubeEast (int x, int y, int z, byte block) {
		
		newVertices.Add(new Vector3 (x + 1, y - 1, z));
		newVertices.Add(new Vector3 (x + 1, y, z));
		newVertices.Add(new Vector3 (x + 1, y, z + 1));
		newVertices.Add(new Vector3 (x + 1, y - 1, z + 1));
		
		Vector2 texturePos=new Vector2(0,0);
		
		if(Block(x,y,z)==1){
			texturePos=tStone;
		} 
		else if(Block(x,y,z)==2){
			texturePos=tGrass;
		}
		else if(Block(x,y,z)==3)
		{
			texturePos=tDirt;
		}
		else if(Block(x,y,z)==4)
		{
			texturePos=tRockwall;
		}
		else if(Block (x,y,z)==5)
		{
			texturePos=tBlackStone;
		}
		else if(Block (x,y,z)==6)
		{
			texturePos=tGravle;
		}
		else if(Block (x,y,z)==7)
		{
			texturePos=tLog;
		}
		else if(Block (x,y,z)==8)
		{
			texturePos=tMud;
		}
		else if(Block (x,y,z)==9)
		{
			texturePos=tWoodBox;
		}
		Cube (texturePos);
		
	}
	
	void CubeSouth (int x, int y, int z, byte block) {
		
		newVertices.Add(new Vector3 (x, y - 1, z));
		newVertices.Add(new Vector3 (x, y, z));
		newVertices.Add(new Vector3 (x + 1, y, z));
		newVertices.Add(new Vector3 (x + 1, y - 1, z));
		
		Vector2 texturePos=new Vector2(0,0);
		
		if(Block(x,y,z)==1){
			texturePos=tStone;
		} 
		else if(Block(x,y,z)==2){
			texturePos=tGrass;
		}
		else if(Block(x,y,z)==3)
		{
			texturePos=tDirt;
		}
		else if(Block(x,y,z)==4)
		{
			texturePos=tRockwall;
		}
		else if(Block (x,y,z)==5)
		{
			texturePos=tBlackStone;
		}
		else if(Block (x,y,z)==6)
		{
			texturePos=tGravle;
		}
		else if(Block (x,y,z)==7)
		{
			texturePos=tLog;
		}
		else if(Block (x,y,z)==8)
		{
			texturePos=tMud;
		}
		else if(Block (x,y,z)==9)
		{
			texturePos=tWoodBox;
		}
		Cube (texturePos);
		
	}
	
	void CubeWest (int x, int y, int z, byte block) {
		
		newVertices.Add(new Vector3 (x, y- 1, z + 1));
		newVertices.Add(new Vector3 (x, y, z + 1));
		newVertices.Add(new Vector3 (x, y, z));
		newVertices.Add(new Vector3 (x, y - 1, z));
		
		Vector2 texturePos=new Vector2(0,0);
		
		if(Block(x,y,z)==1){
			texturePos=tStone;
		} 
		else if(Block(x,y,z)==2){
			texturePos=tGrass;
		}
		else if(Block(x,y,z)==3)
		{
			texturePos=tDirt;
		}
		else if(Block(x,y,z)==4)
		{
			texturePos=tRockwall;
		}
		else if(Block (x,y,z)==5)
		{
			texturePos=tBlackStone;
		}
		else if(Block (x,y,z)==6)
		{
			texturePos=tGravle;
		}
		else if(Block (x,y,z)==7)
		{
			texturePos=tLog;
		}
		else if(Block (x,y,z)==8)
		{
			texturePos=tMud;
		}
		else if(Block (x,y,z)==9)
		{
			texturePos=tWoodBox;
		}
		Cube (texturePos);
		
	}
	
	void CubeBot (int x, int y, int z, byte block) {
		
		newVertices.Add(new Vector3 (x,  y-1,  z ));
		newVertices.Add(new Vector3 (x + 1, y-1,  z ));
		newVertices.Add(new Vector3 (x + 1, y-1,  z + 1));
		newVertices.Add(new Vector3 (x,  y-1,  z + 1));
		
		Vector2 texturePos=new Vector2(0,0);
		
		if(Block(x,y,z)==1){
			texturePos=tStone;
		} 
		else if(Block(x,y,z)==2){
			texturePos=tGrass;
		}
		else if(Block(x,y,z)==3)
		{
			texturePos=tDirt;
		}
		else if(Block(x,y,z)==4)
		{
			texturePos=tRockwall;
		}
		else if(Block (x,y,z)==5)
		{
			texturePos=tBlackStone;
		}
		else if(Block (x,y,z)==6)
		{
			texturePos=tGravle;
		}
		else if(Block (x,y,z)==7)
		{
			texturePos=tLog;
		}
		else if(Block (x,y,z)==8)
		{
			texturePos=tMud;
		}
		else if(Block (x,y,z)==9)
		{
			texturePos=tWoodBox;
		}
		Cube (texturePos);
		
	}
	
	void Cube (Vector2 texturePos) {
		
		//2개의 삼각형을 만든다. -> 결국 사각형1개..
		newTriangles.Add(faceCount * 4  ); //1
		newTriangles.Add(faceCount * 4 + 1 ); //2
		newTriangles.Add(faceCount * 4 + 2 ); //3
		newTriangles.Add(faceCount * 4  ); //1
		newTriangles.Add(faceCount * 4 + 2 ); //3
		newTriangles.Add(faceCount * 4 + 3 ); //4
		
		newUV.Add(new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
		newUV.Add(new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
		newUV.Add(new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
		newUV.Add(new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y));
		
		faceCount++; // Add this line
	}
	
	void UpdateMesh ()
	{
		
		mesh.Clear ();
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();
		
		col.sharedMesh=null;
		col.sharedMesh=mesh;
		
		newVertices.Clear();
		newUV.Clear();
		newTriangles.Clear();
		faceCount=0;
		
	}
}