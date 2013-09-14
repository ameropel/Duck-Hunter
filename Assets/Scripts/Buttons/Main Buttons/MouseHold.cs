using UnityEngine;
using System.Collections;

public abstract class MouseHold : MonoBehaviour 
{
	public virtual void Start()
	{
		// Get camera for hud
		Camera _camera = GameObject.Find("HUD").gameObject.camera;	
		
		// Scale button to screen ratio
		transform.position = ScriptHelper.ScaleToViewport(_camera, 1, transform.position);
 	}
	
	public virtual void PerfromTransition()
	{
	}

}

