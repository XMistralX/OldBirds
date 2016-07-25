using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControl : MonoBehaviour {

	private GameObject holder;
	private GameObject birds;
	// Use this for initialization
	void Start () {
		holder = GameObject.Find("Text");

	}

	// Update is called once per frame
	void Update () {
		holder.GetComponent<RectTransform> ().anchoredPosition3D = Input.mousePosition;
		print (Input.mousePosition);
	}
}
