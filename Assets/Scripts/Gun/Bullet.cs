using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	float velocity = 1500;
	float death_time = 5;
	
	int max_distance = 100;
	Vector3 start_pos;
	Vector3 end_pos;

	void Start()
	{
		start_pos = transform.position;
		end_pos = start_pos + (transform.forward.normalized * max_distance);
		Debug.DrawLine(start_pos, end_pos, Color.green, death_time);
	
		StartCoroutine( Bullet_Death_Timer() );
		rigidbody.AddForce(transform.forward * velocity);
	}
			
	IEnumerator Bullet_Death_Timer()
	{
		float time = 0;
		while (time < 1)
		{
			time += Time.deltaTime / death_time;
			yield return null;
		}
		
		DestroyImmediate(gameObject);
	}
}
