using UnityEngine;
using System.Collections;

public class FOH_Aura_Modifier : Base_Modifier
{
	public float range;
	// Use this for initialization
	public void Awake()
	{
		modifierName = "Smitted!";
		modifierDescription = "GOT ZAPPED!";
		modifierTypes.Add(Modifier_Types.AGILITY);
		modifierTypes.Add(Modifier_Types.INTELLIGENCE);
		modifierTypes.Add(Modifier_Types.STRENGTH);
		modifierValues.Add(-25);
		modifierValues.Add(-25);
		modifierValues.Add(-25);
		isPercentages.Add(false);
		hasDuration = true;
		duration = 10.0f;
	}
	
	public override void Start () 
	{
		
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		//		Collider[] colliders = Physics.OverlapSphere(transform.position, range);
		//		foreach (Collider col in colliders)
		//		{
		//			if (col.GetComponent<Test_Unit>() != null && col.GetComponent<Test_Unit>().teamNumber != gameObject.GetComponent<Test_Unit>()teamNumber)
		//			{
		//				target = col.gameObject;
		//				currentState = Unit_States.ATTACK;
		//			}
		//		}
	}
}
