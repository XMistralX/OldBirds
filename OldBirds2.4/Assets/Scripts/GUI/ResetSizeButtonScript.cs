using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResetSizeButtonScript : MonoBehaviour ,IEventSystemHandler  {

	// Use this for initialization
	private bool pressed;
	private GameObject holder;
	private ManagementPanel managementScript;

	public void Pressed (BaseEventData eventData){
		pressed = true;
	}
	public void NotPressed(BaseEventData eventData){
		pressed = false;
	}
	void Start () {
		holder = GameObject.Find("MainController");
		managementScript = holder.GetComponent<ManagementPanel> ();
	}
	// Update is called once per frame
	void Update () {
		if(pressed){
			managementScript.changeObjectScale (0);
		}
	}
}
