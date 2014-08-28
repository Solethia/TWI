using UnityEngine;
using System.Collections;

public class SelectGameMode : MonoBehaviour {

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
		GUI.skin = mainMenuSkins[1];
		switch (interfaceAbilitiesGridSelect)
		{
		case 0:
			if (GUI.Button(goButton, "Start"))
			{
				GameRef.GameMode = 1;
				Application.LoadLevel(5);
			}
			break;
		case 1:
			if (GUI.Button(goButton, "Start"))
			{
				GameRef.GameMode = 2;
				Application.LoadLevel(5);
			}
			break;
		case 2:
			if (GUI.Button(goButton, "Start"))
			{
				GameRef.GameMode = 3;
				Application.LoadLevel(5);
			}
			break;
		}
		
	}

}
