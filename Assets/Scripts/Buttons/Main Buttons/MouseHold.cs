using UnityEngine;
using System.Collections;

public abstract class MouseHold : MonoBehaviour 
{
	
	public bool Resize;
		
	public virtual void Start()
	{
		
		Camera _camera = Camera.main;	
		
		// Get camera for hud
		if (Resize)
		{
			// Scale button to screen ratio
			transform.position = ScriptHelper.ScaleToViewport(_camera, 1, transform.position);
		}
	}
	
	public virtual void PerfromTransition()
	{
	}

}

