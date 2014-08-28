using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SniperChar : Character {

	[SerializeField]
	private GameObject snipeEffect;

	protected override void SetClass()
	{
		base.CharClass = 2;
		ClassIcon = GameRef.AbilitiesReference.SniperClassIcon;
	}
	protected override void SetStats()
	{
		MaxHealthPoints = (int)((float)MaxHealthPoints * (float)healthModifier);
		HealthPoints = MaxHealthPoints;
		PlusAccuracyRangedWeapons = 5; //+5% accuracy with ranged weapons
		VisualSightRange += 1;// +1 to Sight Range
	}
	protected override void SetAbilities()
	{
		base.Abilities = GameRef.AbilitiesReference.SniperAbilitySet.ToArray();
	}

	private int specialCost = 5;
	protected override bool CanSpecialAbility(Tile targetedTile, Path attackPath, bool writeMessage)
	{
		int maxRange = 7;

		Character targetedCharacter = targetedTile.CharacterOnTile;
		
		int pathLenght = attackPath.Lenght;
		
		if (targetedTile.Fog || !targetedTile.HasCharacter || targetedCharacter.Friendly) {ErrorMessage(4,writeMessage); return false;}
		
		if (ActionPoints < specialCost) {ErrorMessage(2,writeMessage); return false;}
		
		if (pathLenght > maxRange) {ErrorMessage(3,writeMessage);return false;}
		
		if (Application.loadedLevel == 1)
		{
			if (GameRef.TutorialReference.TutorialStep < 14) {ErrorMessage(7, writeMessage);return false;}
		}
		
		return true;

	}

	public override int SpecialAccuracy(Path attackPath, Character targetedCharacter)
	{
		switch (attackPath.Lenght)
		{
		case 1: return 1;
		case 2: return 15;
		case 3: return 45;
		case 4: return 65;
		case 5: return 85;
		case 6: return 95;
		default: return 85;
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
			GameRef.PlaySound.Snipe(true);
		}
		else
		{
			Vector3 spawnPosition = new Vector3(targetedTile.Coordinates.X + 0.5f, targetedTile.Coordinates.Y + 0.5f, 9);
			GameObject.Instantiate(missEffect, spawnPosition, Quaternion.identity);

			GameRef.NewMessage = gameObject.name + "'s snipe attempt missed " + targetedCharacter.name + "." ;
			GameRef.PlaySound.Snipe(false);
		}
	}
	
}
