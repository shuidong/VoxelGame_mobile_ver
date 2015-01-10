using UnityEngine;
using System.Collections;
/*
	data array : 전체 월드를 나타내는 배열 
	chunks array : 청크들의 묶음을 나타낸다.

	ex) data[1024,128,1024] 의 월드가 있다면, 134,217,728크기를 지니고 ( 약 1억 3천만개 )
		그리고 chunk size 가 128인 경우, chunks 의 크기는 비례해서 [8,1,8]크기의 배열이 된다.
		개별 chunk는 128*128*128 크기의 다면체가 되고, 이 chunk들이 위에서 언급된 chunks array에 할당이 된다.
		결국 게임내에 런타임으로 존재하는 GameObject의 갯수는 64개가 최대이다. (-> 상당한 연산을 줄여준다. )
		이제, 월드와 청크스의 상대적인 좌표값을 다시 계산하면서 ( chunk 좌표에 곱하기 128[chunk size]를 해준다 )

 */

public class World : MonoBehaviour
{
	
	public GameObject chunk;
	public Chunk[,,] chunks;  //블록들의 집합인 청크들의 묶음.
	public byte[,,] data; // 월드내에 존재하는 모든블록의 개별성질을 가지고있다. ex: grass, stone..etc.
	
	public int chunkSize;
	public int worldX;
	public int worldY;
	public int worldZ;

	private UnityThreading.ActionThread initMapThread;
	private UnityThreading.ActionThread loadingMapThread;

	private GameObject getManager;
	private SaveAndLoad saveAndLoadManager;
	
	// Use this for initialization
	void Awake ()
	{
		// 세이브 & 로드 매니저 오브젝트를 구한다.
		getManager = GameObject.Find ("SaveAndLoadManager");
		saveAndLoadManager = getManager.GetComponent("SaveAndLoad") as SaveAndLoad;

		if(saveAndLoadManager.GetIsLoaded() == false)
		{
			initMapThread = UnityThreadHelper.CreateThread((System.Action)InitMapData);
		}
		else 
		{
			loadingMapThread = UnityThreadHelper.CreateThread((System.Action)LoadGameMapData);
		}

		//청크배열에 메모리 할당.
		chunks = new Chunk[Mathf.FloorToInt (worldX / chunkSize),
		                   Mathf.FloorToInt (worldY / chunkSize),
		                   Mathf.FloorToInt (worldZ / chunkSize)];
	}
	// 저장된 게임맵 data를 로드합니다.
	public void LoadGameMapData()
	{
		byte[] loadByteArray = new byte[worldX * worldY * worldZ];
		data = new byte[worldX, worldY, worldZ]; 
		loadByteArray = saveAndLoadManager.GetGameLoadData().GetLoadByteArray();
		
		for(int x=0; x < worldX  ; x++)
			for(int y=0; y < worldY  ; y++)
				for(int z=0; z < worldZ ; z++)
					data[x,y,z] = loadByteArray[x + worldY * (y + worldZ * z)]; 

		UnityThreadHelper.Dispatcher.Dispatch (() =>
	    {
			// 월드 데이터 로딩이 완료되었다.
			saveAndLoadManager.SetIsLoaded(false);
			SysLogManager.SetLogMessage("<SYSTEM> : 저장내용을 불러왔습니다. ");
		});

	}
	// 맵정보를 저장하는 data[xyz] 배열을 초기화 합니다.
	public void InitMapData()
	{
		data = new byte[worldX, worldY, worldZ]; 
		
		//=========================
		//초기 맵 펄린 노이즈 초기화 과정.
		// : 펄린노이즈 값으로 data 배열을 초기화시키며
		//	 이값은 결국 전체 월드의 블록 성질이 된다.
		//=========================
		for (int x=0; x<worldX; x++) {
			for (int z=0; z<worldZ; z++) {
				int stone = PerlinNoise (x, 0, z, 10.0f,
				                         2.5f,
				                         1.5f);
				stone += PerlinNoise (x, 300, z, 10, 4, 0) + 10;
				int dirt = PerlinNoise (x, 100, z, 50, 3, 0) + 1;
				
				for (int y=0; y<worldY; y++) {
					if (y <= stone) {
						data [x, y, z] = 1;
					} else if (y <= dirt + stone) {
						data [x, y, z] = 2;
					} 
					
				}
			}
		}

	}
	

	
	int PerlinNoise (int x, int y, int z, float scale, float height, float power)
	{ 
		float rValue;
		rValue = Noise.GetNoise (((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
		rValue *= height;
		
		if (power != 0) {
			rValue = Mathf.Pow (rValue, power);
		}
		
		return (int)rValue;
	}
	
	//개별 블록의 성질을 리턴한다.
	public byte Block (int x, int y, int z)
	{
		if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0) {
			return (byte)1;
		}
		
		return data [x, y, z];
	}
}