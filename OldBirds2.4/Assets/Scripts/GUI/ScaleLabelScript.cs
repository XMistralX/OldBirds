using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScaleLabelScript : MonoBehaviour {

	// Use this for initialization
	private GameObject holder;
	private ManagementPanel managementScript;

	Text Scale;

	void Start () {
		holder = GameObject.Find("MainController");
		managementScript = holder.GetComponent<ManagementPanel> ();
		Scale = gameObject.GetComponent<Text> ();
		Scale.text = managementScript.getObjectScale ().ToString();
	}

	// Update is called once per frame
	void Update () {
		Scale.text = managementScript.getObjectScale ().ToString();
	}
}