using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour 
{
	public bool OneTimeUse;
	
	
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
		
		// Get camera for hud
		Camera _camera = Camera.main;	
		
		// Scale button to screen ratio
		transform.position = ScriptHelper.ScaleToViewport(_camera, 1, transform.position);
		
		// Create click scale
		currentScale = gameObject.transform.localScale;
		clickScale = new Vector3(currentScale.x * clickReduction, 
								 currentScale.y * clickReduction, 
								 currentScale.z * clickReduction);
	}
	
	public virtual void PerfromTransition()
	{	
		// If button can only be used once check if button was already hit
		// if hit skip the rest of the function
		if (OneTimeUse)
			buttonCollider.enabled = false;  // Button was used, deactivate collider
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
