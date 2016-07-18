using UnityEngine;
using System.Collections;

/// <summary>
/// Navigation GUI script for user to navigate through scenes manually.
/// </summary>
public class NavigationGUIScript : MonoBehaviour {
	private bool hide = false;
	private GameObject holder;
	private ManagementPanel managementScript;
	private string currentObjectRotation;
	private string xRotation = "";
	private string yRotation = "0";
	private string zRotation = "0";
	void Start() {
		holder = GameObject.Find("Eye");
		managementScript = holder.GetComponent<ManagementPanel> ();
	}
	void Update() {
		if(Input.GetKeyDown(KeyCode.H)) {
			if(!hide)
				hide = true;
			else
				hide = false;
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
			xRotation = GUI.TextArea (new Rect(20,180,40 , 20),xRotation);
			if (GUI.Button (new Rect (60, 180, 120, 20), "ChangeXRotation")) {
				managementScript.setObjectRotation (0, int.Parse (xRotation));
			}
		}
	}

}
