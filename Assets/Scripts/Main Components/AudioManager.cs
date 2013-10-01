using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour 
{
	#region Audio Clips
	[SerializeField] AudioClip Shotgun_Fire;
	[SerializeField] AudioClip Shotgun_Reload;
	[SerializeField] AudioClip Shotgun_Reload_Final;
	[SerializeField] AudioClip Shotgun_Out_Of_Ammo;
	[SerializeField] AudioClip Duck_Call_1;
	[SerializeField] AudioClip Duck_Call_2;
	#endregion
	
	AudioSource audioSource;
	float PitchDecrement = 0.3f;	// Every time gameSpeed is increased by 1, pitch gets reduced by this value 
	
	public Dictionary<int, AudioClip> soundDictionary;
	
	public enum SoundClips
	{
		// Shotgun sounds
		SHOTGUN_FIRE = 0,
		SHOTGUN_RELOAD,
		SHOTGUN_RELOAD_FINAL,
		SHOTGUN_OUT_OF_AMMO,
		
		// Duck sounds
		DUCK_CALL_1,
		DUCK_CALL_2
	}
	
	void Awake()
	{
		// Set up sound dictionary
		soundDictionary = new Dictionary<int, AudioClip>();
		
		// Add audio clips to dictionary
		
		// Shotgun
		soundDictionary.Add((int)SoundClips.SHOTGUN_FIRE, Shotgun_Fire);
		soundDictionary.Add((int)SoundClips.SHOTGUN_RELOAD, Shotgun_Reload);
		soundDictionary.Add((int)SoundClips.SHOTGUN_RELOAD_FINAL, Shotgun_Reload_Final);
		soundDictionary.Add((int)SoundClips.SHOTGUN_OUT_OF_AMMO, Shotgun_Out_Of_Ammo);
		//Duck
		soundDictionary.Add((int)SoundClips.DUCK_CALL_1, Duck_Call_1);
		soundDictionary.Add((int)SoundClips.DUCK_CALL_2, Duck_Call_2);
	}
	
	void Start()
	{
		// Store Audio Source
		audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	public void PlayAudioClip(int key)
	{
		if( soundDictionary.ContainsKey(key) && soundDictionary[key])
			audio.PlayOneShot(soundDictionary[key], 0.75f);
	}
	
	public void ChangePitch(float gameSpeed)
	{
		// Normal pitch value is 1. To make sound pitch decrease (sound slower) reduce value
		// Take normal pitch and subtract the gamespeed value times the decrement value
		float pitch = 1 - (PitchDecrement * (gameSpeed-1));
		audioSource.pitch = pitch;
	}
	
	public void PlayRandom_DuckDeath()
	{
		// Play random duck death sound from preselected list
		int random_int = Random.Range((int)SoundClips.DUCK_CALL_1, (int)SoundClips.DUCK_CALL_2+1);
	
		if( soundDictionary.ContainsKey(random_int) && soundDictionary[random_int])
			audio.PlayOneShot(soundDictionary[random_int], 10.0f);
	}
}
