using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour 
{
	public bool Resize;
	
	
	[HideInInspector] public Collider buttonCollider;
	[HideInInspector] public bool canUseButton = true;
	[HideInInspector] public bool isHoveringOver = false;
	
	Vector3 currentScale, clickScale;
	float clickReduction = .95f;	// Percent of image still visible	
	float totalClickTime = .2f;
	
	public virtual void Start()
	{
		// Attach gameobjects collider
		buttonCollider = gameObject.collider;
		
		
		Camera _camera = Camera.main;	
		
		// Get camera for hud
		if (Resize)
		{
			// Scale button to screen ratio
			transform.position = ScriptHelper.ScaleToViewport(_camera, 1, transform.position);
		}
		
		// Create click scale
		currentScale = gameObject.transform.localScale;
		clickScale = new Vector3(currentScale.x * clickReduction, 
								 currentScale.y * clickReduction, 
								 currentScale.z * clickReduction);
	}
	
	public virtual void PerfromTransition()
	{	
	}
	
	public virtual void PerformAfterClick()
	{
	}
	
	#region Button Click Type
	public void SingleButtonClick()
	{
		StartCoroutine( ButtonClicked() );	
	}
	
	IEnumerator ButtonClicked()
	{
		canUseButton = false;
		
		float time = 0;
		while(time < 1)
		{
			time += Time.deltaTime / (totalClickTime/2);
			gameObject.transform.localScale = Vector3.Lerp(currentScale, clickScale, time);
			yield return null;
		}
			
		time = 0;
		while(time < 1)
		{
			time += Time.deltaTime / (totalClickTime/2);
			gameObject.transform.localScale = Vector3.Lerp(clickScale, currentScale, time);
			yield return null;
		}
		
		PerformAfterClick();
		
		canUseButton = true;
	}
	
	#endregion
}
