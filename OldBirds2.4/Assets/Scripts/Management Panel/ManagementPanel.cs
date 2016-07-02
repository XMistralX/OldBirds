using UnityEngine;
using System.Collections;

public class ManagementPanel : MonoBehaviour {

	bool creating;
	public GameObject start;
	public GameObject end;
	public GameObject sphere;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		getInput ();

	}
	void getInput () {
		if (Input.GetMouseButtonDown (0)) {
			//setStart ();
			createBall();
		} else if ( Input.GetMouseButton(0)) {
			setBallPosition ();
		}
		else if (Input.GetMouseButtonUp(0)) {
			//setEnd ();

		} //else {
		//if (creating) {
		//	adjust ();
		//}
		//}
	}
	void createBall(){
		sphere = Instantiate (sphere, getWorldPoint(), Quaternion.identity) as GameObject;
	}
	void setBallPosition(){
		sphere.transform.position = getWorldPoint ();
	}
	void setStart (){
		creating = true;
		start.transform.position = getWorldPoint ();
	}
	void setEnd (){
		creating = false;
		end.transform.position = getWorldPoint ();
	}
	void adjust (){

	}
	Vector3 getWorldPoint(){
		Ray ray = GetComponent<Camera>().ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			return hit.point;
		}
		return Vector3.zero;
	}
}
