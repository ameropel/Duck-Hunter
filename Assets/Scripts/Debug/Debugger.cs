using UnityEngine;
using System.Collections;

public class Debugger : MonoBehaviour 
{
	[SerializeField] GameObject Main_Camera;
	
	#region Gui Text compass
	[SerializeField] GUIText Camera_Pitch;
	[SerializeField] GUIText Camera_Yaw;
	[SerializeField] GUIText Camera_Roll;
	#endregion
	
	
	void Update()
	{
		//CameraUpdate();
	}

	void CameraUpdate()
	{
		Camera_Pitch.text = Main_Camera.transform.eulerAngles.x.ToString();
    	Camera_Yaw.text   = Main_Camera.transform.eulerAngles.y.ToString();
		Camera_Roll.text  = Main_Camera.transform.eulerAngles.z.ToString();
	}
}
