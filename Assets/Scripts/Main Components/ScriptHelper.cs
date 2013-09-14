using UnityEngine;
using System.Collections;

public static class ScriptHelper 
{	
	public static Vector2 DEFAULT_SIZE = new Vector2(1024, 600);	
	public static Vector2 SCREEN_RIGHT_CORNER = new Vector2(21.36533f, 2.8f);
	public static Vector2 SCREEN_LEFT_CORNER = new Vector2(18.63467f, 1.2f);
	
	// Round float value
	public static float RoundValue(float what, float to)
	{
		return to * Mathf.Round(what/to);
	}
	
	// Quaternion Management
	public static Quaternion ToQ (Vector3 v)
	{
	    return ToQ (v.y, v.x, v.z);
	}

	public static Quaternion ToQ (float yaw, float pitch, float roll)
	{
	    yaw *= Mathf.Deg2Rad;
	    pitch *= Mathf.Deg2Rad;
	    roll *= Mathf.Deg2Rad;
	    float rollOver2 = roll * 0.5f;
	    float sinRollOver2 = (float)Mathf.Sin (rollOver2);
	    float cosRollOver2 = (float)Mathf.Cos (rollOver2);
	    float pitchOver2 = pitch * 0.5f;
	    float sinPitchOver2 = (float)Mathf.Sin (pitchOver2);
	    float cosPitchOver2 = (float)Mathf.Cos (pitchOver2);
	    float yawOver2 = yaw * 0.5f;
	    float sinYawOver2 = (float)Mathf.Sin (yawOver2);
	    float cosYawOver2 = (float)Mathf.Cos (yawOver2);
	    Quaternion result;
	    result.w = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
	    result.x = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
	    result.y = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
	    result.z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;
	
	    return result;
	}

	public static Vector3 FromQ2 (Quaternion q1)
	{
	    float sqw = q1.w * q1.w;
	    float sqx = q1.x * q1.x;
	    float sqy = q1.y * q1.y;
	    float sqz = q1.z * q1.z;
	    float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
	    float test = q1.x * q1.w - q1.y * q1.z;
	    Vector3 v;
	
		// singularity at north pole
	    if (test>0.4995f*unit) 
		{ 
	        v.y = 2f * Mathf.Atan2 (q1.y, q1.x);
	        v.x = Mathf.PI / 2;
	        v.z = 0;
	        return NormalizeAngles (v * Mathf.Rad2Deg);
	    }
		
		 // singularity at south pole
	    if (test<-0.4995f*unit)
		{
	        v.y = -2f * Mathf.Atan2 (q1.y, q1.x);
	        v.x = -Mathf.PI / 2;
	        v.z = 0;
	        return NormalizeAngles (v * Mathf.Rad2Deg);
	    }
		
	    Quaternion q = new Quaternion (q1.w, q1.z, q1.x, q1.y);
	    v.y = (float)Mathf.Atan2 (2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));    // Yaw
	    v.x = (float)Mathf.Asin (2f * (q.x * q.z - q.w * q.y));                             				// Pitch
	    v.z = (float)Mathf.Atan2 (2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));    // Roll
	    return NormalizeAngles (v * Mathf.Rad2Deg);
	}
	
	static Vector3 NormalizeAngles (Vector3 angles)
	{
	    angles.x = NormalizeAngle (angles.x);
	    angles.y = NormalizeAngle (angles.y);
	    angles.z = NormalizeAngle (angles.z);
	    return angles;
	}
	
	static float NormalizeAngle (float angle)
	{
	    while (angle>360)
	        angle -= 360;
	    while (angle<0)
	        angle += 360;
	    return angle;
	}
	
	// Scale on range
	public static float Scale_With_Range(float OldMax, float OldMin, float OldValue, float NewMax, float NewMin)
	{
		float OldRange, NewRange, NewValue = 0;
		
		OldRange = (OldMax - OldMin);
		NewRange = (NewMax - NewMin);

		NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
		
		return NewValue;
	}
	
	// Scale GameObject to Viewport
	public static Vector3 ScaleToViewport( Camera _camera, float distance_from_camera, Vector3 t_position)
	{
		Vector3 v3ViewPort, v3BottomLeft, v3TopRight, position;
		
		v3ViewPort = new Vector3(0,0,distance_from_camera);
		v3BottomLeft = _camera.ViewportToWorldPoint(v3ViewPort);
		v3ViewPort.Set(1,1,distance_from_camera);
		v3TopRight = _camera.ViewportToWorldPoint(v3ViewPort);
	
		position = new Vector3(ScriptHelper.Scale_With_Range(SCREEN_RIGHT_CORNER.x, SCREEN_LEFT_CORNER.x, t_position.x, v3TopRight.x, v3BottomLeft.x), 
							   ScriptHelper.Scale_With_Range(SCREEN_RIGHT_CORNER.y, SCREEN_LEFT_CORNER.y, t_position.y, v3TopRight.y, v3BottomLeft.y),
				     		   t_position.z);		
		
		return position;
	}
}
