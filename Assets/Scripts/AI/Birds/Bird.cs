using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour 
{
	public enum BirdType
	{	DUCK = 0, GOOSE	}
	public BirdType TypeofBird;
	
	// Script holder
	[HideInInspector] public AIManager      sc_AIManager;
	[HideInInspector] public AudioManager   sc_AudioManager;
	[HideInInspector] public GameController sc_GameController;
	
	[HideInInspector] public GameObject GraveYard;
	GameObject Head, Body, Right_Wing, Left_Wing;
	[SerializeField] AnimationClip anim_Flying, anim_DeathFly;
	[HideInInspector] public bool InGroup;
	AudioManager.SoundClips DeathSound;
	
	float death_time = 5.0f;	// Time till bird floats on water dead
	float decay_time = 2.0f;	// Time bird decays to grave
	float death_speed = 150.0f;	
	float death_floor = -10;	// If bird is past this point, automatically destroyed
	bool  death_timer_started;
	
	void Start()
	{
		// Take note if the bird is flying in a pack or a lone wolf.
		// This will be used later to tell if it should remove spline script from gameObject
		// or if should remove gameObject from group of birds
		if (transform.parent.tag == "Bird Group")
			InGroup = true;
		
		// Setup birds death sound
		if (TypeofBird == BirdType.DUCK)
			DeathSound = AudioManager.SoundClips.DUCK_CALL_2;
		else if (TypeofBird == BirdType.GOOSE)
			DeathSound = AudioManager.SoundClips.GOOSE_CALL_1;
		
		// Find birds body parts
		Head = transform.FindChild("Head").gameObject;
		Body = transform.FindChild("Body").gameObject;
		Right_Wing = transform.FindChild("Right_Wing").gameObject;
		Left_Wing = transform.FindChild("Left_Wing").gameObject;
		
		
	}
	
	public void Bird_Hit(string object_name, Vector3 hitPoint)
	{			
		// Store the wave gameobject before giveing bird a new parent
		GameObject wave = transform.parent.gameObject;
		
		// Add bird to graveyard
		transform.parent = GraveYard.transform;
		
		// Play bird death sound
		sc_AudioManager.PlayAudioClip((int)DeathSound);
		
		// Turn on birds gravity so it falls out of the sky
		rigidbody.useGravity = true;
		
		// Turn death animation on
		gameObject.animation.CrossFade(anim_DeathFly.name);
		
		// Rotate bird where bullet was hit
		rigidbody.AddTorque(hitPoint * death_speed);

		// Remove all body part colliders
		Destroy(Head.collider);
		Destroy(Body.collider);
		Destroy(Right_Wing.collider);
		Destroy(Left_Wing.collider);
		
		// Turn on main body collider
		gameObject.collider.enabled = true;
		
		// Apply a death force
		rigidbody.AddForce( transform.right * death_speed);
		
		// Remove the bird from the wave
		sc_AIManager.RemoveBirdFromWave(wave);
	}
	
	void Update()
	{
		DetectBoundaries();
		
		if (sc_GameController.GameState == GameController.GameStatus.PAUSED)
		{
			foreach (AnimationState state in animation)
				state.speed = 0.0f;
		}
		else
		{
			foreach (AnimationState state in animation)
				state.speed = 1.0f;
		}
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
		
		// If bird is beneath death_floor, destroy
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
		
		// Position and rotate dead bird correctly
		StartCoroutine( PositionDeadBird() );
		
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
		
		// Destroy bird
		Destroy(gameObject);
	}

	IEnumerator PositionDeadBird()
	{
		Quaternion rot   = transform.rotation;
		Vector3 pos      = transform.position;
		Vector3 deathPos = new Vector3( pos.x, 0, pos.z);
		float positioning_time = 0.25f;
		
		
		float time = 0;
		while (time < 1)
		{
			time += Time.deltaTime / positioning_time;
			
			// Rotate and position bird so its flat on water
			transform.rotation = Quaternion.Lerp(rot, Quaternion.LookRotation(transform.forward), time);
			transform.position = Vector3.Lerp(pos, deathPos, time);
			
			yield return null;
		}
	}			
}
