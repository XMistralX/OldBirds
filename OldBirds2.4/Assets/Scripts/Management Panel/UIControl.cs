using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControl : MonoBehaviour {

	private Text text;
	private GameObject holder;
	private ManagementPanel managementScript;
	private Vector3 birdLocation;
	public GameObject UIElement;
	private GameObject currentUIelement;

	// Use this for initialization
	void Start () {
		holder = GameObject.Find("MainController");
		managementScript = holder.GetComponent<ManagementPanel> ();
	}
		
	void Update () {
		checkBird();

	}
	void checkBird(){
		if (managementScript.getPointerObject () != null && managementScript.getPointerObject ().tag == "Bird" && currentUIelement == null ) {
			Vector3 birdLoc = Camera.main.WorldToScreenPoint (birdLocation);
			Vector2 viewportPoint = Camera.main.WorldToViewportPoint (birdLocation);
			if (viewportPoint.x < 0.6) {
				viewportPoint = new Vector2 (
					viewportPoint.x + 0.275f,
					viewportPoint.y - 0.2f
				);
			}
			else if(viewportPoint.x >= 0.6){
				viewportPoint = new Vector2 (
					viewportPoint.x + 0.05f,
					viewportPoint.y - 0.2f
				);
			}
			currentUIelement = Instantiate (UIElement, birdLoc, Quaternion.identity) as GameObject;
			//currentUIelement.AddComponent<RectTransform> ();
			currentUIelement.transform.SetParent (GameObject.Find("Canvas").transform);
			currentUIelement.GetComponent<RectTransform>().anchorMin = viewportPoint;
			currentUIelement.GetComponent<RectTransform>().anchorMax = viewportPoint;
		} else {}
	}
}
