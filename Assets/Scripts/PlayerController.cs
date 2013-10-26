﻿using UnityEngine;
using System.Collections;

public class PlayerController : HumanoidController {
	
	public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
	public float rotationFactor = 5.0F;
    public float gravity = 20.0F;
	
    private Vector3 moveDirection = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	
	private CharacterController controller;
	
	private int xp=0;
	public GUIText healthText;
	public GUIText experienceText;
	public GUIText gameOverText;
	private bool gameOver = false;
	
	// Use this for initialization
	void Start () {
		gameObject.renderer.material.color = new Color(255, 0, 0);
		controller = GetComponent<CharacterController>();
		experienceText.color = new Color(0,0,255);
		pvMax = 200;
		pv = pvMax;
		statuUpdate();
		gameOverText.enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver) {
			gameOverText.enabled=true;
			return;
		}
		
        if (controller.isGrounded) {
			// Moves forward, left, right, backward
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
			// Handle jumps
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
		// Applies move
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
		
		// Rotation
		rotation = new Vector3(0, Input.GetAxis("Mouse X"), 0);
		rotation *= rotationFactor;
		transform.Rotate(rotation);
		
		if (Input.GetButton("Fire1"))
		{
			EnemyController target = (EnemyController)FindObjectOfType(System.Type.GetType("EnemyController"));
			Vector3 distance = transform.position-target.transform.position;
			if(distance.magnitude <= 4f)
			{
				target.healthUpdate(-1);
			}
		}
	}
	
	
	
	void OnTriggerEnter (Collider other)
	{
		// Collects items
		if (other.gameObject.tag == "Item")
		{
			other.gameObject.SetActive(false);
			healthUpdate(50);
		}
		if (other.gameObject.tag == "Weapon")
		{
			other.gameObject.SetActive(false);
			healthUpdate(-50);
			experienceUpdate(40);
		}
	}
	
	public override void healthUpdate(int change)
	{
		base.healthUpdate(change);
		statuUpdate();
	}
	
	void experienceUpdate(int change)
	{
		xp += change;
		statuUpdate();
	}
	
	void statuUpdate()
	{
		if (pv<=0)
		{
			healthText.color = new Color(255,0,0);
        	healthText.text = "0";
			gameOver = true;
		}
        else if (pv<= pvMax * 1/4)
		{
			healthText.color = new Color(255,0,0);
			healthText.text = pv.ToString();
		}
		else if (pv<= pvMax * 3/4)
		{
			healthText.color = new Color(125,125,0);
			healthText.text = pv.ToString();
		}
		else
		{
			healthText.color = new Color(0,255,0);
			healthText.text = pv.ToString();
		}
	
		experienceText.text = xp.ToString();
	}
}
