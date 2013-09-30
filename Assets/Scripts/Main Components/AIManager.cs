using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIManager : MonoBehaviour 
{
	// Script Holder
	[SerializeField] AudioManager   sc_AudioManager;
	[SerializeField] GameController sc_GameController;
	[SerializeField] GameTimer 		sc_GameTimer;
	
	GameObject GraveYard;
	[SerializeField] Transform DuckWave;
	GameObject duckWave_One;
	GameObject duckWave_Three;
	float timeBetweenWaves = 15;
	float aiPath_Duration = 40;
	
	[HideInInspector]
	public List<GameObject> DuckWaves = new List<GameObject>();		// Holds duck waves
	List<GameObject> DuckWaveTypes = new List<GameObject>();		// Holds duck types
	public List<GameObject> AI_Paths = new List<GameObject>();				// Holds ai paths ducks can take
	public List<GameObject> AI_PathsUsed = new List<GameObject>();			// Holds ai paths ducks are using
	
	void Start()
	{
		// Find Graveyard
		GraveYard = GameObject.Find("GraveYard");
		
		// Get all duck waves
		duckWave_One = Resources.Load("Prefabs/Targets/Ducks/duckWave_One") as GameObject;
		duckWave_Three = Resources.Load("Prefabs/Targets/Ducks/duckWave_Three") as GameObject;
		
		// Get all possible ai paths ducks can take
		Object[] All_AI_Paths = Resources.LoadAll("Prefabs/AI/Duck Paths", typeof(GameObject));

		// Get all duckwaves
		DuckWaveTypes.Add(duckWave_One);
		DuckWaveTypes.Add(duckWave_Three);
		
		// Get all ai paths ducks can follow
		for (int i=0; i < All_AI_Paths.Length; i++)
			AI_Paths.Add(All_AI_Paths[i] as GameObject);

		// Start timer		
		StartCoroutine( WaveTimer() );
	}
	
	IEnumerator WaveTimer()
	{
		// Create two waves
		CreateDuckWave(2);
		
		// Calculate time between each wave. If game is loading or paused
		// do not calculate the time
		float time = 0;
		while (time < 1)
		{
			if (sc_GameController.GameState != GameController.GameStatus.PLAYING)
				yield return null;
			else
				time += sc_GameTimer.Game_deltaTime / timeBetweenWaves;
			yield return null;	
		}
		
		// Restart wave timer
		StartCoroutine( WaveTimer() );
	}
	
	
	void CreateDuckWave(int waveCount)
	{		
		for (int i = 0; i < waveCount; i++)
		{	
			// Instantiate and add wave to list
			GameObject wave = Instantiate( DuckWaveTypes[PickRandom_DuckWave()], DuckWave.position, Quaternion.identity) as GameObject;
			wave.transform.parent = DuckWave;
			SetupDuckWave(wave, PickRandom_AIPath());
			DuckWaves.Add(wave);
			wave.SetActive(true);
		}
	}
	
	void SetupDuckWave(GameObject wave, GameObject path)
	{
		// Attach spline path and duration time
		SplineController   spline = wave.GetComponent<SplineController>();
		spline.SplineRoot = path;
		spline.Duration   = aiPath_Duration;
		
		SplineInterpolator interp = wave.GetComponent<SplineInterpolator>();
		interp.sc_AIManager      = this;
		interp.sc_GameController = sc_GameController;
		interp.sc_GameTimer      = sc_GameTimer;
		
		// Get duck in each wave and attach components
		foreach(Transform child in wave.transform)
		{
			Duck duck = child.GetComponent<Duck>();
			duck.sc_AIManager      = this;
			duck.sc_AudioManager   = sc_AudioManager;
			duck.sc_GameController = sc_GameController;
			duck.GraveYard = GraveYard;
		}
	}
	
	public void RemoveDuckFromWave( GameObject duck )
	{
		Transform wave = duck.transform.parent;
		
		int childCount = wave.childCount;
		
		// Create another wave if it is the last duck in wave
		if (childCount == 1)
		{
			CreateDuckWave(1);
			
			// Re-add ai path if not before
			if (!wave.GetComponent<SplineInterpolator>().ReachedQuaterTime)
				Add_AIPath(wave.GetComponent<SplineController>().SplineRoot);
			
			// Remove wave frome list
			DuckWaves.Remove(wave.gameObject);		
		}
	}	
	
	int PickRandom_DuckWave()
	{
		return Random.Range(0, DuckWaveTypes.Count);
	}
	
	GameObject PickRandom_AIPath()
	{
		// Find a new path from selected list
		GameObject path = AI_Paths[Random.Range(0, AI_Paths.Count)];
		// Duplicate that path and set new ducks paths to it
		GameObject newPath = Instantiate(path, Vector3.zero, Quaternion.identity) as GameObject;
		newPath.transform.parent = transform;
		
		AI_Paths.Remove(path);
		AI_PathsUsed.Add(path);
		ScriptHelper.DebugString("Took away " + path.name);
		
		return newPath; 
	}
	
	public void Add_AIPath(GameObject path)
	{
		bool addPath = true;
		for (int i=0; i < AI_Paths.Count; i++)
		{
			if (AI_Paths[i].name == path.name)
			{
				ScriptHelper.DebugString(AI_Paths[i].name + " == " + path.name + "    CANT ADD");		
				addPath = false;
			}
		}
		
		if(addPath)
		{
			ScriptHelper.DebugString("Readded path name: " + path.name);
			AI_Paths.Add(path);
			AI_PathsUsed.Remove(path);
		}
	}
}
