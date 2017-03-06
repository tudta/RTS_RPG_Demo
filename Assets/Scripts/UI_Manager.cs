using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour 
{
	public static UI_Manager instance;
	
	#region Cursor Image & Projector Variables
	public Texture2D[] cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;
	public Material[] targetingMaterials;
	public Projector projector;
	public float defaultProjectorSize = 4.0f;
	public float projectorAlpha = 0.0f;
	#endregion

	public bool unitsAreSelected;
	public List<Test_Unit> selectedUnits;

	#region Unit UI Stats
//	public string damage;
//	public string defense;
//	public string health;
//	public string mana;
//	public string agility;
//	public string strength;
//	public string willpower;
//	public string name;
//	public string classInfo;
	#endregion

	#region Unit UI Elements
	private Text damageText;
	private Text defenseText;
	private Text healthText;
	private Text manaText;
	private Text agilityText;
	private Text strengthText;
	private Text willpowerText;
	private Text nameText;
	private Text classInfoText;
	#endregion

	void Awake()
	{
		GetInstance();
		projector.gameObject.SetActive(true);
		projector.material.color = new Color (projector.material.color.r, projector.material.color.g, projector.material.color.b, projectorAlpha);
	}

	// Use this for initialization
	void Start () 
	{
		hotSpot = new Vector2 (0.0f, 0.0f);
		Cursor.SetCursor(cursorTexture[0],hotSpot, CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (projector.material.color.a > 0)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			LayerMask mask = 1 << 8;
			if (Physics.Raycast(ray, out hit, 200, mask)) 
			{
				projector.transform.position = new Vector3 (hit.point.x, projector.transform.position.y, hit.point.z);
			}
		}
		if (unitsAreSelected)
		{
			UpdateUnitStats();
		}
	}

	void GetInstance()
	{
		instance = this;
	}

	public void ChangeCursorImage(string style, bool isFriendly, float radiusInFeet)
	{
		if (style == "Default")
		{
			projector.material.color = new Color (projector.material.color.r, projector.material.color.g, projector.material.color.b, 0.0f);
			Cursor.visible = true;
		}
		else
		{
			projector.material.color = new Color (projector.material.color.r, projector.material.color.g, projector.material.color.b, 1.0f);
			Cursor.visible = false;
		}
		if (style == "AOE")
		{
			projector.orthographicSize = radiusInFeet / 5.86f;
			projector.material = targetingMaterials [0];
			if (isFriendly)
			{
				projector.material.color = Color.green;
			}
			else
			{
				projector.material.color = Color.red;
			}
		}
		else if (style == "Target")
		{
			projector.orthographicSize = defaultProjectorSize;
			projector.material = targetingMaterials [1];
			if (isFriendly)
			{
				projector.material.color = Color.green;
			}
			else
			{
				projector.material.color = Color.red;
			}
		}
	}

	public void SelectUnits(List<Test_Unit> units)
	{
		selectedUnits.AddRange(units);
		if (selectedUnits.Count > 0)
		{
			unitsAreSelected = true;
		}
	}

	public void DeselectUnits(List<Test_Unit> units)
	{
		for (int i = 0; i < units.Count; i++)
		{
			selectedUnits.Remove(selectedUnits[i]);
		}
		if (selectedUnits.Count < 1)
		{
			unitsAreSelected = false;
		}
	}

	void UpdateUnitStats()
	{
		if (selectedUnits.Count == 0)
		{
			damageText.text = "0";
			defenseText.text = "0";
			healthText.text = "0";
			manaText.text = "0";
			agilityText.text = "0";
			strengthText.text = "0";
			willpowerText.text = "0";
			nameText.text = string.Empty;
		}
		else if (selectedUnits.Count == 1)
		{
			damageText.text = selectedUnits[0].currentDamage.ToString();
			defenseText.text = (selectedUnits[0].currentArmor + selectedUnits[0].currentSpellResistance).ToString();
			healthText.text = selectedUnits[0].currentHealth.ToString();
			manaText.text = selectedUnits[0].currentMana.ToString();
			agilityText.text = selectedUnits[0].currentAgility.ToString();
			strengthText.text = selectedUnits[0].currentStrength.ToString();
			willpowerText.text = selectedUnits[0].currentIntelligence.ToString();
			nameText.text = selectedUnits[0].name;
		}
	}

	void OnMouseEnter() 
	{
		Cursor.SetCursor(cursorTexture[0], hotSpot, cursorMode);
	}
	void OnMouseExit() 
	{
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
	}
}
