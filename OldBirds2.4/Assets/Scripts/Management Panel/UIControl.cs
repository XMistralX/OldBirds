using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControl : MonoBehaviour {

	private Text text;
	private GameObject holder;
	private ManagementPanel managementScript;
	private Vector3 birdLocation;
	private bool check = true;
	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text>();
		holder = GameObject.Find("MainController");
		managementScript = holder.GetComponent<ManagementPanel> ();
	}
		
	void Update () {
		checkBird();

	}
	void checkBird(){
		if (managementScript.getPointerObject () != null && managementScript.getPointerObject ().tag == "Bird" ) {
			Vector2 viewportPoint = Camera.main.WorldToViewportPoint (birdLocation);
			print(viewportPoint);

			viewportPoint = new Vector2 (
				viewportPoint.x + 0.275f,
				viewportPoint.y - 0.2f
			);
			text.rectTransform.anchorMin = viewportPoint;  
			text.rectTransform.anchorMax = viewportPoint; 
			print (text.rectTransform.anchoredPosition);
			check = false;

		} else {}
	}
}
