﻿using UnityEngine;
using System.Collections;

public class PauseMenu : PauseSystem
{
	
	protected override void Start()
	{
		base.Start();
	}
	
	
	protected override void Update()
	{		
		// si on appuie sur "Escape" change l'état du jeu
		if(Input.GetButtonDown("Menu"))
		{
			paused = !paused;
			
			UpdateState();
		}
	}
	
	void OnGUI()
	{
		if(paused)
		{
			GUILayout.BeginArea(new Rect(Screen.width/2-50,Screen.height/2-50, 100,100));
			
			if(GUILayout.Button("Continuer"))
			{
				paused = false;
				
				UpdateState();
			}
			if(GUILayout.Button("Menu"))
			{
				 Application.LoadLevel(0);
			}
			
			GUILayout.EndArea();
			
		}
	}
	
	
	protected override void UpdateState()
	{
		base.UpdateState();
		GetComponent<Inventory>().enabled = !paused;
	}
	
}
