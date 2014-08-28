/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Point
{
	public int X, Y;
	
	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}
}

public class PathfindingBackup : MonoBehaviour {

	
	private TileBehavior tileBehavior;
	
	private int Cost;
	
	private List<Point> path = new List<Point>();
	private List<Point> openNodes = new List<Point>();
	private List<Point> closedNodes = new List<Point>();
	
	private List<GameObject> pathVisuals = new List<GameObject>();
	
	private Point startNode;
	
	private Point searchNode;
	private Point tempMove;
	
	private bool pathFound = false;
	public bool PathFound
	{
		set {pathFound = value;}
		get {return pathFound;}
	}
	
	[SerializeField]
	private GameObject pathCircle;
	private void Awake()
	{
		GameRef.PathfindingReference = this;
	}
	private void Start()
	{
		tileBehavior = GameRef.TileBehaviorReference;
	}
	
	public void ClearPathfinding()
	{
		pathFound = false;
		closedNodes.Clear();
		path.Clear();
		ClearPath();
	}
	
	public void InitMovePathfinding(bool ignoreInhabited)
	{
		pathFound = false;
		searchNode = tileBehavior.SelectedCharacter.CurrentTile.Coordinates;
		startNode = searchNode;
		closedNodes.Clear();
		path.Clear();
		path.Add (startNode);
		ClearPath();
		FindPath(ignoreInhabited);
	}
	
	private void FindPath(bool ignoreInhabited)
	{
		
		while ( pathFound == false)
		{
			if (searchNode.X == tileBehavior.TileHovering.Coordinates.X && searchNode.Y == tileBehavior.TileHovering.Coordinates.Y)
			{
				pathFound = true;
			}
			else
			{
				CheckNeighbours(ignoreInhabited);
			}
		}
	}
	
	
	private void CheckNeighbours(bool ignoreInhabited)
	{
		Point left = new Point(Mathf.Clamp(searchNode.X - 1, 0, GameRef.GridWidth - 1), searchNode.Y);
		Point right = new Point(Mathf.Clamp(searchNode.X + 1, 0, GameRef.GridWidth - 1), searchNode.Y);
		Point above = new Point(searchNode.X, Mathf.Clamp(searchNode.Y + 1, 0, GameRef.GridHeight - 1));
		Point below = new Point(searchNode.X, Mathf.Clamp(searchNode.Y - 1, 0, GameRef.GridHeight - 1));
		
		Cost = 99999;
		
		closedNodes.Add(searchNode);
		
		if (!ignoreInhabited)
		{
			CalculateMoveEstimate(left);
			CalculateMoveEstimate(right);
			CalculateMoveEstimate(above);
			CalculateMoveEstimate(below);
		}
		else
		{
			CalculateAttackEstimate(left);
			CalculateAttackEstimate(right);
			CalculateAttackEstimate(above);
			CalculateAttackEstimate(below);
		}
		
		if (Cost == 99999)
		{	
			if (path.Count > 1)
			{
				path.Remove(path[path.Count - 1]);
				searchNode = path[path.Count -1];
			}
		}
		else
		{
			path.Add(tempMove);
			searchNode = tempMove;
		}
	}
	
	private void CalculateMoveEstimate(Point tileCoordinate)
	{
		Tile tile = GameRef.GridManagerReference.GetTile(tileCoordinate);
		
		if (tile.Traversable && !closedNodes.Contains(tileCoordinate))
		{
			Point currentTile = tile.Coordinates;
			Point targetTile = tileBehavior.TileHovering.Coordinates;
			
			int G = 10;
			int H = 10 * (Mathf.Abs(currentTile.X - targetTile.X) + Mathf.Abs(currentTile.Y - targetTile.Y));
			
			int F = G + H;
			//Debug.Log(F + " < " + Cost);
			if (F < Cost)
			{
				//Debug.Log ("newTempMove: " + currentTile.X +", "+ currentTile.Y + " | Cost: " + F);
				Cost = F;
				tempMove = currentTile;
			}
		}
		
	}
	
	private void CalculateAttackEstimate(Point tileCoordinate)
	{
		Tile tile = GameRef.GridManagerReference.GetTile(tileCoordinate);
		
		if (tile.Traversable || tile.Inhabited && !closedNodes.Contains(tileCoordinate))
		{
			Point currentTile = tile.Coordinates;
			Point targetTile = tileBehavior.TileHovering.Coordinates;
			
			int G = 10;
			int H = 10 * (Mathf.Abs(currentTile.X - targetTile.X) + Mathf.Abs(currentTile.Y - targetTile.Y));
			
			int F = G + H;
			//Debug.Log(F + " < " + Cost);
			if (F < Cost)
			{
				//Debug.Log ("newTempMove: " + currentTile.X +", "+ currentTile.Y + " | Cost: " + F);
				Cost = F;
				tempMove = currentTile;
			}
		}
		
	}
	
	public void DrawPath()
	{
		if (path.Count > 0)
		{
			for (int i = 1; i < path.Count; i++)
			{
				Vector3 startNode = new Vector3(path[i].X + 0.5f,path[i].Y + 0.5f,0);
				GameObject tempPathObj = GameObject.Instantiate(pathCircle, startNode, Quaternion.identity) as GameObject;
				pathVisuals.Add(tempPathObj);
			}
		}
	}
	
	public void ClearPath()
	{
		if (pathVisuals.Count > 0)
		{
			for (int i = 0; i < pathVisuals.Count; i++)
			{
				Destroy(pathVisuals[i]);
				
			}
			pathVisuals.Clear();
		}
	}
	
	private void DrawDebugPath()
	{
		//Debug.Log ("Drawing");
		for (int i = 0; i < path.Count-1; i++)
		{
			//Debug only (scene view)
			Vector3 startNode = new Vector3(path[i].X + 0.5f,path[i].Y + 0.5f,0);
			Vector3 endNode = new Vector3(path[i+1].X + 0.5f,path[i+1].Y + 0.5f,0);
			Debug.DrawLine(startNode,endNode, Color.green);
		}
	}
	
	public int PathLenght()
	{
		return path.Count;
	}

}
*/