using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetDummy: Character {

	[SerializeField]
	private GameObject snipeEffect;

	protected override void SetClass()
	{
		base.CharClass = 0;
		//ClassIcon = GameRef.AbilitiesReference.SniperClassIcon;
	}
	protected override void SetStats()
	{
		HealthPoints = 100;
		//PlusAccuracyRangedWeapons = 5; //+5% accuracy with ranged weapons
		//VisualSightRange += 1;// +1 to Sight Range
	}
	protected override void SetAbilities()
	{
		//base.Abilities = GameRef.AbilitiesReference.SniperAbilitySet.ToArray();
	}

	private int specialCost = 5;
	protected override bool CanSpecialAbility(Tile targetedTile, Path attackPath, bool writeMessage)
	{
		int minRange = 2;
		int maxRange = 7;

		Character targetedCharacter = targetedTile.CharacterOnTile;
		
		int pathLenght = attackPath.Lenght;
		
		bool enemyTargeted, canPayCost, inRange;

		if (!targetedTile.Fog && targetedTile.HasCharacter && !targetedCharacter.Friendly) {enemyTargeted = true;}
		else { enemyTargeted = false;ErrorMessage(1,writeMessage); }
		
		if (ActionPoints >= specialCost) {canPayCost = true;}
		else { canPayCost = false;ErrorMessage(2,writeMessage); }
		
		if (pathLenght <= maxRange && pathLenght >= minRange) {inRange = true;}
		else { inRange = false;ErrorMessage(3,writeMessage); }

		return enemyTargeted && canPayCost && inRange;

	}

	public override int SpecialAccuracy(Path attackPath, Character targetedCharacter)
	{
		switch (attackPath.Lenght)
		{
		case 1: return 65 + PlusAccuracyRangedWeapons - 50;
		case 2: return 65 + PlusAccuracyRangedWeapons - 25;
		default: return 65 + PlusAccuracyRangedWeapons + ((attackPath.Lenght - 3) * 4);
		}
	}

	protected override void SpecialAbility(Tile targetedTile, Path attackPath)
	{
		//Snipe

		Character targetedCharacter = targetedTile.CharacterOnTile;

		ActionPoints -= specialCost;
		int accuracyRoll = Random.Range (0, 101);
		if (accuracyRoll <= SpecialAccuracy(attackPath, targetedCharacter))
		{
			int damageDone = Random.Range(35,45); 
			targetedCharacter.HealthPoints -= damageDone; //Apply Damage, to targeted character

			Vector3 spawnPosition = new Vector3(targetedTile.Coordinates.X + 0.5f, targetedTile.Coordinates.Y + 0.5f, 9);
			GameObject.Instantiate(snipeEffect, spawnPosition, Quaternion.identity);

			GameRef.NewMessage = gameObject.name + " sniped " + targetedCharacter.name + " for " + damageDone + " damage." ;
		}
		else
		{
			Vector3 spawnPosition = new Vector3(targetedTile.Coordinates.X + 0.5f, targetedTile.Coordinates.Y + 0.5f, 9);
			GameObject.Instantiate(missEffect, spawnPosition, Quaternion.identity);

			GameRef.NewMessage = gameObject.name + "'s snipe attempt missed " + targetedCharacter.name + "." ;
		}
	}
	
}
