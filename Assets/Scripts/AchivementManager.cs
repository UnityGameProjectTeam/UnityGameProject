﻿using UnityEngine;
using System.Collections;

public class AchivementManager : MonoBehaviour {
	
	// Texture achivement
	public Texture texture; 
	
	// Son achivement
	public AudioClip soundAchivement; 
	
	// Booléens des achivements
	private bool firstMove = false;
	
	private bool firstKill = false;
	private bool tenKills = false;
	private bool hundredKills = false;
	private bool thousandKills = false;
	
	private bool untouch30s = false;
	private bool untouch1min = false;
	
	private bool beginner = false;
	private bool amateur = false;
	private bool ghost = false;
	
	private bool assassin = false;
	private bool masterAssassin = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void FirstMoveAchievement()
	{
		if (!firstMove)
		{
			Debug.Log("Achivement First Move !");
			unlockAchivement(texture);
			firstMove = !firstMove;
		}
	}
	
	public void firstBloodAchievement()
	{
		if (!firstKill)
		{
			Debug.Log("Achivement First Blood !");
			unlockAchivement(texture);
			firstKill = !firstKill;
		}
	}
	
	public void littleKillerAchievement()
	{
		if (!tenKills)
		{
			Debug.Log("Achivement Ten kills !");
			unlockAchivement(texture);
			tenKills = !tenKills;
		}
	}
	
	public void killerAchievement()
	{
		if (!hundredKills)
		{
			Debug.Log("Achivement a hundred kills !");
			unlockAchivement(texture);
			hundredKills = !hundredKills;
		}
	}
	
	public void serialKillerAchievement()
	{
		if (!thousandKills)
		{
			Debug.Log("Achivement 1 thousand kills !");
			unlockAchivement(texture);
			thousandKills = !thousandKills;
		}
	}
	
	public void uncatchableAchievement()
	{
		if (!untouch30s)
		{
			Debug.Log("Achivement Not touch during 30s !");
			unlockAchivement(texture);
			untouch30s = !untouch30s;
		}
	}
	
	public void reallyUncatchableAchievement()
	{
		if (!untouch1min)
		{
			Debug.Log("Achivement Not touch during 1 min !");
			unlockAchivement(texture);
			untouch1min = !untouch1min;
		}
	}
	
	public void surviveOneMinuteAchievement()
	{
		if (!beginner)
		{
			Debug.Log("Achivement Survive during 1 min !");
			unlockAchivement(texture);
			beginner = !beginner;
		}
	}
	
	public void surviveTwentyMinutesAchievement()
	{
		if (!amateur)
		{
			Debug.Log("Achivement Survive during 20 mins !");
			unlockAchivement(texture);
			amateur = !amateur;
		}
	}
	
	public void surviveOneHourAchievement()
	{
		if (!ghost)
		{
			Debug.Log("Achivement Survive during 1 h !");
			unlockAchivement(texture);
			ghost = !ghost;
		}
	}
	
	public void assassinAchievement()
	{
		if (!assassin)
		{
			Debug.Log("Achivement Kill 10 enemy and not be touch !");
			unlockAchivement(texture);
			assassin = !assassin;
		}
	}
	
	public void masterAssassinAchievement()
	{
		if (!masterAssassin)
		{
			Debug.Log("Achivement Kill 100 enemy and not be touch !");
			unlockAchivement(texture);
			masterAssassin = !masterAssassin;
		}
	}
	
	void unlockAchivement(Texture textureAchivement)
	{
		// TODO
		audio.PlayOneShot(soundAchivement);
	}
}