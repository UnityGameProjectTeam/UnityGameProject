﻿using UnityEngine;
using System.Collections;

public class MeleeController : EnemyController 
{

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		
		pvMax = 6;
		pv = pvMax;
		
		damageAttack = -3;
		damageMagic = -2;
		manaCost = -50;
		timeAttack = 1F;
		probabilityAttack = 0.1F;
		
		xp = 15;
		
		manaMax = 50;
		mana = manaMax;
		
		timeRegen = 6;
		
		//gameObject.renderer.material.color = new Color(0.725F, 0.478F, 0.341F);
	}
	
	protected override void Update ()
	{
		base.Update();
	}
}