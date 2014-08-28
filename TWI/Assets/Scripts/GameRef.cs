using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameRef 
{
	public static void ResetLevel()
	{
		paused = false;
		playerCharacters = null;
		computerCharacters = null;

	}

	private static bool paused = false;
	public static bool Paused
	{
		set {paused = value;}
		get {return paused;}
	}

	private static GridManager gridManagerReference;
	private static GUIBehavior tileBehaviorReference;
	private static FogOfWar fogOfWarReference;
	private static ComputerAI computerAIReference;
	private static Tutorial tutorialReference;
	private static Sound playSound;

	private static Abilities abilitiesReference;

	private static int gridHeight;
	private static int gridWidth;

	private static int gameMode = 3;
	public static int GameMode
	{
		set {gameMode = value;}
		get {return gameMode;}
	}

	private static List<Character> playerCharacters;
	private static List<Character> computerCharacters;
	private static List<Character> neutralCharacters;

	public static GridManager GridManagerReference
	{
		set {gridManagerReference = value;}
		get {return gridManagerReference;}
	}

	public static GUIBehavior GUIBehaviorReference
	{
		set {tileBehaviorReference = value;}
		get {return tileBehaviorReference;}
	}


	public static Abilities AbilitiesReference
	{
		set {abilitiesReference = value;}
		get {return abilitiesReference;}
	}

	public static FogOfWar FogOfWarReference
	{
		set {fogOfWarReference = value;}
		get {return fogOfWarReference;}
	}

	public static ComputerAI ComputerAIReference
	{
		set {computerAIReference = value;}
		get {return computerAIReference;}
	}

	public static Tutorial TutorialReference
	{
		set {tutorialReference = value;}
		get {return tutorialReference;}
	}

	public static Sound PlaySound
	{
		set {playSound = value;}
		get {return playSound;}
	}

	public static int GridHeight
	{
		set {gridHeight = value;}
		get {return gridHeight;}
	}

	public static int GridWidth
	{
		set {gridWidth = value;}
		get {return gridWidth;}
	}

	public static List<Character> PlayerCharacters
	{
		set {playerCharacters = value;}
		get {return playerCharacters;}
	}

	public static List<Character> ComputerCharacters
	{
		set {computerCharacters = value;}
		get {return computerCharacters;}
	}

	public static List<Character> NeutralCharacters
	{
		set {neutralCharacters = value;}
		get {return neutralCharacters;}
	}

	public delegate void NewTurn();
	public static NewTurn OnNewTurn;

	public delegate void NewPlayerMove(Character playerCharacter);
	public static NewPlayerMove OnPlayerMove;

	public static Tile GetTile(int x, int y)
	{
		return gridManagerReference.GetTile(x,y);
	}
	public static Tile GetTile(Point coordinates)
	{
		return gridManagerReference.GetTile(coordinates.X,coordinates.Y);
	}

	public static string NewMessage
	{
		set {GUIBehaviorReference.Message = value;}
	}
}
