using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Abilities : MonoBehaviour {

	private List<GUIContent> medicAbilitySet = new List<GUIContent>();
	public List<GUIContent> MedicAbilitySet
	{
		get {return medicAbilitySet;}
	}

	private List<GUIContent> sniperAbilitySet = new List<GUIContent>();
	public List<GUIContent> SniperAbilitySet
	{
		get {return sniperAbilitySet;}
	}

	private List<GUIContent> commandoAbilitySet = new List<GUIContent>();
	public List<GUIContent> CommandoAbilitySet
	{
		get {return commandoAbilitySet;}
	}
	[SerializeField]
	private GUIContent medicClassIcon;

	public GUIContent MedicClassIcon
	{
		get {return medicClassIcon;}
	}
	[SerializeField]
	private GUIContent sniperClassIcon;
	
	public GUIContent SniperClassIcon
	{
		get {return sniperClassIcon;}
	}
	[SerializeField]
	private GUIContent commandoClassIcon;
	
	public GUIContent CommandoClassIcon
	{
		get {return commandoClassIcon;}
	}


	[SerializeField]
	private Texture[] abilityIcons;


	private void Awake()
	{
		GameRef.AbilitiesReference = this;
		CreateAbilitySets();
	}

	private void CreateAbilitySets()
	{
		//Medic Abilities
		MedicAbilitySet.Add(new GUIContent("Move", abilityIcons[0], "0"));
		MedicAbilitySet.Add(new GUIContent("Shoot", abilityIcons[1], "1"));
		MedicAbilitySet.Add(new GUIContent("Stab", abilityIcons[2], "2"));
		MedicAbilitySet.Add(new GUIContent("Bandage", abilityIcons[3], "3"));

		//Sniper Abilities
		sniperAbilitySet.Add(new GUIContent("Move", abilityIcons[0], "0"));
		sniperAbilitySet.Add(new GUIContent("Shoot", abilityIcons[1], "1"));
		sniperAbilitySet.Add(new GUIContent("Stab", abilityIcons[2], "2"));
		sniperAbilitySet.Add(new GUIContent("Snipe", abilityIcons[4], "3"));

		//Commando Abilities
		commandoAbilitySet.Add(new GUIContent("Move", abilityIcons[0], "0"));
		commandoAbilitySet.Add(new GUIContent("Shoot", abilityIcons[1], "1"));
		commandoAbilitySet.Add(new GUIContent("Stab", abilityIcons[2], "2"));
		commandoAbilitySet.Add(new GUIContent("Throw\nGrenade", abilityIcons[5], "3"));

	}

	private string[,] medicAbilityTooltip = new string[,]
	{
		{"100", 
			"<color=green><b>Move</b></color>\nTarget a tile to move towards it, at the cost of <color=red>1 Action Point</color> per tile traversed."},
		{"120", 
			"<color=green><b>Shoot</b></color>\n" +
			"Target an enemy character to shoot, at the cost of <color=red>3 Action Points</color>. Shoot is a good medium range attack.\n\n" +
			"<b>Damage:</b> 30 - 40\n" +
			"<b>Range:</b> 5"},
		{"120", 
			"<color=green><b>Stab</b></color>\n" +
			"Target a nearby enemy character to stab, at the cost of <color=red>3 Action Points</color>. Stab is a good short range attack.\n\n" +
			"<b>Base Damage:</b> 40 - 50\n" +
			"<b>Range:</b> 1"},
		{"120", 
			"<color=green><b>Bandage</b></color>\n" +
			"Target a friendly character to bandage, at the cost of <color=red>5 Action Points</color>. Chance of sucess drop drastically with repeated use on the same character.\n\n" +
			"<b>Heal:</b> 20 - 25\n" +
			"<b>Range:</b> 2 - 7"},
		{"120", 
			"<color=green><b>Combat Medic </b></color>\n" +
			"The Combat Medic is a superb support officer, with good defense.\n\n" +
			"<b>Body Armor:</b> +25 Health Points\n" +
			"<b>Anatomy:</b> +0-5 DMG with All Weapons"}
	}
	;

	private string[,] sniperAbilityTooltip = new string[,]
	{
		{"100", 
			"<color=green><b>Move</b></color>\n" +
			"Target a tile to move towards it, at the cost of <color=red>1 Action Point</color> per tile traversed."},
		{"120", 
			"<color=green><b>Shoot</b></color>\n" +
				"Target an enemy character to shoot, at the cost of <color=red>3 Action Points</color>. Shoot is a good medium range attack.\n\n" +
			"<b>Damage:</b> 25 - 35\n" +
			"<b>Range:</b> 5"},
		{"120", 
			"<color=green><b>Stab</b></color>\n" +
				"Target a nearby enemy character to stab, at the cost of <color=red>3 Action Points</color>. Stab is a good short range attack.\n\n" +
			"<b>Base Damage:</b> 35 - 45\n" +
			"<b>Range:</b> 1"},
		{"120", 
			"<color=green><b>Snipe</b></color>\n" +
				"Target an enemy character to snipe, at the cost of <color=red>5 Action Points</color>.Snipe is a good long range attack.\n\n" +
			"<b>Damage:</b> 25 - 45\n" +
			"<b>Range:</b> 2 - 7"},
		{"120", 
			"<color=green><b>Sniper</b></color>\n" +
			"The Snipers are experts at long range combat.\n\n" +
			"<b>Trained Eye:</b> +1 Line Of Sight\n" +
			"<b>Steady Hands:</b> +5% Accuracy with Ranged Weapons"}
	}
	;

	private string[,] commandoAbilityTooltip = new string[,]
	{
		{"100", 
			"<color=green><b>Move</b></color>\n" +
			"Target a tile to move towards it, at the cost of <color=red>1 Action Point</color> per tile traversed."},
		{"120", "<color=green><b>Shoot</b></color>\n" +
			"Target an enemy character to shoot, at the cost of <color=red>3 Action Points</color>.Shoot is a good medium ranged attack.\n\n" +
			"<b>Damage:</b> 25 - 35\n" +
			"<b>Range:</b> 5"},
		{"120", "<color=green><b>Stab</b></color>\n" +
			"Target a nearby enemy character to stab, at the cost of <color=red>3 Action Points</color>.Stab is a good short ranged attack.\n\n" +
			"<b>Base Damage:</b> 35 - 45\n" +
			"<b>Range:</b> 1"},
		{"120", "<color=green><b>Throw Grenade</b></color>\n" +
			"Target an area to throw a granade, at the cost of <color=red>6 Action Points</color>. Grenades are good area attacks, though not very accurate.\n\n" +
			"<b>Damage:</b> 0 - 75\n" +
			"<b>Range:</b> 2 - 7"},
		{"120", "<color=green><b>Commando </b></color>\n" +
			"The Commando are all'round soldiers, who excel at melee and area attacks.\n\n" +
			"<b>Rigorous Training:</b> +1 Action Point\n" +
			"<b>Extra Strong:</b> +5 DMG with Melee Weapons"}
	}
	;


	public string TooltipHeight(int classID, int abilityID)
	{
		switch (classID)
		{
		case 1:
			return medicAbilityTooltip[abilityID, 0];
		case 2:
			return sniperAbilityTooltip[abilityID, 0];
		case 3:
			return commandoAbilityTooltip[abilityID, 0];
		}
		return " ";
	}

	public string TooltipText(int classID, int abilityID)
	{
		switch (classID)
		{
		case 1:
			return medicAbilityTooltip[abilityID, 1];
		case 2:
			return sniperAbilityTooltip[abilityID, 1];
		case 3:
			return commandoAbilityTooltip[abilityID, 1];
		}
		return " ";
	}
}
