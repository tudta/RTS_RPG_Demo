using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stat_Debuff_Ability : Base_Ability 
{

	public void Awake ()
	{
		abilityName = this.name;
		abilityDescription = "Reduces enemy's stats by 25 each for 10 seconds.";
		//abilityImage = ;
		cooldown = 15.0f;	
		//radius;
		targetTypes.Add("Unit_Enemy");
		modifierScriptNames.Add("Stat_Debuff_Modifier");
		isPassive = false;
		isAura = false;
		isToggle = false;
		isAOE = false;
		currentLevel = 1;
		maxLevel = 8;
	}
	// Use this for initialization
	public override void Start () 
	{

	}
	
	// Update is called once per frame
	public override void Update () 
	{
	
	}

	public override IEnumerator CastTargetAbility (Test_Unit target)
	{
		if (isReady)
		{
			print("Casting Ability!");
			AddModifiers(target);
		}
		yield break;
	}
}
