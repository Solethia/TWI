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

	public bool Equals(Point p)
	{
		return (X == p.X) && (Y == p.Y);
	}

	public override bool Equals(object obj)
	{
		if (obj is Point)
		{
			return this.Equals((Point)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return X ^ Y;
	}

	public static bool operator ==(Point lhs, Point rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Point lhs, Point rhs)
	{
		return !(lhs.Equals(rhs));
	}
}

public struct PathPossibleMoves
{
	public bool MoveExist;
	public PathMove[] PossibleMoves;
	
	public PathPossibleMoves(bool moveExist, PathMove[] possibleMoves)
	{
		MoveExist = moveExist;
		PossibleMoves = possibleMoves;
	}
}

public struct PathMove
{
	public enum MoveTypes
	{
		straight,
		diagonal
	}
	public MoveTypes MoveType; 
	public Point PossibleMove;
	
	public PathMove(MoveTypes moveType, Point possibleMove)
	{
		MoveType = moveType;
		PossibleMove = possibleMove;
	}
}

public struct PathNode
{
	public Point Node;
	public Point Parent;
	public int GenericCost;
	public int HeuristicCost;
	public int FullCost;
	
	public PathNode(Point node, Point parent, int genericCost, int heuristicCost, int fullCost)
	{
		Node = node;
		Parent = parent;
		GenericCost = genericCost;
		HeuristicCost = heuristicCost;
		FullCost = fullCost;
	}
}

public static class Pathfinding {

	public enum PathType
	{
		move,
		action,
		AImove
	}

	public static Path FindPathTwo(Point startTile, Point endTile, PathType pathType)
	{
		
		List<Point> route = new List<Point>();
		List<PathNode> openNodes = new List<PathNode>();
		List<PathNode> closedNodes = new List<PathNode>();

		int cost = 10 * (Mathf.Abs(startTile.X - endTile.X) + Mathf.Abs(startTile.Y - endTile.Y));
		openNodes.Add(new PathNode(startTile, startTile, 0, cost, cost));

		bool pathFound = false;
		bool pathExist = true;
		while (!pathFound && pathExist)
		{
			if (openNodes.Count == 0)
			{
				pathExist = false;
			}
			else 
			{
				PathNode currentNode = openNodes[0];
				foreach (PathNode node in openNodes)
				{
					if (node.FullCost <= currentNode.FullCost)
					{
						currentNode = node;
					}
				}
				openNodes.Remove(currentNode);
				closedNodes.Add (currentNode);
				if (currentNode.Node == endTile)
				{
					pathFound = true;
				}
				else
				{
					PathPossibleMoves moveSearch = NewMoveSearchTwo(currentNode.Node, endTile, closedNodes.ToArray(), pathType);
					if (moveSearch.MoveExist)
					{
						foreach (PathMove move in moveSearch.PossibleMoves)
						{
							Point currentMove = move.PossibleMove;
							if (!ContainsNode(currentMove, openNodes.ToArray()))
							{
								int G;
								if (move.MoveType == PathMove.MoveTypes.diagonal) {G = 14;}
								else {G = 10;}
								int H = 10 * (Mathf.Abs(currentMove.X - endTile.X) + Mathf.Abs(currentMove.Y - endTile.Y));
								
								int F = G + H;
								openNodes.Add(new PathNode(currentMove, currentNode.Node, G, H, F));
							}
							else
							{

							}
						}
					}
				}
			}

		}
		if (pathExist)
		{
			bool routeFound = false;
			route.Add(endTile);
			while (!routeFound)
			{
				Point pathSegment = PointToNode(route[0], closedNodes.ToArray()).Parent;
				if (pathSegment == startTile)
				{
					routeFound = true;
				}
				else
				{
					route.Insert(0, pathSegment);
				}
			}
		}

		return new Path(route.ToArray(), startTile, endTile, route.Count, pathExist);
	}

	/*public static Path FindPath(Point startNode, Point endNode, PathType pathType)
	{

		List<Point> route = new List<Point>();
		List<Point> openNodes = new List<Point>();
		List<Point> closedNodes = new List<Point>();

		route.Add(startNode);

		bool pathFound = false;
		bool pathExist = true;
		while (!pathFound && pathExist)
		{
			if (route[route.Count-1] == endNode){pathFound = true;}
			else
			{

				PathPossibleMoves moveSearch = NewMoveSearch(route[route.Count-1], closedNodes, pathType);
				if (moveSearch.MoveExist)
				{
					Point bestMove = FindCheapestMove(moveSearch.PossibleMoves, endNode);
					closedNodes.Add(bestMove);
					route.Add(bestMove);
				}
				else
				{
					if (route.Count > 1)
					{
						route.Remove(route[route.Count - 1]);
					}
					else
					{
						pathExist = false;
					}
				}
			}
		}

		return new Path(route.ToArray(), startNode, endNode, route.Count-1, pathExist);
	}

	private static PathPossibleMoves NewMoveSearch(Point originNode, List<Point> closedNodes, PathType pathType)
	{
		bool moveExist = false;
		List<PathMove> possibleMoves = new List<PathMove>();
		
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				Point nTilePosi = new Point(originNode.X + x, originNode.Y + y);
				if (ShadowCaster.IsWithinMap(nTilePosi.X, nTilePosi.Y, GameRef.GridWidth, GameRef.GridHeight))
				{
					Tile nTile = GameRef.GetTile(originNode.X + x, originNode.Y + y);
					if (!closedNodes.Contains(nTile.Coordinates) && CanMoveToTile(nTile, pathType))
					{
						moveExist = true;
						possibleMoves.Add(new PathMove(DetermineMoveType(x,y), nTile.Coordinates));
					}
				}
			}
		}
		return new PathPossibleMoves(moveExist, possibleMoves.ToArray());
	}

	private static Point FindCheapestMove(PathMove[] possibleMoves, Point endTile)
	{
		int bestCost = 999;
		Point bestMove = possibleMoves[0].PossibleMove;
		foreach (PathMove move in possibleMoves)
		{
			Point currentMove = move.PossibleMove;

			int G;
			if (move.MoveType == PathMove.MoveTypes.diagonal) {G = 14;}
			else {G = 10;}
			int H = 10 * (Mathf.Abs(currentMove.X - endTile.X) + Mathf.Abs(currentMove.Y - endTile.Y));
			
			int F = G + H;
			if (F < bestCost)
			{
				bestCost = F;
				bestMove = currentMove;
			}
		}
		return bestMove;
	}*/

	private static bool CanMoveToTile(Tile tile, Point endPoint, PathType pathType)
	{
		return (pathType == PathType.move && tile.Traversable) || (pathType == PathType.action && !tile.WallTile) || (pathType == PathType.AImove && (tile.Traversable || tile.Coordinates == endPoint));
		
	}

	private static PathMove.MoveTypes DetermineMoveType(int x, int y)
	{
		if (x == 0 || y == 0) {return PathMove.MoveTypes.straight;}
		return PathMove.MoveTypes.diagonal;
		/*
		[-1, 1][ 0, 1][ 1, 1]
		[-1, 0][ 0, 0][ 1, 0]
		[-1,-1][ 0,-1][ 1,-1]
		 */
	}

	private static bool ContainsNode(Point compareNode,PathNode[] nodes)
	{
		foreach (PathNode pathNode in nodes)
		{
			if (pathNode.Node == compareNode)
			{
				return true;
			}
		}
		return false;
	}

	private static PathNode PointToNode(Point compareNode,PathNode[] nodes)
	{
		foreach(PathNode pathNode in nodes)
		{
			if (pathNode.Node == compareNode)
			{
				return pathNode;
			}
		}
		Debug.LogWarning("PointToNode, found no node which contained the point.");
		return nodes[0];
	}

	private static PathPossibleMoves NewMoveSearchTwo(Point originNode, Point endPoint, PathNode[] closedNodes, PathType pathType)
	{
		bool moveExist = false;
		List<PathMove> possibleMoves = new List<PathMove>();
		
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				Point nTilePosi = new Point(originNode.X + x, originNode.Y + y);
				if (ShadowCaster.IsWithinMap(nTilePosi.X, nTilePosi.Y, GameRef.GridWidth, GameRef.GridHeight))
				{
					Tile nTile = GameRef.GetTile(originNode.X + x, originNode.Y + y);
					if (!ContainsNode(nTile.Coordinates, closedNodes) && CanMoveToTile(nTile, endPoint, pathType))
					{
						moveExist = true;
						possibleMoves.Add(new PathMove(DetermineMoveType(x,y), nTile.Coordinates));
					}
				}
			}
		}
		return new PathPossibleMoves(moveExist, possibleMoves.ToArray());
	}


}
