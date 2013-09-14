using UnityEngine;
using System.Collections;

public class GameCountDown : MonoBehaviour 
{
	// Script Holder
	[SerializeField] btn_Start_Game sc_btn_Start_Game;
	[SerializeField] GameFader		sc_GameFader;
	
	[SerializeField] GameObject Press_To_Start;
	[SerializeField] GameObject CountDown_3;
	[SerializeField] GameObject CountDown_2;
	[SerializeField] GameObject CountDown_1;
	
	[HideInInspector] public bool GameCounterCompleted;
	
	float time_delay = 1;
	
	void Start()
	{
		Press_To_Start.SetActive(true);
		StartCoroutine( CoutDown_Timer() );
	}
	
	IEnumerator CoutDown_Timer()
	{
		// Wait for start button to be pressed
		while(!sc_btn_Start_Game.GameStart)
			yield return null;
		
		// Play start button animation and wait for it to finish plus an extra .5 seconds
		Press_To_Start.animation.Play();
		yield return new WaitForSeconds(1.5f);	
		
		// Start countdown timer
		yield return StartCoroutine ( CountDown(CountDown_3) );
		yield return StartCoroutine ( CountDown(CountDown_2) );
		yield return StartCoroutine ( CountDown(CountDown_1) );
		
		sc_GameFader.Fade_To_Clear();
		yield return new WaitForSeconds(sc_GameFader.Fade_time);
		
		DestroyImmediate(sc_btn_Start_Game.gameObject);
		
		GameCounterCompleted = true;
	}
	
	IEnumerator CountDown( GameObject count )
	{
		count.SetActive(true);
		float time = 0;
		while (time < 1)
		{
			time += Time.deltaTime / time_delay;
			yield return null;
		}
		DestroyImmediate(count);
	}
}
