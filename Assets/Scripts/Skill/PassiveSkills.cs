﻿using UnityEngine;
using System.Collections;

public class PassiveSkills : Skills 
{
	private float m_firstAd;
	private string m_nameFirstAd;
	private string m_descriptionFirstAd;
	private int m_lvlFirstAd = 0;
	private int m_costIncFirstAd;

	private float m_secAd;
	private string m_nameSecAd;
	private string m_descriptionSecAd;
	private int m_lvlSecAd = 0;
	private int m_costIncSecAd;
	
	//acsessor
	public void setLvlFirstAd(int lvl)
	{
		m_lvlFirstAd = lvl;	
	}

	public int getLvlFirstAd()
	{
		return m_lvlFirstAd;
	}
	
	public void setLvlSecAd(int lvl)
	{
		m_lvlSecAd = lvl;	
	}

	public int getLvlSecAd()
	{
		return m_lvlSecAd;
	}
	
	public void setCostIncFirstAd(int cost)
	{
		m_costIncFirstAd = cost;	
	}
	
	public int getCostIncFirstAd()
	{
		return (int)(m_costIncFirstAd + Mathf.Pow((m_lvlFirstAd/2f),3)*5f + (Mathf.Cos(m_lvlFirstAd)*1.5f*Mathf.Pow(m_lvlFirstAd,2)));	
	}
	
	public void setCostIncSecAd(int cost)
	{
		m_costIncSecAd = cost;	
	}
	
	public int getCostIncSecAd()
	{
		return (int)(m_costIncSecAd + Mathf.Pow((m_lvlSecAd/2f),3)*5f + (Mathf.Cos(m_lvlSecAd)*1.5f*Mathf.Pow(m_lvlSecAd,2)));	
	}

	public string getNameFirstAd()
	{
		return m_nameFirstAd + " (" + m_lvlFirstAd + ")";	
	}

	public string getDescriptionFirstAd()
	{
		return m_descriptionFirstAd;	
	}

	public string getNameSecAd()
	{
		return m_nameSecAd + " (" + m_lvlSecAd + ")";	
	}

	public string getDescriptionSecAd()
	{
		return m_descriptionSecAd;	
	}
	
	// Use this for initialization
	public PassiveSkills (string name, string description, int price, Skills father, int costIncFirstAd, int costIncSecAd, float firstAd, float secAd, string nameFirstAd, string nameSecAd, string descriptionFirstAd, string descriptionSecAd) 
		:base (name, description, price, father)
	{
		m_costIncFirstAd = costIncFirstAd;
		m_costIncSecAd = costIncSecAd;
		m_firstAd = firstAd;
		m_secAd = secAd;
		m_nameFirstAd = nameFirstAd;
		m_nameSecAd = nameSecAd;
		m_descriptionFirstAd = descriptionFirstAd;
		m_descriptionSecAd = descriptionSecAd;
	}

	public void update(ref float firstAd, float baseFirstAd, ref float secAd, float basesecAd)
	{
		firstAd = baseFirstAd + m_firstAd * m_lvlFirstAd;
		secAd = basesecAd + m_secAd * m_lvlSecAd;
	}
}
