using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base_Ability : MonoBehaviour 
{
	public string abilityName;
	public string abilityDescription;
	public Sprite abilityImage;
	public float cooldown;	
	public float radius;
	public List <string> targetTypes;
	public bool isReady = true;
	public bool isPassive = false;
	public bool isAura = false;
	public bool isToggle = false;
	public bool isAOE = false;
	public int currentLevel;
	public int maxLevel;
//	public Base_Modifier[] modifiers;
	public List<string> modifierScriptNames = new List<string>();
	//public Base_Modifier[] modifierScripts;


	// Use this for initialization
	public virtual void Start () 
	{

	}		
	
	// Update is called once per frame
	public virtual void Update () 
	{
	
	}

	public virtual IEnumerator CastGroundAbility (Vector3 position)
	{
		//		print("Casting Ability!");
		//		AddModifiers(target);
		//		print ("Added_Modifier!");
		//		FireProjectiles (Resources.Load("Box"), 1, launchPositions, launchRotations, launchDelays);
		yield break;
	}

	public virtual IEnumerator CastTargetAbility (Test_Unit target)
	{
//		print("Casting Ability!");
//		AddModifiers(target);
//		print ("Added_Modifier!");
//		FireProjectiles (Resources.Load("Box"), 1, launchPositions, launchRotations, launchDelays);
		yield break;
	}

	//USE A FOR LOOP FROM THE DERIVED CLASS FOR EACH MODIFIER TYPE IN THE ARRAY
	public virtual void AddModifiers(Test_Unit target)
	{
		StartCoroutine (target.AddModifiers (modifierScriptNames));
	}

//	\\ LAUNCH PARAMETERS == [0] = launchPosition and [1] = launchRotation//
	//CHANGE FOR LOOPS TO IF(TRUE) STATEMENTS LIKE IN UNIT APPLY MODIFIERS!
	public virtual IEnumerator FireProjectiles(GameObject projectile, int projectileCount, List<Vector3> launchPositions, List<Vector3> launchRotations, List<float> launchDelays)
	{
		Vector3[] launchParameters = new Vector3[2];
		float launchDelay = 0.0f;
		int x = 0;
		int y = 0;
		int z = 0;
		// PROJECTILE COUNT LOOP
		for (int i = 0; i < projectileCount; i++)	
		{
			// POSITION LOOP
			if (true)
			{
				launchParameters[0] = launchPositions[x];
				if (x < launchPositions.Count)
				{
					x++;
				}
				if (x == launchPositions.Count)
				{
					x--;
				} 
				print ("X Loop = " + x);
			}
			if (true)
			{
				launchParameters[1] = launchRotations[y];
				if (y < launchRotations.Count)
				{
					y++;
				}
				if (y == launchRotations.Count)
				{
					y--;
				}
				print ("Y Loop = " + y);
			}
			if (true)
			{
				launchDelay = launchDelays[z];
				if (z < launchDelays.Count)
				{
					z++;
				}
				if (z == launchDelays.Count)
				{
					z--;
				} 
				print ("Z Loop = " + z);
			}
//			for (int x = 0; x < launchPositions.Count; x++)
//			{
//				if (x == launchPositions.Count)
//				{
//					x--;
//				}
//				launchParameters[0] = launchPositions[x];
//			}
			// ROTATION LOOP
//			for (int y = 0; y < launchRotations.Count; y++)
//			{
//				if (y == launchRotations.Count)
//				{
//					y--;
//				}
//				launchParameters[1] = launchRotations[y];
//			}
			// DELAY LOOP
//			for (int z = 0; z < launchDelays.Count; z++)
//			{
//				if (z == launchDelays.Count)
//				{
//					z--;
//				}
//				launchDelay = launchDelays[z];
//			}
			// WAITS AND FIRES!
			yield return new WaitForSeconds (launchDelay);
			Instantiate (projectile, launchParameters[0], Quaternion.Euler (launchParameters[1]));
		}

		//TERMINATES COROUTINE!
		yield break;
	}

	public IEnumerator StartCooldown ()
	{
		isReady = false;
		yield return new WaitForSeconds(cooldown);
		isReady = true;
	}

	public virtual void LevelSkill()
	{
		if (currentLevel < maxLevel)
		{
			currentLevel++;
		}
	}
}
