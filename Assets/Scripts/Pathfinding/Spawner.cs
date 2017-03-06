using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	public GameObject spawnEntity;

	public Transform[] spawnLocations;

	public float spawnInterval;

	// Use this for initialization
	void Start () 
	{
		SpawnEntities(spawnEntity, spawnLocations);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SpawnEntities (GameObject entity, Transform[] locations)
	{
		for (int i = 0; i < spawnLocations.Length; i++)
		{
			Instantiate (entity, locations[i].position, transform.rotation);
		}
	}
}
