using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager{
	
	private AchievementManager achievementManager;
	private DayNightCycleManager timeManager;
	private SkillManager skillManager;
	private PlayerController player;

	// Chemin vers les fichiers de sauvegarde
	private string achievementPath = "./save/achievement.dat";
	private string externalPath = "./save/external.dat";
	private string skillsPath = "./save/skills.dat";
	private string statsPath = "./save/stats.dat";

	// Lieu de spawn pour le joueur
	Vector3 spawn1 = new Vector3 (-14, 5.3f, -20); 	// Crypte du cimetière
	Vector3 spawn2 = new Vector3 (120, -4.8f, 479); 	// Crypte du village
	Vector3 spawn3 = new Vector3 (697, -4, 17);		// Crypte de la ferme

	public SaveManager(AchievementManager otherA, SkillManager otherS, PlayerController p, DayNightCycleManager otherD)
	{
		achievementManager = otherA;
		skillManager = otherS;
		if(!Directory.Exists("./save/"))
			Directory.CreateDirectory("./save/");
		player = p;
		timeManager = otherD;
	}

	// Fonction de sauvegarde
	public void save()
	{
		saveExternal();
		saveStats();
		saveSkills();
		saveAchievements();
	}

	// Fonction de chargement
	public void load()
	{
		loadExternal();
		loadStats();
		loadSkills();
		loadAchievements();
	}

	/****************************/
	/* Gestion des achievements */
	/****************************/
	private void saveAchievements()
	{
		/****************************/
		/* Sauvegarde des débloqués */
		/****************************/
		List<Achievement> achievements = achievementManager.getAchievementsUnlocked();
		
		List<string> achievementList = new List<string>();
		achievementList.Capacity = achievements.Capacity;
		for (int i=0; i<achievements.Count; i++)
			achievementList.Add(achievements[i].getName());
		
		// Créé le formater
		BinaryFormatter formater = new BinaryFormatter();
		// Crée le fichier
		Stream saveFile = File.Create(achievementPath);
		// Sauvegarde les achivements
		formater.Serialize(saveFile, achievementList);
		// Libère la mémoire
		saveFile.Close();
	}

	private void loadAchievements()
	{
		// Si le fichier existe
		if(File.Exists(achievementPath))
		{
			// Créé le formateur
			BinaryFormatter formater = new BinaryFormatter();
			
			// Créé le fichier
			Stream file = File.Open (achievementPath, FileMode.Open);
			
			// Charge la liste des achievements
			List<string> achievementsLoaded = formater.Deserialize(file) as List<string>;
			
			achievementManager.loadAchievements(achievementsLoaded);
			
			file.Close();
		}
	}

	/***************************/
	/* Gestion des compétences */
	/***************************/
	private void saveSkills()
	{
		List<string> skills = new List<string>();
		List<Skills> skillList = skillManager.getListOfSkills();

		// Récupération de l'experience du joueur
		skills.Add(player.getExperience().ToString());

		Skills skill;
		for (int i=0; i<skillList.Count; i++)
		{
			skill = skillList[i];
			if (skill.getIsBought())
			{
				if (skill.GetType().ToString() == "PassiveSkills")
				{
					// Récupération du nom et des niveaux
					PassiveSkills tmp = skill as PassiveSkills;
					skills.Add (tmp.getName());
					skills.Add (tmp.getLvlFirstAd().ToString());
					skills.Add (tmp.getLvlSecAd().ToString());
				}
				else if (skill.GetType().ToString() == "PorteeSkills" ||
				         skill.GetType().ToString() == "ZoneSkills")
				{
					// Récupération du non et des niveaux
					BaseSkills tmp = skill as BaseSkills;
					skills.Add (tmp.getName());
					skills.Add (tmp.getLvlAd().ToString());
					skills.Add (tmp.getLvlDamage().ToString());
				}
				else
				{
					// Récupération du nom
					skills.Add (skill.getName());
				}
			}
		}

		// Créé le formater
		BinaryFormatter formater = new BinaryFormatter();
		// Crée le fichier
		Stream saveFile = File.Create(skillsPath);
		// Sauvegarde les achivements
		formater.Serialize(saveFile, skills);
		// Libère la mémoire
		saveFile.Close();
	}

	private void loadSkills()
	{
		// Si le fichier existe
		if(File.Exists(skillsPath))
		{
			// Créé le formateur
			BinaryFormatter formater = new BinaryFormatter();
			// Créé le fichier
			Stream file = File.Open (skillsPath, FileMode.Open);
			// Charge la liste des compétences
			List<string> skillsLoaded = formater.Deserialize(file) as List<string>;

			// Récupère la liste des compétences du manager
			List<Skills> skillsList = skillManager.getListOfSkills();
			// Réinitiale la liste
			for (int i=0; i<skillsList.Count; i++)
				skillsList[i].setIsBought(false);

			// Réinitialise l'expérience du joueur
			player.experienceUpdateOnLoad(-player.getExperience());
			// Récupère l'expérience sauvegardée
			player.experienceUpdateOnLoad(int.Parse (skillsLoaded[0]));

			// Récupère les compétences déjà achetées
			int index;
			for (int i=1; i<skillsLoaded.Count; i++)
			{
				index = skillsList.FindIndex(
					delegate(Skills obj) {
					return obj.getName() == skillsLoaded[i];
				});
				// Si la compétence a été trouvée
				if(index != -1)
				{
					// Si c'est une compétence passive
					PassiveSkills passive = skillsList[index] as PassiveSkills;
					if (passive != null)
					{
						passive.setIsBought(true);
						passive.setLvlFirstAd(int.Parse (skillsLoaded[i+1]));
						passive.setLvlSecAd(int.Parse (skillsLoaded[i+2]));
						skillsList[index] = passive;
						i+=2;
					}
					else
					{
						// Si c'est une compétence héritant de BaseSkills
						PorteeSkills portee = skillsList[index] as PorteeSkills;
						if (portee != null)
						{
							portee.setIsBought(true);
							portee.setLvlAd(int.Parse (skillsLoaded[i+1]));
							portee.setLvlDamage(int.Parse (skillsLoaded[i+2]));
							skillsList[index] = portee;
							i+=2;
						}
						else
						{
							// Si c'est une compétence héritant de BaseSkills
							ZoneSkills zone = skillsList[index] as ZoneSkills;
							if (zone != null)
							{
								zone.setIsBought(true);
								zone.setLvlAd(int.Parse (skillsLoaded[i+1]));
								zone.setLvlDamage(int.Parse (skillsLoaded[i+2]));
								skillsList[index] = zone;
								i+=2;
							}
							else
							{
								// Sinon
								skillsList[index].setIsBought(true);
							}
						}
					}
				}
			}
			
			// Retrouve les compétences débloquées
			for (int i=0; i<skillsList.Count; i++)
				skillsList[i].unlockedSkill();

			skillManager.setListOfSkills(skillsList);
			// Met à jour les compétences du player
			player.getSkillManager().updateSkill();
			player.getSkillManager().updateSpetialSkill();

			file.Close();
		}
	}

	/*********************/
	/* Gestion des stats */
	/*********************/
	private void saveStats()
	{
		List<string> stats = new List<string>();
		
		stats.Add (achievementManager.getTravelledDistance().ToString());
		stats.Add (achievementManager.getNbKilledEnemy().ToString());
		stats.Add (achievementManager.getLastNbEnnemyKilled().ToString());
		stats.Add (achievementManager.getNbKilledBerseker().ToString());
		stats.Add (achievementManager.getNbAssassinKills().ToString());
		
		// Créé le formater
		BinaryFormatter formater = new BinaryFormatter();
		// Crée le fichier
		Stream saveFile = File.Create(statsPath);
		// Sauvegarde les achivements
		formater.Serialize(saveFile, stats);
		// Libère la mémoire
		saveFile.Close();
	}

	private void loadStats()
	{
		// Si le fichier existe
		if(File.Exists(statsPath))
		{
			// Créé le formateur
			BinaryFormatter formater = new BinaryFormatter();
			// Créé le fichier
			Stream file = File.Open (statsPath, FileMode.Open);
			// Récupère les informations
			List<string> stats = formater.Deserialize(file) as List<string>;
			
			achievementManager.setTravelledDistance(float.Parse (stats[0]));
			achievementManager.setNbKilledEnemy(int.Parse (stats[1]));
			achievementManager.setLastNbEnnemyKilled(int.Parse(stats[2]));
			achievementManager.setNbKilledBerseker(int.Parse (stats[3]));
			achievementManager.setNbAssassinKills(int.Parse (stats[4]));
			
			file.Close();
		}
	}

	/********************/
	/* Gestion du reste */
	/********************/
	private void saveExternal()
	{
		List<string> external = new List<string>();

		Vector3 position = player.transform.position;

		if (Vector3.Distance(position, spawn1) < Vector3.Distance(position, spawn2))
		{
			if (Vector3.Distance(position, spawn1) < Vector3.Distance(position, spawn3))
				position = spawn1;
			else
				position = spawn3;
		}
		else
		{
			if (Vector3.Distance(position, spawn2) < Vector3.Distance(position, spawn3))
				position = spawn2;
			else
				position = spawn3;
		}

		external.Add (position.x.ToString());
		external.Add (position.y.ToString());
		external.Add (position.z.ToString());
		
		external.Add (timeManager.dayTime.ToString());

		external.Add (player.getSkillManager().getPv().ToString());
		external.Add (player.getSkillManager().getMana().ToString());
		external.Add (player.getMobsController().getMaxPlayerXpWin().ToString());

		// Créé le formater
		BinaryFormatter formater = new BinaryFormatter();
		// Crée le fichier
		Stream saveFile = File.Create(externalPath);
		// Sauvegarde les achivements
		formater.Serialize(saveFile, external);
		// Libère la mémoire
		saveFile.Close();
	}

	private void loadExternal()
	{
		// Si le fichier existe
		if(File.Exists(externalPath))
		{
			// Créé le formateur
			BinaryFormatter formater = new BinaryFormatter();
			// Créé le fichier
			Stream file = File.Open (externalPath, FileMode.Open);
			// Récupère les informations
			List<string> external = formater.Deserialize(file) as List<string>;

			player.transform.position = new Vector3(float.Parse(external[0]),
			                                        float.Parse(external[1]),
			            							float.Parse(external[2]));
			timeManager.dayTime = float.Parse (external[3]);
			player.getSkillManager().setPv(float.Parse (external[4]));
			player.getSkillManager().setMana(float.Parse (external[5]));
			player.getMobsController().setMaxPlayerXpWin(int.Parse(external[6]));
			player.getMobsController().upMob();
			
			file.Close();
		}
	}
}
