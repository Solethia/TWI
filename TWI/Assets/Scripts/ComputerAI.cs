using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public struct TargetPath
{
	public Character PlayerCharacter;
	public Path MoveAIPath;
	
	public TargetPath(Character playerCharacter, Path moveAIPath)
	{
		PlayerCharacter = playerCharacter;
		MoveAIPath = moveAIPath;
	}
}

public class ComputerAI : MonoBehaviour {

	private List<Point> pointsOfInterest;
		
	public List<Point> PointsOfInterest
	{
		set {pointsOfInterest = value;}
		get {return pointsOfInterest;}
	}

	private void Awake()
	{
		GameRef.ComputerAIReference = this;
	}

	void Start () 
	{
		GameRef.OnNewTurn += NewTurnEvent;
	}

	public virtual void NewTurnEvent()
	{
		GameRef.Paused = true;
		StartComputerTurn();

	}

	private void StartComputerTurn()
	{
		//Debug.Log("EChars: " + GameRef.EnemyAICharacters.Count);
		if(GameRef.ComputerCharacters != null)
		{
			foreach (Character computerCharacter in GameRef.ComputerCharacters)
			{
				if (computerCharacter.AIControlled)
				{
					//Debug.Log ("Start Enemy " + computerCharacter.name);
					AICoRoutine(computerCharacter);
				}
			}
			//Debug.Log ("End Enemy Turn");
		}
		GameRef.Paused = false;
	}

	private void AICoRoutine(Character computerCharacter)
	{
		bool doNothing = false;
		while (computerCharacter.ActionPoints > 0 && !doNothing)
		{
			//Debug.Log ("Start Loop " + computerCharacter.name);
			TargetPath[] visiblePlayerCharacters = VisiblePlayerCharacters(computerCharacter);
			//Debug.Log ("visiblePlayerCharacters: " + visiblePlayerCharacters.Length);
			/*foreach(TargetPath charchar in visiblePlayerCharacters)
			{
				Debug.Log("visChar - " + charchar.PlayerCharacter.name);
			}*/
			if (visiblePlayerCharacters.Length > 0)
			{

				TargetPath[] attackablePlayerCharacters = AttackablePlayerCharacters(visiblePlayerCharacters, computerCharacter);
				//Debug.Log ("attackablePlayerCharacters: " + attackablePlayerCharacters.Length);
				/*foreach(TargetPath charchar in attackablePlayerCharacters)
				{
					Debug.Log("attchar - " + charchar.PlayerCharacter.name);
				}*/
				if (attackablePlayerCharacters.Length > 0)
				{

					foreach(TargetPath attPlayerChar in attackablePlayerCharacters)
					{
						//Debug.Log ("attPlayerChar: " + attPlayerChar.PlayerCharacter.name + " | Dist: " + (attPlayerChar.MoveAIPath.Lenght - 1));
						if (attPlayerChar.MoveAIPath.Lenght <= (1 + (computerCharacter.ActionPoints - 3)))
						{
							int debug = (1 + (computerCharacter.ActionPoints - 3));
							Debug.Log ("CanStab: (" + attPlayerChar.MoveAIPath.Lenght + " <= " + debug.ToString() + ") | ActionPoints: " + computerCharacter.ActionPoints);
							//Stab
							if (attPlayerChar.MoveAIPath.Lenght == 1)
							{
								Debug.Log ("Stabbing");
								Path attackPath = Pathfinding.FindPathTwo(computerCharacter.CurrentTile.Coordinates, attPlayerChar.PlayerCharacter.CurrentTile.Coordinates, Pathfinding.PathType.action);
								computerCharacter.UseAbility(2, attPlayerChar.PlayerCharacter.CurrentTile, attackPath);
								//yield return new WaitForSeconds(1.0F);
								break;
							}
							else
							{
								Debug.Log ("MovingCloser");
								MoveCloserToVisibleCharacter(computerCharacter, attPlayerChar);
								break;
							}
						}
						else if (attPlayerChar.MoveAIPath.Lenght <= 5 + (computerCharacter.ActionPoints - 3))
						{
							//Shoot
							Debug.Log ("CanShoot");
							if (attPlayerChar.MoveAIPath.Lenght <= 5)
							{
								Debug.Log ("Shooting");
								Path attackPath = Pathfinding.FindPathTwo(computerCharacter.CurrentTile.Coordinates, attPlayerChar.PlayerCharacter.CurrentTile.Coordinates, Pathfinding.PathType.action);
								computerCharacter.UseAbility(1, attPlayerChar.PlayerCharacter.CurrentTile, attackPath);
								//yield return new WaitForSeconds(1.0F);
								break;
							}
							else
							{
								Debug.Log ("MovingCloser");
								MoveCloserToVisibleCharacter(computerCharacter, attPlayerChar);
								break;
							}
						}
						else
						{
							Debug.Log ("Error, cant attack even though its in attackable | Att");
							doNothing = true;
						}
					}
				}
				else
				{
					//Debug.Log ("NoEnemiesAttackable");
					TargetPath closestPlayerChar = visiblePlayerCharacters.OrderBy(any => any.MoveAIPath.Lenght).ToArray()[0];
					int borderLine = 8;
					if (closestPlayerChar.MoveAIPath.Lenght < borderLine)
					{
						//Debug.Log ("TakeCover");
						//TakeCover
						TakeCover(computerCharacter);
					}
					else if (closestPlayerChar.MoveAIPath.Lenght == borderLine)
					{
						//Debug.Log ("DoNothing");
						doNothing = true;
					}
					else if (closestPlayerChar.MoveAIPath.Lenght > borderLine)
					{
						//MoveCloser
						///Debug.Log ("MoveCloser");
						MoveCloserToVisibleCharacter(computerCharacter, visiblePlayerCharacters[0]);
					}
				}
			}
			else
			{
				//Debug.Log ("NoEnemiesVisible - DoNothing");
				//Consider creating points of interest system
				Point nullablePointValueHack = new Point(99,99);
				if(
					(computerCharacter.LastSeenAt[0] != nullablePointValueHack || computerCharacter.LastSeenAt[1] != nullablePointValueHack || computerCharacter.LastSeenAt[2] != nullablePointValueHack || computerCharacter.LastSeenAt[3] != nullablePointValueHack) 
					&& computerCharacter.ActionPoints > 3
					)
				{
					foreach(Point pointOfInterest in computerCharacter.LastSeenAt)
					{
						if (pointOfInterest != nullablePointValueHack)
						{
							Tile targetTile = GameRef.GetTile(pointOfInterest);
							Path pathToInterestPoint = AIPathToTile(targetTile, computerCharacter);
							MoveCloserToTile(pathToInterestPoint, computerCharacter);
						}
					}
				}
				else
				{
					doNothing = true;
				}

			}
		}
	}

