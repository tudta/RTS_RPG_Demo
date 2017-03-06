using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bone_Prison_Ability : Base_Ability 
{
	public List<Vector3> launchPositions = new List<Vector3>();
	public List<Vector3> launchRotations = new List<Vector3>();
	public List<float> launchDelays = new List<float>();
	public int numObjects;
	public GameObject boneWall;
	public void Awake ()
	{
		abilityName = this.name;
		abilityDescription = "Creates a circle of bone walls with a radius of 30 feet.";
		//abilityImage = ;
		cooldown = 15.0f;	
		radius = 30.0f;
		targetTypes.Add("AOE");
		targetTypes.Add("Ground");
		targetTypes.Add("Unit_Enemy");
		//targetTypes.Add("Unit_Enemy");
		isPassive = false;
		isAura = false;
		isToggle = false;
		isAOE = false;
		currentLevel = 1;
		maxLevel = 8;
		boneWall = (GameObject)Resources.Load("Block");
		launchDelays.Add(0.1f);
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
		launchPositions.Clear();
		launchRotations.Clear();
		launchDelays.Clear();
		Vector3 center = hit;
		for (int i = 0; i < numObjects; i++)
		{
			float a = i * (360.0f / numObjects);
			Vector3 pos = RandomCircle(center, (radius * 0.3048f) / 2.0f , a);
			Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center-pos);
			launchPositions.Add(pos);
			launchRotations.Add(rot.eulerAngles);
			//Instantiate(boneWall, pos, rot);
		}
		//Debug.Break();
		launchDelays.Add(0.0f);
		StartCoroutine(FireProjectiles(boneWall, numObjects, launchPositions, launchRotations, launchDelays));
		yield break;
		//FireProjectiles (Resources.Load("Box"), 1, launchPositions, launchRotations, launchDelays);
	}

	public override IEnumerator CastTargetAbility (Test_Unit target)
	{
		print("Casting Ability!");
		Vector3 center = target.transform.position;
		for (int i = 0; i < numObjects; i++)
		{
			float a = i * (360.0f / numObjects);
			Vector3 pos = RandomCircle(center, 5.0f, a);
			Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center-pos);
			launchPositions.Add(pos);
			launchRotations.Add(rot.eulerAngles);
			//Instantiate(boneWall, pos, rot);
		}
		print (numObjects);
		print (launchPositions.Count);
		print (launchRotations.Count);
		print (launchDelays.Count);
		//Debug.Break();
		StartCoroutine(FireProjectiles(boneWall, numObjects, launchPositions, launchRotations, launchDelays));
		yield break;
		//FireProjectiles (Resources.Load("Box"), 1, launchPositions, launchRotations, launchDelays);
	}

	public Vector3 RandomCircle(Vector3 center, float radius,float a)
	{
		//Debug.Log(a);
		float ang = a;
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y; //+ radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		return pos;
	}
}
