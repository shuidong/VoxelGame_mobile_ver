using UnityEngine;
using System.Collections;
using UnityThreading;

public class ModifyTerrain : MonoBehaviour {
	
	private World world;
	private GameObject cameraGO;
	private Vector3 playerPos;
	[SerializeField]
	private int chunkSize;

	public float distToLoad;
	public float distToUnLoad;

	public World worldScript;


	void Start ()
	{
		world=this.GetComponent("World") as World;
		cameraGO=GameObject.FindGameObjectWithTag("MainCamera");

		StartLoadChunks();
	}
	public void StartLoadChunks()
	{
		StartCoroutine("LoadChunks");
	}
	
	IEnumerator GenColumns(int x, int z)
	{
		for (int y=0; y<world.chunks.GetLength(1); y++)
		{
			GameObject newChunk = Instantiate (world.chunk, new Vector3 (x * chunkSize - 0.5f,
			                                                       y * chunkSize + 0.5f, z * chunkSize - 0.5f), 
			                                   new Quaternion (0, 0, 0, 0)) as GameObject;
			
			world.chunks [x, y, z] = newChunk.GetComponent ("Chunk") as Chunk;
			world.chunks [x, y, z].worldGO = gameObject;
			world.chunks [x, y, z].chunkSize = chunkSize;
			//청크가 위치할 좌표를 설정. 청크사이즈를 곱하는건 월드좌표로 변환.
			world.chunks [x, y, z].chunkX = x * chunkSize;
			world.chunks [x, y, z].chunkY = y * chunkSize;
			world.chunks [x, y, z].chunkZ = z * chunkSize;

			yield return new WaitForSeconds(0.15f);
		}
	}
	IEnumerator UnloadColumn(int x, int z)
	{
		for (int y=0; y<world.chunks.GetLength(1); y++)
		{
			Object.Destroy(world.chunks [x, y, z].gameObject);
			yield return new WaitForSeconds(0.3f);
		}
	}
	//플레이어와의 거리에 따라 분기하여 Chunk를 Load합니다.
	IEnumerator LoadChunks()
	{
		float dist = 0.0f;
		int chunksX = 0;
		int chunksZ = 0;

		playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		chunksX = world.chunks.GetLength(0);
		chunksZ = world.chunks.GetLength(2);
		
		// 3차원배열 [xyz] 중에 x 서브배열 길이 , 아래는 마찬가지로 z 서브배열 길이를 구해온다.
		for(int x=0 ; x < chunksX; x++)
		{
			for(int z=0; z < chunksZ; z++)
			{
				dist=Vector2.Distance(new Vector2(x*world.chunkSize,z*world.chunkSize),new Vector2(playerPos.x,playerPos.z));
				
				//플레이어와 청크와의 거리를 계산하여 화면에 보일지 아닐지 결정하는 if구조문.
				if(dist < distToLoad)
				{
					if(world.chunks[x,0,z]==null)
					{
						yield return StartCoroutine(GenColumns(x,z));
					}
				}
				else if(dist > distToUnLoad)
				{
					if(world.chunks[x,0,z]!=null)
					{
						yield return StartCoroutine(UnloadColumn(x,z));

					}
				}
			}
		}
		
	}


	public void ReplaceBlockCenter(byte block){
		//Replaces the block directly in front of the player	
		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit))
		{
			ReplaceBlockAt(hit, block);
		}
	}
	
	public void AddBlockCenter(byte block){
		//Adds the block specified directly in front of the player
		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit))
		{
			AddBlockAt(hit,block);
			Debug.DrawLine(ray.origin,ray.origin+( ray.direction*hit.distance),Color.green,2);
		}
	}

	public void ReplaceBlockAt(RaycastHit hit, byte block) {
		//removes a block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
		Vector3 position = hit.point;
		position+=(hit.normal*-0.5f);
		
		SetBlockAt(position, block);
	}
	
	public void AddBlockAt(RaycastHit hit, byte block) {
		//adds the specified block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
		Vector3 position = hit.point;
		position+=(hit.normal*0.5f);
		
		SetBlockAt(position,block);
	}
	
	public void SetBlockAt(Vector3 position, byte block) {
		//sets the specified block at these coordinates
		
		int x= Mathf.RoundToInt( position.x );
		int y= Mathf.RoundToInt( position.y );
		int z= Mathf.RoundToInt( position.z );
		
		SetBlockAt(x,y,z,block);
	}
	
	public void SetBlockAt(int x, int y, int z, byte block) {
		//adds the specified block at these coordinates
		
		print("Adding: " + x + ", " + y + ", " + z);
		
		if(world.data[x+1,y,z]==254){
			world.data[x+1,y,z]=255;
		}
		if(world.data[x-1,y,z]==254){
			world.data[x-1,y,z]=255;
		}
		if(world.data[x,y,z+1]==254){
			world.data[x,y,z+1]=255;
		}
		if(world.data[x,y,z-1]==254){
			world.data[x,y,z-1]=255;
		}
		if(world.data[x,y+1,z]==254){
			world.data[x,y+1,z]=255;
		}
		world.data[x,y,z]=block;
		
		UpdateChunkAt(x,y,z,block);
		
	}
	
	public void UpdateChunkAt(int x, int y, int z, byte block){		
		//To do: add a way to just flag the chunk for update and then it updates in lateupdate
		//Updates the chunk containing this block
		
		int updateX= Mathf.FloorToInt( x/world.chunkSize);
		int updateY= Mathf.FloorToInt( y/world.chunkSize);
		int updateZ= Mathf.FloorToInt( z/world.chunkSize);
		
		print("Updating: " + updateX + ", " + updateY + ", " + updateZ); 

		world.chunks[updateX,updateY, updateZ].update=true;

		if(x-(world.chunkSize*updateX)==0 && updateX!=0){
			world.chunks[updateX-1,updateY, updateZ].update=true;
		}
		
		if(x-(world.chunkSize*updateX)== (chunkSize-1) && updateX!=world.chunks.GetLength(0)-1){
			world.chunks[updateX+1,updateY, updateZ].update=true;
		}
		
		if(y-(world.chunkSize*updateY)==0 && updateY!=0){
			world.chunks[updateX,updateY-1, updateZ].update=true;
		}
		
		if(y-(world.chunkSize*updateY)== (chunkSize-1) && updateY!=world.chunks.GetLength(1)-1){
			world.chunks[updateX,updateY+1, updateZ].update=true;
		}
		
		if(z-(world.chunkSize*updateZ)==0 && updateZ!=0){
			world.chunks[updateX,updateY, updateZ-1].update=true;
		}
		
		if(z-(world.chunkSize*updateZ)== (chunkSize-1) && updateZ!=world.chunks.GetLength(2)-1){
			world.chunks[updateX,updateY, updateZ+1].update=true;
		}


		
	}
	
}
