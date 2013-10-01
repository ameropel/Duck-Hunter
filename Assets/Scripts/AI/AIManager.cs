using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIManager : MonoBehaviour 
{
	// Script Holder
	[SerializeField] AudioManager   sc_AudioManager;
	[SerializeField] GameController sc_GameController;
	[SerializeField] GameTimer 		sc_GameTimer;
	
	[SerializeField] Transform BirdWave;
	GameObject GraveYard;
	GameObject duckWave_One, duckWave_Two, duckWave_Three;
	GameObject gooseWave_One;
	float timeBetweenWaves = 15;	// Time to launch a new wave
	float aiPath_Duration = 40;		// Time it takes bird to reach point a to b
	
	List<GameObject> BirdWaveTypes = new List<GameObject>();							// Holds bird types
	[HideInInspector] public List<GameObject> BirdWaves = new List<GameObject>();		// Holds bird waves
	[HideInInspector] public List<GameObject> AI_Paths = new List<GameObject>();		// Holds ai paths birds can take
	[HideInInspector] public List<GameObject> AI_PathsUsed = new List<GameObject>();	// Holds ai paths birds are using
	
	void Start()
	{
		// Find Graveyard
		GraveYard = GameObject.Find("GraveYard");
		
		// Get all duck waves
		duckWave_One = Resources.Load("Prefabs/Targets/Birds/Ducks/duckWave_One") as GameObject;
		duckWave_Two = Resources.Load("Prefabs/Targets/Birds/Ducks/duckWave_Two") as GameObject;
		duckWave_Three = Resources.Load("Prefabs/Targets/Birds/Ducks/duckWave_Three") as GameObject;
		gooseWave_One = Resources.Load("Prefabs/Targets/Birds/Geese/gooseWave_One") as GameObject;
		
		// Get all possible ai paths birds can take
		Object[] All_AI_Paths = Resources.LoadAll("Prefabs/AI/Paths", typeof(GameObject));

		// Get all duckwaves
		BirdWaveTypes.Add(duckWave_One);
		BirdWaveTypes.Add(duckWave_Two);
		BirdWaveTypes.Add(duckWave_Three);
		// Get all other birds
		BirdWaveTypes.Add(gooseWave_One);
		
		// Get all ai paths ducks can follow	
		for (int i=0; i < All_AI_Paths.Length; i++)
		{
			GameObject path = All_AI_Paths[i] as GameObject;
			AI_Paths.Add(path);
		}
		
		// Start timer		
		StartCoroutine( WaveTimer() );
	}
	
	IEnumerator WaveTimer()
	{
		if (sc_GameController.GameState != GameController.GameStatus.PLAYING)
				yield return null;
		
		// Create two waves
		CreateBirdWave(2);
		
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
		
		if (sc_GameTimer.CountDownTimer > aiPath_Duration)
		{
			// Restart wave timer
			StartCoroutine( WaveTimer() );
		}
	}
	
	
	void CreateBirdWave(int waveCount)
	{		
		for (int i = 0; i < waveCount; i++)
		{	
			// Instantiate and add wave to list
			GameObject wave = Instantiate( BirdWaveTypes[PickRandom_BirdWave()], BirdWave.position, Quaternion.identity) as GameObject;
			wave.transform.parent = BirdWave;
			SetupBirdWave(wave, PickRandom_AIPath());
			BirdWaves.Add(wave);
			wave.SetActive(true);
		}
	}
	
	void SetupBirdWave(GameObject wave, GameObject path)
	{
		// Attach spline path and duration time
		SplineController   spline = wave.GetComponent<SplineController>();
		spline.SplineRoot = path;
		spline.Duration   = aiPath_Duration;
		
		SplineInterpolator interp = wave.GetComponent<SplineInterpolator>();
		interp.sc_AIManager      = this;
		interp.sc_GameController = sc_GameController;
		interp.sc_GameTimer      = sc_GameTimer;
		
		// Get bird in each wave and attach components
		foreach(Transform child in wave.transform)
		{
			Bird bird = child.GetComponent<Bird>();
			bird.sc_AIManager      = this;
			bird.sc_AudioManager   = sc_AudioManager;
			bird.sc_GameController = sc_GameController;
			bird.GraveYard = GraveYard;
		}
	}
	
	public void RemoveBirdFromWave( GameObject bird )
	{
		Transform wave = bird.transform.parent;
		
		int childCount = wave.childCount;
		
		// Create another wave if it is the last bird in wave
		if (childCount == 1)
		{
			CreateBirdWave(1);
			
			// Re-add ai path if not before
			if (!wave.GetComponent<SplineInterpolator>().ReachedQuaterTime)
				Add_AIPath(wave.GetComponent<SplineController>().SplineRoot);
			
			// Remove wave frome list
			BirdWaves.Remove(wave.gameObject);
			Destroy(wave.gameObject);
		}
	}	
	
	int PickRandom_BirdWave()
	{
		return Random.Range(0, BirdWaveTypes.Count);
	}
	
	GameObject PickRandom_AIPath()
	{
		// Find a new path from selected list
		GameObject path = AI_Paths[Random.Range(0, AI_Paths.Count)];
		// Duplicate that path and set new birds paths to it
		GameObject newPath = Instantiate(path, Vector3.zero, Quaternion.identity) as GameObject;
		newPath.transform.parent = transform;
		
		AI_Paths.Remove(path);
		AI_PathsUsed.Add(path);
		ScriptHelper.DebugString("Took away " + newPath.name);
		
		return newPath; 
	}
	
	public void Add_AIPath(GameObject path)
	{
		int pathId = path.GetComponent<ai_BirdPath>().Path_Id;
		
		bool addPath = true;
		for (int i=0; i < AI_Paths.Count; i++)
		{
			if (AI_Paths[i].GetComponent<ai_BirdPath>().Path_Id == pathId)
			{
				addPath = false;
				break;
			}
		}
		
		if(addPath)
		{
			for (int i=0; i < AI_PathsUsed.Count; i++)
			{
				if (AI_PathsUsed[i].GetComponent<ai_BirdPath>().Path_Id == pathId)
				{
					ScriptHelper.DebugString("Readded path name: " + path.name);
					AI_Paths.Add(AI_PathsUsed[i]);
					AI_PathsUsed.Remove(AI_PathsUsed[i]);
					break;
				}
			}
				
		}
	}
}
