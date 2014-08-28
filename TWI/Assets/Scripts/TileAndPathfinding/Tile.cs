using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private bool hasCharacter;
	private Character characterOnTile;

	private bool traversable;
	private bool fog;
	public bool Fog
	{
		set 
		{
			fog = value;
			if (hasCharacter) 
			{
				Renderer[] charRenderers = CharacterOnTile.transform.GetComponentsInChildren<Renderer>() as Renderer[]; 
				foreach (Renderer charRenderer in charRenderers) 
				{ 
					charRenderer.enabled = !value;
				}
			}
		}
		get {return fog;}
	}

	private bool wallTile = false;
	public bool WallTile 
	{
		set {wallTile = value;}
		get {return wallTile;}
	}

	private Transform thisTransform;
	
	private Point coordinates;

	public Transform ThisTransform
	{
		get {return thisTransform;}
	}

	public bool HasCharacter
	{
		set {hasCharacter = value;}
		get {return hasCharacter;}
	}

	public Character CharacterOnTile
	{
		set 
		{
			characterOnTile = value;
			if (value == null) {HasCharacter = false; Traversable = true;}
			else {HasCharacter = true; Traversable = false;}
		}
		get {return characterOnTile;}
	}

	public bool Traversable
	{
		set {traversable = value;}
		get {return traversable;}
	}

	public Point Coordinates
	{
		set {coordinates = value;}
		get {return coordinates;}
	}

	private void Awake()
	{
		thisTransform = transform;
	}

	//Add information that tiles need to contain
}
