using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FOH_Aura_Ability : Base_Ability 
{
	public bool isActive = false;
	public Test_Unit player;
	public float interval;

	public void Awake ()
	{
		player = gameObject.GetComponent<Test_Unit>();
		abilityName = this.name;
		abilityDescription = "Get Smitten!";
		//abilityImage = ;
		cooldown = 15.0f;	
		radius = 50.0f;
		targetTypes.Add("AOE");
		targetTypes.Add("Ground");
		targetTypes.Add("Unit_Enemy");
		modifierScriptNames.Add("FOH_Aura_Modifier");
		//targetTypes.Add("Unit_Enemy");
		isPassive = true;
		isAura = true;
		isToggle = false;
		isAOE = false;
		currentLevel = 1;
		maxLevel = 8;
		interval = 0.25f;
		isActive = true;
	}
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isActive)
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, (radius * 0.3048f) / 2);
			foreach (Collider col in colliders)
			{
				if (col.GetComponent<Test_Unit>() != null && col.GetComponent<Test_Unit>().teamNumber != player.teamNumber)
				{
					col.GetComponent<Test_Unit>().ApplyDamage(10.0f, "Spell");
					col.GetComponent<Test_Unit>().AddModifiers(modifierScriptNames);
					isActive = false;
					StartCoroutine("ProcOverTime");
				}
			}
		}
	}

	public IEnumerator ProcOverTime()
	{
		yield return new WaitForSeconds(interval);
		isActive = true;
		yield break;
	}

	void OnDrawGizmosSelected() 
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, (radius * 0.3048f) / 2);
	}
}
