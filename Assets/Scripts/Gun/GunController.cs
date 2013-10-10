using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour 
{
	#region Script Variables
	
	// Scripts
	[SerializeField] AudioManager   sc_AudioManager;
	[SerializeField] GameController sc_GameController;
	[SerializeField] GameScore      sc_GameScore;
	[SerializeField] GameTimer      sc_GameTimer;
	
	// Screen Resolution
	[HideInInspector] public float screen_footer_height;		// Footer height
	
	// Application Text
	[SerializeField] TextMesh text_ammo;
	
	// Bullet
	[SerializeField] GameObject BulletPrefab;
	[SerializeField] GameObject Bullet_Spawn_Point;
	
	// Particles
	[SerializeField] GameObject Shotgun_Particle;
	[SerializeField] GameObject Particle_Spawn_Point;
	
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
	public string weapon_reload;
	public string weapon_final_reload;
	bool game_pause;
	
	#endregion
	
	void Start()
	{
		//weapon = GameObject.FindGameObjectWithTag("Weapon");	// Find Weapon in scene
		weapon_anim = gameObject.GetComponent<Animation>();			// Get Weapons animation
		
		// Height for footer
		screen_footer_height = Screen.height - Screen.height/6;
			
		// Set current ammo to maximum ammuntion
		current_ammo = maximum_ammo;
		
		// Set ammunition text to default value;
		text_ammo.text = (current_ammo.ToString() + "/" + maximum_ammo.ToString());
		
		GameController.Gameplay_Pause += PauseAnimations;
		GameController.Gameplay_UnPause += UnPauseAnimations;
	}
	
	void Update()
	{
		#if UNITY_EDITOR
		EditorInput();
		#endif
		Weapon_Zoom_In(Zoom_In);
	}
	
	void PauseAnimations()
	{
		game_pause = true;
		
		foreach (AnimationState state in animation)
			state.speed = 0.0f;
	}
	
	void UnPauseAnimations()
	{
		game_pause = false;
		
		foreach (AnimationState state in animation)
			state.speed = 1.0f;
	}
	
	void EditorInput()
	{
		if (Input.GetKey(KeyCode.Z))
			Zoom_In = true;
		else if (Input.GetKeyUp(KeyCode.Z))
			Zoom_In = false;
		
		if (Input.GetKeyDown(KeyCode.X))
			FireShotgun();
		
		if (Input.GetKeyDown(KeyCode.C))
			ReloadWeapon();
	}
	
	#region Fire Weapon
	public void FireShotgun()
	{
		// Do nothing if game paused
		if (sc_GameController.GameState == GameController.GameStatus.PAUSED)
			return;
		
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
				ScriptHelper.DebugString("No Ammo");
			}
		}
		else
		{
		
			// If time delay is up, not reloading, and ammo is greater then 0... FIRE
			if (can_fire_weapon &&
				!reloading_weapon)
			{
				// Debug Text
				ScriptHelper.DebugString("Fire!");
				
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
				GameObject bullet = Instantiate(BulletPrefab, Bullet_Spawn_Point.transform.position, Quaternion.LookRotation(Bullet_Spawn_Point.transform.forward)) as GameObject;
				bullet.GetComponent<Bullet>().sc_GameScore = sc_GameScore;
				
				// Start Delay Timer for Firing
				StartCoroutine( FireWeaponDelay() );
			}
			// If can't shoot and not reloading
			else if (!can_fire_weapon &&
					!reloading_weapon)
			{
				// Debug Text
				ScriptHelper.DebugString("Must Wait to Shoot");		
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
			// Do nothing if game paused
			if (sc_GameController.GameState == GameController.GameStatus.PAUSED)
				yield return null;
			else
			{
				time += sc_GameTimer.Game_deltaTime / time_between_no_ammo;
				yield return null;
			}
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
			time += sc_GameTimer.Game_deltaTime / time_between_shots;
			yield return null;
		}
		
		// Unlock shooting (can shoot again)
		can_fire_weapon = true;
	}
	#endregion
	
	#region Reload Weapon
	public void ReloadWeapon ()
	{
		// Do nothing if game paused
		if (sc_GameController.GameState == GameController.GameStatus.PAUSED)
			return;
		
		// if not reloading and ammo is not maxed... reload
		if (!reloading_weapon &&
			current_ammo != maximum_ammo)
		{
			// Play animation to start reload process (bring gun close)
			weapon_anim.animation[weapon_reload].speed = 1;
			weapon_anim.animation.CrossFade(weapon_reload);
			
			// Debug Text
			ScriptHelper.DebugString("Reloading");
			
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
				// Do nothing if game paused
				if (sc_GameController.GameState == GameController.GameStatus.PAUSED)
					yield return null;
				else
				{
					time += sc_GameTimer.Game_deltaTime / time_between_reload;
					yield return null;
				}
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
		
		// Reverse reload animation (bring gun away from user)
		weapon_anim.animation[weapon_reload].normalizedTime = 1;
		weapon_anim.animation[weapon_reload].speed = -1;
		weapon_anim.animation.Play(weapon_reload);
		
		StartCoroutine( FinalReload() );
		
	}
	
	IEnumerator FinalReload()
	{
		
		// Time till next reload clip
		float time = 0;
		while (time < 1)
		{	
			// Do nothing if game paused
			if (sc_GameController.GameState == GameController.GameStatus.PAUSED)
				yield return null;
			else
			{
				time += sc_GameTimer.Game_deltaTime / time_between_reload;
				yield return null;
			}
		}
		
		// Play reload pump animation
		weapon_anim.animation.CrossFade(weapon_final_reload);
		
		// Play shotgun final reload audioclip
		sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.SHOTGUN_RELOAD_FINAL);
		
		// Time till weapon can shoot again
		time = 0;
		while (time < 1)
		{
			// Do nothing if game paused
			if (sc_GameController.GameState == GameController.GameStatus.PAUSED)
				yield return null;
			else
			{
				time += sc_GameTimer.Game_deltaTime / time_final_reload;
				yield return null;
			}
		}
		
		// Unlock gun so can shoot
		reloading_weapon = false;
	}
	#endregion
	
	void UpdateAmmunitionText()
	{
		text_ammo.text = (current_ammo.ToString() + "/" + maximum_ammo.ToString());
	}
	
	void Weapon_Zoom_In(bool zoom_in)
	{	
		// If weapon is reloading user cannot zoom in 
		if (reloading_weapon || game_pause)
			return;
		
		if (zoom_in)
		{
			if (gameObject.camera.fieldOfView == 40 && !weapon_anim.isPlaying)
			{
				Zoom_In = false;
				return;
			}
			else if (weapon_anim.isPlaying)
			{
				weapon_anim.animation[weapon_zoom_in].speed = 1.0f;
				Zoom_In = false;
				return;	
			}
			else if (weapon_anim.animation[weapon_zoom_in].normalizedTime == 0)
			{
				weapon_anim.animation[weapon_zoom_in].speed = 1.0f;
				weapon_anim.Play(weapon_zoom_in);
			}
		}
		else
		{
			if (gameObject.camera.fieldOfView == 40 && !weapon_anim.isPlaying)
			{
				weapon_anim.animation[weapon_zoom_in].normalizedTime = 1.0f;
				weapon_anim.animation[weapon_zoom_in].speed = -1.0f;
				weapon_anim.Play(weapon_zoom_in);
			}
			else if (weapon_anim.animation[weapon_zoom_in].normalizedTime == 0)
				return;
			else
				weapon_anim.animation[weapon_zoom_in].speed = -1.0f;
		}
	}
}
