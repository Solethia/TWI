using UnityEngine;
using System.Collections;

public class Path
{
	private Point[] _route;
	private Point _startNode, _endNode;
	private int _lenght;
	private bool _exists, _drawn;
	private GameObject[] _visuals;

	//Getters
	public Point[] Route		{get {return _route;}}
	public Point StartNode		{get {return _startNode;}}
	public Point EndNode		{get {return _endNode;}}
	public int Lenght			{get {return _lenght;}}
	public bool Exists			{get {return _exists;}}
	public bool Drawn			{get {return _drawn;}}
	public GameObject[] Visuals	{get {return _visuals;}}
	
	public Path(Point[] route, Point startTile, Point endTile, int pathLenght, bool pathExists)
	{
		_route = route;
		_startNode = startTile;
		_endNode = endTile;
		_lenght = pathLenght;
		_exists = pathExists;
		_drawn = false;
		_visuals = new GameObject[_lenght];
	}
	
	public void Draw(GameObject pathVisual)
	{
		if (!_drawn)
		{
			_drawn = true;
			if (_lenght >= 1)
			{
				for (int i = 0; i < _route.Length; i++)
				{
					Vector3 position = new Vector3(_route[i].X + 0.5f,_route[i].Y + 0.5f,0);
					_visuals[i] = GameObject.Instantiate(pathVisual, position, Quaternion.identity) as GameObject;
				}
			}
		}
	}
	
	public void Clear()
	{
		if (_drawn)
		{
			_drawn = false;
			foreach (GameObject visual in _visuals)
			{
				Object.Destroy(visual);
			}
		}
	}

}
