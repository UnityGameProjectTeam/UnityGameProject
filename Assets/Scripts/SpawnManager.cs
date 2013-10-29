﻿using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {
	
	private SpawnController[] spawns;
	
	private float timeLastSpawn;
	private float timeStep = 5.0F;
	private float spawnDelay;
	
	private int _nbEnnemies = 0;// Should be modified only using the NbEnnemies property
	private int NbEnnemies {
		get {
            return _nbEnnemies;
        }
        set {
            _nbEnnemies = value;
			spawnDelay = timeStep - timeStep * 1/Mathf.Sqrt((_nbEnnemies+2)/2);
        }
	}
	
	
	// Use this for initialization
	void Start () {
		spawns = FindObjectsOfType(System.Type.GetType("SpawnController")) as SpawnController[];
		
		// Start with 4 ennemies
		for (int i=0; i < 4; ++i)
			addEnnemy();
		
		timeLastSpawn = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		// Create an ennemy if the specified time is elapsed
		if (Time.time - timeLastSpawn >= spawnDelay)
		{
			timeLastSpawn += spawnDelay;
			addEnnemy();
		}
	}
	
	private void addEnnemy()
	{
		// Add an ennemy at a random spawn point.
		int P = Random.Range(0, spawns.Length);
		int i = P;
		do { // Add the ennemy at the first active spawn point
			SpawnController sp = spawns[i];
			if (sp.isActive())
			{
				sp.spawn();
				NbEnnemies++;
				break;
			}
			i = (i + 1) % spawns.Length;
		} while (i != P);
	}
	
	public void decNbEnnemies()
	{
		NbEnnemies--;
	}
}