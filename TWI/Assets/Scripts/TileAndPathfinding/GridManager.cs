using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class GridManager : MonoBehaviour {

	[SerializeField]
	private GameObject TilePrefab;
	[SerializeField]
	private Texture2D traverseMap;
	
	[SerializeField]
	private int gridHeight;
	[SerializeField]
	private int gridWidth;


	private Tile[,] GridTiles;
	private Transform thisTransform;

	void Awake()
	{
		GameRef.GridManagerReference = this;
		GameRef.GridHeight = gridHeight;
		GameRef.GridWidth = gridWidth;
		GridTiles = new Tile[gridHeight, gridWidth];
		thisTransform = transform;
		GenerateTiles();
	}

	private void GenerateTiles()
	{
		for(int x=0; x<gridWidth; x++) 
		{
			for(int y=0; y<gridHeight; y++) 
			{
				// Create your tile object.
				GameObject tile = Object.Instantiate(TilePrefab) as GameObject;
				//We find the Tile script attached to the gameobject
				Tile tileScript = tile.GetComponent(typeof(Tile)) as Tile;
				//Let the Tile Script know what coordinates it has xD
				tileScript.Coordinates = new Point(x,y);
				//Assign the Tile Script to an array, for easy reference for a reference of each tile.
				GridTiles[x,y] = tileScript;
				// Change its position to this grid reference.
				tile.transform.position = new Vector3(x + 0.5f, y + 0.5f , 0);
				//Clean up the heirachy, by parenting each tile under GridSystem -> Tiles.
				tile.transform.parent = thisTransform;
				//Use the TraverseMap, to determine whether traversable or not.
				Color pixelColor = traverseMap.GetPixel(x,y);
				//Debug.Log (Color.black);
				if (pixelColor == Color.black)
				{

					tileScript.Traversable = false;
					tileScript.WallTile = true;
				}
				else
				{
					tileScript.Traversable = true;
				}

				//Debug.Log ("Traversable: " + tileScript.Traversable + " | Inhabited: " + tileScript.Inhabited + " | GridCoords: (" + tileScript.GridX + ", " + tileScript.GridY + ")" );
			}
		}
	}

	public Tile GetTile(int x, int y)
	{
		return GridTiles[x,y];
	}


}
