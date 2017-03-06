using UnityEngine;
using System.Collections;
using Pathfinding;

public class Test_Path_Unit : MonoBehaviour 
{
	public Transform target;
	public float moveSpeed = 10.0f;
	private Path path;
	private Seeker seeker;
	private int currentWaypoint;
	private CharacterController characterController;
	private float maxNodeCollisionDetectionDistance = 2.0f;

	void Awake()
	{

	}

	// Use this for initialization
	void Start () 
	{
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		target = GameObject.Find("Target").GetComponent<Transform>();
		seeker.StartPath(transform.position, target.position, OnPathComplete);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void OnPathComplete (Path p)
	{
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
		}
		else
		{
			print (p.error);
		}
	}

	void FixedUpdate()
	{
		if (path == null)
		{
			return;
		}
		if (currentWaypoint >= path.vectorPath.Count)
		{
			return;
		}
//		if (Vector3.Distance (transform.position, ))
//		{
//
//		}
		Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized * moveSpeed;
		characterController.SimpleMove(direction);
		if (Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]) < maxNodeCollisionDetectionDistance)
		{
			currentWaypoint++;
		}
		//transform.position = path.vectorPath[currentWaypoint];
	}
}
