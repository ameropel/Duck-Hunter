package com.Ameropel.DuckHunter;
import com.unity3d.player.UnityPlayerActivity;

import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;

public class CompassActivity extends UnityPlayerActivity 
{
	
	private SensorManager mSensorManager;
	Sensor accelerometer;
	Sensor magnetometer;
	
	static public float xmag;
	static public float ymag;
	static public float zmag;


	private final SensorEventListener mListener = new SensorEventListener() 
	{
		float[] mGravity;
		float[] mGeomagnetic;
		public void onSensorChanged(SensorEvent event) 
		{
			if (event.sensor.getType() == Sensor.TYPE_ACCELEROMETER)
				mGravity = event.values;
			if (event.sensor.getType() == Sensor.TYPE_MAGNETIC_FIELD)
				mGeomagnetic = event.values;
		    
			if (mGravity != null && mGeomagnetic != null) 
			{
				float R[] = new float[9];
				float I[] = new float[9];
				boolean success = SensorManager.getRotationMatrix(R, I, mGravity, mGeomagnetic);
				if (success) 
				{
					float orientation[] = new float[3];
					SensorManager.getOrientation(R, orientation);
					
					xmag = orientation[0];
					ymag = orientation[1];
					zmag = orientation[2];
				}
			}			  
		}
		
		public void onAccuracyChanged(Sensor sensor, int accuracy) 
		{
		}
	};
	
	@Override
	protected void onCreate(Bundle savedInstanceState) 
	{
		super.onCreate(savedInstanceState);
		 mSensorManager = (SensorManager)getSystemService(SENSOR_SERVICE);
	     accelerometer = mSensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
	     magnetometer = mSensorManager.getDefaultSensor(Sensor.TYPE_MAGNETIC_FIELD);
	}
	
	@Override
	protected void onResume()
	{
		super.onResume();
		mSensorManager.registerListener(mListener, accelerometer, SensorManager.SENSOR_DELAY_UI);
	    mSensorManager.registerListener(mListener, magnetometer, SensorManager.SENSOR_DELAY_UI);
	}
	
	@Override
	protected void onStop()
	{
		mSensorManager.unregisterListener(mListener);
		super.onStop();
	}
	
	protected void onPause() 
	{
	    super.onPause();
	    mSensorManager.unregisterListener(mListener);
	  }
	
	public static float getX() 
	{
		return xmag;
	}
	
	public static float getY() 
	{
		return ymag;
	}
	
	public static float getZ() 
	{
		return zmag;
	}
}
