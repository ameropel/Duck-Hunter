using UnityEngine;
using System.Collections;

public abstract class Slider : MonoBehaviour 
{
	[HideInInspector] public float SliderValue;
	public ButtonManager sc_ButtonManager;
	public float RangeMin;
	public float RangeMax;
	
	[HideInInspector] public GameObject Bar, Node;
	
	Vector3 barPos;
	float   barScale;
	
	// Store values of slider, min and max
	[HideInInspector] public float ScreenMax, ScreenMin;
	[HideInInspector] public float BarMax, BarMin;
	[HideInInspector] public float BarRange, NewRange, ScreenRange;
	public bool SetSlider;
	[HideInInspector] public bool rangeSetOnce = false;
	
	public virtual void Start()
	{	
		SetupSlider();
		InitiateNodeSpot();
		DetermineScale();
	}
	
	public void SetupSlider()
	{
		// Get Node and slider
		foreach( Transform child in transform)
		{
			if (child.name == "Node")
				Node = child.gameObject;
			
			if (child.name == "Bar")
				Bar = child.gameObject;
		}
		
		barScale = Bar.transform.localScale.x;
		
		BarMax = barScale/2;
		BarMin = -barScale/2;
		
		// Set up ranges
		BarRange = (BarMax - BarMin);
	}
	
	void InitiateNodeSpot()
	{
		NewRange = (RangeMax - RangeMin);
		
		// Set slider node to center 
		if (SetSlider)
			SliderValue = (((Node.transform.localPosition.x - BarMin) * NewRange) / BarRange) + RangeMin;
		else
			SliderValue = (RangeMax + RangeMin)/2;

		
		Vector3 NodePos = Node.transform.localPosition;
		NodePos.x = (((SliderValue - RangeMin) * BarRange) / NewRange) + BarMin;
		
		// Update the node value from 0-1 scale
		SliderValue = (((NodePos.x - BarMin) * NewRange) / BarRange) + RangeMin;
		Node.transform.localPosition = new Vector3(NodePos.x, NodePos.y, NodePos.z);
	}
	
	public virtual void MouseOn()
	{
	}
	
	public virtual void MouseOff()
	{
	}
	
	public void DetermineScale()
	{		
		if (Bar == null)
			SetupSlider();
		
		barPos = Bar.transform.position;
		
		// Get screen positions of bar
		Vector3 screenPos;
		screenPos = sc_ButtonManager.camera.WorldToScreenPoint(new Vector3(barPos.x + BarMax, barPos.y, barPos.z));
		ScreenMin = screenPos.x;
		
		screenPos = sc_ButtonManager.camera.WorldToScreenPoint(new Vector3(barPos.x + BarMin, barPos.y, barPos.z));
		ScreenMax = screenPos.x;
		
		// Set up Screen ranges
		ScreenRange = (ScreenMax - ScreenMin);
	}
	
	public virtual void UpdateNodePosition(float mousePos)
	{ 
		DetermineScale();
	}
	
	public void UpdateNodeValue(float pos_x)
	{
		SliderValue = (((pos_x - BarMin) * NewRange) / BarRange) + RangeMin;
	}
}

