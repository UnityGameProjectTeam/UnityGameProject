﻿using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour
{
	public float maximumTimeDelay = 1.5f;		        // The maximum time between shots.
	public float minimumTimeDelay = 0.8f;		        // The minimum time between shots.
	
	private EnemyController enemyController;            // Reference to the EnemyController script.
	private PlayerController player;                    // Reference to the player.
	//private bool shooting;							// A bool to say whether or not the enemy is currently shooting.
	private float scaledDamage;							// Amount of damage that is scaled by the distance from the player.
	private float scaledTimeDelay;						// maximumTimeDelay - minimumTimeDelay
	private float timeDelay;
	private float timeCount;

	void Awake ()
	{
		scaledTimeDelay = maximumTimeDelay - minimumTimeDelay;
		timeCount = 0;
		timeDelay = 0;
	}

	// Use this for initialization
	void Start () {
		enemyController = GetComponent<EnemyController>();
		player = (PlayerController)FindObjectOfType(System.Type.GetType("PlayerController"));
	}
	
	// Update is called once per frame
	void Update () {
		timeCount += Time.deltaTime;

		if (enemyController.isShooting() && !player.getSkillManager().getInvincible())
			Shoot();
	}

	void Shoot ()
	{
		float d = Vector3.Distance(transform.position, player.transform.position);

		if (timeCount > timeDelay)
		{
			float manaMax = enemyController.getSkillManager().getManaMax();
			float mana = enemyController.getSkillManager().getMana();
			float manaAttackProbability = Mathf.Sqrt(mana/manaMax)*0.8f;

			PorteeSkills skill = enemyController.getSkillManager().getSkill(0) as PorteeSkills;

			if (Random.value < manaAttackProbability && enemyController.getSkillManager().getMana() >= skill.getManaCost()) // Mana attack
			{
				Vector3 position = new Vector3(transform.position.x+transform.forward.x*2, transform.position.y/4, transform.position.z+transform.forward.z*2);
			
				skill.launch(position, transform.forward, enemyController.getSkillManager().getMagicAttack(), 0.4f, Random.value * 0.4f + 0.8f);	//player.healthUpdate(damage);
				enemyController.manaUpdate(-skill.getManaCost());
			}
			else // Melee attack
			{
				// The fractional distance from the player, 1 is next to the player, 0.5 is at the max shooting distance.
				float fractionalDistance = (enemyController.shootDistance * 2 - d) / enemyController.shootDistance;
				// The damage depend of...
				float damage = -enemyController.getSkillManager().getPhysicAttack();
				// ...the skill
				damage += (-enemyController.getSkillManager().getPhysicAttack()/100 * player.getSkillManager().getPhysicalResistance());
				// ...a random factor (luck)
				damage *= (Random.value * 0.4f + 0.8f);
				// ...the distance with the target
				damage *= fractionalDistance;
				player.healthUpdate(damage);
			}

			player.achievementManager.updateTimeNotTouched(0);
			timeDelay = Random.value*scaledTimeDelay + minimumTimeDelay;
			timeCount = 0;
		}
	}
}
