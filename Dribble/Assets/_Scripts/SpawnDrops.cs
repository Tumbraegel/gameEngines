﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnDrops : MonoBehaviour {
	public Camera cam; 
	//drop is prefab, template for a game object
	public GameObject drop;
	public float timeRemaining;
	public Text timerText;
	//width of game area
	private float maxWidth;

	void Start () {
		if(cam == null){
			cam = Camera.main;
		}
		//use Screen class to find corners of screen
		//translate screen space > ScreenToWorldPoint: transforms pos from screen space to world space
		Vector3 upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
		Vector3 targetWidth = cam.ScreenToWorldPoint(upperCorner);
		//find drop by using renderer to get the bounds/size of it
		float dropWidth = drop.GetComponent<Renderer>().bounds.extents.x;
		maxWidth = targetWidth.x - dropWidth;
		StartCoroutine(Spawn());
		UpdateTimerText();
	}

	//fixedUpdate comes at a regular period of time, not every frame
	//every time we hit this update, remove from the timeRemaining amount of time we spent since last time in this update
	void FixedUpdate(){
		timeRemaining -= Time.deltaTime;
		if(timeRemaining < 0){
			timeRemaining = 0;
		}
		UpdateTimerText();
	}

	//instatiate drops at random spawn position
	//this is a coroutine (pausable loop) that has to be called manually
	//IEnumerator = allows to stop process at a specific moment
	IEnumerator Spawn(){
		yield return new WaitForSeconds (2.0f);
		while(timeRemaining > 0){
			Vector3 spawnPos = new Vector3(Random.Range(-maxWidth,maxWidth), transform.position.y,0.0f);

			//used to represent rotation, "identitiy" essentially means no rotation
			Quaternion spawnRot = Quaternion.identity;
			Instantiate(drop, spawnPos, spawnRot);
			//so we don't freeze programme with infinite loop, wait between 1 and 2 seconds before starting loop again
			yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
		}
	}

	void UpdateTimerText(){
		//round float to an int
		timerText.text = "Time Remaining:\n" + Mathf.RoundToInt(timeRemaining);
	}
}
