using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour 
{
	[SerializeField] GameController sc_GameController;
	[SerializeField] GameObject Head, Body, Right_Wing, Left_Wing;
	[SerializeField] AnimationClip anim_Flying, anim_DeathFly;
	
	float death_time = 5.0f;			// Time till duck can decay
	float decay_time = 2.0f;			// Time duck decays
	bool  death_timer_started;
	[SerializeField] Vector3 velocity;	// Speed at which duck travels
	
	float death_floor = -10;	// If duck is past this point, automatically destroyed
	
	void Start()
	{
		StartCoroutine( DebugForce() );
	}
	
	IEnumerator DebugForce()
	{
		while(sc_GameController.GameState != GameController.GameStatus.PLAYING)
			yield return null;
		
		// Apply force
		rigidbody.AddForce(velocity);
	}
	
	public void Duck_Hit(string object_name)
	{
		// Turn on ducks gravity so it falls out of the sky
		rigidbody.useGravity = true;
		rigidbody.drag = 2.0f;
		
		// Turn death animation on
		gameObject.animation.CrossFade(anim_DeathFly.name);
				
		// Remove all body part colliders
		Destroy(Head.collider);
		Destroy(Body.collider);
		Destroy(Right_Wing.collider);
		Destroy(Left_Wing.collider);
		
		// Turn on main body collider
		gameObject.collider.enabled = true;

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
		
		// Destroy main collider
		Destroy(gameObject.collider);
		
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
}
