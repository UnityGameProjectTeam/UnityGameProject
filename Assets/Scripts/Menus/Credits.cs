﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Credits : SubMenu 
{
	//Timer du credit pour afficher des credit a interval de temps régulié.
	float timeCredit;
	
	//Intervale de temps entre deux credits
	float intervalTime;
	
	//Numero du texte en cours d'affichage
	int currentText;
	
	//Liste des Texte 3D des credits
	public GameObject[] listCredit;
	
	public bool isReturn = false;
	
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		timeCredit = 0;
		currentText = 0;
		intervalTime = 15f;

		foreach ( GameObject item in listCredit ) {
			item.GetComponent<TextMesh>().text = item.GetComponent<TextMesh>().text.Replace("\\n", "\n");
		}

		listCredit [currentText].renderer.enabled = true;
		Color couleur = new Color(1f, 1f, 1f, 0f);
		listCredit [currentText].renderer.material.color = couleur;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space)) {
			timeCredit = 0;
			changeText();
		}
	}

	public override void setInfFrontOf(bool state)
	{
		base.setInfFrontOf(state);
		if (!state) {
			restartCred();
		}
	}
	
	void OnGUI()
	{
		if (!inFrontOf)
		{
			return;
		}
		manageText ();
		
		Debug.Log("dessine credits");
	}
	
	//Gere s'il faut changer de texte ou non
	void manageText()
	{
		timeCredit += Time.deltaTime;
		if(timeCredit > intervalTime)
		{
			timeCredit = 0;
			changeText();
		}

		Color couleur = new Color(1f, 1f, 1f, (Mathf.Sin((timeCredit/intervalTime)*Mathf.PI)));
		listCredit[currentText].renderer.material.color = couleur;
		Debug.Log (listCredit [currentText].name.ToString ());
	}
	
	//Change le texte en cours d'affichage
	void changeText()
	{
		listCredit [currentText].renderer.enabled = false;
		if(currentText < listCredit.Length-1)
		{
			currentText += 1;
			listCredit [currentText].renderer.enabled = true;
		}
		
	}
	
	//Redemarre les credits a zero
	protected void restartCred()
	{
		currentText = 0;
		timeCredit = 0;
	}
}
