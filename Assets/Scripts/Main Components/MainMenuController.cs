using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour 
{
	public enum MenuToAccess
	{	StartGame = 0, Leaderboards, Credits	};
	MenuToAccess currentMenu;
	
	[SerializeField] GameObject MainMenu;
	[SerializeField] GameObject Leaderboards;
	[SerializeField] GameObject Credits;
	
	void Start()
	{
		// Set all menus except main menu to be inactive
		Leaderboards.SetActive(false);
		Credits.SetActive(false);
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
			
			case MenuToAccess.Credits:
				StartCoroutine( Menu_TransitionOut(Credits) );
				break;
		}
	}
	
	IEnumerator Menu_TransitionOut(GameObject go)
	{
		// Wait for animation to stop playing to deactivate
		PlayAnimation(go, false);
		yield return new WaitForSeconds(go.animation.clip.length);
		go.SetActive(false);
		
		MainMenu.SetActive(true);
		PlayAnimation(MainMenu, true);
		
	}
	
	IEnumerator MainMenu_TransitionOut(MenuToAccess menu)
	{
		MainMenu.animation.Play("menu_out");
		// Wait for animation to stop playing to deactivate
		yield return new WaitForSeconds(MainMenu.animation["menu_out"].length);
		MainMenu.SetActive(false);
		
		switch(menu)
		{
			case MenuToAccess.Leaderboards:
				Leaderboards.SetActive(true);
				PlayAnimation(Leaderboards, true);
				break;
			
			case MenuToAccess.Credits:
				Credits.SetActive(true);
				PlayAnimation(Credits, true);
				break;
		}
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
