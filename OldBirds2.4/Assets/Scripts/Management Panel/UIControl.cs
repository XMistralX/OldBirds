using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControl : MonoBehaviour {

	private Text holder;
	private GameObject birds;
	// Use this for initialization
	void Start () {
		holder = gameObject.GetComponent<Text>();

	}

	// Update is called once per frame
	void Update () {
		holder.rectTransform.anchoredPosition3D = Input.mousePosition;
		print (Input.mousePosition);
	}
}
