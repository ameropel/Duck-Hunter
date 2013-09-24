using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	// Script Holder
	#if UNITY_ANDROID && !UNITY_EDITOR
	Android_Accelerometer sc_Android_Accelerometer;
	Android_Compass 	  sc_Android_Compass;
	#endif
	[SerializeField] GameController sc_GameController;
		
	// Calculate pitch
	float pitch = 0;
	float old_pitch, new_pitch, current_pitch, starting_pitch;
	float pitch_angle_max = 60;
	float pitch_angle_min = -20;
	#if UNITY_ANDROID && !UNITY_EDITOR
	float pitch_angle_range = 0;
	#endif
	float camera_pitch_drag = 0.55f;	// Time it takes for user to look up and down (drag value)
	
	// Calculate yaw
	float yaw = 0;
	float old_yaw, new_yaw, current_yaw, starting_yaw, yaw_max, yaw_min;
	float yaw_angle_max = 110;
	float yaw_angle_min = -110;
	float camera_yaw_drag = 0.65f;	// Time it takes for user to look left to right (drag value)
	
	
	void Awake()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		// Find main android game component. This gameobject has all android related scripts attached to it.
		GameObject Android_Component = GameObject.FindGameObjectWithTag("Android Component").gameObject;
		
		// Attach scripts
		sc_Android_Accelerometer   = Android_Component.GetComponent<Android_Accelerometer>();
		sc_Android_Compass = Android_Component.GetComponent<Android_Compass>();
		#endif
	}
	
	void Start()
	{
		// Tell screen to not dim
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		
		// Set current pitch to 360 degrees
		current_pitch = 360;
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		pitch_angle_range = (pitch_angle_max * 2);
		
		// Wait for compass to be initialized
		StartCoroutine ( WaitForCompass() );
		#endif
	}
	
	IEnumerator WaitForCompass()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		// Wait until android compass is active
		while(sc_GameController.GameState != GameController.GameStatus.PLAYING)
			yield return null;
		
		starting_yaw = (( (sc_Android_Compass.Yaw - sc_Android_Compass.Yaw_Min)
						* 360) / sc_Android_Compass.Yaw_Range) + 0;
		
		yaw_max = starting_yaw + yaw_angle_max;
		yaw_min = starting_yaw + yaw_angle_min; 
		#endif
		
		yield return null;
	}
	
	void Update()
	{		
		if (sc_GameController.GameState == GameController.GameStatus.PLAYING)
		{
			#if UNITY_EDITOR
			GetCameraInput();
			#endif
			CameraDragUpdate();
		}
	}
	
	void GetCameraInput()
	{
		// Get arrow input. Only used in editor to get camera rotation
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			if (yaw >= yaw_angle_min)
				yaw -= 1;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			if (yaw <= yaw_angle_max)
				yaw += 1;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			if (pitch <= pitch_angle_max)
				pitch += 1;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			if (pitch >= pitch_angle_min)
				pitch -= 1;
		}
	}
	
	#if UNITY_ANDROID && !UNITY_EDITOR
	float Get_Pitch()
	{
		// Get pitch by finding its accelerometer value, and compare it to the max value.
		pitch = (( (sc_Android_Accelerometer.Pitch - sc_Android_Accelerometer.Pitch_Min)
				* pitch_angle_range) / sc_Android_Accelerometer.Pitch_Range) - pitch_angle_max;	
		
		// pitch may not be less then this value, if so level it off at it.
		if (pitch <= pitch_angle_min)
			return 360 - pitch_angle_min;
		else
			return 360 - pitch;
	}
	
	float Get_Yaw()
	{
		// NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin
		yaw = (( (sc_Android_Compass.Yaw - sc_Android_Compass.Yaw_Min)
				* 360) / sc_Android_Compass.Yaw_Range) + 0;
		
		// Subtract find zero value for yaw
		yaw = starting_yaw - yaw;
		
		
		// Return positive value for yaw.
		return -yaw;
	}
	#endif
	
	void CameraDragUpdate()
	{
		// Save old camera position
		old_pitch = current_pitch;
		old_yaw   = current_yaw;
		
		// Store new camera position
		#if UNITY_ANDROID && !UNITY_EDITOR
		new_pitch = Get_Pitch();
		new_yaw   = Get_Yaw();
		#elif UNITY_EDITOR
		new_pitch = 360 - pitch;
		new_yaw   = yaw;
		#endif
		
				
		// Change current position to laged position
		current_pitch = Mathf.Lerp(old_pitch, new_pitch, (Time.deltaTime / camera_pitch_drag));
		current_yaw   = Mathf.LerpAngle(old_yaw, new_yaw, (Time.deltaTime / camera_yaw_drag));
			
		// Set cameras position the lagged position
		transform.rotation = ScriptHelper.ToQ(current_yaw, current_pitch, 0);
	}	
}
