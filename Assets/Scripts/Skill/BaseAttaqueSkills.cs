﻿using UnityEngine;
using System.Collections;

public class BaseAttaqueSkills : PassiveSkills 
{
	private int m_damageCac;
	private int m_damageDist;
	
	//acsessor
	public int getDamageCac()
	{
		return m_damageCac;	
	}
	
	public int getDamageDist()
	{
		return m_damageDist;	
	}
	
	// Use this for initialization
	public BaseAttaqueSkills (string name, int price, Skills father, int costIncFirstAd, int costIncSecAd, int damageCac, int damageDist) 
		:base (name, price, father, costIncFirstAd, costIncSecAd)
	{
		m_damageCac = damageCac;
		m_damageDist = damageDist;
	}
}
