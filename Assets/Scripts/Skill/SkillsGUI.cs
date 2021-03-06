﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillsGUI : MonoBehaviour 
{
	// Indique si le joueur est à proximité
	private bool isActive = false;
	// Indique si la fenetre des arbre de compétences est ouverte
	private bool skillsOpen = false;
	
	//Fenetre d'achat des skills
	private Rect skillsWindowRect;

	// Joueur
	public PlayerController player;
	// Caméra du joueur
	private CameraController cam;

	// Message affiché lorsque on s'approche de l'élément portant ce script
	public GUIText openSkills;

	// Marge en pixels entre 2 arbres de compétences
	private int marginBetweenSkillTree = 150;
	// Pourcentage de décalage sur la droite après chaque bouton
	private float horizontalMarginBetweenButton = 0.05f;
	// Permet de régler le décalage vertical des branches des arbres de compétences (en pixels)
	private int verticalSpace = 25;
	// Coefficient pour la largeur des boutons en fonction du texte à afficher
	private int coefSize = 9;
	// Largeur en pixel de l'arbre de compétence le plus long
	private int maxTreeWidth = 0;

	// Position sur la scrollBar
	private Vector2 scrollPosition = Vector2.zero;

	// Nombre d'arbre de compétences déjà affichés
	private int nbTreeAlreadyDrawn = 0;
	
	// Use this for initialization
	void Start () 
	{
		openSkills.text = LanguageManager.Instance.GetTextValue("Interface.openCrypt");
		openSkills.enabled = false;
		cam = FindObjectOfType<CameraController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isActive)
			return;

		if (Input.GetKeyUp(KeyCode.H))
		{
			//update les stats skills
			player.getSkillManager().updateSkill();
			// Met le joueur en pause pour qu'il ne se déplace pas en meme temps qu'il achète ses skills
			player.onPause();
			// Change l'état affiché/masqué du panneau d'achat des skills
			skillsOpen = !skillsOpen;
		}

		// Affiche ou non le message
		if (!skillsOpen)
		{
			cam.setIsActive(true);
			openSkills.enabled = true;
			// Masque le curseur
			Screen.showCursor = false;
			Screen.lockCursor = true;
		}
		else
		{
			cam.setIsActive(false);
			openSkills.enabled = false;
			// Affiche le curseur
			Screen.showCursor = true;
			Screen.lockCursor = false;
		}
	}
	
	void OnGUI()
	{
		// Ouvre la fenetre des arbres de compétences
		if (isActive && skillsOpen)
		{
			skillsWindowRect = new Rect(0,0, Screen.width, Screen.height);
			skillsWindowRect = GUI.Window(1, skillsWindowRect, skillsWindowOpen, LanguageManager.Instance.GetTextValue("Interface.skillsTrees"));
		}
	}


	// Rempli la fenetre des arbres des compétences avec les arbres !
	void skillsWindowOpen(int windowId)
	{
		
		GUILayout.BeginArea(new Rect(20, 20, Screen.width-10, Screen.height-20));

		// Nombre de sous arbre de compétence à afficher
		float nbTree = (int)(player.getSkillManager().getListOfSkills().Count/3);
		// Mise en place des scroll Bars
		scrollPosition = GUI.BeginScrollView(	new Rect(0, 0, Screen.width-30, Screen.height-100),
		                                     	scrollPosition,
		                                     	new Rect(0, 0, maxTreeWidth, nbTree*marginBetweenSkillTree+verticalSpace));

		nbTreeAlreadyDrawn = 0;

		// Affiche les arbres de compétences
		showGUIPassiveSkills(1, 2);
		showGUIMagicSkills(3, 3);

		GUI.EndScrollView();

		// Affiche le tooltip du sort survolé : description
		if (GUI.tooltip != null
		    && GUI.tooltip != "")
		{
			GUI.Label(new Rect(0.08f*Screen.width,
			                   Screen.height - 85,
			                   Screen.width - 50,
			                   50f),
			          GUI.tooltip);
		}

		GUILayout.EndArea();
	}


	/*
	*	Affiche un/des arbre(s) de compétence(s) Magique
	*	params 	=> firstTreePosition : numéro du premier arbre à dessiner dans l'ordre de la liste des skills du player
	*		   	=> nbSkillTree : nombre d'arbre à afficher à partir de firstTreePosition
	*/
	void showGUIMagicSkills(int firstTreePosition, int nbSkillTree)
	{
		List<Skills> listSkills = player.getSkillManager().getListOfSkills();

		// Pour le parcours de la liste des compétences, assigne les bons index
		int beginFor = (firstTreePosition-1)*3;
		int endFor = beginFor + nbSkillTree*3;

		// On démarre à 50 pixels du bord supérieur
		// et on se décale du si d'autre arbre sont avant
		int heightFromTop = -50 + nbTreeAlreadyDrawn*marginBetweenSkillTree;
		
		// Parcours de la liste des skills pour le nombre de sous arbre demandés
		// Un tour de boucle affiche 1 arbre
		for (int i = beginFor ; i < endFor ; i+=3)
		{
			heightFromTop += marginBetweenSkillTree;

			// Largeur du bouton en fonction du texte
			float firstSkillButtonWidth = listSkills[i].getName().Length * coefSize;
			// Met à jour la marge gauche
			float marginLeft = 0.05f*Screen.width;
			// Si le premier skill est déjà acheté
			if (listSkills[i].getIsBought())
			{
				BaseSkills baseSkillRank1 = listSkills[i] as BaseSkills;

				// Le skill étant acheté on l'affiche avec un bouton grisé
				GUI.enabled = false;
				GUI.Button(new Rect(marginLeft,
				                    heightFromTop,
				                    firstSkillButtonWidth,
				                    30),
				           new GUIContent(	listSkills[i].getName(),
				               				generateToolTip(listSkills[i].getDescription(),
				                							listSkills[i].getPrice())));

				// Met à jour la marge gauche
				marginLeft += (firstSkillButtonWidth + horizontalMarginBetweenButton*Screen.width);

				// Largeur des boutons en fonction du texte
				float firstLittleSkillButtonWidthUp = baseSkillRank1.getNameDamage().Length * coefSize;
				float firstLittleSkillButtonWidthDown = baseSkillRank1.getNameAd().Length * coefSize;
				// On affiche les boutons des 2 branches de skills qui suivent
				GUI.enabled = baseSkillRank1.getCostIncDamage() <= player.getExperience();
				if (GUI.Button(new Rect(marginLeft,
				                        heightFromTop-verticalSpace,
				                        firstLittleSkillButtonWidthUp,
				                        30),
				               new GUIContent(	baseSkillRank1.getNameDamage(),
				               					generateToolTip(baseSkillRank1.getDescriptionDamage(),
				                								baseSkillRank1.getCostIncDamage()))))
				{
					upgradeMagicLittleSkill(baseSkillRank1, true);
				}
				
				GUI.enabled = baseSkillRank1.getCostIncAd() <= player.getExperience();
				if (GUI.Button(new Rect(marginLeft,
				                        heightFromTop+verticalSpace,
				                        firstLittleSkillButtonWidthDown,
				                        30),
				               new GUIContent(	baseSkillRank1.getNameAd(),
				               					generateToolTip(baseSkillRank1.getDescriptionAd(),
				                								baseSkillRank1.getCostIncAd()))))
				{
					upgradeMagicLittleSkill(baseSkillRank1, false);
				}

				// Met à jour la marge gauche
				if (marginLeft + firstLittleSkillButtonWidthUp > marginLeft + firstLittleSkillButtonWidthDown)
					marginLeft += (firstLittleSkillButtonWidthUp + horizontalMarginBetweenButton*Screen.width);
				else
					marginLeft += (firstLittleSkillButtonWidthDown + horizontalMarginBetweenButton*Screen.width);

				// Vérifie si le second skill peut etre débloqué
				listSkills[i+1].unlockedSkill();

				// Largeur du bouton en fonction du texte
				float secondSkillButtonWidth = listSkills[i+1].getName().Length * coefSize;
				// Si le second skill est déjà acheté
				if (listSkills[i+1].getIsBought())
				{
					BaseSkills baseSkillRank2 = listSkills[i+1] as BaseSkills;
					
					// Le skill étant acheté on l'affiche avec un bouton grisé
					GUI.enabled = false;
					GUI.Button(new Rect(marginLeft,
					                    heightFromTop,
					                    secondSkillButtonWidth,
					                    30),
					           new GUIContent(	listSkills[i+1].getName(),
					               				generateToolTip(listSkills[i+1].getDescription(),
					                							listSkills[i+1].getPrice())));

					// Met à jour la marge gauche
					marginLeft += (secondSkillButtonWidth + horizontalMarginBetweenButton*Screen.width);

					// Largeur des boutons en fonction du texte
					float secondLittleSkillButtonWidthUp = baseSkillRank2.getNameDamage().Length * coefSize;
					float secondLittleSkillButtonWidthDown = baseSkillRank2.getNameAd().Length * coefSize;
					// On affiche les boutons des 2 branches de skills qui suivent
					GUI.enabled = baseSkillRank2.getCostIncDamage() <= player.getExperience();
					if (GUI.Button(new Rect(marginLeft,
					                        heightFromTop-verticalSpace,
					                        secondLittleSkillButtonWidthUp,
					                        30),
					               new GUIContent(	baseSkillRank2.getNameDamage(),
					               					generateToolTip(baseSkillRank2.getDescriptionDamage(),
					                								baseSkillRank2.getCostIncDamage()))))
					{
						upgradeMagicLittleSkill(baseSkillRank2, true);
					}
					
					GUI.enabled = baseSkillRank2.getCostIncAd() <= player.getExperience();
					if (GUI.Button(new Rect(marginLeft,
					                        heightFromTop+verticalSpace,
					                        secondLittleSkillButtonWidthDown,
					                        30),
					               new GUIContent(	baseSkillRank2.getNameAd(),
					               					generateToolTip(baseSkillRank2.getDescriptionAd(),
					                								baseSkillRank2.getCostIncAd()))))
					{
						upgradeMagicLittleSkill(baseSkillRank2, false);
					}

					// Met à jour la marge gauche
					if (marginLeft + secondLittleSkillButtonWidthUp > marginLeft + secondLittleSkillButtonWidthDown)
						marginLeft += (secondLittleSkillButtonWidthUp + horizontalMarginBetweenButton*Screen.width);
					else
						marginLeft += (secondLittleSkillButtonWidthDown + horizontalMarginBetweenButton*Screen.width);
					
					// Vérifie si le dernier skill peut etre débloqué
					listSkills[i+2].unlockedSkill();

					// Largeur du bouton en fonction du texte
					float lastSkillButtonWidth = listSkills[i+2].getName().Length * coefSize;
					// Si le dernier skill est déjà acheté
					if (listSkills[i+2].getIsBought())
					{
						// Le skill étant acheté on l'affiche avec un bouton grisé
						GUI.enabled = false;
						GUI.Button(new Rect(marginLeft,
						                    heightFromTop,
						                    lastSkillButtonWidth,
						                    30),
						           new GUIContent(	listSkills[i+2].getName(),
						               				generateToolTip(listSkills[i+2].getDescription(),
						                							listSkills[i+2].getPrice())));

						// Met à jour la marge gauche
						marginLeft += (lastSkillButtonWidth + 20);
					}
					// Si le dernier skill est débloqué
					else if (listSkills[i+2].getIsUnlock())
					{
						// On active ou non le bouton si on a suffisamment d'expérience
						GUI.enabled = listSkills[i+2].getPrice() <= player.getExperience();
						
						// Si on achète le skill on met à jour l'expérience du joueur
						if (GUI.Button(new Rect(marginLeft,
						                        heightFromTop,
						                        lastSkillButtonWidth,
						                        30),
						               new GUIContent(	listSkills[i+2].getName(),
						               					generateToolTip(listSkills[i+2].getDescription(),
						                								listSkills[i+2].getPrice()))))
						{
							unlockSkill(listSkills[i+2]);
						}

						// Met à jour la marge gauche
						marginLeft += (lastSkillButtonWidth + 20);
					}
					// Met à jour la taille de l'arbre le plus long a affich
					updateMaxTreeWidth((int)marginLeft);

				}
				// Si le deuxième skill est débloqué
				else if (listSkills[i+1].getIsUnlock())
				{
					// On active ou non le bouton si on a suffisamment d'expérience
					GUI.enabled = listSkills[i+1].getPrice() <= player.getExperience();
					
					// Si on achète le skill on met à jour l'expérience du joueur
					if (GUI.Button(new Rect(marginLeft,
					                        heightFromTop,
					                        secondSkillButtonWidth,
					                        30),
					               new GUIContent(	listSkills[i+1].getName(),
					               					generateToolTip(listSkills[i+1].getDescription(),
					                								listSkills[i+1].getPrice()))))
					{
						unlockSkill(listSkills[i+1]);
					}
				}

			}
			// Si le premier skill est débloqué
			else if (listSkills[i].getIsUnlock())
			{
				// On active ou non le bouton si on a suffisamment d'expérience
				GUI.enabled = listSkills[i].getPrice() <= player.getExperience();
				
				// Si on achète le skill on met à jour l'expérience du joueur
				if (GUI.Button(new Rect(marginLeft,
				                        heightFromTop,
				                        firstSkillButtonWidth,
				                        30),
				               new GUIContent(	listSkills[i].getName(),
				               					generateToolTip(listSkills[i].getDescription(),
				                								listSkills[i].getPrice()))))
				{
					unlockSkill(listSkills[i]);
				}
			}

			nbTreeAlreadyDrawn++;
		}
	}

	/*
	*	Affiche un/des arbre(s) de compétence(s) passives
	*	params 	=> firstTreePosition : numéro du premier arbre à dessiner dans l'ordre de la liste des skills du player
	*		   	=> nbSkillTree : nombre d'arbre à afficher à partir de firstTreePosition
	*/
	void showGUIPassiveSkills(int firstTreePosition, int nbSkillTree)
	{
		List<Skills> listSkills = player.getSkillManager().getListOfSkills();
		
		// Pour le parcours de la liste des compétences, assigne les bons index
		int beginFor = (firstTreePosition-1)*3;
		int endFor = beginFor + nbSkillTree*3;
		
		// On démarre à 50 pixels du bord supérieur
		// et on se décale du si d'autre arbre sont avant
		int heightFromTop = -50 + nbTreeAlreadyDrawn*marginBetweenSkillTree;
		
		// Parcours de la liste des skills pour le nombre de sous arbre demandés
		// Un tour de boucle affiche 1 arbre
		for (int i = beginFor ; i < endFor ; i+=3)
		{
			heightFromTop += marginBetweenSkillTree;
			
			float firstSkillButtonWidth = listSkills[i].getName().Length * coefSize;
			// Met à jour la marge gauche
			float marginLeft = 0.05f*Screen.width;
			// Si le premier skill est déjà acheté
			if (listSkills[i].getIsBought())
			{
				PassiveSkills passiveSkillRank1 = listSkills[i] as PassiveSkills;
				
				// Le skill étant acheté on l'affiche avec un bouton grisé
				GUI.enabled = false;
				GUI.Button(new Rect(marginLeft,
				                    heightFromTop,
				                    firstSkillButtonWidth,
				                    30),
				           new GUIContent(	listSkills[i].getName(),
				               				generateToolTip(listSkills[i].getDescription(),
				                							listSkills[i].getPrice())));
				
				// Met à jour la marge gauche
				marginLeft += (firstSkillButtonWidth + horizontalMarginBetweenButton*Screen.width);
				
				float firstLittleSkillButtonWidthUp = passiveSkillRank1.getNameFirstAd().Length * coefSize;
				float firstLittleSkillButtonWidthDown = passiveSkillRank1.getNameSecAd().Length * coefSize;
				// On affiche les boutons des 2 branches de skills qui suivent
				GUI.enabled = passiveSkillRank1.getCostIncFirstAd() <= player.getExperience();
				if (GUI.Button(new Rect(marginLeft,
				                        heightFromTop-verticalSpace,
				                        firstLittleSkillButtonWidthUp,
				                        30),
				               new GUIContent(	passiveSkillRank1.getNameFirstAd(), 
				               					generateToolTip(passiveSkillRank1.getDescriptionFirstAd(),
				                                				passiveSkillRank1.getCostIncFirstAd()))))
				{
					upgradePassiveLittleSkill(passiveSkillRank1, true);
				}
				
				GUI.enabled = passiveSkillRank1.getCostIncSecAd() <= player.getExperience();
				if (GUI.Button(new Rect(marginLeft,
				                        heightFromTop+verticalSpace,
				                        firstLittleSkillButtonWidthDown,
				                        30),
				               new GUIContent(	passiveSkillRank1.getNameSecAd(),
				               					generateToolTip(passiveSkillRank1.getDescriptionSecAd(),
				               		 							passiveSkillRank1.getCostIncSecAd()))))
				{
					upgradePassiveLittleSkill(passiveSkillRank1, false);
				}
				
				// Met à jour la marge gauche
				if (marginLeft + firstLittleSkillButtonWidthUp > marginLeft + firstLittleSkillButtonWidthDown)
					marginLeft += (firstLittleSkillButtonWidthUp + horizontalMarginBetweenButton*Screen.width);
				else
					marginLeft += (firstLittleSkillButtonWidthDown + horizontalMarginBetweenButton*Screen.width);
				
				// Vérifie si le second skill peut etre débloqué
				listSkills[i+1].unlockedSkill();
				
				float secondSkillButtonWidth = listSkills[i+1].getName().Length * coefSize;
				// Si le second skill est déjà acheté
				if (listSkills[i+1].getIsBought())
				{
					PassiveSkills passiveSkillRank2 = listSkills[i+1] as PassiveSkills;
					
					// Le skill étant acheté on l'affiche avec un bouton grisé
					GUI.enabled = false;
					GUI.Button(new Rect(marginLeft,
					                    heightFromTop,
					                    secondSkillButtonWidth,
					                    30),
					           new GUIContent(	listSkills[i+1].getName(),
					               				generateToolTip(listSkills[i+1].getDescription(),
					                							listSkills[i+1].getPrice())));
					
					// Met à jour la marge gauche
					marginLeft += (secondSkillButtonWidth + horizontalMarginBetweenButton*Screen.width);
					
					float secondLittleSkillButtonWidthUp = passiveSkillRank2.getNameFirstAd().Length * coefSize;
					float secondLittleSkillButtonWidthDown = passiveSkillRank2.getNameSecAd().Length * coefSize;
					// On affiche les boutons des 2 branches de skills qui suivent
					GUI.enabled = passiveSkillRank2.getCostIncFirstAd() <= player.getExperience();
					if (GUI.Button(new Rect(marginLeft,
					                        heightFromTop-verticalSpace,
					                        secondLittleSkillButtonWidthUp,
					                        30),
					               new GUIContent(	passiveSkillRank2.getNameFirstAd(),
					               					generateToolTip(passiveSkillRank2.getDescriptionFirstAd(),
					                								passiveSkillRank2.getCostIncFirstAd()))))
					{
						upgradePassiveLittleSkill(passiveSkillRank2, true);
					}
					
					GUI.enabled = passiveSkillRank2.getCostIncSecAd() <= player.getExperience();
					if (GUI.Button(new Rect(marginLeft,
					                        heightFromTop+verticalSpace,
					                        secondLittleSkillButtonWidthDown,
					                        30),
					               new GUIContent(	passiveSkillRank2.getNameSecAd(),
					               					generateToolTip(passiveSkillRank2.getDescriptionSecAd(),
					                								passiveSkillRank2.getCostIncSecAd()))))
					{
						upgradePassiveLittleSkill(passiveSkillRank2, false);
					}
					
					// Met à jour la marge gauche
					if (marginLeft + secondLittleSkillButtonWidthUp > marginLeft + secondLittleSkillButtonWidthDown)
						marginLeft += (secondLittleSkillButtonWidthUp + horizontalMarginBetweenButton*Screen.width);
					else
						marginLeft += (secondLittleSkillButtonWidthDown + horizontalMarginBetweenButton*Screen.width);
					
					// Vérifie si le dernier skill peut etre débloqué
					listSkills[i+2].unlockedSkill();
					
					float lastSkillButtonWidth = listSkills[i+2].getName().Length * coefSize;
					// Si le dernier skill est déjà acheté
					if (listSkills[i+2].getIsBought())
					{
						// Le skill étant acheté on l'affiche avec un bouton grisé
						GUI.enabled = false;
						GUI.Button(new Rect(marginLeft,
						                    heightFromTop,
						                    lastSkillButtonWidth,
						                    30),
						           new GUIContent(	listSkills[i+2].getName(),
						               				generateToolTip(listSkills[i+2].getDescription(),
						                							listSkills[i+2].getPrice())));

						// Met à jour la marge gauche
						marginLeft += (lastSkillButtonWidth + 20);
					}
					// Si le dernier skill est débloqué
					else if (listSkills[i+2].getIsUnlock())
					{
						// On active ou non le bouton si on a suffisamment d'expérience
						GUI.enabled = listSkills[i+2].getPrice() <= player.getExperience();
						
						// Si on achète le skill on met à jour l'expérience du joueur
						if (GUI.Button(new Rect(marginLeft,
						                        heightFromTop,
						                        lastSkillButtonWidth,
						                        30),
						               new GUIContent(	listSkills[i+2].getName(),
						               					generateToolTip(listSkills[i+2].getDescription(),
						                								listSkills[i+2].getPrice()))))
						{
							unlockSkill(listSkills[i+2]);
						}

						// Met à jour la marge gauche
						marginLeft += (lastSkillButtonWidth + 20);
					}
					// Met à jour la taille de l'arbre le plus long a affich
					updateMaxTreeWidth((int)marginLeft);
					
				}
				// Si le deuxième skill est débloqué
				else if (listSkills[i+1].getIsUnlock())
				{
					// On active ou non le bouton si on a suffisamment d'expérience
					GUI.enabled = listSkills[i+1].getPrice() <= player.getExperience();
					
					// Si on achète le skill on met à jour l'expérience du joueur
					if (GUI.Button(new Rect(marginLeft,
					                        heightFromTop,
					                        secondSkillButtonWidth,
					                        30),
					               new GUIContent(	listSkills[i+1].getName(),
							               			generateToolTip(listSkills[i+1].getDescription(),
							                						listSkills[i+1].getPrice()))))
					{
						unlockSkill(listSkills[i+1]);
					}
				}
				
			}
			// Si le premier skill est débloqué
			else if (listSkills[i].getIsUnlock())
			{
				// On active ou non le bouton si on a suffisamment d'expérience
				GUI.enabled = listSkills[i].getPrice() <= player.getExperience();
				
				// Si on achète le skill on met à jour l'expérience du joueur
				if (GUI.Button(new Rect(marginLeft,
				                        heightFromTop,
				                        firstSkillButtonWidth,
				                        30),
				               new GUIContent(	listSkills[i].getName(),
								               	generateToolTip(listSkills[i].getDescription(),
								                				listSkills[i].getPrice()))))
				{
					unlockSkill(listSkills[i]);
				}
			}
			
			nbTreeAlreadyDrawn++;
		}
	}
	
	/*
	*	Augmente le niveau d'un sous skill passif de 1
	*	params 	=> skillRank : Skill qui dont le niveau doit etre augmenté
	*		   	=> first : 	true si c'est le premier skill qui doit etre augmenté
	*						false si c'est le second
	*/
	void upgradePassiveLittleSkill(PassiveSkills skillRank, bool first)
	{
		if (first)
		{
			player.experienceUpdate(-skillRank.getCostIncFirstAd());
			int newLevel = skillRank.getLvlFirstAd() + 1;
			skillRank.setLvlFirstAd(newLevel);
		}
		else
		{
			player.experienceUpdate(-skillRank.getCostIncSecAd());
			int newLevel = skillRank.getLvlSecAd() + 1;
			skillRank.setLvlSecAd(newLevel);
		}
	}

	/*
	*	Augmente le niveau d'un sous skill magique de 1
	*	params 	=> skillRank : Skill qui dont le niveau doit etre augmenté
	*		   	=> first : 	true si c'est le premier skill qui doit etre augmenté
	*						false si c'est le second
	*/
	void upgradeMagicLittleSkill(BaseSkills skillRank, bool first)
	{
		if (first)
		{
			player.experienceUpdate(-skillRank.getCostIncDamage());
			int newLevel = skillRank.getLvlDamage() + 1;
			skillRank.setLvlDamage(newLevel);
		}
		else
		{
			player.experienceUpdate(-skillRank.getCostIncAd());
			int newLevel = skillRank.getLvlAd() + 1;
			skillRank.setLvlAd(newLevel);
		}
	}

	/*
	*	Débloque un skill est met à jour l'expérience du joueur
	*	params 	=> skill : débloque le skill skill
	*/
	void unlockSkill(Skills skill)
	{
		skill.setIsBought(true);
		player.experienceUpdate(-skill.getPrice());
	}

	/*
	*	Met à jour la taille en pixel de l'arbre le plus long
	*	params 	=> witdh : taille en pixel d'un arbre
	*/
	void updateMaxTreeWidth(int width)
	{
		if (maxTreeWidth < width)
			maxTreeWidth = width;
	}

	/*
	*	Génère le texte du tooltip
	*	params 	=> description : description du skill
	*			=> price : prix du skill
	*/
	string generateToolTip(string description, int price)
	{
	
		return 	LanguageManager.Instance.GetTextValue("GUISkills.description")
			    + System.Environment.NewLine
				+ description
				+ System.Environment.NewLine
				+ LanguageManager.Instance.GetTextValue("GUISkills.price")
				+ price
				+ "Xp";
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			isActive = true;
			return;
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			isActive = false;
			openSkills.enabled = false;
			return;
		}
	}
}