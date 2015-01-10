using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Text;



// 상수모음 클래스.
public static class ConstantGroup
{
	public const int saveBtyeArraySize = 1000000; // 1 million
	public const int loadByteArraySize = 4194304; //256*64*256
}

public class SaveAndLoad : MonoBehaviour {
	
	// 월드데이터 컴포넌트를 가져오기 위한 임시변수.
	private GameObject getWorld; 
	private World m_world;
	//게임 월드 데이터를 저장하고 압축을 위해 쓰이는 변수.
	private GameData gameWorldData; 
	// 로드 되었는가?
	private bool IsLoadedData;
	private string fileDirectory;
	// Save&Load 시에 게임데이터를 압축할 클래스 인스턴스.
	private LZF lzf_Compression;

	public void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		fileDirectory = Application.persistentDataPath + "/GameSavefile.dat";
	}
	
	public void SaveGame()
	{
		lzf_Compression = new LZF();
		UnityThreadHelper.Dispatcher.Dispatch (() =>
		                                       {
			// 월드 오브젝트에서 컴포넌트 추출.
			getWorld = GameObject.Find ("World");
			m_world = getWorld.GetComponent("World") as World;
		});
		// Init GameData class
		gameWorldData = new GameData();
		// 게임월드 배열을 저장 게임데이터로 옴긴다.
		gameWorldData.Flatten3dArray(m_world); 
		// 압축시작.
		lzf_Compression.Compress(gameWorldData.GetWorldData(),
		                         gameWorldData.GetWorldData().Length,
		                         gameWorldData.GetSaveByteArray(),
		                         ConstantGroup.saveBtyeArraySize);
		// 파일 생성.
		BinaryFormatter bf = new BinaryFormatter(); 
		FileStream fileStream = File.Open (fileDirectory, FileMode.OpenOrCreate);
		// 시리얼라이징.
		bf.Serialize(fileStream, gameWorldData);
		fileStream.Close ();
		
		UnityThreadHelper.Dispatcher.Dispatch (() =>
		{
			SysLogManager.SetLogMessage("<SYSTEM> : 현재 내용이 저장되었습니다. ");
		});
	}
	
	public void LoadGame()
	{
		lzf_Compression = new LZF();
		gameWorldData = new GameData();
		//파일 생성.
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fileStream = File.Open (fileDirectory, FileMode.Open);
		// DeSerialzing ( decode..)
		gameWorldData = (GameData)bf.Deserialize(fileStream);
		gameWorldData.MemoryAllocLoadArray(); // loadArray의 경우 serializing 이 되지 않았으므로, 다시 재할당한다.

		fileStream.Close ();
		// 압축해제 작업.
		lzf_Compression.Decompress(gameWorldData.GetSaveByteArray(),
		                           ConstantGroup.saveBtyeArraySize,
		                           gameWorldData.GetLoadByteArray(),
		                           ConstantGroup.loadByteArraySize);
		// Load가 완료되었으므로, true로 값을 설정.
		IsLoadedData = true;

		UnityThreadHelper.Dispatcher.Dispatch (() =>
		{
			Application.LoadLevel("LoadingNextScene");
		});

	}
	public GameData GetGameLoadData()
	{
		return gameWorldData;
	}
	public bool GetIsLoaded()
	{
		return IsLoadedData;
	}
	public void SetIsLoaded(bool _b)
	{
		IsLoadedData = _b;
	}
}

//
//
// GaemData 는 Save & Load 시에 쓰이는 클래스, 따라서 Serializable 한다.
// gaeWorldData, loadGameByteArr 의 두개의 byte 어레이는 저장할 필요 없다.
// 
[Serializable]
public class GameData
{
	//게임 오브젝트를 배치하는 world.data[x,y,z]는 3차원
	//저장할 때 메모리를 줄이기 위해, 1차원 평면 배열로 옴겨준다. ( 이 1차원 어레이를 저장에 이용.)
	[field:NonSerialized] //해당필드 시리얼라이징 X.
	private byte[] gameWorldData; 
	[field:NonSerialized] //해당필드 시리얼라이징 X.
	private byte[] loadGameByteArr; // (월드사이즈) byte 만큼의 영역을 가진 로드된 월드정보를 가지고있는 임시배열.
	// 세이브시에 쓰이는 게임 byte 배열. 
	private byte[] saveGameByteArr; 
	// world x,y,z and Size .
	private int worldHeight;
	private int worldWidth;
	private int worldDepth;
	private int worldSize;
	
	public GameData()
	{
		saveGameByteArr = new byte[ConstantGroup.saveBtyeArraySize];
		loadGameByteArr = new byte[ConstantGroup.loadByteArraySize];
		worldHeight=0;
		worldWidth=0;
		worldDepth=0;
		worldSize=0;
	}
	// 3d 배열 to 1d 배열로 평면화.
	public void Flatten3dArray(World _world)
	{
		worldHeight = _world.worldX;
		worldWidth  = _world.worldY;
		worldDepth  = _world.worldZ;
		worldSize   = worldHeight * worldWidth * worldDepth;
		
		gameWorldData = new byte[worldSize];
		
		for(int x=0; x < worldHeight; x++)
			for(int y=0; y < worldWidth; y++)
				for(int z=0; z < worldDepth; z++)
					if(_world.data[x,y,z] != 0 )
						gameWorldData[x + worldWidth * (y + worldDepth * z)] = _world.data[x,y,z];

	}
	
	public byte[] GetWorldData()
	{
		return gameWorldData;
	}
	
	public byte[] GetSaveByteArray()
	{
		return saveGameByteArr;
	}
	public byte[] GetLoadByteArray()
	{
		return loadGameByteArr;
	}
	public void MemoryAllocLoadArray()
	{
		loadGameByteArr = new byte[ConstantGroup.loadByteArraySize];
	}
	
}