	private TargetPath[] VisiblePlayerCharacters(Character computerCharacter)
	{
		List<Character> visiblePlayerCharacters = new List<Character>();
		List<TargetPath> potentialTargets = new List<TargetPath>();
		if (GameRef.PlayerCharacters != null)
		{
			int width = GameRef.GridWidth;
			int height = GameRef.GridHeight;

			bool[,] lit = new bool[width, height];

			int radius = computerCharacter.VisualSightRange;
			ShadowCaster.ComputeFieldOfViewWithShadowCasting(
				computerCharacter.CurrentTile.Coordinates.X, computerCharacter.CurrentTile.Coordinates.Y, radius, width, height,
				(x1, y1) => ShadowCaster.IsWithinMap(x1,y1,width,height) && GameRef.GridManagerReference.GetTile(x1, y1).WallTile == true,
				(x2, y2) => {if (ShadowCaster.IsWithinMap(x2,y2,width,height)){lit[x2, y2] = true; }});
			if (computerCharacter.LinkedWith != null)
			{
				foreach(Character linkedComputerCharacter in computerCharacter.LinkedWith)
				{
					int radiusLink = computerCharacter.VisualSightRange;
					ShadowCaster.ComputeFieldOfViewWithShadowCasting(
						linkedComputerCharacter.CurrentTile.Coordinates.X, linkedComputerCharacter.CurrentTile.Coordinates.Y, radiusLink, width, height,
						(x1, y1) => ShadowCaster.IsWithinMap(x1,y1,width,height) && GameRef.GridManagerReference.GetTile(x1, y1).WallTile == true,
						(x2, y2) => {if (ShadowCaster.IsWithinMap(x2,y2,width,height)){lit[x2, y2] = true; }});
				}
			}
			foreach (Character playerCharacter in GameRef.PlayerCharacters)
			{
				int x = playerCharacter.CurrentTile.Coordinates.X;
				int y = playerCharacter.CurrentTile.Coordinates.Y;
				if (lit[x,y])
		        {
					visiblePlayerCharacters.Add(playerCharacter);
		        }
			}
			foreach (Character playerCharacter in visiblePlayerCharacters)
			{
				Path PathToPlayer = AIPathToTile(playerCharacter.CurrentTile, computerCharacter);
				if (PathToPlayer.Exists)
				{
					potentialTargets.Add(new TargetPath(playerCharacter, PathToPlayer));
				}

			}
		}
		return potentialTargets.OrderBy(target => target.PlayerCharacter.HealthPoints).ToArray();
	}

