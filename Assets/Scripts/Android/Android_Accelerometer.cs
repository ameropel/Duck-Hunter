using UnityEngine;
using System.Collections;

public class Android_Accelerometer : MonoBehaviour 
{
	float _pitch = 0.0f;	// x axis
	//float _yaw   = 0.0f;	// y axis
	//float _roll  = 0.0f;	// z axis
	
	// Round value for accelerometer
	float accel_round = 0.00001f;
	
	// Properties
	//public float Roll { get{ return _roll;} }
	//public float Yaw   { get{ return _yaw;} }
	public float Pitch  { get{ return _pitch;} }
	
	public float Pitch_Range { get{ return (Pitch_Max - Pitch_Min);} }	// Accelerometer range, ranges from -1 to 1
	public float Pitch_Max   { get{ return  1;} }	// Max value for accelerometer is 1
	public float Pitch_Min   { get{ return -1;} }	// Min value for accelerometer is -1
	
	void Update()
	{
		// Get android accelerometers values, and round them
		//_roll = ScriptHelper.RoundValue( Input.acceleration.x, accel_round);
		//_yaw   = ScriptHelper.RoundValue( Input.acceleration.y, accel_round);
		_pitch  = ScriptHelper.RoundValue( Input.acceleration.z, accel_round);
	}
}