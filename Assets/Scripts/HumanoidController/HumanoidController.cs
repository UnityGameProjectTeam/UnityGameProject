﻿using UnityEngine;
using System.Collections;

// Permet de gérer la vie
public class HumanoidController : MonoBehaviour 
{	
	//regen de mana
	private float regen = 0;
	protected float timeRegen;
	
	//manager des skills
	protected SkillManager skillManager;

	//SoundHitManager
	private SoundHitManager soundHit;

	public SkillManager getSkillManager()
	{
		return skillManager;
	}
	
	protected virtual void Awake()
	{
		skillManager = GetComponent<SkillManager>();
		soundHit = GetComponent<SoundHitManager> ();
	}

	// Use this for initialization
	protected virtual void Start () 
	{
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		if (regen >= timeRegen)
		{
			manaUpdate(1);	
			regen = 0;
		}
		regen += Time.deltaTime;
	}
	
	public virtual void healthUpdate(float change)
	{
		skillManager.setPv(skillManager.getPv() + change);
		if (skillManager.getPv() > skillManager.getPvMax())
			skillManager.setPv(skillManager.getPvMax());
		if (change < 0)
			soundHit.playHitSound ();
	}
	
	public virtual void manaUpdate(int change)
	{
		skillManager.setMana(skillManager.getMana() + change);
		if (skillManager.getMana()>skillManager.getManaMax())
			skillManager.setMana(skillManager.getManaMax());
	}
}
