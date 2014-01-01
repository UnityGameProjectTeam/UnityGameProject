﻿using UnityEngine;
using System.Collections;

public class CameraMenu : CameraPath
{
	public GameObject mainMenu;
	public GameObject launchMenu;
	public SubMenu settingsMenu;
	public SubMenu advancedSettingsMenu;
	public SubMenu achievementsMenu;
	public SubMenu creditsMenu;

	public Vector3 cameraPositionOffset;

	private bool arrived;
	private SubMenu callbackObj;

	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		arrived = true;
		callbackObj = null;
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		base.Update();

		arrived = isArrived();
		if (arrived) {
			if (callbackObj != null) {
				callbackObj.setInfFrontOf(true);
				callbackObj = null;
			}
		}
	}

	
	public void goToMainMenu()
	{
		goTo(mainMenu.transform.position + cameraPositionOffset);
		callbackObj = null;
	}
	
	public void goToLaunchMenu()
	{
		goTo(launchMenu.transform.position + cameraPositionOffset);
		callbackObj = null;
	}

	public void goToSettingsMenu()
	{
		goTo(settingsMenu.transform.position + cameraPositionOffset);
		callbackObj = settingsMenu;
	}

	public void goToAdvancedSettingsMenu()
	{
		goTo(advancedSettingsMenu.transform.position + cameraPositionOffset);
		callbackObj = advancedSettingsMenu;
	}

	public void goToAchievementsMenu()
	{
		goTo(achievementsMenu.transform.position + cameraPositionOffset);
		callbackObj = achievementsMenu;
	}

	public void goToCreditsMenu()
	{
		goTo(creditsMenu.transform.position + cameraPositionOffset);
		callbackObj = creditsMenu;
	}

}