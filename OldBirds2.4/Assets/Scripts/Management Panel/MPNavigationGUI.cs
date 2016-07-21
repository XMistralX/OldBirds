﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Navigation GUI script for user to navigate through scenes manually.
/// </summary>
public class MPNavigationGUI : MonoBehaviour {
	private bool hide = false;
	private GameObject holder;
	private ManagementPanel managementScript;
	private string currentObjectRotation;
	private string currentObjectLocation;
	private string Rotation = "";
	private bool birdInfo = false;
	private Vector3 birdLocation;

	void Start() {
		holder = GameObject.Find("Eye");
		managementScript = holder.GetComponent<ManagementPanel> ();
	}
	void Update() {
		if(managementScript.selectedObject != null)
			currentObjectLocation = managementScript.getObjectLocation ().ToString();
		if (managementScript.selectedObject != null) 
			currentObjectRotation = managementScript.getObjectRotation ().ToString ();
		if(Input.GetKeyDown(KeyCode.H)) {
			if(!hide)
				hide = true;
			else
				hide = false;
		}
		if (managementScript.isOverBird () != Vector3.zero) {
			birdInfo = true;
			birdLocation = managementScript.isOverBird ();
		} else {

			birdInfo = false;
		}


	}

	void OnGUI () {
		if(!hide) {
			// Background
			GUI.Box(new Rect(10,10,140,120), "Simulations");

			// Create buttons
			if(GUI.Button(new Rect(20,40,120,20), "Caregiver UI")) {
				Application.LoadLevel("Marjattas_house");
			}

			if(GUI.Button(new Rect(20,70,120,20), "Smart glasses")) {
				Application.LoadLevel("Marjatta_goes_Pirkko");
			}
			if(Application.loadedLevelName == "Marjattas_house" || Application.loadedLevelName == "Oulu_city") {
				// Skip scene button
				if(GUI.Button(new Rect(20,100,120,20), "Skip scene")) {
					Application.LoadLevel(Application.loadedLevel + 1);
				}
			}
			if (GUI.RepeatButton (new Rect (20, 130, 120, 20), "Scale Up")) {
				managementScript.changeObjectScale (0);
			}
			if (GUI.RepeatButton (new Rect (140, 130, 120, 20), "Scale Down")) {
				managementScript.changeObjectScale (1);
			}
			if (GUI.RepeatButton (new Rect (20, 150, 120, 20), "Rotate Left")) {
				managementScript.incrementObjectRotation (1, 1);
			}
			if (GUI.RepeatButton(new Rect (140, 150, 120, 20), "Rotate Right")) {
				managementScript.incrementObjectRotation (1, -1);
			}
			GUI.Label (new Rect(260 , 150 , 120 ,20), currentObjectRotation);
			Rotation = GUI.TextArea (new Rect(20,180,40 , 20),Rotation);
			if (GUI.Button (new Rect (60, 180, 120, 20), "ChangeYRotation")) {
				managementScript.setObjectRotation (1, int.Parse (Rotation));
			}
			GUI.Label (new Rect(60 , 200 , 120 ,20), currentObjectLocation);
			if (GUI.Button (new Rect (60, 220, 120, 20), "Destroy Object")) {
				managementScript.deleteObject();
			}
			if (birdInfo) {
				GUI.TextField(new Rect (birdLocation.x + 50, birdLocation.y,100,50),"Hello World!");
			}
		}
	}

}