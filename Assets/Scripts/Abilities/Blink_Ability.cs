using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blink_Ability : Base_Ability 
{
	public List<Vector3> launchPositions = new List<Vector3>();
	public List<Vector3> launchRotations = new List<Vector3>();
	public List<float> launchDelays = new List<float>();
	public int numObjects;
	public GameObject boneWall;
	public void Awake ()
	{
		abilityName = "Blink";
		abilityDescription = "Teleports the character 50 feet towards the target location";
		//abilityImage = ;
		cooldown = 1.0f;	
		radius = 50.0f;
		targetTypes.Add("Target");
		targetTypes.Add("Ground");
		//targetTypes.Add("Unit_Enemy");
		isPassive = false;
		isAura = false;
		isToggle = false;
		isAOE = false;
		currentLevel = 1;
		maxLevel = 8;
		launchDelays.Add(0.0f);
	}
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public override IEnumerator CastGroundAbility (Vector3 hit)
	{
		print("Casting Ability!");
		if (Vector3.Distance(transform.position, hit) <= radius)
		{
			transform.position = hit;
		}
		else
		{
			transform.LookAt(hit);
			transform.Translate(Vector3.forward * (radius * 0.3048f) / 2);
		}
		yield break;
		//FireProjectiles (Resources.Load("Box"), 1, launchPositions, launchRotations, launchDelays);
	}
}
