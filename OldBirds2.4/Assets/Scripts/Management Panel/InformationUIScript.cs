using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



public class InformationUIScript : MonoBehaviour {
	public Texture birdAvatar;

	public string birdName;
	public string birdStatus;
	public string nextEvent;
	public string[] birdInfo;
	public RectTransform informationWindowObject;
	public Canvas UICanvas;

	public GameObject KeyValue;

	public Dictionary<string, string> infoMap;
	public Dictionary<Transform, Transform> UIMap;

	private RectTransform informationWindowInstance;
	private Text nameText;
	private Text statusText;
	private Text nextEventText;
	private Text infoText;

	private bool enabled = false;

	void Update() {
		updateInfo ();
	}

	void Start () {	

		this.infoMap = new Dictionary<string, string> ();
		this.UIMap = new Dictionary<Transform, Transform> ();
		this.infoMap.Add ("Name", birdName);
		this.infoMap.Add ("Status", birdStatus);
		this.infoMap.Add ("NextEvent", nextEvent);
		renderUI ();

	}

	public Dictionary<string, string> getInfoMap() {
		return this.infoMap;
	}

	public void addNewKeyValue(string key, string value) {
		this.infoMap.Add (key, value);
	}

	public void changeValueInKey(string key, string value) {
		this.infoMap [key] = value;
	}

	public void removeKey(string key) {
		this.infoMap.Remove (key);
	}

	private void renderUI() {
		foreach (string key in this.infoMap.Keys) {
			GameObject eachKeyValue = Instantiate (KeyValue, new Vector3(40f,40f,0f), Quaternion.identity) as GameObject;
			eachKeyValue.transform.SetParent (GameObject.Find("Canvas").transform);
			Transform keyTransform = eachKeyValue.transform.FindChild ("Key");
			keyTransform.GetComponent<Text> ().text = key;
			eachKeyValue.transform.name = string.Format ("KeyValue ({0})",key);

			Transform valueTransform = eachKeyValue.transform.FindChild ("Value");
			valueTransform.GetComponent<InputField> ().text = this.infoMap[key];
			UIMap.Add (keyTransform, valueTransform);

		}
		updateInfo ();
	}

	public void updateInfo() {
		foreach (Transform keyTransform in this.UIMap.Keys) {
			changeValueInKey (keyTransform.GetComponent<Text>().text, this.UIMap[keyTransform].GetComponent<InputField>().text);
			//this.UIMap [keyTransform].GetComponent<InputField> ().onEndEdit.AddListener ((value) => changeValueInKey(keyTransform.GetComponent<Text>().text, value));
			Debug.Log (keyTransform.GetComponent<Text>().text + " " + this.UIMap[keyTransform].GetComponent<InputField>().text + " " + this.infoMap[keyTransform.GetComponent<Text>().text]);
		}
	}

	public void show() {
		if(enabled) {
			this.enabled = false;
			disableWindow();
		} else {
			this.enabled = true;
			enableWindow();
		}

	}

	public void setShow(bool show) {
		this.enabled = show;
		if(show) enableWindow();
		else disableWindow();
	}


	void enableWindow ()
	{
		if(informationWindowInstance)
			informationWindowInstance.gameObject.SetActive(true);
	}

	void disableWindow ()
	{
		if(informationWindowInstance)
			informationWindowInstance.gameObject.SetActive(false);
	}
}
