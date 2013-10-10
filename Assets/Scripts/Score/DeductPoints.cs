using UnityEngine;
using System.Collections;

public class DeductPoints : MonoBehaviour 
{
	[SerializeField] TextMesh PointsText;
	private float fade_time = 1.0f;
	
	void Start()
	{	
		// Fade color out
		StartCoroutine( FadeColor() );
	}

	
	IEnumerator FadeColor()
	{
		float alpha = 0;
		float time = 0;
		Color c = PointsText.renderer.material.color;
		while (time < 1)
		{
			time += Time.deltaTime / fade_time;
			alpha = Mathf.Lerp(1,0,time);
			PointsText.renderer.material.color = new Color(c.r, c.g, c.b, alpha);
			yield return null;
		}
		
		// Destroy guiText
		Destroy(gameObject);
	}
}