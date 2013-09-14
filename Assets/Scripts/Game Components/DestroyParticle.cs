using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour 
{
	float time = 0.5f;
	
	void Start()
	{
		Destroy(gameObject, time);
	}
}
