using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour 
{
	public enum MenuToAccess
	{	StartGame = 0, Leaderboards, Credits	};
	MenuToAccess currentMenu;
	
	[SerializeField] GameObject Leaderboards;
	[SerializeField] GameObject MainMenu;
	[SerializeField] Collider ScreenBlocker;
	
	void Start()
	{
		// Set all menus except main menu to be inactive
		Leaderboards.SetActive(false);
	}
	
	public void Access_Menu(MenuToAccess menuTo)
	{
		// Set current menu
		currentMenu = menuTo;
		// Start transitioning out to a new menu
		StartCoroutine( MainMenu_TransitionOut(menuTo) );
	}
	
	public void Access_MainMenu()
	{		
		switch(currentMenu)
		{
			case MenuToAccess.Leaderboards:
				StartCoroutine( Menu_TransitionOut(Leaderboards) );
				break;
		}
	}
	
	IEnumerator Menu_TransitionOut(GameObject go)
	{
		// Activate screen blocker so user cant press buttons during transition 
		ScreenBlocker.enabled = true;
		MainMenu.SetActive(true);
		PlayAnimation(go, false);
		
		// Add a hiccup before transitioning in new menu
		yield return new WaitForSeconds(0.5f);
		PlayAnimation(MainMenu, true);
		
		// Wait for animation to stop playing to deactivate collider
		yield return new WaitForSeconds(go.animation.clip.length);
		go.SetActive(false);
		
		// Deactivate screen blocker so user can press buttons again 
		ScreenBlocker.enabled = false;
	}
	
	IEnumerator MainMenu_TransitionOut(MenuToAccess menu)
	{
		// Activate screen blocker so user cant press buttons during transition 
		ScreenBlocker.enabled = true;
		PlayAnimation(MainMenu, false);
		
		// Add a hiccup before transitioning in new menu
		yield return new WaitForSeconds(0.5f);
		
		switch(menu)
		{
			case MenuToAccess.Leaderboards:
				Leaderboards.SetActive(true);
				PlayAnimation(Leaderboards, true);
				break;
		}
		
		// Wait for animation to stop playing to deactivate collider
		yield return new WaitForSeconds(MainMenu.animation.clip.length);
		MainMenu.SetActive(false);
		
		// Deactivate screen blocker so user can press buttons again 
		ScreenBlocker.enabled = false;
	}
	
	void PlayAnimation(GameObject go, bool forward)
	{
		// Get animation name
		string clip_name = go.animation.clip.name;
		
		// If playing forward set speed to 1
		if (forward)
			go.animation[clip_name].speed = 1.0f;
		// If reverse set speed to negative and start frames from top
		else
		{
			go.animation[clip_name].speed = -1.0f;
			go.animation[clip_name].normalizedTime = 1.0f;
		}
		
		// Play animation
		go.animation.Play();
	}
}