	private Path AIPathToTile(Tile targetTile, Character computerCharacter)
	{
		return Pathfinding.FindPathTwo(computerCharacter.CurrentTile.Coordinates, targetTile.Coordinates, Pathfinding.PathType.AImove);
	}

	private void MoveCloserToTile(Path pathToTile, Character computerCharacter)
	{
		Tile moveToTile = GameRef.GetTile(pathToTile.Route[0].X, pathToTile.Route[0].Y);
		Move(computerCharacter, moveToTile);
	}

	private TargetPath[] AttackablePlayerCharacters(TargetPath[] potentialTargets, Character computerCharacter)
	{
		List<TargetPath> attackablePlayerCharacters = new List<TargetPath>();
		
		foreach (TargetPath potentialTarget in potentialTargets)
		{
			if (potentialTarget.MoveAIPath.Exists && computerCharacter.ActionPoints >= 3 && potentialTarget.MoveAIPath.Lenght <= 5 + (computerCharacter.ActionPoints - 3))
			{
				attackablePlayerCharacters.Add(potentialTarget);
			}
		}
		return attackablePlayerCharacters.ToArray();
	
	}

	private void Move(Character computerCharacter, Tile moveToTile)
	{
		computerCharacter.Move(moveToTile, 1);
		//yield return new WaitForSeconds(1.0F);
	}

	private void MoveCloserToVisibleCharacter(Character computerCharacter, TargetPath playerCharacter)
	{
		Tile moveToTile = GameRef.GetTile(playerCharacter.MoveAIPath.Route[0].X, playerCharacter.MoveAIPath.Route[0].Y);
		Move(computerCharacter, moveToTile);
	}

	private void TakeCover(Character computerCharacter)
	{
		bool[,] lit = GameRef.FogOfWarReference.Lit;
		Path PathToCover = FindCoverFloodFill(computerCharacter, lit);
		if (PathToCover.Exists)
		{
			foreach (Point movePoint in PathToCover.Route)
			{
				Tile moveToTile = GameRef.GetTile(movePoint);

				Move(computerCharacter, moveToTile);
			}
		}
		else
		{
			computerCharacter.ActionPoints = 0;
		}
	}

	private Path FindCoverFloodFill(Character computerCharacter, bool[,] lit)
	{
		List<Point> queue = new List<Point>();
		List<Point> visited = new List<Point>();
		Point origin = computerCharacter.CurrentTile.Coordinates;
		queue.Add(origin);
		visited.Add(origin);

		int tilesChecked = 0;
		int pathsChecked = 0;

		if (pathsChecked == 10)

		while (tilesChecked <= lit.Length || pathsChecked <= 10)
		{
			foreach (Point potentialCover in queue)
			{
				tilesChecked++;
				queue.Remove(potentialCover);
				for (int x = -1; x <= 1; x++)
				{
					for (int y = -1; y <= 1; y++)
					{
						Point gridCoord = new Point(potentialCover.X + x, potentialCover.Y + y);
						if (ShadowCaster.IsWithinMap(potentialCover.X + x, potentialCover.Y + y, GameRef.GridWidth, GameRef.GridHeight))
						{
							if (!lit[potentialCover.X + x, potentialCover.Y + y] && !visited.Contains(gridCoord))
							{
								if (GameRef.GetTile(gridCoord).Traversable)
								{
									pathsChecked++;
									Path pathToCover = Pathfinding.FindPathTwo(computerCharacter.CurrentTile.Coordinates, gridCoord, Pathfinding.PathType.move);
									if (pathToCover.Exists && pathToCover.Lenght <= computerCharacter.ActionPoints)
									{
										return pathToCover;
									}
								}
								queue.Add (gridCoord);
								visited.Add(gridCoord);
							}
						}
					}
				}
			}
		}
		return new Path(visited.ToArray(), origin, origin, 0, false);
	}


}
