using UnityEngine;
using System.Collections;

public class btn_Menu:  Button 
{	
	[SerializeField] MainMenuController sc_MainMenuController;
	
	public enum MenuToAccess
	{	MainMenu = 0, StartGame, Leaderboards, Credits	};
	public MenuToAccess	MenuTransitionTo;
	
	public override void PerfromTransition()
	{
		base.PerfromTransition();
		
		switch (MenuTransitionTo)
		{
			case MenuToAccess.MainMenu:	
				sc_MainMenuController.Access_MainMenu();
				break;
			
			case MenuToAccess.Leaderboards:	
				sc_MainMenuController.Access_Menu(MainMenuController.MenuToAccess.Leaderboards);
				break;
		}
	}
}
