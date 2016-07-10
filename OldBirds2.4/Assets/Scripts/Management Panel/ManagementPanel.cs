using UnityEngine;
using System.Collections;

public class ManagementPanel : MonoBehaviour {

	public Shader silhouette;
	private ArrayList selectedBirds;
	private GameObject[] selectedBirdObjects;

	private bool isCreating;

	public GameObject start;
	public GameObject end;
	public GameObject creatingObject;
	public GameObject selectedObject;
	// Use this for initialization
	void Start () {
		selectedBirds = new ArrayList();
	}

	// Update is called once per frame
	void Update () {
		getInput ();

	}
	void getInput () {
		if (Input.GetMouseButtonDown (0)) {
			handleSelection();
		} else if ( Input.GetMouseButton(0)) {
			if (isCreating) {
				setSelectedObjectPosition ();
			}
		}
		else if (Input.GetMouseButtonUp(0)) {
			setEnd ();

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

	public void changeCreatingObject(GameObject thatGameObject) {
		this.creatingObject = thatGameObject;
	}

	private void createBall(){
		selectedObject = Instantiate (creatingObject, getWorldPoint(), Quaternion.identity) as GameObject;
	}
	private void setSelectedObjectPosition(){
		selectedObject.transform.position = getWorldPoint ();
	}
	private void setStart (){
		isCreating = true;
	}
	private void setEnd (){
		isCreating = false;
	}

	private void handleSelection() {
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;
		if( Physics.Raycast( ray, out hit, 2000 ) )
		{
			Debug.Log (hit.transform.gameObject);
			// Accept only transforms tagged with "Bird"
			if (hit.transform.tag == "Bird") {
				selectBird (hit);
			} else if (hit.transform.tag == "ManagementObject") {
				selectedObject = hit.transform.gameObject;
				setStart ();
			} else {
				if (creatingObject) {
					setStart ();
					createBall ();
				}
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
