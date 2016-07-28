using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RepeatButtonScript : MonoBehaviour ,IEventSystemHandler  {

	// Use this for initialization
	private bool pressed;

	public void Pressed (BaseEventData eventData){
		pressed = true;
	}
	public void notPressed(BaseEventData eventData){
		pressed = false;
	}
	void Start () {

	}
	// Update is called once per frame
	void Update () {
		if(pressed)
			this.gameObject.GetComponent<Button> ().onClick.Invoke ();
	}
}