using UnityEngine;
using System.Collections;

public class Player_Camera : MonoBehaviour 
{
	public float moveSpeed = 0.0f;
	public float minZoomDistance;
	public float maxZoomDistance;
	public int leftScrollLimit;
	public int rightScrollLimit;
	public int topScrollLimit;
	public int bottomScrollLimit;
	public int cameraNumber;
	public bool isLockedOn = false;
	public GameObject lockTarget;

	// Use this for initialization
	void Start () 
	{
		cameraNumber = int.Parse(name.Substring(name.Length - 1));
		//lockTarget = GameObject.Find("Player_" + cameraNumber);
		lockTarget = GameObject.Find ("Base_Unit");
		SetScrollLimits();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isLockedOn)
		{
//			print (Input.mousePosition);
			if (!Input.GetMouseButton(0))
			{
				if (Input.mousePosition.x <= leftScrollLimit)
				{
					transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
				}
				if (Input.mousePosition.x >= rightScrollLimit)
				{
					transform.Translate(Vector3.right* moveSpeed * Time.deltaTime);
				}
				if (Input.mousePosition.y >= topScrollLimit)
				{
					transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
				}
				if (Input.mousePosition.y <= bottomScrollLimit)
				{
					transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
				}
				if (Input.GetKey(KeyCode.UpArrow))
				{
					transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
				}
				if (Input.GetKey(KeyCode.DownArrow))
				{
					transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
				}
				if (Input.GetKey(KeyCode.LeftArrow))
				{
					transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
				}
				if (Input.GetKey(KeyCode.RightArrow))
				{
					transform.Translate(Vector3.right* moveSpeed * Time.deltaTime);
				}
			}
		}
		else
		{
			//print ("Target's name is: " + lockTarget.name + " and its position is " + lockTarget.transform.position);
			transform.position = new Vector3(lockTarget.transform.position.x, transform.position.y, lockTarget.transform.position.z);
		}
	}

	public void ZoomCamera(float axis)
	{
		if (axis > 0)
		{
			if (transform.position.y > minZoomDistance)
			{
				transform.Translate(Vector3.forward);
			}
		}
		else
		{
			if (transform.position.y < maxZoomDistance)
			{
				transform.Translate(Vector3.back);
			}
		}
	}

	public void ToggleCameraLock()
	{
		isLockedOn = !isLockedOn;
	}

	private void SetScrollLimits()
	{
		leftScrollLimit = 0 + (Screen.width / 16);
		rightScrollLimit = Screen.width - (Screen.width / 16);
		topScrollLimit = (int)(Screen.height - (Screen.height / 16) * 1.78f);
		bottomScrollLimit = (int)(0 + (Screen.height / 16) * 1.78f);
	}
}
