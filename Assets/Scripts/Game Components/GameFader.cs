using UnityEngine;
using System.Collections;

public class GameFader : MonoBehaviour 
{	
	GUITexture Fade;
	[HideInInspector] public float Fade_time = 1.5f;	// Time it takes to fade from color1 to color2
	
	Color TransBlack = new Color(.5f,.5f,.5f,.35f);
	
	void Start()
	{
		Fade = gameObject.guiTexture;	// Set Fade to be gameObjects guiTexture
		
		// Make guiTexture fit screen coordinates
		Fade.pixelInset = new Rect(-Screen.width/2, -Screen.height/2, Screen.width, Screen.height);
		Fade.color = TransBlack;	// Make guiTexture at start to be TransBlack
	}
	
	// Fade guiTexture from clear to black
	public void Fade_To_Black()
	{
		StartCoroutine( FadeColor(Color.clear, TransBlack) );
	}
	
	// Fade guiTexture from black to clear
	public void Fade_To_Clear()
	{
		StartCoroutine( FadeColor(TransBlack, Color.clear) );
	}
	
	// Fade guiTexture to color
	IEnumerator FadeColor( Color c_from, Color c_to)
	{
		Fade.color = c_from;	// Start guiTexture as the color fading from
		
		float time = 0;
		while(time < 1)
		{
			// Calculate timer
			time += Time.deltaTime / Fade_time;
			
			// Fade color: c_from to c_to in fade_time seconds
			Fade.color = Color.Lerp(c_from, c_to, time);
			yield return null;
		}
	}
}
