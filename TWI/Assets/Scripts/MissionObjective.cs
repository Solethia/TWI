using UnityEngine;
using System.Collections;

public class MissionObjective : MonoBehaviour {

	private string[] missionObjectiveText;
	private bool showMission = false;
	private bool gameWon = false;
	private bool gameFailed = false;

	[SerializeField]
	private GUISkin MissionObjectiveSkin;

	// Use this for initialization
	void Start () 
	{
		missionObjectiveText = new string[]
		{
			"As a new member of The Watchtower Initiative, you have been assigned to the training course Commander. The training course will give you all the qualifications you need to be a part of Watchtower. To complete the training course, you must complete the tutorial and eliminate all the target dummies placed around the facility.\n - Mr. XYZ\n\n",
			"Our first mission details have arrived. A terrorist cell  have been discovered in old Spain. The terrorist cell have threatened to bomb several key military installations in New Europe. Their leader Albert Einstein, is a very dangerous man. We estimate that the terrorist cell has between 4-8 members, good luck on your first mission Commander.\n - Mr. XYZ\n\n",
			"Word is just in! One of our arctic bases has just been attacked by a unknown organization. The base is a weapons testing facility for weapons of mass destruction, so we have to act quick. Several scientists are reported dead, but some might still be held hostage. Eliminate the enemy attack without harming any hostages.\n - Mr. XYZ\n\n",
			"string4",
			"string5",
			"string6"
		};
	}
	
	// Update is called once per frame
	void Update () 
	{
		MissionCompleteCheck();
		MissionFailCheck();
	}

	private void OnGUI() 
	{
		if (GameRef.TutorialReference.TutorialStep >= 12 || Application.loadedLevel != 1)
		{
			GUI.skin = MissionObjectiveSkin;
			if (!gameWon && !gameFailed)
			{

				if (GUI.Button(new Rect(0,0,150,30), "Mission Objective"))
				{

					showMission = !showMission;
					if (showMission)
					{
						GameRef.Paused = true;
						if ( GameRef.TutorialReference.TutorialStep == 12 && Application.loadedLevel == 1)
						{
							GameRef.TutorialReference.StepCompleted();
						}
					}
					else
					{
						GameRef.Paused = false;
					}
				}
				if (showMission)
				{
					//Show Mission
					MissionBriefing(missionObjectiveText[Application.loadedLevel-1]);
				}
			}
		}
		if (gameWon)
		{
			//Gamewin Screen!
			Victory();
		}
		else if (gameFailed)
		{
			Lose();
		}
	}

	private void MissionBriefing(string bodyText)
	{
		int areaWidth = 750;
		int areaHeight = 500;
		int xPosition = (Screen.width/2) + (areaWidth/2);
		int yPosition = (Screen.height/2) - (areaHeight/2);
		Rect objectiveRect = new Rect(xPosition-areaWidth, yPosition, areaWidth, areaHeight);

		string missionProgress = MissionProgress();

		GUI.Label (objectiveRect, "<Size=18> \n Hello Commander, \n\n " + bodyText + "<b>Summary</b>\n" + missionProgress + "</size>" , "textarea");
		GUI.Box (objectiveRect, "<Size=18><b>Mission Objective</b></size>");
	}

	private void Victory()
	{
		int areaWidth = 300;
		int areaHeight = 300;
		int xPosition = (Screen.width/2) + (areaWidth/2);
		int yPosition = (Screen.height/2) - (areaHeight/2);
		Rect objectiveRect = new Rect(xPosition-areaWidth, yPosition, areaWidth, areaHeight);
		
		GUI.Label (objectiveRect, "<Size=18> \n" + "Congratulations! You have completed the level!" +"</size>" , "textarea");
		GUI.Box (objectiveRect, "<Size=18><b>Victory!</b></size>");
		MainMenuButton();
	}

	private void Lose()
	{
		int areaWidth = 300;
		int areaHeight = 300;
		int xPosition = (Screen.width/2) + (areaWidth/2);
		int yPosition = (Screen.height/2) - (areaHeight/2);
		Rect objectiveRect = new Rect(xPosition-areaWidth, yPosition, areaWidth, areaHeight);
		
		GUI.Label (objectiveRect, "<Size=18> \n" + "You Lost! But you can always try again." +"</size>" , "textarea");
		GUI.Box (objectiveRect, "<Size=18><b>Lose!</b></size>");
		MainMenuButton();
	}

