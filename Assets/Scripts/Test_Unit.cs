using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test_Unit : MonoBehaviour 
{
	public UI_Manager uiManager;
	public bool isSelected;
	public bool isSelectable;
	public bool isInvulnerable;
	private Renderer ren;
	private NavMeshAgent agent;
	public int teamNumber;
	public Player player;
	public enum Unit_States {IDLE, MOVING, ATTACK, PATROL};
	public Unit_States currentState;
	public bool isRanged = false;
	public bool canAttack = true;
	#region Base Stats
	public int baseAgility;
	public int baseIntelligence;
	public int baseStrength;
	public float baseHealth;
	public float baseMana;
	public float baseDamage;
	public float baseAttackSpeed;
	public float baseAttackRange;
	public float baseMoveSpeed;
	public int baseArmor;
	public int baseSpellResistance;
	#endregion
	#region Current Stats
	public int currentAgility;
	public int currentIntelligence;
	public int currentStrength;
	public float currentHealth;
	public float currentMana;
	public float currentDamage;
	public float currentAttackSpeed;
	public float currentAttackRange;
	public float currentMoveSpeed;
	public int currentArmor;
	public int currentSpellResistance;
	public float totalPhysicalReduction = 0.0f;
	public float totalSpellReduction = 0.0f;
	#endregion
	public float maxHealth;
	public float maxMana;
	public Vector3[] patrolPoints;
	public bool isSettingPatrol;
	public bool isTargeting;
	public int currentPatrolIndex;
	public GameObject target;
	public float aggroRange;
	public Vector3 originPoint;
	public float leashDistance;
	public List<Base_Modifier> currentModifiers = new List<Base_Modifier>();
	public List<Base_Ability> abilityList = new List<Base_Ability>();
	public bool isAI;
	public Base_Ability currentAbility;

	public Color debugColor1 = Color.cyan;
	public Color debugColor2 = Color.red;
	public Color debugColor3 = Color.yellow;

	void Start () 
	{
		uiManager = UI_Manager.instance;
		debugColor1.a = 0.75f;
		debugColor2.a = 0.75f;
		debugColor3.a = 0.5f;
		originPoint = transform.position;
		agent = GetComponent<NavMeshAgent>();
		//player = GameObject.Find("Player_" + teamNumber).GetComponent<Player>();
		ren = GetComponent<Renderer>();
	}
	
	void Update () 
	{
		if (isSelected)
		{
			if (!isTargeting)
			{
				if (Input.GetMouseButtonDown(1)) 
				{
					//print ("Right-Clicked!");
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					
					LayerMask mask = 1 << 9;
					if (Physics.Raycast(ray, out hit, 200, mask)) 
					{
						print (hit.transform.name);
						print (hit.transform.gameObject.GetComponent<Test_Unit>().teamNumber);
						if (hit.transform.gameObject.GetComponent<Test_Unit>().teamNumber != teamNumber)
						{
							target = hit.transform.gameObject;
							currentState = Unit_States.ATTACK;
						}
					}
					else
					{
						mask = 1 << 8;
						
						if (Physics.Raycast(ray, out hit, 200, mask)) 
						{
							agent.destination = hit.point;
							currentState = Unit_States.MOVING;
						}
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				isSettingPatrol = false;
				//CHANGE MOUSE ICON BASED ON ABILITY TARGETING TYPE
				currentAbility = abilityList[0];
				isTargeting = true;
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				isSettingPatrol = false;
				//CHANGE MOUSE ICON BASED ON ABILITY TARGETING TYPE
				currentAbility = abilityList[1];
				isTargeting = true;
			}
			if (isTargeting)
			{
				if (currentAbility.targetTypes.Contains("AOE"))
				{
					if (currentAbility.targetTypes.Contains("Unit_Friendly"))
					{
						//AOE GREEN
						uiManager.ChangeCursorImage("AOE", true, currentAbility.radius);
					}
					else
					{
						//AOE RED
						uiManager.ChangeCursorImage("AOE", false, currentAbility.radius);
					}
				}
				else
				{
					if (currentAbility.targetTypes.Contains("Unit_Friendly"))
					{
						//CROSSHAIR GREEN
						uiManager.ChangeCursorImage("Target", true, currentAbility.radius);
					}
					else
					{
						//CROSSHAIR RED
						uiManager.ChangeCursorImage("Target", false, currentAbility.radius);
					}
				}
				if (Input.GetMouseButtonDown(0)) 
				{
					uiManager.ChangeCursorImage("Default", true, 0.0f);
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (currentAbility.targetTypes.Contains("Ground"))
					{
						LayerMask mask = 1 << 8;
						if (Physics.Raycast(ray, out hit, 200, mask)) 
						{
							if (hit.transform.gameObject.name.Contains("Ground"))
							{
								//target = hit.transform.gameObject;
								StartCoroutine(currentAbility.CastGroundAbility(hit.point));
								//currentState = Unit_States.ATTACK;
								isTargeting = false;
							}
						}
					}
					else
					{
						if (currentAbility.targetTypes.Contains("Unit_Friendly"))
						{
							LayerMask mask = 1 << 9;
							if (Physics.Raycast(ray, out hit, 200, mask)) 
							{
								if (hit.transform.gameObject.GetComponent<Test_Unit>().teamNumber == teamNumber)
								{
									target = hit.transform.gameObject;
									StartCoroutine(currentAbility.CastTargetAbility(hit.transform.gameObject.GetComponent<Test_Unit>()));
									currentState = Unit_States.ATTACK;
									isTargeting = false;
								}
							}
						}
						else if (currentAbility.targetTypes.Contains("Unit_Enemy"))
						{
							LayerMask mask = 1 << 9;
							if (Physics.Raycast(ray, out hit, 200, mask)) 
							{
								if (hit.transform.gameObject.GetComponent<Test_Unit>().teamNumber != teamNumber)
								{
									target = hit.transform.gameObject;
									StartCoroutine(currentAbility.CastTargetAbility(hit.transform.gameObject.GetComponent<Test_Unit>()));
									currentState = Unit_States.ATTACK;
									isTargeting = false;
								}
							}
						}
					}
					if (isTargeting)
					{
						isTargeting = false;
					}
				}
				if (Input.GetMouseButtonDown(1)) 
				{
					uiManager.ChangeCursorImage("Default", true, 0.0f);
					isTargeting = false;
				}
			}

			if (Input.GetKeyDown(KeyCode.P))
			{
				StartCoroutine(CalculateDamageResistance());
				isTargeting = false;
				isSettingPatrol = true;
			}

			if (isSettingPatrol)
			{
				if (Input.GetMouseButtonDown(0)) 
				{
					patrolPoints[0] = transform.position;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					
					LayerMask mask = 1 << 8;
					
					if (Physics.Raycast(ray, out hit, 200, mask)) 
					{
						patrolPoints[1] = hit.point;
						currentPatrolIndex = 1;
						agent.destination = patrolPoints[1];
						currentState = Unit_States.PATROL;
						isSettingPatrol = false;
					}
				}
			}
		}
		if (teamNumber == 1 && ren.isVisible && Input.GetMouseButton(0))
		{
			Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
			camPos.y = Player.InvertMouseY(camPos.y);
			isSelected = Player.selection.Contains(camPos);
			//UI_Manager.instance.SelectUnits();
		}
		if (isSelected)
		{
			ren.material.color = Color.green;
		}
		else
		{
			ren.material.color = Color.white;
		}
		switch (currentState)
		{
			case Unit_States.IDLE:
			Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRange);
			foreach (Collider col in colliders)
			{
				if (col.GetComponent<Test_Unit>() != null && col.GetComponent<Test_Unit>().teamNumber != teamNumber)
				{
					target = col.gameObject;
					currentState = Unit_States.ATTACK;
				}
			}
			break;
			case Unit_States.MOVING:
			if (Vector3.Distance(transform.position, agent.destination) <= 0.0011f + transform.localScale.y / 2.0f)
			{
				colliders = Physics.OverlapSphere(transform.position, aggroRange);
				foreach (Collider col in colliders)
				{
					if (col.GetComponent<Test_Unit>() != null && col.GetComponent<Test_Unit>().teamNumber != teamNumber)
					{
						target = col.gameObject;
						currentState = Unit_States.ATTACK;
					}
					if (target == null)
					{
						currentState = Unit_States.IDLE;
					}
				}
			}
			break;
			case Unit_States.PATROL:
			if (Vector3.Distance(transform.position, agent.destination) <= 0.0011f + transform.localScale.y / 2.0f)
			{
				if (currentPatrolIndex > 0)
				{
					currentPatrolIndex--;
					agent.destination = patrolPoints[currentPatrolIndex];
				}
				else
				{
					currentPatrolIndex++;
					agent.destination = patrolPoints[currentPatrolIndex];
				}
			}
			break;
			case Unit_States.ATTACK:
			if (isAI)
			{
				if (Vector3.Distance(transform.position, originPoint) > leashDistance)
				{
					target = null;
					agent.destination = originPoint;
					currentState = Unit_States.MOVING;
				}
			}
			if (target != null)
			{
				if (Vector3.Distance(transform.position, target.transform.position) <= currentAttackRange)
				{
					agent.destination = transform.position;
					if (canAttack)
					{
						print("ATTACK!");
						target.GetComponent<Test_Unit>().ApplyDamage(currentDamage, "Physical");
						canAttack = false;
						StartCoroutine("AttackTimer");
					}
				}
				else
				{
					agent.destination = target.transform.position;
				}
			}
			else
			{
				colliders = Physics.OverlapSphere(transform.position, aggroRange);
				foreach (Collider col in colliders)
				{
					if (col.GetComponent<Test_Unit>() != null && col.GetComponent<Test_Unit>().teamNumber != teamNumber)
					{
						target = col.gameObject;
						currentState = Unit_States.ATTACK;
					}
					if (target == null)
					{
						currentState = Unit_States.IDLE;
					}
				}
			}
			break;
		}
	}

	public void SelectSelf()
	{
		if (isSelectable)
		{
			isSelected = true;
		}
	}

	public void ApplyDamage(float incomingDamage, string damageType)
	{
		if (!isInvulnerable)
		{
			if (damageType == "Physical")
			{
				currentHealth -= incomingDamage * (Mathf.Abs(1.0f - totalPhysicalReduction));
			}
			else if (damageType == "Spell")
			{
				currentHealth -= incomingDamage * (Mathf.Abs(1.0f - totalSpellReduction));
			}
			else if (damageType == "Pure")
			{
				currentHealth -= incomingDamage;
			}
			if (currentHealth <= 0)
			{
				Destroy(gameObject);
			}
			if (currentHealth >= maxHealth)
			{
				currentHealth = maxHealth;
			}
		}
	}

	public IEnumerator CalculateDamageResistance()
	{
		totalPhysicalReduction = ( (currentArmor) * 0.06f ) / (1.0f + 0.06f *(currentArmor) );
		totalSpellReduction = ( (currentSpellResistance) * 0.06f ) / (1.0f + 0.06f *(currentSpellResistance) );
//		totalPhysicalReduction = Mathf.Abs(1.0f - totalPhysicalReduction);
//		totalSpellReduction = Mathf.Abs(1.0f - totalSpellReduction);
		print ("Total Physical Reduction is: " + totalPhysicalReduction + " and Total Spell Reduction is: " + totalSpellReduction);
		yield break;
	}

	public IEnumerator AddModifiers(List<string> modifiers)
	{
		//Base_Modifier tempModifier;
		for (int i = 0; i < modifiers.Count; i++)
		{
			print ("Applying " + modifiers[i]);
			currentModifiers.Add (gameObject.AddComponent(System.Type.GetType(modifiers[i])) as Base_Modifier);
			currentModifiers[i].ActivateModifier();
//			currentModifiers.Add(tempModifier);
			//currentModifiers.Add (gameObject.AddComponent(System.Type.GetType(modifiers[i])) as Base_Modifier);
			print ("After adding the " + modifiers[i] + ", " + name + " currently has " + currentModifiers.Count + " modifiers active");
		}
		yield break;
	}

	public IEnumerator ApplyModifiers(List <Modifier_Types> modifiers, List<float> modifierValues, List<bool> isPercentages)
	{
		#region Apply Modifiers
		Modifier_Types modifierType;
		float modifierValue = 0.0f;
		bool isPercentage = false;
		int x = 0;
		int y = 0;

		//Modifiers Loop
		for (int i = 0; i < modifiers.Count; i++) 
		{
			print ("Aplying Modifier " + i);
			modifierType = modifiers [i];
			modifierValue = modifierValues [x];
			print ("Aplying Modifier Value " + x);
			// Modifier Value Loop
			if (true)
			{
				if (x < modifierValues.Count)
				{
					x++;
				}
				if (x == modifierValues.Count)
				{
					x--;
				} 
			}
			isPercentage = isPercentages [y];
			print ("Aplying isPercentage " + y);
			// isPercentages Loop
			if (true)
			{
				if (y < isPercentages.Count)
				{
					y++;
				}
				if (y == isPercentages.Count)
				{
					y--;
				}
			}
//			for (int y = 0; y < isPercentages.Count; y++) 
//			{
//				if (y == isPercentages.Count) 
//				{
//					y--;
//				}
//				isPercentage = isPercentages [y];
//			}
			switch (modifierType) 
			{
			case Modifier_Types.AGILITY:
				if (isPercentage) 
				{
					if (modifierValue < 0.0f) 
					{
						currentAgility -= (int)(baseAgility * -modifierValue);
					}
					else 
					{
						currentAgility += (int)(baseAgility * modifierValue);
					}
				} 
				else 
				{
					currentAgility += (int)(modifierValue);
				}
				break;
			case Modifier_Types.INTELLIGENCE:
				if (isPercentage) 
				{
					if (modifierValue < 0.0f) 
					{
						currentIntelligence -= (int)(baseIntelligence * -modifierValue);
					} 
					else 
					{
						currentIntelligence += (int)(baseIntelligence * modifierValue);
					}
				} 
				else 
				{
					currentIntelligence += (int)(modifierValue);
				}
				break;
			case Modifier_Types.STRENGTH:
				if (isPercentage) 
				{
					if (modifierValue < 0.0f) 
					{
						currentStrength -= (int)(baseStrength * -modifierValue);
					} 
					else 
					{
						currentStrength += (int)(baseStrength * modifierValue);
					}
				} 
				else 
				{
					currentStrength += (int)(modifierValue);
				}
				break;
			case Modifier_Types.HEALTH:
				if (isPercentage) 
				{
					if (currentHealth + (baseHealth * modifierValue) > maxHealth) 
					{
						currentHealth = maxHealth;
					}
					else 
					{
						if (modifierValue < 0.0f) 
						{
							currentHealth -= (int)(baseHealth * -modifierValue);
						} 
						else 
						{
							currentHealth += (int)(baseHealth * modifierValue);
						}
					}
				} 
				else 
				{
					if (currentHealth + (baseHealth + modifierValue) > maxHealth) 
					{
						currentHealth = maxHealth;
					} 
					else 
					{
						currentHealth += (int)(modifierValue);
					}
				}
				break;
			case Modifier_Types.MANA:
				if (isPercentage) 
				{
					if (currentMana + (baseMana * modifierValue) > maxMana) 
					{
						currentMana = maxMana;
					} 
					else 
					{
						if (modifierValue < 0.0f) 
						{
							currentMana -= (int)(baseMana * -modifierValue);
						}
						else 
						{
							currentMana += (int)(baseMana * modifierValue);
						}
					}
				} 
				else 
				{
					if (currentMana + (baseMana + modifierValue) > maxMana) 
					{
						currentMana = maxMana;
					} 
					else 
					{
						currentMana += (int)(modifierValue);
					}
				}
				break;
			case Modifier_Types.MOVESPEED:
				if (isPercentage) 
				{
					if (modifierValue < 0.0f) 
					{
						currentMoveSpeed -= (int)(baseMoveSpeed * -modifierValue);
					} 
					else 
					{
						currentMoveSpeed += (int)(baseMoveSpeed * modifierValue);
					}
				} 
				else 
				{
					currentMoveSpeed += (modifierValue);
				}
				break;
			case Modifier_Types.ATTACK_SPEED:
				if (isPercentage) 
				{
					if (modifierValue < 0.0f) 
					{
						currentAttackSpeed -= (int)(baseAttackSpeed * -modifierValue);
					} 
					else 
					{
						currentAttackSpeed += (int)(baseAttackSpeed * modifierValue);
					}
				} 
				else 
				{
					currentAttackSpeed += (modifierValue);
				}
				break;
			case Modifier_Types.DAMAGE:
				if (isPercentage) 
				{
					if (modifierValue < 0.0f) 
					{
						currentDamage -= (int)(baseDamage * -modifierValue);
					} 
					else 
					{
						currentDamage += (int)(baseDamage * modifierValue);
					}
				} 
				else 
				{
					currentDamage += (int)(modifierValue);
				}
				break;
			case Modifier_Types.ARMOR:
				if (isPercentage) 
				{
					if (modifierValue < 0.0f) 
					{
						currentArmor -= (int)(baseArmor * -modifierValue);
					} 
					else 
					{
						currentArmor += (int)(baseArmor * modifierValue);
					}
				} 
				else 
				{
					currentArmor += (int)(modifierValue);
				}
				break;
			case Modifier_Types.SPELL_RESISTANCE:
				if (isPercentage) 
				{
					if (modifierValue < 0.0f) 
					{
						currentSpellResistance -= (int)(baseSpellResistance * -modifierValue);
					} 
					else 
					{
						currentSpellResistance += (int)(baseSpellResistance * modifierValue);
					}
				} 
				else 
				{
					currentSpellResistance += (int)(modifierValue);
				}
				break;
			}
		}
		yield break;
		#endregion
	}

	public IEnumerator AttackTimer()
	{
		yield return new WaitForSeconds(currentAttackSpeed);
		canAttack = true;
		yield break;
	}

	void OnDrawGizmosSelected() 
	{
		Gizmos.color = debugColor2;
		Gizmos.DrawSphere(transform.position, aggroRange);
		Gizmos.color = debugColor1;
		Gizmos.DrawSphere(transform.position, currentAttackRange);
		Gizmos.color = debugColor3;
		Gizmos.DrawSphere(transform.position, leashDistance);

	}
	
}







