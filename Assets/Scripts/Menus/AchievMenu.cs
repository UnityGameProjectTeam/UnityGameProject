﻿using UnityEngine;
using System.Collections.Generic;

public class AchievMenu : SubMenu 
{
	private AchievementManager am;

	//Fenetre d'affichage des achievements accomplis
	private Rect achievsWindowRect;

	// Position sur la scrollBar
	private Vector2 scrollPosition = Vector2.zero;
	// Choix d'afficher les achievements débloqués ou non
	private int itemSelected = 0;
	// Nom des boutons
	private string[] toolbarStrings;

	// variables de placement (en pixels)
	private int spaceBetweenItem = 30;
	private int letterSize = 10;
	private int paddingTop = 40;

	// variable de tailles (en %)
	private float coefWidth = 0.4f;
	private float coefHeight = 0.66f;

	// Déterminés automatiquement
	private int windowWidth;
	private int windowHeight;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();

		toolbarStrings = new string[2];
		toolbarStrings[0] = LanguageManager.Instance.GetTextValue("MainMenu.unlock");
		toolbarStrings[1] = LanguageManager.Instance.GetTextValue("MainMenu.lock");

		// Récupère la sauvegarde des achievements accomplis
		AchievementsSaveReader asr = FindObjectOfType<AchievementsSaveReader>();

		am = FindObjectOfType<AchievementManager>();

		List<string> achievCompleted = asr.getAchievementsCompleted();
		if (achievCompleted != null)
			am.loadAchievements(achievCompleted);
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnGUI()
	{
		if (!inFrontOf)
			return;

		// Détermine les dimensions de la fenetre à afficher
		windowWidth = (int)(Screen.width*coefWidth);
		windowHeight = (int)(Screen.height*coefHeight);

		// Fenetre à afficher
		achievsWindowRect = new Rect(Screen.width/2 - windowWidth/2,
		                             Screen.height/2 - windowHeight/2,
		                             windowWidth,
		                             windowHeight);
		achievsWindowRect = GUI.Window(1, achievsWindowRect, achievementsWindowOpen, LanguageManager.Instance.GetTextValue("MainMenu.achievCompleted"));
	}

	// Rempli la fenetre des achievements accomplis
	void achievementsWindowOpen(int windowId)
	{
		// Zone de dessin + scroll bars
		GUILayout.BeginArea(new Rect(10, 20, windowWidth, windowHeight));

		// Bouton sélectionné
		itemSelected = GUI.Toolbar(new Rect(0, 0, 150, 30), itemSelected, toolbarStrings);

		// Achievements réalisés
		if (itemSelected == 0)
		{
			List<Achievement> achievementsCompleted = am.getAchievementsUnlocked();

			scrollPosition = GUI.BeginScrollView(	new Rect(0, paddingTop, windowWidth-20, 0.5f*windowHeight),
		                                        	scrollPosition,
			                                        new Rect(0, paddingTop, windowWidth-40, achievementsCompleted.Count*spaceBetweenItem));

			// Affiche la liste des achievements acquis
			for (int i = 0 ; i < achievementsCompleted.Count ; i++)
			{
				GUI.Label(	new Rect(20,
				                  	 paddingTop + i*spaceBetweenItem,
				                     achievementsCompleted[i].getName().Length * letterSize,
				                     spaceBetweenItem),
				          	new GUIContent(	achievementsCompleted[i].getName(),
				               				generateToolTip(achievementsCompleted[i].getDescription())));
			}
		}
		// Achievements non réalisés
		else
		{
			List<Achievement> achievementsLocked = am.getAchievementsLocked();

			scrollPosition = GUI.BeginScrollView(	new Rect(0, paddingTop, windowWidth-20, 0.5f*windowHeight),
			                                     	scrollPosition,
				                                    new Rect(0, paddingTop, windowWidth-40, achievementsLocked.Count*spaceBetweenItem));

			// Affiche la liste des achievements acquis
			for (int i = 0 ; i < achievementsLocked.Count ; i++)
			{
				GUI.Label(	new Rect(20,
				                    paddingTop + i*spaceBetweenItem,
				                    achievementsLocked[i].getName().Length * letterSize,
				                    spaceBetweenItem),
				          	new GUIContent(	achievementsLocked[i].getName(),
				               				generateToolTip(achievementsLocked[i].getDescription())));
			}
		}

		GUI.EndScrollView();

		// Affiche la description de l'achievement survolé
		GUI.Label(	new Rect(20,
		                     0.63f*windowHeight + 10,
		                   	 windowWidth - 50,
		                   	 0.16f*windowHeight),
		          	GUI.tooltip);

		// Bouton de retour au menu principal
		float buttonWidth = 0.33f * windowWidth;
		float buttonHeight = 0.1f * windowHeight;
		if (GUI.Button(	new Rect((windowWidth-10)/2 - buttonWidth/2,
		                          windowHeight - 2*buttonHeight,
		                          buttonWidth,
		                          buttonHeight),
		                LanguageManager.Instance.GetTextValue("MainMenu.Return")))
		{
			setInfFrontOf(false);
			cam.goToMainMenu();
		}
		
		GUILayout.EndArea();
	}

	string generateToolTip(string description)
	{
		return 	LanguageManager.Instance.GetTextValue("MainMenu.description")
				+ System.Environment.NewLine
				+ description;
	}
}
