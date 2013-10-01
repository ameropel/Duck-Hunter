using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	[HideInInspector] public GameScore sc_GameScore;
	
	public float velocity = 3000;
	float death_time = 5;
	bool hit_object;
	
	int max_distance = 100;
	Vector3 start_pos;
	Vector3 end_pos;
	
	void Start()
	{
		start_pos = new Vector3(transform.position.x, transform.position.y + .01f, transform.position.z);
		end_pos = start_pos + (transform.forward.normalized * max_distance);
		
		// Debug draw a line to display bullets path in editor
		#if UNITY_EDITOR
		Debug.DrawLine(start_pos, end_pos, Color.green, death_time);
		#endif
		
		// Create raycast for bullet
		Bullet_RayCast();
		
		// Start death timer
		StartCoroutine( Bullet_Death_Timer() );
		
		// Project bullet forward at velocity speed
		rigidbody.AddForce(transform.forward * velocity);
	}

	void Bullet_RayCast ()
	{
		RaycastHit hit;
		Bird bird = null;
		bool hitObject = false;
		
		// Check to see if mouse hit collider object
		if (Physics.Raycast(start_pos, end_pos, out hit) )
		{	
			if (hit.collider.name == "Water")
			{
				// Instantiate water_splash
				Instantiate(Resources.Load("Prefabs/Particles/water_splash"), 
							new Vector3(hit.point.x, 0.1f, hit.point.z),
							Quaternion.Euler(new Vector3(-90,0,0)) );
			}
			// If the gameobject that has been hit does not contain a parent, ignore
			else if (hit.collider.transform.parent)
			{
				// If the hit gameObjects parents name is bird
				if (hit.collider.transform.parent.tag == "Bird")
				{				
					// Tell bird that it has been hit by a bullet
					bird = hit.collider.transform.parent.GetComponent<Bird>();
					bird.Bird_Hit(hit.collider.gameObject.name, hit.point);
					
					// Hit point valued object
					hitObject = true;
					
					// Determine scoring on what bird player hit
					Bird.BirdType birdHit = bird.TypeofBird;
					
					// Hit Duck
					if (birdHit == Bird.BirdType.DUCK)
						sc_GameScore.ChangeScore(GameScore.ObjectHit.DUCK);
					// Hit Goose
					else if (birdHit == Bird.BirdType.GOOSE)
						sc_GameScore.ChangeScore(GameScore.ObjectHit.GOOSE);
					
					// Destroy bullet, its duty has been fulfilled
					Destroy(gameObject);
				}
			}
			
		}
		
		// Did not hit anything worthy, deduct hit miss
		if (!hitObject)
			sc_GameScore.ChangeScore(GameScore.ObjectHit.MISS);
	}
			
	IEnumerator Bullet_Death_Timer()
	{
		// Wait until timer is up then destroy bullet
		float time = 0;
		while (time < 1)
		{
			time += Time.deltaTime / death_time;
			yield return null;
		}
		
		// Destroy the bullet immediately
		Destroy(gameObject);
	}
	
	/*
	void OnCollisionEnter( Collision hit)
	{	
		// If already hit an object ignore the rest of function
		if (hit_object)
			return;
		
		// Hit something so destroy model of bullet
		Destroy(transform.GetChild(0).gameObject);
		
		if (hit.collider.transform.name == "Water")
		{
			// Instantiate water_splash
			Instantiate(Resources.Load("Prefabs/Particles/water_splash"), 
						new Vector3(transform.position.x, 0.1f, transform.position.z),
						Quaternion.Euler(new Vector3(-90,0,0)) );
		}
		
		// If the gameobject that has been hit does not contain a parent, ignore
		if (!hit.collider.transform.parent)
			return;
		
		// If the hit gameObjects parents name is duck
		if (hit.collider.transform.parent.name == "duck")
		{
			// Bullet has hit an object
			hit_object = true;
			
			// Tell duck that it has been hit by a bullet
			hit.collider.transform.parent.GetComponent<Duck>().Duck_Hit(hit.collider.gameObject.name);
			
			// Destroy the bullet
			Destroy(gameObject);	
		}	
	}
	*/
}
