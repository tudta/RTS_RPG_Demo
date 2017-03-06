using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	private int playerNumber;
	private Player_Camera playerCamera;
	public List<GameObject> selectedUnits = new List<GameObject>();
	public Texture2D selectionVisual;
	public static Rect selection = new Rect (0,0,0,0);
	private Vector3 startClick = -Vector3.one;

	// Use this for initialization
	void Start () 
	{
		playerNumber = int.Parse(name.Substring(name.Length - 1));
		playerCamera = GameObject.Find("Camera_" + playerNumber).GetComponent<Player_Camera>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Y))
		{
			playerCamera.ToggleCameraLock();
		}

		if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
		{
			playerCamera.ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
		}

		if (Input.GetMouseButtonDown(0))
		{
			startClick = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			startClick = -Vector3.one;
		}
		if (Input.GetMouseButton(0))
		{
			selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));
			if (selection.width < 0)
			{
				selection.x += selection.width;
				selection.width = -selection.width;
			}
			if (selection.height < 0)
			{
				selection.y += selection.height;
				selection.height = -selection.height;
			}
		}
	}

	private void OnGUI()
	{
		if (startClick != -Vector3.one)
		{
			GUI.color = new Color(1,1,1,0.5f);
			GUI.DrawTexture(selection, selectionVisual);
		}
	}

	public static float InvertMouseY(float y)
	{
		return Screen.height - y;
	}
}
