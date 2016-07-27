using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PositionLabelScript : MonoBehaviour {

	// Use this for initialization
	private GameObject holder;
	private ManagementPanel managementScript;

	Text Position;

	void Start () {
		holder = GameObject.Find("MainController");
		managementScript = holder.GetComponent<ManagementPanel> ();
		Position = gameObject.GetComponent<Text> ();
		Position.text = managementScript.getObjectLocation ().ToString();
	}
	
	// Update is called once per frame
	void Update () {
		Position.text = managementScript.getObjectLocation ().ToString();
	}
}
