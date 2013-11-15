﻿using UnityEngine;
using System.Collections;

public class ZoneSkills : BaseSkills
{
	private float m_zone;
	
	//acsessor
	public float getZone()
	{
		return m_zone;	
	}
	
	// Use this for initialization
	protected void Start (string name, int price, Skills father, float timeIncantation, int manaCost, int damage, int costIncDamage, int costIncZone, float zone) 
	{
		base.Start(name, price, father, timeIncantation, manaCost, damage, costIncDamage, costIncZone);
		m_zone = zone;
		
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();
	}
}