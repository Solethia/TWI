using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour {

	public enum control
	{
		player,
		computer,
		neutral
	}
	public control controlType;

	[SerializeField]
	protected GameObject shootEffect;
	[SerializeField]
	protected GameObject stabEffect;
	[SerializeField]
	protected GameObject missEffect;
	[SerializeField]
	protected GameObject deathEffect;

	[SerializeField]
	protected float healthModifier = 1;
	[SerializeField]
	protected float damageModifier = 1;

	protected Transform _transform;
	
	private Tile currentTile;
	public Tile CurrentTile
	{
		set {currentTile = value;}
		get {return currentTile;}
	}

	[SerializeField]
	private Character[] linkedWith;
	public Character[] LinkedWith
	{
		get {return linkedWith;}
	}

	[SerializeField]
	private bool friendly;
	public bool Friendly
	{
		set {friendly = value;}
		get {return friendly;}
	}

	[SerializeField]
	private bool aiControlled;
	public bool AIControlled
	{
		get {return aiControlled;}
	}

	private GUIContent classIcon;
	public GUIContent ClassIcon
	{
		set {classIcon = value;}
		get {return classIcon;}
	}

	private GUIContent[] abilities;
	public GUIContent[] Abilities
	{
		set {abilities = value;}
		get {return abilities;}
	}

	private int visualSightRange = 6;
	public int VisualSightRange
	{
		set {visualSightRange = value;}
		get {return visualSightRange;}
	}

	private int maxActionPoints = 5;
	public int MaxActionPoints
	{
		set {maxActionPoints = value;}
		get {return maxActionPoints;}
	}

	private int maxHealthPoints = 100;
	public int MaxHealthPoints
	{
		set {maxHealthPoints = value;}
		get {return maxHealthPoints;}
	}
	private int timesBandaged = 0;
	public int TimesBandaged
	{
		set {timesBandaged = value;}
		get {return timesBandaged;}
	}

	private int charClass;
	public int CharClass
	{
		set {charClass = value;}
		get {return charClass;}
	}

	private int healthPoints;
	public int HealthPoints
	{
		set 
		{
			healthPoints = value;
			if (healthPoints > maxHealthPoints)
			{
				healthPoints = MaxHealthPoints;
			}
			healthBar.Health = (float)(1 - ((float)healthPoints / maxHealthPoints));
			if (healthPoints < 1)
			{
				OnDeath();
			}
		}
		get {return healthPoints;}
	}

	//Class specific bonuses
	private int plusDamageAllWeapons = 0;
	public int PlusDamageAllWeapons
	{
		set {plusDamageAllWeapons = value;}
		get {return plusDamageAllWeapons;}
	}
	private int plusDamageMeleeWeapons = 0;
	public int PlusDamageMeleeWeapons
	{
		set {plusDamageMeleeWeapons = value;}
		get {return plusDamageMeleeWeapons;}
	}
	private int plusAccuracyRangedWeapons = 0;
	public int PlusAccuracyRangedWeapons
	{
		set {plusAccuracyRangedWeapons = value;}
		get {return plusAccuracyRangedWeapons;}
	}


	private CutoffHealthbarController healthBar;
	
	private int actionPoints;
	public int ActionPoints
	{
		set {actionPoints = value;}
		get {return actionPoints;}
	}

	protected abstract void SetClass();
	protected abstract void SetStats();
	protected abstract void SetAbilities();


	//[0]Sniper//[1]medic//[2]commandoKyle//[3]CommandoYamato
	private Point[] lastSeenAt = new Point[4]{new Point(99,99), new Point(99,99), new Point(99,99), new Point(99,99)};
	public Point[] LastSeenAt
	{
		set {lastSeenAt = value;}
		get {return lastSeenAt;}
	}

	private void Awake()
	{
		if (controlType == control.player)
		{
			if (GameRef.PlayerCharacters == null) {GameRef.PlayerCharacters = new List<Character>();}
			GameRef.PlayerCharacters.Add (this);
		}
		else if (controlType == control.computer)
		{
			if (GameRef.ComputerCharacters == null) {GameRef.ComputerCharacters = new List<Character>();}
			GameRef.ComputerCharacters.Add (this);
		}
		else if (controlType == control.neutral)
		{
			if (GameRef.NeutralCharacters == null) {GameRef.NeutralCharacters = new List<Character>();}
			GameRef.NeutralCharacters.Add (this);
		}
	}

	private void Start () 
	{
		_transform = transform;
		healthBar = GetComponentInChildren(typeof(CutoffHealthbarController)) as CutoffHealthbarController;
		//New Turn Delegate Setup
		GameRef.OnNewTurn += NewTurnEvent;
		if (!friendly && aiControlled) 
		{
			GameRef.OnPlayerMove += PlayerMoveEvent;
		}
		if(currentTile == null)
		{
			Point gridCoord = new Point(Mathf.FloorToInt(_transform.position.x), Mathf.FloorToInt(_transform.position.y));
			currentTile = GameRef.GetTile(gridCoord.X, gridCoord.Y);
			currentTile.CharacterOnTile = this;
		}

		//Setup Character
		SetClass();
		SetStats();
		SetAbilities();

	}

	protected virtual void NewTurnEvent()
	{
		ActionPoints = maxActionPoints;
	}

	protected virtual void PlayerMoveEvent(Character playerCharacter)
	{
		Point playerMoveCoord = playerCharacter.currentTile.Coordinates;
		if (playerCharacterVisible(playerMoveCoord))
		{
			int index = GameRef.PlayerCharacters.IndexOf(playerCharacter);
			lastSeenAt[index] = playerMoveCoord;
		}
	}

	private bool playerCharacterVisible(Point playerMoveCoord)
	{
		int width = GameRef.GridWidth;
		int height = GameRef.GridHeight;
		
		bool[,] lit = new bool[width, height];
		int radius = visualSightRange;
		ShadowCaster.ComputeFieldOfViewWithShadowCasting(
			currentTile.Coordinates.X, currentTile.Coordinates.Y, radius, width, height,
			(x1, y1) => ShadowCaster.IsWithinMap(x1,y1,width,height) && GameRef.GridManagerReference.GetTile(x1, y1).WallTile == true,
			(x2, y2) => {if (ShadowCaster.IsWithinMap(x2,y2,width,height)){lit[x2, y2] = true; }});
			
		int x = playerMoveCoord.X;
		int y = playerMoveCoord.Y;
			return lit[x,y];
	}

	public bool CanUseAbility(int ability, Tile targetedTile, Path attackPath, bool writeMessage)
	{
		switch (ability)
		{
		case 1: return CanShoot (targetedTile, attackPath,  writeMessage);
		case 2: return CanStab (targetedTile, attackPath, writeMessage);
		default: return CanSpecialAbility(targetedTile, attackPath, writeMessage);
		}
	}

	public void UseAbility(int ability, Tile targetedTile, Path attackPath)
	{
		if (friendly)
		{
			if (GameRef.ComputerAIReference.PointsOfInterest == null) {GameRef.ComputerAIReference.PointsOfInterest = new List<Point>();}
			if (!GameRef.ComputerAIReference.PointsOfInterest.Contains(CurrentTile.Coordinates)) {GameRef.ComputerAIReference.PointsOfInterest.Add(CurrentTile.Coordinates);}
		}
		switch (ability)
		{
		case 1: Shoot (targetedTile, attackPath);break;
		case 2: Stab (targetedTile);break;
		default: SpecialAbility(targetedTile, attackPath);break;
		}
	}

	public int AbilitySucessChance(int ability, Tile targetedTile, Path attackPath)
	{
		switch (ability)
		{
		case 1: return ShootAccuracy(attackPath);
		case 2: return StabAccuracy();
		default: return SpecialAccuracy(attackPath, targetedTile.CharacterOnTile);
		}
	}

	public bool CanMove(Tile targetedTile, Path movePath, bool writeMessage)
	{
		//returns true if the character can move to the selected tile

		Point rightMoveCord = new Point(3,4);
		//Is targeted tile visible?
		if (targetedTile.Fog)
		{ErrorMessage(4, writeMessage); return false;}
		//Is targeted tile traversable?
		if (!targetedTile.Traversable)
		{ErrorMessage(9, writeMessage); return false;}
		//Does a path to the targeted tile exist?
		if (!movePath.Exists)
		{ ErrorMessage(10, writeMessage); return false;}
		//Does the character have enough actionpoints to get there?
		if (ActionPoints <= movePath.Lenght - 1)
		{ErrorMessage(2, writeMessage); return false;}

		//Right move in tutorial Or tutorial cleared
		if (Application.loadedLevel == 1)
		{
			if (GameRef.TutorialReference.TutorialStep == 4)
			{
				if (targetedTile.Coordinates == rightMoveCord)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else if (GameRef.TutorialReference.TutorialStep < 13)
			{
				return false;
			}
		}
		return true;
	}

	public void Move(Tile targetedTile, int pathLenght)
	{
		GameRef.PlaySound.Walk();
		//Spend Action Points
		ActionPoints -= pathLenght; 
		//Remove from current tile
		CurrentTile.HasCharacter = false;
		CurrentTile.CharacterOnTile = null;
		//Add to new tile
		targetedTile.HasCharacter = true;
		targetedTile.CharacterOnTile = this;
		CurrentTile = targetedTile;
		//New position
		Vector3 newPos = new Vector3(targetedTile.Coordinates.X + 0.5f, targetedTile.Coordinates.Y + 0.5f, 0);
		//Move character visuals
		_transform.position = newPos;
		Renderer[] charRenderers = _transform.GetComponentsInChildren<Renderer>() as Renderer[]; 

		if (targetedTile.Fog)
		{
			foreach (Renderer charRenderer in charRenderers) 
			{ 
				charRenderer.enabled = false;
			}
		}
		else
		{
			foreach (Renderer charRenderer in charRenderers) 
			{ 
				charRenderer.enabled = true;
			}
		}
		if (Application.loadedLevel == 1)
		{
			if (GameRef.TutorialReference.TutorialStep == 4)
			{
				Point highlightedTile = new Point(3,4);
				Debug.Log("TC: " + targetedTile.Coordinates.X + ", " + targetedTile.Coordinates.Y);
				if (targetedTile.Coordinates == highlightedTile)
				{
					GameRef.TutorialReference.StepCompleted();
				}
			}
		}

	}


	protected abstract bool CanSpecialAbility(Tile targetedTile, Path attackPath, bool writeMessage);
	protected abstract void SpecialAbility(Tile targetedTile, Path attackPath);

	private int shootCost = 3;
	private int stabCost = 3;
	private bool CanShoot(Tile targetedTile, Path attackPath, bool writeMessage)
	{

		int range = 5;

		Character targetedCharacter = targetedTile.CharacterOnTile;

		if (targetedTile.Fog || !targetedTile.HasCharacter || targetedCharacter.Friendly)
		{ErrorMessage(1, writeMessage); return false; }
		
		if (ActionPoints < shootCost)
		{ErrorMessage(2, writeMessage); return false;}
		
		if (attackPath.Lenght > range)
		{ErrorMessage(3, writeMessage); return false;}

		if (Application.loadedLevel == 1)
		{
			if (GameRef.TutorialReference.TutorialStep < 14 && GameRef.TutorialReference.TutorialStep != 8)
			{ErrorMessage(7, writeMessage);return false; }
		}

		return true;
	}

	private bool CanStab(Tile targetedTile, Path attackPath, bool writeMessage)
	{
		int range = 1;

		Character targetedCharacter = targetedTile.CharacterOnTile;
		
		if (targetedTile.Fog || !targetedTile.HasCharacter || targetedCharacter.Friendly)
		{ErrorMessage(1, writeMessage); return false; }
		
		if (ActionPoints < stabCost)
		{ErrorMessage(2, writeMessage); return false;}
		
		if (attackPath.Lenght > range)
		{ErrorMessage(3, writeMessage); return false;}

		if (Application.loadedLevel == 1)
		{
			if (GameRef.TutorialReference.TutorialStep < 14)
			{ErrorMessage(7, writeMessage);return false; }
		}

		return true;
	}
	
	public abstract int SpecialAccuracy(Path attackPath, Character targetedCharacter);

	//private int baseShootAccuracy = 65;
	public int ShootAccuracy(Path attackPath)
	{
		switch (attackPath.Lenght)
		{
		case 1: return 30+plusAccuracyRangedWeapons;
		case 2: return 50+plusAccuracyRangedWeapons;
		case 3: return 75+plusAccuracyRangedWeapons;
		case 4: return 50+plusAccuracyRangedWeapons;
		default: return 30+plusAccuracyRangedWeapons;
		}
	}

	protected void Shoot(Tile targetedTile, Path attackPath)
	{
		//shoot
		int minDMG = 25;
		int maxDMG = (35 + plusDamageAllWeapons) + 1;

		Character target = targetedTile.CharacterOnTile;

		ActionPoints -= shootCost;
		int accuracyRoll = Random.Range (0, 101);
		if (accuracyRoll <= ShootAccuracy(attackPath))
		{
			int damageDone = Random.Range(minDMG,maxDMG); // DMG 25 - 35 (3-4 successfull hits to kill)
			target.HealthPoints -= (int)((float)damageDone * (float)damageModifier); //Apply Damage, to targeted character
			Vector3 spawnPosition = new Vector3(targetedTile.Coordinates.X + 0.5f, targetedTile.Coordinates.Y + 0.5f, 9);
			GameObject.Instantiate(shootEffect, spawnPosition, Quaternion.identity);
			GameRef.NewMessage = gameObject.name + " shot " + target.name + " for " + damageDone + " damage." ;
			GameRef.PlaySound.Shoot(true);
		}
		else
		{
			Vector3 spawnPosition = new Vector3(targetedTile.Coordinates.X + 0.5f, targetedTile.Coordinates.Y + 0.5f, 9);
			GameObject.Instantiate(missEffect, spawnPosition, Quaternion.identity);
			GameRef.NewMessage = gameObject.name + "'s shot missed " + target.name + "." ;
			GameRef.PlaySound.Shoot(false);
		}
		if (Application.loadedLevel == 1)
		{
			if (GameRef.TutorialReference.TutorialStep == 8)
			{
				GameRef.TutorialReference.StepCompleted();
			}
		}
	}

	private int baseStabAccuracy = 75;
	public int StabAccuracy()
	{
		return baseStabAccuracy;
	}

	protected void Stab(Tile targetedTile)
	{
		//stab
		int minDMG = 25;
		int maxDMG = (35 + plusDamageAllWeapons + plusDamageMeleeWeapons) + 1;

		Character target = targetedTile.CharacterOnTile;

		ActionPoints -= stabCost;
		int accuracyRoll = Random.Range (0, 101);
		if (accuracyRoll <= StabAccuracy())
		{
			int damageDone = Random.Range(minDMG, maxDMG); //Randomize Damage to Apply
			target.HealthPoints -= (int)((float)damageDone * (float)damageModifier); //Apply Damage, to targeted character
			Vector3 spawnPosition = new Vector3(targetedTile.Coordinates.X + 0.5f, targetedTile.Coordinates.Y + 0.5f, 9);
			GameObject.Instantiate(stabEffect, spawnPosition, Quaternion.identity);
			GameRef.NewMessage = gameObject.name + " stabbed " + target.name + " for " + damageDone + " damage." ;
			GameRef.PlaySound.Stab(true);
		}
		else
		{
			GameRef.PlaySound.Stab(false);
			//Debug.Log ("You Rolled " + accuracyRoll + " MISS!");
			Vector3 spawnPosition = new Vector3(targetedTile.Coordinates.X + 0.5f, targetedTile.Coordinates.Y + 0.5f, 9);
			GameObject.Instantiate(missEffect, spawnPosition, Quaternion.identity);
			GameRef.NewMessage = gameObject.name + "'s stab missed " + target.name + "." ;
		}
	}


	private void OnDeath()
	{
		GameRef.PlaySound.Death();
		GameObject.Instantiate(deathEffect, _transform.position, Quaternion.identity);
		if (GameRef.GUIBehaviorReference.SelectedCharacter == this)
		{
			GameRef.GUIBehaviorReference.DeselectCharacter();
		}
		CurrentTile.HasCharacter = false;
		CurrentTile.CharacterOnTile = null;
		if (friendly)
		{
			GameRef.PlayerCharacters.Remove (this);
		}
		else
		{
			GameRef.ComputerCharacters.Remove (this);
		}
		Destroy (gameObject);
	}

	protected void ErrorMessage(int error, bool writeMessage)
	{
		if (writeMessage)
		{
			string errorMessage;
			switch (error)
			{
			case 1: errorMessage = "Not an enemy character.";break;
			case 2: errorMessage = "Not enough action points.";break;
			case 3: errorMessage = "Not in Range.";break;
			case 4: errorMessage = "Not in vision.";break;
			case 5: errorMessage = "Cannot target walls.";break;
			case 6: errorMessage = "Not a friendly character.";break;
			case 7: errorMessage = "Tutorial not cleared yet.";break;
			case 8: errorMessage = "Not the highlighted Tile.";break;
			case 9: errorMessage = "That is not a traversable tile.";break;
			case 10: errorMessage = "Path blocked by other character.";break;
			default: errorMessage = "";break;
			}
			GameRef.NewMessage = errorMessage;
		}
	}


}
