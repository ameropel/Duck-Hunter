using UnityEngine;
using System.Collections;

public class Android_Compass : MonoBehaviour 
{
	static float xValue;
	//static float yValue;
	//static float zValue;
	
	[HideInInspector] public float Yaw_Range { get{ return (Yaw_Max - Yaw_Min);} }	// Compass Range, goes from -3.15 to 3.15
	[HideInInspector] public float Yaw_Max   { get{ return  3.15f;} }	// Max compass range is 3.15
	[HideInInspector] public float Yaw_Min   { get{ return -3.15f;} }	// Min compass range is -3.15
	
	[HideInInspector] public float Yaw  { get{ return xValue;} }		// Only allow user to read yaw data, not change it
	
	[HideInInspector] public bool CompassLoaded;	// Detect if compass is loaded
	
	// Use this for initialization
	void Awake () 
	{		
		AndroidJNI.AttachCurrentThread();
		StartCoroutine( WaitForCompass() );
	}
	
	IEnumerator WaitForCompass()
	{
		
		// Wait until we retrieve data from compass.
		// This is done by checking if yaw value is not null/0;
		while(xValue == 0)
			yield return null;
		
		CompassLoaded = true;
		yield return null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Get access to androids hardware
		using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
		using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic <AndroidJavaObject> ("currentActivity")) {
		
		// Get access to android java class "CompassActivity"
		AndroidJavaClass cls_CompassActivity = new AndroidJavaClass("com.Ameropel.DuckHunter.CompassActivity");
				
			// Get compass values (pitch, roll, yaw)
			xValue = cls_CompassActivity.CallStatic<float>("getX");
			
			// Do not need roll and pitch
			//yValue = cls_CompassActivity.CallStatic<float>("getY");
			//zValue = cls_CompassActivity.CallStatic<float>("getZ");
		}
		}
		
	}
}