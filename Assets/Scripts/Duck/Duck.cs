using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour 
{
	// Script holder
	[HideInInspector] public AIManager      sc_AIManager;
	[HideInInspector] public AudioManager   sc_AudioManager;
	[HideInInspector] public GameController sc_GameController;
	
	[HideInInspector] public GameObject GraveYard;
	[SerializeField] GameObject Head, Body, Right_Wing, Left_Wing;
	[SerializeField] AnimationClip anim_Flying, anim_DeathFly;
	
	float death_time = 5.0f;	// Time till duck floats on water dead
	float decay_time = 2.0f;	// Time duck decays to grave
	bool  death_timer_started;
	float death_speed = 150.0f;	
	float death_floor = -10;	// If duck is past this point, automatically destroyed
	[HideInInspector] public bool InGroup;
	
	void Start()
	{
		// Take note if the duck is flying in a pack or a lone wolf.
		// This will be used later to tell if it should remove spline script from gameObject
		// or if should remove gameObject from group of ducks
		if (transform.parent.tag == "Duck Wave")
			InGroup = true;
	}
	
	public void Duck_Hit(string object_name, Vector3 hitPoint)
	{	
		// Remove the duck from the wave
		sc_AIManager.RemoveDuckFromWave(gameObject);
		
		// Add duck to graveyard
		transform.parent = GraveYard.transform;
		
		// Play duck death sound
		//sc_AudioManager.PlayRandom_DuckDeath();
		sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.DUCK_CALL_2);
		
		// Turn on ducks gravity so it falls out of the sky
		rigidbody.useGravity = true;
		
		// Rotate duck where bullet was hit
		rigidbody.AddTorque(hitPoint * death_speed);

		// Turn death animation on
		gameObject.animation.CrossFade(anim_DeathFly.name);
				
		// Remove all body part colliders
		Destroy(Head.collider);
		Destroy(Body.collider);
		Destroy(Right_Wing.collider);
		Destroy(Left_Wing.collider);
		
		// Turn on main body collider
		gameObject.collider.enabled = true;
		
		// Apply a death force
		rigidbody.AddForce( transform.right * death_speed);
	}
	
	void Update()
	{
		DetectBoundaries();
	}
	
	void OnCollisionEnter( Collision hit)
	{
		// If death timer has started, stop collision detection
		if (death_timer_started)
			return;
		
		// Find out when can the death timer start
		if (hit.collider.transform.name == "Water" ||
			hit.collider.transform.name == "Terrain")
			StartCoroutine( Death_Timer() );
	}
	
	void DetectBoundaries()
	{
		// If death timer has started, stop collision detection
		if (death_timer_started)
			return;	
		
		// If duck is beneath death_floor, destroy
		if (transform.position.y < death_floor)
			Destroy(gameObject);
	}
	
	IEnumerator Death_Timer()
	{		
		// Death timer has started
		death_timer_started = true;
		
		// Turn gravity off and make object kinematic
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
		
		// Instantiate water_splash
		Instantiate(Resources.Load("Prefabs/Particles/water_splash_big"), 
			new Vector3(transform.position.x, 0.1f, transform.position.z),
			Quaternion.Euler(new Vector3(-90,0,0)) );
		
		// Destroy main collider
		Destroy(gameObject.collider);
		
		// Position and rotate dead duck correctly
		StartCoroutine( PositionDeadDuck() );
		
		// Calculate death timer
		float time = 0;
		while (time < 1)
		{
			time += Time.deltaTime / death_time;
			yield return null;
		}
		
		Vector3 currentPos = transform.position;
		Vector3 finalPos = new Vector3( transform.position.x, 
										transform.position.y - 1, 
										transform.position.z);
		
		// Delay time for animation till destroy object completely
		time = 0;
		while (time < 1)
		{
			time += Time.deltaTime / decay_time;
			
			transform.position = Vector3.Lerp(currentPos, finalPos, time);
			yield return null;
		}
		
		// Destroy Duck
		Destroy(gameObject);
	}

	IEnumerator PositionDeadDuck()
	{
		Quaternion rot   = transform.rotation;
		Vector3 pos      = transform.position;
		Vector3 deathPos = new Vector3( pos.x, 0, pos.z);
		float positioning_time = 0.25f;
		
		
		float time = 0;
		while (time < 1)
		{
			time += Time.deltaTime / positioning_time;
			
			// Rotate and position duck so its flat on water
			transform.rotation = Quaternion.Lerp(rot, Quaternion.LookRotation(transform.forward), time);
			transform.position = Vector3.Lerp(pos, deathPos, time);
			
			yield return null;
		}
	}			
}
