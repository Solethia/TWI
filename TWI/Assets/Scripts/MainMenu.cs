using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	[SerializeField]
	private GUISkin[] mainMenuSkins;

	private int interfaceAbilitiesGridSelect = 0;
	[SerializeField]
	private GUIContent[] MenuPoints;
	private void OnGUI() 
	{

		GUI.skin = mainMenuSkins[0];
		Rect menuRect = new Rect((10*Screen.width / 100),(10*Screen.height / 100), (50*Screen.width / 100), (80*Screen.height / 100));

		Rect goButton = new Rect((70*Screen.width / 100),(75*Screen.height / 100), (20*Screen.width / 100), (10*Screen.height / 100));
		interfaceAbilitiesGridSelect = GUI.SelectionGrid(menuRect, interfaceAbilitiesGridSelect, MenuPoints, 1, "textarea");
		string infoTitle;
		string infoText;
		GUI.skin = mainMenuSkins[1];
		switch (interfaceAbilitiesGridSelect)
		{
		case 0:
			infoTitle = "Training Course";
			infoText = "As a new member of The Watchtower Initiative, you have been assigned to the training course Commander. The training course will give you all the qualifications you need to be a part of Watchtower. To complete the training course, you must complete the tutorial and eliminate all the target dummies placed around the facility.\n - Mr. XYZ";
			;
			InfoBox(infoTitle, infoText);
			if (GUI.Button(goButton, "Play"))
			{
				Application.LoadLevel(1);
			}
			break;
		case 1:
			infoTitle = "Operation Wildfires";
			infoText = "Our first mission details have arrived. A terrorist cell  have been discovered in old Spain. The terrorist cell have threatened to bomb several key military installations in New Europe. Their leader Albert Einstein, is a very dangerous man. We estimate that the terrorist cell has between 4-8 members, good luck on your first mission Commander.\n - Mr. XYZ";
			InfoBox(infoTitle, infoText);
			if (GUI.Button(goButton, "Play"))
			{
				Application.LoadLevel(2);
			}
			break;
		case 2:
			infoTitle = "Operation Snowman";
			infoText = "Word is just in! One of our arctic bases has just been attacked by a unknown organization. The base is a weapons testing facility for weapons of mass destruction, so we have to act quick. Several scientists are reported dead, but some might still be held hostage. Eliminate the enemy attack without harming any hostages.\n - Mr. XYZ\n\n";
			InfoBox(infoTitle, infoText);
			if (GUI.Button(goButton, "Play"))
			{
				Application.LoadLevel(3);
			}
			break;
		case 3:
			infoTitle = "Restart Intro";
			infoText = "";
			InfoBox(infoTitle, infoText);
			if (GUI.Button(goButton, "Start"))
			{
				Application.LoadLevel(0);
			}
			break;
		case 4:
			infoTitle = "Credits";
			infoText = " ";
			InfoBox(infoTitle, infoText);
			break;
		}

	}

	private void InfoBox(string infoTitle, string infoText)
	{
		Rect infoBox = new Rect((65*Screen.width / 100),(10*Screen.height / 100), (30*Screen.width / 100), (80*Screen.height / 100));
		GUI.Label(infoBox,  "<Size=18><b>" + infoTitle + "</b>\n" + infoText + "</Size>", "textarea");
	}
}
