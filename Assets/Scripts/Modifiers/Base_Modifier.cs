using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base_Modifier : MonoBehaviour 
{
	public string modifierName;
	public string modifierDescription;
	public Modifier_Types tempModType;
	public List<Modifier_Types> modifierTypes = new List<Modifier_Types>();
	public List<float> modifierValues = new List<float>();
	public List<bool> isPercentages = new List<bool>();
	public bool hasDuration = true;
	public float duration;

	// Use this for initialization
	public virtual void Start () 
	{

	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
	
	}

	public void ActivateModifier()
	{
		if (hasDuration)
		{
			StartCoroutine("ModifierLifetime");
		}
		else
		{
			AddModifier();
		}
	}

	public void AddModifier()
	{
		StartCoroutine(gameObject.GetComponent<Test_Unit>().ApplyModifiers(modifierTypes, modifierValues, isPercentages));
	}

	public void RemoveModifier()
	{
		for (int i = 0; i < modifierValues.Count; i++)
		{
			modifierValues[i] = - modifierValues[i];
		}
		StartCoroutine(gameObject.GetComponent<Test_Unit>().ApplyModifiers(modifierTypes, modifierValues, isPercentages));
		Destroy(this);
	}

	public IEnumerator ModifierLifetime ()
	{
		print (modifierName + " has started life!");
		AddModifier();
		yield return new WaitForSeconds(duration);
		print (modifierName + " has ended life!");
		RemoveModifier();
	}
}
