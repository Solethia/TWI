using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	[SerializeField]
	private GameObject highlight;

	private int tutorialStep = 1;
	private int maxSteps = 13;
	public int TutorialStep 
	{
		get {return tutorialStep;}
	}
	[SerializeField]
	private bool isActive;
	public  bool IsActive
	{
		set {isActive = value;}
		get {return isActive;}
	}

	private void Awake()
	{
		GameRef.TutorialReference = this;
		if (!isActive)
		{
			tutorialStep = 15;
		}
	}
	

	int sizeTitle = 18;
	int sizeBody = 18;

	void OnGUI()
	{
		if (isActive)
		{
			GUI.skin = GameRef.GUIBehaviorReference.tooltipGUISkin;
			switch (tutorialStep)
			{
			//Introduction
			case 1:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Introduction</b></size>\n<size=" + sizeBody + ">Welcome to the strategy turn based game; The watchtower Initiative. This tutorial level will introduce you to the main controls of the game. Click continue to continue the tutorial.</size>");
				NextStepButton("Continue");
				break;
			//Select Character
			case 2:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Selecting Units</b></size>\n<size=" + sizeBody + ">In The Watchtower Initiative, friendly units are marked with a blue background and enemy units with a red background. Try selecing a friendly unit by left-clicking it.</size>");

				break;
			case 3:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Selecting Units</b></size>\n<size=" + sizeBody + ">In The Watchtower Initiative, friendly units are marked with a blue background and enemy units with a red background. Try selecing a friendly unit by left-clicking it.\n\nGood Job!</size>");
				NextStepButton("Continue");
				break;
			//Move
			case 4:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Moving</b></size>\n<size=" + sizeBody + ">Now that you have selected a unit, the character menu will be displayed. Each unit have four abilities, move, stab, shoot and a special class ability. Select the move ability and right click the highlighted square to move there.</size>");

				break;
			case 5:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Moving</b></size>\n<size=" + sizeBody + ">Now that you have selected a unit, the character menu will be displayed. Each unit have four abilities, move, stab, shoot and a special class ability. Select the move ability and right click the highlighted square to move there.\n\nNice! You're improving incredibly fast.</size>");
				NextStepButton("Continue");
				break;
				//Next Turn
			case 6:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Action Points</b></size>\n<size=" + sizeBody + ">All abilities consumes a set amount of Action Points. You can see how many Action Points a character has left in the character menu, under AP. You lost a couple of Action Points by moving, replenish your Action Points by clicking the Next Turn button.</size>");

				break;
			case 7:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Action Points</b></size>\n<size=" + sizeBody + ">All abilities consumes a set amount of Action Points. You can see how many Action Points a character has left in the character menu, under AP. You lost a couple of Action Points by moving, replenish your Action Points by clicking the Next Turn button.\n\nWell done!</size>");
				NextStepButton("Continue");
				break;
				//Attacking
			case 8:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Attacking</b></size>\n<size=" + sizeBody + ">Now that you have replenished your action points, you have enough to do an attack. Select the Shoot attack and right click the target dummy which has come into view.</size>");
				break;
			case 9:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Attacking</b></size>\n<size=" + sizeBody + ">Now that you have replenished your action points, you have enough to do an attack. Select the Shoot attack and right click the target dummy which has come into view.\n\n Good Job! </size>");
				NextStepButton("Continue");
				break;
				//Camera Control
			case 10:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Attacking</b></size>\n<size=" + sizeBody + ">Attacks dont always hit, they have a probability to succed. This probability may depend on the range you attack from or the attack you use. Your success in battle will largely depend on using the right attack for the situation. \n\n (Read more by hovering over each ability)</size>");
				NextStepButton("Continue");
				break;
			case 11:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Camera Control</b></size>\n<size=" + sizeBody + ">Moving and attacking are essential, but having a good view of the battle is also important. To move the camera around use the 'W', 'A', 'S', 'D' keys or the arrow keys on your keyboard.</size>");
				NextStepButton("Continue");
				break;
				//Mission Objective
			case 12:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Mission Objective</b></size>\n<size=" + sizeBody + ">To complete a level, you must complete it's Mission Objective. A Mission Objective holds information on what you need to do before you win. To see the Mission Objective press the 'Mission Objective' button on the top-left part of the screen.</size>");
				break;
			case 13:
				NewTutorialSegment("<size=" + sizeTitle + "><b>Tutorial (" + tutorialStep + "/" + maxSteps + ") - Mission Objective</b></size>\n<size=" + sizeBody + ">Congratulations on completing the tutorial, now use what you have learned to complete the Mission Objective.</size>");
				NextStepButton("Close");
				break;
			}
		}
	}

	private GameObject instantiatedHighlight;
	public void StepCompleted()
	{
		switch (tutorialStep)
		{
		case 3:
			//Activate Highlight
			instantiatedHighlight = GameObject.Instantiate(highlight, new Vector3(3.5f, 4.5f, 0), Quaternion.identity) as GameObject;
			break;
		case 4:
			//Deactivate Highlight
			GameObject.Destroy(instantiatedHighlight);
			break;
		}
		tutorialStep++;
		Debug.Log("Step: " + tutorialStep);
	}



	private void NewTutorialSegment(string tooltipText)
	{
		int areaWidth = 300;
		int areaHeight = 300;
		int xPosition = Screen.width - 25;
		int yPosition = (Screen.height/2) - (areaHeight/2);;
		Rect tutorialRect = new Rect(xPosition-areaWidth, yPosition, areaWidth, areaHeight);
		
		GUI.Label (tutorialRect, tooltipText, "textarea");
	}

	private void NextStepButton(string buttonText)
	{
		int areaWidth = 100;
		int areaHeight = 30;
		int tutorialSegmentHeight = 300;
		int xPosition = Screen.width - 30;
		int yPosition = (Screen.height/2) + (tutorialSegmentHeight/2) - areaHeight - 5;
		Rect nextStepRect = new Rect(xPosition-areaWidth, yPosition, areaWidth, areaHeight);

		if (GUI.Button(nextStepRect, buttonText))
		{
			StepCompleted();
		}
	}
}
