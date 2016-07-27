using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RotationLabelScript : MonoBehaviour {

	// Use this for initialization
	private GameObject holder;
	private ManagementPanel managementScript;

	Text Rotation;

	void Start () {
		holder = GameObject.Find("MainController");
		managementScript = holder.GetComponent<ManagementPanel> ();
		Rotation = gameObject.GetComponent<Text> ();
		Rotation.text = managementScript.getObjectRotation ().ToString();
	}

	// Update is called once per frame
	void Update () {
		Rotation.text = managementScript.getObjectRotation ().ToString();
	}
}