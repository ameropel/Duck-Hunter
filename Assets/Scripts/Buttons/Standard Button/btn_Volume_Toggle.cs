using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class btn_Volume_Toggle : Toggle 
{
	// Script Holder
	[SerializeField] AudioManager sc_AudioManager;
	
	[SerializeField] TextMesh MuteText;
	
	public override void Start() 
	{
		base.Start();
		
		bool mute = PlayerPrefs.GetBool("audio_sfx", false);
		
		// Change gameplay text
		if (mute)
			MuteText.text = "Mute  On";
		else
			MuteText.text = "Mute  Off";
	}
	
	public override void PerfromTransition()
	{
		base.PerfromTransition();
		
		sc_AudioManager.VolumeMute = !sc_AudioManager.VolumeMute;
		
		// Change gameplay text
		if (!sc_AudioManager.VolumeMute)
			MuteText.text = "Mute  Off";
		else
			MuteText.text = "Mute  On";
	}
}
