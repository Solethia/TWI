using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogOfWar : MonoBehaviour {

	[SerializeField]
	private GameObject fogPrefab;
	private GameObject[,] fogOfWar;

	private int width;
	private int height;
	private bool[,] lit;
	private bool[,] enemyVision;
	public bool[,] Lit
	{
		get {return lit;}
	}

	private void Awake()
	{
		GameRef.FogOfWarReference = this;
	}

	// Use this for initialization
	private void Start () 
	{
		fogOfWar = new GameObject[GameRef.GridHeight, GameRef.GridWidth];
		width = GameRef.GridWidth;
		height = GameRef.GridHeight;
		GenerateFog ();
		ComputePlayerSight();
	}

	private void GenerateFog()
	{
		for(int x=0; x<width; x++) 
		{
			for(int y=0; y<height; y++) 
			{
				// Create your fog object.
				GameObject fog = Object.Instantiate(fogPrefab) as GameObject;
				// Save fog reference
				fogOfWar[x,y] = fog;
				// Change its position to the correct grid position.
				fog.transform.position = new Vector3(x + 0.5f, y + 0.5f , 0);
				GameRef.GetTile(x,y).Fog = true;
				fog.transform.parent = this.transform;
			}
		}
	}

	public void ComputePlayerSight()
	{
		if (GameRef.PlayerCharacters != null)
		{
			lit = new bool[width, height];

			foreach (Character playerCharacter in GameRef.PlayerCharacters)
			{
				int radius = playerCharacter.VisualSightRange;
				ShadowCaster.ComputeFieldOfViewWithShadowCasting(
					playerCharacter.CurrentTile.Coordinates.X, playerCharacter.CurrentTile.Coordinates.Y, radius, width, height,
					(x1, y1) => ShadowCaster.IsWithinMap(x1,y1,width,height) && GameRef.GridManagerReference.GetTile(x1, y1).WallTile == true,
					(x2, y2) => {if (ShadowCaster.IsWithinMap(x2,y2,width,height)){lit[x2, y2] = true; }});
			}
			for(int x=0; x<width; x++) 
			{
				for(int y=0; y<height; y++) 
				{
					if (lit[x,y])
					{
						fogOfWar[x,y].renderer.enabled = false;
						GameRef.GridManagerReference.GetTile(x,y).Fog = false;
					}
					else
					{
						fogOfWar[x,y].renderer.enabled = true;
						GameRef.GridManagerReference.GetTile(x,y).Fog = true;
					}

				}
			}
		}
	}

	public void ComputeAISight()
	{
		if (GameRef.ComputerCharacters != null)
		{
			bool[,] lit = new bool[width, height];
			int radius = 6;
			foreach (Character enemyAICharacter in GameRef.ComputerCharacters)
			{
				ShadowCaster.ComputeFieldOfViewWithShadowCasting(
					enemyAICharacter.CurrentTile.Coordinates.X, enemyAICharacter.CurrentTile.Coordinates.Y, radius, width, height,
					(x1, y1) => ShadowCaster.IsWithinMap(x1,y1,width,height) && GameRef.GridManagerReference.GetTile(x1, y1).WallTile == true,
					(x2, y2) => {if (ShadowCaster.IsWithinMap(x2,y2,width,height)){lit[x2, y2] = true; }});
			}
		}
	}




}
