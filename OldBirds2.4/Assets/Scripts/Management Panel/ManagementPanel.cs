using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ManagementPanel : MonoBehaviour {

	public Shader silhouette;
	private ArrayList selectedBirds;
	private GameObject[] selectedBirdObjects;

	private bool isCreating;
	private bool isSelecting;

	public GameObject creatingObject;
	public GameObject selectedObject;

	private const int x = 0;
	private const int y = 1;
	private const int z = 2;

	private const int scaleUp = 0;
	private const int scaleDown = 1;
	// Use this for initialization
	void Start () {
		selectedBirds = new ArrayList();
	}

	// Update is called once per frame
	void Update () {
		handleInput ();

	}

	void handleInput () {
		Debug.Log (this.selectedObject);
		if (isCreating) {
			setSelectedObjectPosition (getWorldPoint());
			if (Input.GetMouseButtonDown (0)) {
				// Check if mouse isn't over a UI Element
				if(!EventSystem.current.IsPointerOverGameObject()) {
					this.isCreating = false;
					selectedObject = null;
				}
			}
		} else {
			if (Input.GetMouseButtonDown (0)) {
				if (!isSelecting) {
					handleSelection ();
				}

			} else if ( Input.GetMouseButton(1)) {
				if (isSelecting) {
					isSelecting = false;
					selectedObject = null;
				}
			} else if ( Input.GetMouseButton(0)) {
				Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit hit;
				if (isSelecting && getPointerObject() == this.selectedObject) {
					setSelectedObjectPosition (getWorldPoint());
				}
			}
			else if (Input.GetMouseButtonUp(0)) {
			}
		}
	}

	private Vector3 getWorldPoint(){
		Ray ray = GetComponent<Camera>().ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			return hit.point;
		}
		return Vector3.zero;
	}

	public void changeCreatingObject(GameObject creatingGameObject) {
		//toggle on/off
		if (this.isCreating) {
			Destroy (this.selectedObject);

			if(this.creatingObject == creatingGameObject){
				this.isCreating = false;
				this.creatingObject = null;
				return;
			}
		}
		this.isCreating = true;
		this.creatingObject = creatingGameObject;
		createObject ();
	}

	private void createObject(){
		selectedObject = Instantiate (creatingObject, getWorldPoint(), Quaternion.identity) as GameObject;
	}
	private void setSelectedObjectPosition(Vector3 pos){
		selectedObject.transform.position = pos;
	}
	public void changeObjectRotation(int axis , int angle){
		switch(axis){
		case x:
			selectedObject.transform.eulerAngles = new Vector3 (
				selectedObject.transform.eulerAngles.x + angle,
				selectedObject.transform.eulerAngles.y,
				selectedObject.transform.eulerAngles.z
			);
			break;
		case y:
			selectedObject.transform.eulerAngles = new Vector3 (
				selectedObject.transform.eulerAngles.x,
				selectedObject.transform.eulerAngles.y  + angle,
				selectedObject.transform.eulerAngles.z
			);
			break;
		case z:
			selectedObject.transform.eulerAngles = new Vector3 (
				selectedObject.transform.eulerAngles.x,
				selectedObject.transform.eulerAngles.y,
				selectedObject.transform.eulerAngles.z   + angle
			);
			break;
		default:
			break;
		}
	}
	public void changeObjectScale(int choice){
		switch (choice) {
		case scaleUp:
			selectedObject.transform.localScale = new Vector3(
				selectedObject.transform.localScale.x + 1 ,
				selectedObject.transform.localScale.y + 1 ,
				selectedObject.transform.localScale.z + 1 
			);
			break;
		case scaleDown:
			selectedObject.transform.localScale = new Vector3(
				selectedObject.transform.localScale.x - 1 ,
				selectedObject.transform.localScale.y - 1 ,
				selectedObject.transform.localScale.z - 1 
			);
			break;
		}

	}

	private GameObject getPointerObject() {
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 2000)) {
			return hit.transform.gameObject;
		}
		return null;
	}

	private void handleSelection() {
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;
		if( Physics.Raycast( ray, out hit, 2000 ) )
		{
			// Accept only transforms tagged with "Bird"
			if (hit.transform.tag == "Bird") {
				selectBird (hit);
				this.isSelecting = true;
			} else if (hit.transform.tag == "ManagementObject") {
				selectedObject = hit.transform.gameObject;
				this.isSelecting = true;
			} else {
				//other
			}

		}
	}
		
	void selectBird(RaycastHit hit) {
		// Check if gameobject found in list already
		bool found = false;
		foreach(GameObject selection in selectedBirds) {
			if(selection == hit.transform.gameObject) {
				found = true;
			}
		}

		// Close info window
		updateBirdList();
		foreach(GameObject bird in selectedBirdObjects) {
			bird.GetComponent<InformationTextScript>().setShow(false);
		}

		if(!found) {
			// Add to list and change shader for the silhouette
			selectedBirds.Add(hit.transform.gameObject);
			foreach(Transform child in hit.transform) {
				if(child.name == "Body") {
					// Add silhouette only to the body object
					child.GetComponent<Renderer>().material.shader = silhouette;
					child.GetComponent<Renderer>().material.SetColor ("_OutlineColor", Color.green);
				}
			}
		} else {
			// Remove from list and remove silhouette
			selectedBirds.Remove(hit.transform.gameObject);
			foreach(Transform child in hit.transform) {
				if(child.name == "Body") {
					child.GetComponent<Renderer>().material.shader = Shader.Find ("Diffuse");

				}
			}
		}

		found = false;
	}

	public ArrayList getSelectionList() {
		return this.selectedBirds;
	}

	public void addToSelectionList(GameObject bird) {
		selectedBirds.Add(bird);
		foreach(Transform child in bird.transform) {
			if(child.name == "Body") {
				child.GetComponent<Renderer>().material.shader = silhouette;
				child.GetComponent<Renderer>().material.SetColor ("_OutlineColor", Color.green);
			}
		}
	}

	public void updateBirdList() {
		selectedBirdObjects = new GameObject[selectedBirds.Count];
		for(int i = 0; i < selectedBirds.Count; i++) {
			selectedBirdObjects[i] = (GameObject)selectedBirds.ToArray()[i];
		}
	}

	public void showInfoWindow() {
		updateBirdList();

		foreach(GameObject bird in selectedBirdObjects) {
			bird.GetComponent<InformationTextScript>().show();
		}
	}

	public GameObject getSelectedObject() {
		return this.selectedObject;
	}
}
