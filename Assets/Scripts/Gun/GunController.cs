using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour 
{
	#region Script Variables
	
	// Scripts
	AudioManager sc_AudioManager;
	
	// Screen Resolution
	[HideInInspector] public float screen_footer_height;						// Footer height
	Vector2 screen_ratio = new Vector2(1024, 600);	// Default screen size
	
	// Application Text
	[SerializeField] GUIText[] text_reload;
	[SerializeField] GUIText[] text_ammo;
	private int reload_text_size = 50;
	private int ammo_text_size = 40;
	
	// Bullet
	[SerializeField] GameObject Bullet;
	[SerializeField] GameObject Bullet_Spawn_Point;
	
	// Particles
	[SerializeField] GameObject Shotgun_Particle;
	[SerializeField] GameObject Particle_Spawn_Point;
	
	// GameObject Weapon
	//GameObject weapon;
	
	// Ammunition
	private int maximum_ammo = 8;
	private int current_ammo;
	
	// Timers
	float time_between_shots = 0.2f;
	float time_between_no_ammo = 0.2f;
	float time_between_reload = 0.3f;
	float time_final_reload = 0.65f;
	
	// Booleans
	private bool can_fire_weapon = true;
	private bool no_ammuntion = false;
	private bool reloading_weapon = false;
	private bool user_fired_weapon = false;
	[HideInInspector] public bool Zoom_In;
	
	// Animations
	Animation  weapon_anim;
	public string weapon_zoom_in;
	
	#endregion
	
	
	void Awake()
	{
		// Get AudioManager script
		sc_AudioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<AudioManager>();	
	}
	
	void Start()
	{
		//weapon = GameObject.FindGameObjectWithTag("Weapon");	// Find Weapon in scene
		weapon_anim = gameObject.GetComponent<Animation>();			// Get Weapons animation
		
		// Height for footer
		screen_footer_height = Screen.height - Screen.height/6;
		
		// Scale reload text to screen size
		for (int i=0; i < text_reload.Length; i++)
			text_reload[i].fontSize = (int)( (reload_text_size * Screen.height) / screen_ratio.y);
		
		// Scale ammo text to screen size
		for (int i = 0; i < text_ammo.Length; i++)
			text_ammo[i].fontSize = (int)( (ammo_text_size * Screen.height) / screen_ratio.y);
		
		// Set current ammo to maximum ammuntion
		current_ammo = maximum_ammo;
		
		// Set ammunition text to default value;
		for (int i = 0; i < text_ammo.Length; i++)
			text_ammo[i].text = (current_ammo.ToString() + "/" + maximum_ammo.ToString());
	}
	
	void Update()
	{
		Weapon_Zoom_In(Zoom_In);
	}
	
	#region Fire Weapon
	public void FireShotgun()
	{
		// User pressed trigger
		user_fired_weapon = true;
		
		if ( current_ammo == 0)
		{
			if (!no_ammuntion)
			{
				// Play shotgun out of ammo audioclip
				sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.SHOTGUN_OUT_OF_AMMO);
				
				// Start Delay Timer for Firing
				StartCoroutine( NoAmmoDelay() );
				
				// Debug Text
				Debug.Log("No Ammo");
			}
		}
		else
		{
		
			// If time delay is up, not reloading, and ammo is greater then 0... FIRE
			if (can_fire_weapon &&
				!reloading_weapon)
			{
				// Debug Text
				Debug.Log("Fire!");
				
				// Vibrate Device when shoot
				Handheld.Vibrate();
	
				// Play shotgun fire audioclip
				sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.SHOTGUN_FIRE);
				
				// Deduct one bullet from ammunition and update text
				current_ammo--;
				UpdateAmmunitionText();
				
				//Add particles to shot
				Instantiate(Shotgun_Particle, Particle_Spawn_Point.transform.position, Quaternion.LookRotation(Particle_Spawn_Point.transform.forward));
				// Instantiate bullet
				Instantiate(Bullet, Bullet_Spawn_Point.transform.position, Quaternion.LookRotation(Bullet_Spawn_Point.transform.forward));
				
				// Start Delay Timer for Firing
				StartCoroutine( FireWeaponDelay() );
			}
			// If can't shoot and not reloading
			else if (!can_fire_weapon &&
					!reloading_weapon)
			{
				// Debug Text
				Debug.Log("Must Wait to Shoot");		
			}
		}
	}
	
	IEnumerator NoAmmoDelay()
	{
		// Lock gun so can delay
		no_ammuntion = true;
		
		// Set time delay counter
		float time = 0;
		while (time < 1)
		{
			time += Time.deltaTime / time_between_no_ammo;
			yield return null;
		}
		
		// Unlock gun
		no_ammuntion = false;
	}
	
	IEnumerator FireWeaponDelay()
	{
		// Lock shooting (unavailable)
		can_fire_weapon = false;
	
		// Set time delay counter
		float time = 0;		// time between shots
		while (time < 1)
		{
			time += Time.deltaTime / time_between_shots;
			yield return null;
		}
		
		// Unlock shooting (can shoot again)
		can_fire_weapon = true;
	}
	#endregion
	
	#region Reload Weapon
	public void ReloadWeapon ()
	{
		// if not reloading and ammo is not maxed... reload
		if (!reloading_weapon &&
			current_ammo != maximum_ammo)
		{
			// Debug Text
			Debug.Log("Reloading");
			
			// Start reloading weapon
			StartCoroutine( ReloadingWeaponDelay() ); 
		}
	}
	
	IEnumerator ReloadingWeaponDelay()
	{
		// Release trigger
		user_fired_weapon = false;
		
		// Lock gun so can reload
		reloading_weapon = true;
		
		while (current_ammo != maximum_ammo && !user_fired_weapon)
		{	
			// Time till next reload clip
			float time = 0;
			while (time < 1)
			{
				time += Time.deltaTime / time_between_reload;
				yield return null;
			}
			
			// Play shotgun reload audioclip
			sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.SHOTGUN_RELOAD);
			
			// Increase Ammunition by one and update text
			current_ammo++;
			UpdateAmmunitionText();
			
			if (user_fired_weapon)
				break;
			
			yield return null;	
		}
		
		StartCoroutine( FinalReload() );
		
	}
	
	IEnumerator FinalReload()
	{
		
		// Time till next reload clip
		float time = 0;
		while (time < 1)
		{
			time += Time.deltaTime / time_between_reload;
			yield return null;
		}
		
		// Play shotgun final reload audioclip
		sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.SHOTGUN_RELOAD_FINAL);
		
		// Time till weapon can shoot again
		time = 0;
		while (time < 1)
		{
			time += Time.deltaTime / time_final_reload;
			yield return null;
		}
		
		// Unlock gun so can shoot
		reloading_weapon = false;
	}
	#endregion
	
	void UpdateAmmunitionText()
	{
		for (int i = 0; i < text_ammo.Length; i++)
			text_ammo[i].text = (current_ammo.ToString() + "/" + maximum_ammo.ToString());
	}
	
	void Weapon_Zoom_In(bool zoom_in)
	{
		if (zoom_in)
		{
			if (Camera.main.fieldOfView == 40 && !weapon_anim.isPlaying)
			{
				//Debug.Log("No more zooming in");
				Zoom_In = false;
				return;
			}
			else if (weapon_anim.isPlaying)
			{
				//Debug.Log("zooming");
				weapon_anim.animation[weapon_zoom_in].speed = 1.0f;
				Zoom_In = false;
				return;	
			}
			else if (weapon_anim.animation[weapon_zoom_in].normalizedTime == 0)
			{
				//Debug.Log("zoom start");
				weapon_anim.animation[weapon_zoom_in].speed = 1.0f;
				weapon_anim.Play(weapon_zoom_in);
			}
		}
		else
		{
			if (Camera.main.fieldOfView == 40 && !weapon_anim.isPlaying)
			{
				//Debug.Log("Reverse  Top");
				weapon_anim.animation[weapon_zoom_in].normalizedTime = 1.0f;
				weapon_anim.animation[weapon_zoom_in].speed = -1.0f;
				weapon_anim.Play(weapon_zoom_in);
			}
			else if (weapon_anim.animation[weapon_zoom_in].normalizedTime == 0)
			{
				//Debug.Log("no more zooming out");
				return;
			}
			else
			{
				//Debug.Log("reverse");
				weapon_anim.animation[weapon_zoom_in].speed = -1.0f;
			}
		}
	}
}
