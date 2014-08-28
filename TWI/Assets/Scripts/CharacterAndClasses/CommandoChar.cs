using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandoChar : Character {

	[SerializeField]
	private GameObject grenadeEffect;

	protected override void SetClass()
	{
		base.CharClass = 3;
		ClassIcon = GameRef.AbilitiesReference.CommandoClassIcon;
	}
	protected override void SetStats()
	{
		MaxHealthPoints = (int)((float)MaxHealthPoints * (float)healthModifier);
		HealthPoints = MaxHealthPoints;
		//Passives
		PlusDamageMeleeWeapons = 5;//+5 Damage with Melee Weapons
		MaxActionPoints = 6; //+1 AP
	}
	protected override void SetAbilities()
	{
		base.Abilities = GameRef.AbilitiesReference.CommandoAbilitySet.ToArray();
	}

	private int specialCost = 6;

	public override int SpecialAccuracy(Path attackPath, Character targetedCharacter)
	{
		return 0;
	}

	protected override bool CanSpecialAbility(Tile targetedTile, Path attackPath, bool writeMessage)
	{

		int pathLenght = attackPath.Lenght;

		int minRange = 2;
		int maxRange = 5;
		
		if (targetedTile.Fog) {ErrorMessage(4,writeMessage); return false;}
		
		if (targetedTile.WallTile) {ErrorMessage(5,writeMessage); return false;}
		
		if (ActionPoints < specialCost) {ErrorMessage(2,writeMessage); return false;}
		
		if (pathLenght > maxRange || pathLenght < minRange) {ErrorMessage(3,writeMessage);return false;}

		if (Application.loadedLevel == 1)
		{
			if (GameRef.TutorialReference.TutorialStep < 14) {ErrorMessage(7, writeMessage);return false;}
		}
		return true;
	}
	
	protected override void SpecialAbility(Tile targetedTile, Path attackPat)
	{
		//Grenade
		ActionPoints -= specialCost; //Detract action points
		Point grenadeLandingPoint = GrenadeLandingPoint(targetedTile); //Determine where the grenade lands
		GameRef.PlaySound.Grenade();
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				Point currentPosition = new Point((grenadeLandingPoint.X + x), (grenadeLandingPoint.Y + y));
				if (ShadowCaster.IsWithinMap(currentPosition.X, currentPosition.Y, GameRef.GridWidth, GameRef.GridHeight))
				{
					Tile CurrentTile = GameRef.GetTile(currentPosition);

					Character targetedCharacter = CurrentTile.CharacterOnTile;
					if (grenadeLandingPoint == currentPosition)
					{
						if (CurrentTile.HasCharacter)
						{
							int damageDone = Random.Range(25,75); //Randomize Damage
							targetedCharacter.HealthPoints -= damageDone; //Apply Damage, to targeted character
							GameRef.NewMessage = gameObject.name + "'s grenade hit " + targetedCharacter.name + " for " + damageDone + " damage." ;
						}
						//Spawn Effect at the Grenade Landing Point
						Vector3 spawnPosition = new Vector3(CurrentTile.Coordinates.X + 0.5f, CurrentTile.Coordinates.Y + 0.5f, 9);
						GameObject.Instantiate(grenadeEffect, spawnPosition, Quaternion.identity);
					}
					else
					{
						if (CurrentTile.HasCharacter)
						{
							int damageDone = Random.Range(15,25); //Randomize Damage
							targetedCharacter.HealthPoints -= damageDone; //Apply Damage, to targeted character
							GameRef.NewMessage = gameObject.name + "'s grenade hit " + targetedCharacter.name + " for " + damageDone + " damage." ;
						}
					}
				}
			}
		}
	}

	private Point GrenadeLandingPoint(Tile targetedTile)
	{
		int[,] GrenadeAccuracyMatrix = 
		{
			{9,	9,	9},
			{9,	28,	9},
			{9,	9,	9}
		}
		;
		int accuracyRoll = Random.Range (0, 101);
		int accuracy = 0;
		for (int x = 0; x <= 2; x++)
		{
			for (int y = 0; y <= 2; y++)
			{
				accuracy += GrenadeAccuracyMatrix[x,y];
				if (accuracyRoll <= accuracy)
				{
					Point LandingPoint = new Point(targetedTile.Coordinates.X + (x-1), targetedTile.Coordinates.Y + (y-1));
					return LandingPoint;
				}
			}
		}
		return new Point(0,0);
	}
	
}
