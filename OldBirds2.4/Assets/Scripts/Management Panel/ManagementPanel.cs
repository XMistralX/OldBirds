using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ManagementPanel : MonoBehaviour {

	public Shader silhouette;
	private ArrayList selectedBirds;
	private GameObject[] selectedBirdObjects;

	private bool isCreating;
	private bool isSelecting;
	private bool isMovable;

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
		//Debug.Log (getPointerObject());
		//Debug.Log (this.selectedObject);
		handleInput ();

	}

	void handleInput () {
		if (isCreating) {
			setSelectedObjectPosition (getWorldPointIgnoreObject(this.selectedObject));
			if (Input.GetMouseButtonDown (0)) {
				//if mouse isn't over a UI Element
				if(!EventSystem.current.IsPointerOverGameObject()) {
					this.isCreating = false;
					selectObject (null);
				}
			}

		} else {
			if (Input.GetMouseButtonDown (0)) {
				// start moveable if click on the same object
				if (isSelecting && getPointerObject () == this.selectedObject) {
					this.isMovable = true;

				// select other object
				} else {
					this.isMovable = false;
					handleSelection ();
				}

			} else if ( Input.GetMouseButton(1)) {
				if (isSelecting) {
					isSelecting = false;
					isMovable = false;
					dehighlightObject ();
					selectedObject = null;
				}
			} else if ( Input.GetMouseButton(0)) {
				if (isSelecting && isMovable) {
					
					setSelectedObjectPosition (getWorldPointIgnoreObject(this.selectedObject));
				}
			}
		}
	}

	private Vector3 getWorldPointIgnoreObject(GameObject obj){
		obj.transform.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		Ray ray = GetComponent<Camera>().ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			obj.transform.gameObject.layer = LayerMask.NameToLayer("Default");
			return hit.point;
		}
		obj.transform.gameObject.layer = LayerMask.NameToLayer("Default");
		return Vector3.zero;
	}

	private Vector3 getWorldPoint(){
		Ray ray = GetComponent<Camera>().ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			return hit.point;
		}
		return Vector3.zero;
	}

	public Vector3 getObjectLocation(){
		return selectedObject.transform.position;
	}
	public void changeCreatingObject(GameObject creatingGameObject) {

		// if already in creating state, destroy the previously creating one.
		if (this.isCreating) {
			Destroy (this.selectedObject);

			if(this.creatingObject == creatingGameObject){
				this.isCreating = false;
				this.creatingObject = null;
				return;
			}
		}
		this.isCreating = true;
		this.isSelecting = false;
		this.creatingObject = creatingGameObject;
		selectObject(createObject());
	}

	public void selectObject(GameObject obj) {
		if(this.selectedObject) {
			dehighlightObject ();
		}
		this.selectedObject = obj;
		highlightObject();
	}

	private GameObject createObject(){
		return Instantiate (creatingObject, getWorldPoint(), Quaternion.identity) as GameObject;
	}
	public void deleteObject(){
		Destroy (selectedObject);
	}
	private void setSelectedObjectPosition(Vector3 pos){

		//float yOffset = this.selectedObject.transform.position.y - this.selectedObject.GetComponent<Collider> ().bounds.min.y;
		float yOffset = this.selectedObject.transform.position.y - getBounds(this.selectedObject).min.y;
		selectedObject.transform.position = new Vector3(pos.x, pos.y+=yOffset, pos.z);
	}

	private void highlightObject() {
		if (this.selectedObject) {
			if (this.selectedObject.GetComponent<Renderer> ()) {
				this.selectedObject.GetComponent<Renderer> ().material.shader = silhouette;
				this.selectedObject.GetComponent<Renderer> ().material.SetColor ("_OutlineColor", Color.green);
			} else {
				foreach (Transform child in this.selectedObject.transform) {
					if (child.GetComponent<Renderer> ()) {
						child.GetComponent<Renderer> ().material.shader = silhouette;
						child.GetComponent<Renderer> ().material.SetColor ("_OutlineColor", Color.green);
					}
				}
			}
		}
	}

	private void dehighlightObject() {
		if (this.selectedObject) {
			if (this.selectedObject.GetComponent<Renderer> ()) {
				this.selectedObject.GetComponent<Renderer> ().material.shader = Shader.Find ("Diffuse");
			}
			else {
				foreach (Transform child in this.selectedObject.transform) {
					if (child.GetComponent<Renderer> ()) {
						child.GetComponent<Renderer>().material.shader = Shader.Find ("Diffuse");
					}
				}
			}
		} 
	}

	public GameObject getSelectedObject() {
		return this.selectedObject;
	}



	public void incrementObjectRotation(int axis , int angle){
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
	public void setObjectRotation(int axis , int newAngle){
		switch(axis){
		case x:
			selectedObject.transform.eulerAngles = new Vector3 (
				newAngle,
				selectedObject.transform.eulerAngles.y,
				selectedObject.transform.eulerAngles.z
			);
			break;
		case y:
			selectedObject.transform.eulerAngles = new Vector3 (
				selectedObject.transform.eulerAngles.x,
				newAngle,
				selectedObject.transform.eulerAngles.z
			);
			break;
		case z:
			selectedObject.transform.eulerAngles = new Vector3 (
				selectedObject.transform.eulerAngles.x,
				selectedObject.transform.eulerAngles.y,
				newAngle
			);
			break;
		default:
			break;
		}
	}
	public Vector3 getObjectRotation(){
		if(selectedObject != null){
			return selectedObject.transform.eulerAngles;
		}
		return Vector3.zero;
	}
	public void changeObjectScale(int choice){
		switch (choice) {
		case scaleUp:
			selectedObject.transform.localScale = new Vector3(
				selectedObject.transform.localScale.x + 1 ,
				selectedObject.transform.localScale.y + 1,
				selectedObject.transform.localScale.z + 1
			);
			break;
		case scaleDown:
			if (selectedObject.transform.localScale.x > 1) {
				selectedObject.transform.localScale = new Vector3 (
					selectedObject.transform.localScale.x - 1,
					selectedObject.transform.localScale.y - 1,
					selectedObject.transform.localScale.z - 1 
				);
			}
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
				selectObject(hit.transform.gameObject);
				this.isSelecting = true;
				highlightObject ();
			} else {
				//other
			}

		}
	}

	public Bounds getBounds(GameObject obj){
		Bounds bounds;
		Renderer childRender;
		bounds = getRenderBounds(obj);
		if(bounds.extents.x == 0){
			bounds = new Bounds(obj.transform.position,Vector3.zero);
			foreach (Transform child in obj.transform) {
				childRender = child.GetComponent<Renderer>();
				if (childRender) {
					bounds.Encapsulate(childRender.bounds);
				}else{
					bounds.Encapsulate(getBounds(child.gameObject));
				}
			}
		}
		return bounds;
	}

	public	Bounds getRenderBounds(GameObject obj){
		Bounds bounds = new  Bounds(Vector3.zero,Vector3.zero);
		Renderer render = obj.GetComponent<Renderer>();
		if(render!=null){
			return render.bounds;
		}
		return bounds;
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

}
