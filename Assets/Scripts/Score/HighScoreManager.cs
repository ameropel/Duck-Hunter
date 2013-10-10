using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;


public class HighScoreManager : MonoBehaviour 
{
	const int MAX_SCORES = 6;
	public bool ResetToDefault;
	
	[SerializeField] TextMesh[] UserNames  = new TextMesh[MAX_SCORES];
	[SerializeField] TextMesh[] UserScores = new TextMesh[MAX_SCORES];
	
	private int[]    lb_scores = new int[MAX_SCORES];
	private string[] lb_names = new string[MAX_SCORES]; 
	
	void Start()
	{	
		// If list is empty, means new game, setup default list
		string checklist = 	GetName(0);
		if (checklist == null || ResetToDefault)
			InitalizeList();
				
		// If in main menu then add correct text to the leaderboards
		if (Application.loadedLevelName == "Menu")
			SetupText();
		
		// Set arrays up with scores and names
		for (int i = 0; i < MAX_SCORES; i++)
		{
			// Set leaderboards
			lb_scores[i] = GetScore(i);
			lb_names[i] = GetName(i);
		}
	}
	
	void InitalizeList()
	{
		// Default List
		// Set default scores and names		
		DefaultSetup(0, 50000, "KAT");
		DefaultSetup(1, 40000, "SHA");
		DefaultSetup(2, 30000, "MAL");
		DefaultSetup(3, 20000, "JON");
		DefaultSetup(4, 10000, "LIL");
		DefaultSetup(5, 5000,  "STA");
		
		Debug.Log("Default Names & Scores");
	}
	void SetupText ()
	{
		// Set up text meshes in scene to proper scores
		for (int i = 0; i < MAX_SCORES; i++)
		{
			UserNames[i].text  = GetName(i);
			UserScores[i].text = GetScore(i).ToString();
		}
	}
	private void DefaultSetup(int recordIndex, int score, string name)
	{
		PlayerPrefs.SetInt(("leader_score_" + recordIndex).ToString(), score);
		PlayerPrefs.SetString(("leader_name_" + recordIndex).ToString(), name);
	}
	
	
	// Getters
	int GetScore(int recordIndex)
	{
		// Return score at given index
	    return PlayerPrefs.GetInt(("leader_score_" + recordIndex).ToString());
	}
	string GetName(int recordIndex)
	{
		// Return name at given index
		return PlayerPrefs.GetString(("leader_name_" + recordIndex).ToString());
	}   
	 
	// Setters
	void SetOldScore(int recordIndex, int score, string name)
	{
		// Leave all players that should be on the leaderboards,
		// just move the list down by one to fit new score.
		// Rerecord scores
	    PlayerPrefs.SetInt(("leader_score_" + recordIndex).ToString(), score);
		PlayerPrefs.SetString(("leader_name_" + recordIndex).ToString(), name);
	}
	
	IEnumerator SetNewScore(int recordIndex, int score)
	{	
		// Get the players name so it can be added to the leaderboards
		string name = "";
		#if UNITY_ANDROID && !UNITY_EDITOR
		TouchScreenKeyboard keyboard;

		while (true)
		{
			keyboard = TouchScreenKeyboard.Open(name, TouchScreenKeyboardType.NamePhonePad, false, false, false, false, "Enter Four Initials");
			while (keyboard.active) 
				yield return null;
			
			name = keyboard.text;
		
			if (name != "" && name.Length <= 4)
				break;
		}
		#else
		name = "NAME";
		
		#endif
		
		// Record name and score
		PlayerPrefs.SetInt(("leader_score_" + recordIndex).ToString(), score);
		PlayerPrefs.SetString(("leader_name_" + recordIndex).ToString(), name);
		
		yield return null;
	}
	
	bool isHighScore(int score)
	{
		// If score is greater than or equal to top score,
		// it is the new high score return true to celebrate
		if (score >= lb_scores[0]) return true;
		else return false;
	}
	
	public void SaveScore()
	{
		int score = gameObject.GetComponent<GameScore>().PlayerScore;
		
		// If high score celebrate
		if (isHighScore(score))
		{
			// Celebrate	
		}
		
		// If score does not beat last ignore rest
		if (score < lb_scores[MAX_SCORES-1]) return;
		
		for (int i=0; i < MAX_SCORES; i++)
		{
			if (score >= lb_scores[i])
			{				
				// Rearrange the leaderboard
				ResetLeaderboards(i, score);
				break;		
			}
		} 
	}
	
	void ResetLeaderboards(int index, int score)
	{
		// Set Players Score
		StartCoroutine( SetNewScore(index, score) );
		
		// Set the rest of the scores
		for (int i = index+1; i < MAX_SCORES; i++)
			SetOldScore(i, lb_scores[i-1], lb_names[i-1]);
	}
}