	private string[] objectivesCompletedText;
	private string MissionProgress()
	{
		switch(Application.loadedLevel)
		{
		case 1: 
			if (objectivesCompletedText == null)
			{
				objectivesCompletedText = new string[] {"", ""};
			}
			return 
				" - " + "Complete the tutorial. " + objectivesCompletedText[0] + " \n" +
					" - Eliminate all the practice targets. (" + GameRef.ComputerCharacters.Count.ToString() + " Remaining.)" + objectivesCompletedText[1];
		case 2: 
			if (objectivesCompletedText == null)
			{
				objectivesCompletedText = new string[] {""};
				Debug.Log ("Created 'objectivesCompletedText' array");
			}
			return 
				" - " + "Eliminate all hostile threaths. (" + GameRef.ComputerCharacters.Count.ToString() + " Remaining)" + objectivesCompletedText[0] + "\n" +
				" - " + "All team members must survive.";
		case 3: if (objectivesCompletedText == null)
			{
				objectivesCompletedText = new string[] {""};
			}
			return 
				" - " + "Eliminate all hostile threaths. (" + GameRef.ComputerCharacters.Count.ToString() + " Remaining)" + objectivesCompletedText[0] + "\n" +
				" - " + "All team members must survive. \n" +
				" - " + "All hostages must survive.";
		case 4: return " - ";
		default: return null;
		}
	}
	private bool[] objectivesCompleted;
	private void MissionCompleteCheck()
	{
		if (!gameWon)
		{
			switch(Application.loadedLevel)
			{
			case 1: 
				if(objectivesCompleted == null)
				{
					objectivesCompleted = new bool[]{false, false};
					objectivesCompletedText = new string[] {"", ""};
				}
				if(!objectivesCompleted[0] && GameRef.TutorialReference.TutorialStep >= 14)
				{
					objectivesCompleted[0] = true;
					objectivesCompletedText[0] = " (Completed)";
				}
				if(!objectivesCompleted[1] && GameRef.ComputerCharacters.Count == 0)
				{
					objectivesCompleted[1] = true;
					objectivesCompletedText[1] = " (Completed)";
				}
				if (objectivesCompleted[0] && objectivesCompleted[1])
				{
					gameWon = true;
					GameRef.Paused = true;
				}

				break;
			case 2: 
				if(objectivesCompleted == null)
				{
					objectivesCompleted = new bool[]{false};
					objectivesCompletedText = new string[] {""};
				}
				if(!objectivesCompleted[0] && GameRef.ComputerCharacters.Count == 0)
				{
					objectivesCompleted[0] = true;
					objectivesCompletedText[0] = " (Completed)";

				}
				if (objectivesCompleted[0])
				{
					gameWon = true;
					GameRef.Paused = true;
				}
				
				break;
			case 3: 
				if(objectivesCompleted == null)
				{
					objectivesCompleted = new bool[]{false};
					objectivesCompletedText = new string[] {""};
				}
				if(!objectivesCompleted[0] && GameRef.ComputerCharacters.Count == 0)
				{
					objectivesCompleted[0] = true;
					objectivesCompletedText[0] = " (Completed)";
				}
				if (objectivesCompleted[0])
				{
					gameWon = true;
					GameRef.Paused = true;
				}
				
				break;
			default: break;
			}
		}
	}

	private void MissionFailCheck()
	{
		if (!gameWon)
		{
			if (GameRef.PlayerCharacters.Count < 4)
			{
				gameFailed = true;
				GameRef.Paused = true;
			}
			switch(Application.loadedLevel)
			{
			case 1: 

				break;
			case 2: 
				
				break;
			case 3: 
				if (GameRef.NeutralCharacters.Count < 2)
				{
					gameFailed = true;
					GameRef.Paused = true;
				}
				break;
			case 4: 
				
				break;
			default: 

				break;
			}
		}
	}

	private void MainMenuButton()
	{

		int areaWidth = 100;
		int areaHeight = 30;
		int segmentHeight = 300;
		int segmentWidth = 300;
		int xPosition = (Screen.width/2) + (segmentWidth/2) - areaWidth;
		int yPosition = (Screen.height/2) + (segmentHeight/2) - areaHeight;
		Rect buttonRect = new Rect(xPosition-areaWidth, yPosition, areaWidth, areaHeight);

		if (GUI.Button (buttonRect, "Main Menu"))
		{
			Application.LoadLevel(5);
		}
	}
}
