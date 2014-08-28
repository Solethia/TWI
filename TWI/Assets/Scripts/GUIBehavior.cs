using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIBehavior : MonoBehaviour {

	private Tile tileHovering;
	
	private Point gridCoord;
	private Point lastGridCoord;

	private bool newTileMouseover = false;

	private Transform thisTransform;

	private bool hasCharacterSelected = false;
	private bool showSuccessTooltip = false;
	private Character selectedCharacter;

	[SerializeField]
	private GameObject[] mouseOverBorders;
	[SerializeField]
	private GameObject[] shootIcons;
	[SerializeField]
	private GameObject[] stabIcons;
	[SerializeField]
	private GameObject selectedVisual;

	[SerializeField]
	private GameObject drawPathVisual;

	private GameObject activeMouseOverVisual;

	private Rect interfaceBox;

	public Tile TileHovering
	{
		//set {tileHovering = value;}
		get {return tileHovering;}
	}

	public Character SelectedCharacter
	{
		set 
		{
			selectedCharacter = value;
			if (value == null) {hasCharacterSelected = false;}
			else {hasCharacterSelected = true;}
		}
		get {return selectedCharacter;}
	}

	public bool NewTileMouseover
	{
		set {newTileMouseover = value;}
		get {return newTileMouseover;}
	}

	public bool HasCharacterSelected
	{
		set {hasCharacterSelected = value;}
		get {return hasCharacterSelected;}
	}

	// Use this for initialization
	void Awake () 
	{
		interfaceBox = new Rect (0,Screen.height - 74,Screen.width,74);
		thisTransform = transform;
		GameRef.GUIBehaviorReference = this;

		foreach (GameObject mouseoverBorder in mouseOverBorders)
		{
			mouseoverBorder.renderer.enabled = false;
		}
		foreach (GameObject shootIcon in shootIcons)
		{
			shootIcon.renderer.enabled = false;
		}
		foreach (GameObject stabIcon in stabIcons)
		{
			stabIcon.renderer.enabled = false;
		}
		selectedVisual.renderer.enabled = false;
		mouseOverBorders[0].renderer.enabled = true;
		activeMouseOverVisual = mouseOverBorders[0];

	}

	private Abilities Abilities;
	private Path currentPath;
	public Path CurrentPath
	{
		get {return currentPath;}
		//set {currentPath = value;}
	}

	private void Start()
	{
		Abilities = GameRef.AbilitiesReference;
		GameRef.OnNewTurn();
	}

	// Update is called once per frame
	void Update () 
	{
		if (!GameRef.Paused)
		{
			TileMouseOver();
			PlayerPathfinding();
			UpdateMouseOverVisuals();
			TileMouseClick();
			KeyboardCommands();
		}
	}

	private void TileMouseOver()
	{
		RaycastHit2D rayHit =  Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		gridCoord = new Point(Mathf.FloorToInt(rayHit.point.x), Mathf.FloorToInt(rayHit.point.y));

		if ((gridCoord.X != lastGridCoord.X || gridCoord.Y != lastGridCoord.Y) &&
		    ShadowCaster.IsWithinMap(gridCoord.X, gridCoord.Y, GameRef.GridWidth, GameRef.GridHeight)) 
		{
			newTileMouseover = true;
			tileHovering = GameRef.GetTile(gridCoord);
		}
		else 
		{
			newTileMouseover = false;
		}

		lastGridCoord = gridCoord;
	}

	private void PlayerPathfinding()
	{
		if (hasCharacterSelected)
		{
			if (newTileMouseover)
			{
				if (currentPath != null)
				{
					currentPath.Clear();
				}
				if (interfaceAbilitiesGridSelect != 0 && !TileHovering.WallTile)
				{
					currentPath = Pathfinding.FindPathTwo(selectedCharacter.CurrentTile.Coordinates, tileHovering.Coordinates, Pathfinding.PathType.action);
				}
				else if (tileHovering.Traversable)
				{
					currentPath = Pathfinding.FindPathTwo(selectedCharacter.CurrentTile.Coordinates, tileHovering.Coordinates, Pathfinding.PathType.move);
					if (SelectedCharacter.CanMove(TileHovering, CurrentPath, false))
					{
						CurrentPath.Draw(drawPathVisual);
					}
				}
			}
		}
		else if (currentPath != null)
		{
			CurrentPath.Clear();
		}
		
	}
	
	private void UpdateMouseOverVisuals()
	{
		if (NewTileMouseover)
		{
			//Move Mouse Over Icon to the correct position

			thisTransform.position = tileHovering.ThisTransform.position;
			//Color the icon, depending on the Game & Tile state
			if (hasCharacterSelected)
			{
				switch (interfaceAbilitiesGridSelect)
				{
				case 0:MouseOverBorder();break;
				default:MouseOverAction();break;
				}
			}
			else
			{
				MouseOverBorder();
			}
		}
	}

	private void MouseOverBorder()
	{
		showSuccessTooltip = false;
		if (tileHovering.HasCharacter)
		{
			ActivateTileVisual(mouseOverBorders[0]);
		}
		else if (!tileHovering.Traversable)
		{
			ActivateTileVisual(mouseOverBorders[2]);
		}
		else if (hasCharacterSelected)
		{
			ActivateTileVisual(mouseOverBorders[1]);
		}
		else
		{
			ActivateTileVisual(mouseOverBorders[0]);
		}
	}

	private void MouseOverAction()
	{
		if (SelectedCharacter.CanUseAbility(interfaceAbilitiesGridSelect, TileHovering, CurrentPath, false))
		{
			ActivateTileVisual(shootIcons[0]);
			showSuccessTooltip = true;
		}
		else
		{
			ActivateTileVisual(shootIcons[1]);
			showSuccessTooltip = false;
		}
	}

	private void ActivateTileVisual(GameObject VisualToActivate)
	{
		if (activeMouseOverVisual != VisualToActivate)
		{
			activeMouseOverVisual.renderer.enabled = false;
			activeMouseOverVisual = VisualToActivate;
			VisualToActivate.renderer.enabled = true;
		}
	}

	private void TileMouseClick()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!IsMouseHoveringGUI())
			{
				if (hasCharacterSelected)
				{
					if (tileHovering.CharacterOnTile == selectedCharacter)
					{
						DeselectCharacter();
					}
					else if (tileHovering.CharacterOnTile != null && TileHovering.CharacterOnTile.Friendly)
					{
						DeselectCharacter();
						SelectHoveringCharacter();
					}
				}
				else
				{
					if (TileHovering.HasCharacter && TileHovering.CharacterOnTile.Friendly)
					{
						SelectHoveringCharacter();
					}
				}
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			if (!IsMouseHoveringGUI())
			{
				if (hasCharacterSelected)
				{
					if (interfaceAbilitiesGridSelect == 0)
					{
						if (SelectedCharacter.CanMove(TileHovering, CurrentPath, true)) 
						{
							MoveCharacter();
							GameRef.FogOfWarReference.ComputePlayerSight();
						}
					}
					else
					{
						if (SelectedCharacter.CanUseAbility(interfaceAbilitiesGridSelect, tileHovering, CurrentPath, true))
						{
							SelectedCharacter.UseAbility(interfaceAbilitiesGridSelect, tileHovering, CurrentPath);
						}

					}
				}
			}
		}
	}

	private bool IsMouseHoveringGUI()
	{
		Vector2 GUIMousePosition = new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		if(hasCharacterSelected && interfaceBox.Contains(GUIMousePosition))
		{//if mouse is inside the GUI interface.
			return true;
		}
		else
		{
			return false;
		}
	}

	private void MoveCharacter()
	{
		SelectedCharacter.Move(TileHovering, CurrentPath.Lenght);
		//move selected visuals
		selectedVisual.transform.position = new Vector3(TileHovering.Coordinates.X + 0.5f, TileHovering.Coordinates.Y + 0.5f, 0);
		CurrentPath.Clear();
	}

	private void SelectHoveringCharacter()
	{
		if (GameRef.TutorialReference.TutorialStep >= 2 || Application.loadedLevel != 1)
		{
			hasCharacterSelected = true;
			selectedCharacter = tileHovering.CharacterOnTile;
			selectedVisual.renderer.enabled = true;
			selectedVisual.transform.position = new Vector3(tileHovering.Coordinates.X + 0.5f, tileHovering.Coordinates.Y + 0.5f, 0);
			if (selectedCharacter.Friendly){interfaceAbilitiesGridSelect = 0;}
			else {interfaceAbilitiesGridSelect = 99;}
			//Tutorial
			if (GameRef.TutorialReference.TutorialStep == 2 && Application.loadedLevel == 1)
			{
				GameRef.TutorialReference.StepCompleted();
			}
		}
	}

	public void DeselectCharacter()
	{
		hasCharacterSelected = false;
		selectedCharacter = null;
		selectedVisual.renderer.enabled = false;
	}

	private void KeyboardCommands()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log ("E was pressed");
			GameRef.OnNewTurn();

		}
	}

	//Interface
	private int interfaceAbilitiesGridSelect = 0;

	[SerializeField]
	public GUISkin interfaceGUISkin;
	[SerializeField]
	public GUISkin interfaceRedGUISkin;
	[SerializeField]
	public GUISkin tooltipGUISkin;
	[SerializeField]
	public GUISkin messageGUISkin;

	private void OnGUI() 
	{
		if (!GameRef.Paused)
		{
			GUI.skin = interfaceGUISkin;
			CharacterInterface();
			NextTurnButton();
			GUI.skin = tooltipGUISkin;
			CharacterTooltip();
			SuccessTooltip();
			GUI.skin = messageGUISkin;
			DisplayMessages();
		}

	}

	private void CharacterInterface()
	{
		if (hasCharacterSelected)
		{
			GUI.Box (interfaceBox, " ");
			//STATS
			GUI.Label(new Rect(5, Screen.height - 69, 64, 64), SelectedCharacter.ClassIcon);
			//Ability Select
			if (selectedCharacter.Friendly)
			{
				GUI.Box (new Rect(5+64+10, Screen.height - 69, 32, 64), "AP\n" + SelectedCharacter.ActionPoints + "/" + SelectedCharacter.MaxActionPoints);
				GUI.Box (new Rect(79+32+10, Screen.height - 69, 64, 64), "HP\n" + SelectedCharacter.HealthPoints + "/" + SelectedCharacter.MaxHealthPoints);
				interfaceAbilitiesGridSelect = GUI.SelectionGrid(new Rect(121+64+10, Screen.height - 69, 636, 64), interfaceAbilitiesGridSelect, selectedCharacter.Abilities, 4, "textarea");
			}
		}
	}

	private void NextTurnButton()
	{
		if (!HasCharacterSelected)
		{
			GUI.Box(new Rect(Screen.width - 140, Screen.height - 74, 140, 74), " ");
		}
		if (GUI.Button(new Rect(Screen.width - 120, Screen.height - 69, 100, 64), "Next Turn"))
		{
			if (GameRef.TutorialReference.TutorialStep == 6 || 
			    GameRef.TutorialReference.TutorialStep >= 13 ||
			    Application.loadedLevel != 1)
			{
				GameRef.OnNewTurn();
				if (GameRef.TutorialReference.TutorialStep == 6 && Application.loadedLevel == 1)
				{
					GameRef.TutorialReference.StepCompleted();
				}
			}
		}
	}

	private void CharacterTooltip()
	{

		//Ability Tooltip
		if (!string.IsNullOrEmpty(GUI.tooltip))
		{
			int abilityID = int.Parse(GUI.tooltip);
			int classID = SelectedCharacter.CharClass;
			string tooltipText = Abilities.TooltipText(classID, abilityID);
			int areaHeight = int.Parse (Abilities.TooltipHeight(classID, abilityID));
			int areaWidth = 320;

			NewTooltip(areaWidth, areaHeight, tooltipText); 
		}
	}

	private void SuccessTooltip()
	{
		if (showSuccessTooltip)
		{
			int successChance = SelectedCharacter.AbilitySucessChance(interfaceAbilitiesGridSelect, TileHovering, CurrentPath);
			switch(GameRef.GameMode)
			{
			case 1:
				if (!(SelectedCharacter.CharClass == 3 && interfaceAbilitiesGridSelect == 3))
				{

					string tooltipText = "This action has <color=green>" + successChance + "%</color> chance to succeed, and <color=red>" + (100 - successChance) +"%</color> chance to fail.";
					NewTooltip(200, 60, tooltipText);
				}
				else
				{
					string tooltipText = "This action has <color=green>28%</color> chance to hit the targeted tile, and <color=red>72%</color> chance to hit an adjacent tile.";
					NewTooltip(200, 60, tooltipText);
				}
				break;
			case 2:
				if (!(SelectedCharacter.CharClass == 3 && interfaceAbilitiesGridSelect == 3))
				{
					string probabilitySuccess = NumericalToVerbalProbability(successChance);
					string probabilityFail = NumericalToVerbalProbability(100 - successChance);
					string tooltipText = "It is  <color=green>" + probabilitySuccess + "</color> this action will succeed, and <color=red>" + probabilityFail +"</color> it will fail.";
					NewTooltip(200, 60, tooltipText);
				}
				else
				{
					string tooltipText = "It is <color=green>unlikely</color> this action will hit the targeted tile, and <color=red>likely</color> it will hit an adjacent tile.";
					NewTooltip(200, 60, tooltipText);
				}
				break;
			case 3:
				//TODO Graphical Representation
				if (!(SelectedCharacter.CharClass == 3 && interfaceAbilitiesGridSelect == 3))
				{
					NewGraph(200, 80, "Action Success Chance", "Action Failure Chance", successChance);


				}
				else
				{
					NewGraph(200, 80, "Chance to Hit Targeted Square", "Chance to Hit Adjacent Square", 28);
				}
				break;
			}

		}
	}

	private string NumericalToVerbalProbability(int probability)
	{
		string[] verbalProbability = new string[]{"Virtually Certain", "Very Likely", "Likely", "About as Likely as Not", "Unlikely", "Very Unlikely", "Exceptionally Unlikely"};
		string probabilityExpression;
		if (probability < 1){probabilityExpression = verbalProbability[6];}
		else if (probability < 10){probabilityExpression = verbalProbability[5];}
		else if (probability < 33){probabilityExpression = verbalProbability[4];}
		else if (probability < 66){probabilityExpression = verbalProbability[3];}
		else if (probability < 90){probabilityExpression = verbalProbability[2];}
		else if (probability < 99){probabilityExpression = verbalProbability[1];}
		else {probabilityExpression = verbalProbability[0];}
		return probabilityExpression;
	}

	private void NewTooltip(int areaWidth, int areaHeight, string tooltipText)
	{
		//[0][1]
		//[2][3]


		Vector2 mouseposition = new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		Rect[] tooltipRect = new Rect[]
		{
			new Rect(mouseposition.x-areaWidth, mouseposition.y-areaHeight, areaWidth, areaHeight),
			new Rect(mouseposition.x, mouseposition.y-areaHeight, areaWidth, areaHeight),
			new Rect(mouseposition.x-areaWidth, mouseposition.y, areaWidth, areaHeight),
			new Rect(mouseposition.x, mouseposition.y, areaWidth, areaHeight)
		};
		
		if (RectIsWithinScreen(tooltipRect[0])){GUI.Label (tooltipRect[0], tooltipText, "textarea");}
		else if (RectIsWithinScreen(tooltipRect[1])){GUI.Label (tooltipRect[1], tooltipText, "textarea");}
		else if (RectIsWithinScreen(tooltipRect[2])){GUI.Label (tooltipRect[2], tooltipText, "textarea");}
		else if (RectIsWithinScreen(tooltipRect[3])){GUI.Label (tooltipRect[3], tooltipText, "textarea");}
	}

	private void NewGraph(int areaWidth, int areaHeight, string gText, string rText, int success)
	{
		//[0][1]
		//[2][3]
		
		
		Vector2 mouseposition = new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		Rect[] tooltipRect = new Rect[]
		{
			new Rect(mouseposition.x-areaWidth, mouseposition.y-areaHeight, areaWidth, areaHeight),
			new Rect(mouseposition.x, mouseposition.y-areaHeight, areaWidth, areaHeight),
			new Rect(mouseposition.x-areaWidth, mouseposition.y, areaWidth, areaHeight),
			new Rect(mouseposition.x, mouseposition.y, areaWidth, areaHeight)
		};
		int direction;
		int margin = 2;
		if (RectIsWithinScreen(tooltipRect[0]))
		{
			direction = 0;

		}
		else if (RectIsWithinScreen(tooltipRect[1]))
		{
			direction = 1;
		}
		else if (RectIsWithinScreen(tooltipRect[2]))
		{
			direction = 2;
		}
		else
		{
			direction = 3;
		}
		float totalWidth = tooltipRect[direction].width - (2*margin);
		float decimalSuccess = (float)success / 100.0f;
		float successAsFloat = decimalSuccess*totalWidth;
		GUI.Label (tooltipRect[direction], "", "textarea");
		Rect gBox = new Rect(tooltipRect[direction].xMin + margin, tooltipRect[direction].yMin + margin, successAsFloat, 40 - (2*margin)); 
		Rect rBox = new Rect(tooltipRect[direction].xMin + margin + gBox.width, tooltipRect[direction].yMin + margin, totalWidth - successAsFloat, 40 - (2*margin));
		Rect gIndicator = new Rect((tooltipRect[direction].xMin + (2*margin)) + 6, (gBox.yMax + (2*margin)) + 6, 4, 4);
		Rect rIndicator = new Rect((tooltipRect[direction].xMin + (2*margin)) + 6, ((gBox.yMax + 20) + (2*margin)) + 6, 4, 4);
		Rect gIndiText = new Rect((tooltipRect[direction].xMin + (2*margin)), (gBox.yMax + (2*margin)), areaWidth - (4*margin) + 12, 16);
		Rect rIndiText = new Rect((tooltipRect[direction].xMin + (2*margin)), ((gBox.yMax + 20) + (2*margin)), areaWidth - (4*margin) + 12, 16);

		GUI.Box (gBox, "", "gpgreen");
		GUI.Box (gIndicator, "", "gpgreen");
		GUI.Box (rBox, "", "gpred");
		GUI.Box (rIndicator, "", "gpred");
		GUI.Label(gIndiText, gText, "gptext");
		GUI.Label(rIndiText, rText, "gptext");


		//Debug.Log("Success: " + success + " | totalWidth: " + totalWidth + " | decimalSuccess: " + decimalSuccess + "| SuccessAsFloat: " + successAsFloat);
	}

	private bool RectIsWithinScreen(Rect rect)
	{
		bool botLeft, botRight, topLeft, topRight;
		botLeft = PointIsWithinScreen(rect.xMin,rect.yMax);
		botRight = PointIsWithinScreen(rect.xMax,rect.yMax);
		topLeft = PointIsWithinScreen(rect.xMin,rect.yMin);
		topRight = PointIsWithinScreen(rect.xMax,rect.yMin);

		return botLeft && botRight && topLeft && topRight;
	}

	private bool PointIsWithinScreen(float x, float y)
	{
		if (x >= 0 && y>=0 && x<=Screen.width && y<=Screen.height)
		{
			return true;
		}
		return false;
	}


	private string message = " ";
	public string Message 
	{
		set 
		{
			message = value;
			console += ("\n" + message);
		}
	}
	private string console = " ";
	private bool toggleConsole = false;


	private void DisplayMessages()
	{
		//Message Bar
		GUILayout.BeginArea(new Rect((Screen.width / 4), 0, (Screen.width / 2), 80));
		GUILayout.Label(message, "textfield");
		GUILayout.EndArea();
		//Console
		GUI.Box (new Rect((Screen.width / 4) - 25, 0, 21, 20), " ");
		toggleConsole = GUI.Toggle(new Rect((Screen.width / 4) - 22, -1, 20, 20), toggleConsole, " ");
		if (toggleConsole)
		{
			Rect area = new Rect ((Screen.width / 4), 20, (Screen.width / 2), Screen.height - 220);
			GUILayout.BeginArea(new Rect(area));
			Vector2 scrollPosition = new Vector2(area.x, area.y);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(area.width), GUILayout.Height(area.height));
			GUILayout.Label(console, "textfield");
			GUILayout.EndScrollView();


			//GUILayout.Label(console, "textfield");
			GUILayout.EndArea();
		}
	}
	
}
