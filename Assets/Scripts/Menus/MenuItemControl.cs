﻿using UnityEngine;
using System.Collections;

public class MenuItemControl : MonoBehaviour 
{
	
	public bool isQuitButton = false;
	public bool isPlayButton = false;
	public bool isNewButton = false;
	public bool isLoadButton = false;
	public bool isReturn = false;
	
	private CameraMenu cam;
		
	void Start()
	{
		cam = FindObjectOfType(System.Type.GetType("CameraMenu")) as CameraMenu;
	}
	
	void OnMouseEnter() 
	{
		renderer.material.color = Color.red;
	}
	
	void OnMouseExit() 
	{
		renderer.material.color = Color.white;
	}
	
	void OnMouseUp()
    {
		if(isQuitButton)
			Application.Quit();
		
		if(isPlayButton)
		{	
			cam.goToLaunchMenu();
			return;
		}
		
		if(isNewButton)
       		 Application.LoadLevel(1);
 		
		if(isLoadButton)
		{
			//plus tard
			return;
		}
		
		if(isReturn)
		{
			cam.goToMainMenu();
		}
    }
	//20 1.5 5
	//25.4 1.5 19.5
}
