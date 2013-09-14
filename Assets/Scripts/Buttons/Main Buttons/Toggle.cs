using UnityEngine;
using System.Collections;

public abstract class Toggle: MonoBehaviour 
{	
	public bool OneTimeUse;
	public bool toggleOn;
		
	[HideInInspector] public Collider buttonCollider;
	[HideInInspector] public bool canUseButton = true;
	
	Vector3 currentScale, clickScale;
	float clickReduction = .95f;	// Percent of image still visible	
	float totalClickTime = .2f;
	
	public virtual void Start()
	{
		// Attach gameobjects collider
		buttonCollider = gameObject.collider;
		
		// Create click scale
		currentScale = gameObject.transform.localScale;
		clickScale = new Vector3(currentScale.x * clickReduction, 
								 currentScale.y * clickReduction, 
								 currentScale.z * clickReduction);
		
		// Default toggle is on
		toggleOn = true;
	}
	
	public virtual void PerfromTransition()
	{	
		// If button can only be used once check if button was already hit
		// if hit skip the rest of the function
		if (OneTimeUse)
			buttonCollider.enabled = false;  // Button was used, deactivate collider
		
		// Switch Toggle
		toggleOn = !toggleOn;
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
