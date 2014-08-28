using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MedicChar : Character {

	[SerializeField]
	private GameObject bandageEffect;

	protected override void SetClass()
	{
		base.CharClass = 1;
		ClassIcon = GameRef.AbilitiesReference.MedicClassIcon;
	}
	protected override void SetStats()
	{
		MaxHealthPoints = 125;
		MaxHealthPoints = (int)((float)MaxHealthPoints * (float)healthModifier);
		HealthPoints = MaxHealthPoints;
		PlusDamageAllWeapons = 5;
	}
	protected override void SetAbilities()
	{
		base.Abilities = GameRef.AbilitiesReference.MedicAbilitySet.ToArray();
	}

	private int specialCost = 5;
	protected override bool CanSpecialAbility(Tile targetedTile, Path attackPath, bool writeMessage)
	{
		int range = 1;

		Character targetedCharacter = targetedTile.CharacterOnTile;

		if (targetedTile.Fog || !targetedTile.HasCharacter || !targetedCharacter.Friendly) {ErrorMessage(4,writeMessage); return false;}
		
		if (ActionPoints < specialCost) {ErrorMessage(2,writeMessage); return false;}
		
		if (attackPath.Lenght > range) {ErrorMessage(3,writeMessage);return false;}
		
		if (Application.loadedLevel == 1)
		{
			if (GameRef.TutorialReference.TutorialStep < 14) {ErrorMessage(7, writeMessage);return false;}
		}

		return true;
	}

	public override int SpecialAccuracy(Path attackPath, Character targetedCharacter)
	{
		return Mathf.Clamp(100 - (targetedCharacter.TimesBandaged * 40), 0, 100);
	}

	protected override void SpecialAbility(Tile targetedTile, Path attackPath)
	{
		//Bandage
		Character targetedCharacter = targetedTile.CharacterOnTile;

		ActionPoints -= specialCost;
		int accuracyRoll = Random.Range (0, 101);
		if (accuracyRoll <= SpecialAccuracy(attackPath, targetedCharacter))
		{
			targetedCharacter.TimesBandaged++;
			int healDone = Random.Range(20,26); // DMG 25 - 35 (3-4 successfull hits to kill)
			if (healDone + targetedCharacter.HealthPoints > targetedCharacter.MaxHealthPoints)
			{
				healDone = targetedCharacter.MaxHealthPoints - targetedCharacter.HealthPoints;
			}
			targetedCharacter.HealthPoints += healDone; //Apply heal, to targeted character
			Vector3 spawnPosition = new Vector3(targetedTile.Coordinates.X + 0.5f, targetedTile.Coordinates.Y + 0.5f, 9);
			GameObject.Instantiate(bandageEffect, spawnPosition, Quaternion.identity);
			GameRef.NewMessage = gameObject.name + " healed " + targetedCharacter.name + " for " + healDone + " damage." ;
			GameRef.PlaySound.Bandage(true);
		}
		else
		{
			GameRef.PlaySound.Bandage(false);
			Vector3 spawnPosition = new Vector3(targetedTile.Coordinates.X + 0.5f, targetedTile.Coordinates.Y + 0.5f, 9);
			GameObject.Instantiate(missEffect, spawnPosition, Quaternion.identity);
			GameRef.NewMessage = gameObject.name + " failed to bandage " + targetedCharacter.name + "'s wounds." ;
		}
	}
	
}
