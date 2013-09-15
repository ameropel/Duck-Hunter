using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour 
{			
	void Update()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		CheckButtonHit_Android();
		#else
		CheckButtonHit_Editor();
		#endif
	}

	void CheckButtonHit_Android()
	{
		if(Input.touchCount > 0)
		{			
		    for(int i=0; i<Input.touchCount; i++)
			{
				Vector3 mousePos = Input.GetTouch(i).position;
				RaycastHit hit;
				Button button = null;
				Slider slider = null;
				Toggle toggle = null;
				Scroll scroll = null;	GameObject arrow = null;
				MouseHold hold = null;
				
				// Check to see if mouse hit collider object
				if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit))
				{
					// Add button
					button = hit.collider.GetComponent<Button>();
					
					// Add scroll
					if (hit.collider.transform.parent)
						scroll = hit.collider.transform.parent.GetComponent<Scroll>();
					arrow = hit.collider.gameObject;
					
					// Add toggle
					toggle = hit.collider.transform.GetComponent<Toggle>();
					
					// Add slider
					if (hit.collider.transform.parent)
						slider = hit.collider.transform.parent.GetComponent<Slider>();
					
					// Add hold
					hold = hit.collider.GetComponent<MouseHold>();
				}
				
				// Get Mouse Input
				// If mouse button clicked
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{
					// Check to see if hit a button
					if (button != null)
						button.PerfromTransition();
					
					
					// Check to see if hit a toggle
					if (toggle != null)
						toggle.PerfromTransition();
				}
				
				// If mouse button held down
				if (Input.GetTouch(i).phase == TouchPhase.Began ||
					Input.GetTouch(i).phase == TouchPhase.Moved ||
					Input.GetTouch(i).phase == TouchPhase.Stationary)
				{
					// Check to see if hit a slider
					if (slider != null)
						slider.UpdateNodePosition(mousePos.x);
					
					// Check to see if hit a scroll button
					if (scroll != null)
						scroll.PerfromTransition(arrow);
					
					// Check to see if hit a scroll button
					if (hold != null)
						hold.PerfromTransition();
				}
		    }
		}
		
	}
	
	void CheckButtonHit_Editor()
	{
		
		Vector3 mousePos = Input.mousePosition;
		RaycastHit hit;
		Button button = null;
		Slider slider = null;
		Toggle toggle = null;
		Scroll scroll = null;	GameObject arrow = null;
		MouseHold hold = null;
		
		// Check to see if mouse hit collider object
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit))
		{
			// Add button
			button = hit.collider.GetComponent<Button>();
			
			// Add scroll
			if (hit.collider.transform.parent)
				scroll = hit.collider.transform.parent.GetComponent<Scroll>();
			arrow = hit.collider.gameObject;
			
			// Add toggle
			toggle = hit.collider.transform.GetComponent<Toggle>();
			
			// Add slider
			if (hit.collider.transform.parent)
				slider = hit.collider.transform.parent.GetComponent<Slider>();
			
			// Add hold
			hold = hit.collider.GetComponent<MouseHold>();
		}
		
		// Get Mouse Input
		// If mouse button clicked
		if (Input.GetMouseButtonDown(0))
		{
			// Check to see if hit a button
			if (button != null)
				button.PerfromTransition();
			
			
			// Check to see if hit a toggle
			if (toggle != null)
				toggle.PerfromTransition();
		}
		
		// If mouse button held down
		if (Input.GetMouseButton(0))
		{
			// Check to see if hit a slider
			if (slider != null)
				slider.UpdateNodePosition(mousePos.x);
			
			// Check to see if hit a scroll button
			if (scroll != null)
				scroll.PerfromTransition(arrow);
			
			// Check to see if hit a scroll button
			if (hold != null)
				hold.PerfromTransition();
		}
	}
}