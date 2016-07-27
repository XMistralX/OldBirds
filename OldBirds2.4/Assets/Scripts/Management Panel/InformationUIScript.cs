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
	private Dictionary<string, GameObject> infoUIMap;

	private RectTransform informationWindowInstance;
	private Text nameText;
	private Text statusText;
	private Text nextEventText;
	private Text infoText;

	private GameObject holder;
	private ManagementPanel managementScript;

	private bool enabled = true;

	void Update() {
		updateInfo ();
		mouseOverBird ();
	}

	void Start () {	
		if (informationWindowObject) {
			informationWindowInstance = Instantiate(informationWindowObject);
			informationWindowObject.transform.name = string.Format ("InformationPanel");
			informationWindowInstance.SetParent(GameObject.Find("Canvas").transform);
			informationWindowInstance.GetComponent<RectTransform>().localPosition = new Vector3 (0f, 0f, 0f);


			this.infoMap = new Dictionary<string, string> ();
			this.infoUIMap = new Dictionary<string, GameObject> ();
			this.infoMap.Add ("Name", birdName);
			this.infoMap.Add ("Status", birdStatus);
			this.infoMap.Add ("NextEvent", nextEvent);
			holder = GameObject.Find("MainController");
			managementScript = holder.GetComponent<ManagementPanel> ();
			renderUI ();
			show ();
		}

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

	public string getKeyTextFromParentObject(GameObject parent) {
		return parent.transform.FindChild ("Key").GetComponent<Text> ().text;
	}

	public string getValueTextFromParentTransform(GameObject parent) {
		return parent.transform.FindChild ("Value").GetComponent<InputField>().text;
	}
	private void mouseOverBird(){
		if (managementScript.getPointerObject () != null && managementScript.getPointerObject ().tag == "Bird" && this.enabled == false) {
			show ();
		} else if (managementScript.getPointerObject () == null && this.enabled == true) {
			show ();
		} else if (managementScript.getPointerObject () != null && managementScript.getPointerObject ().tag != "Bird" && this.enabled == true) {
			show ();
		}
	}
	private void renderUI() {
		int counter = 0;

		if (this.infoUIMap.Keys.Count > 0) {
			foreach (string key in this.infoMap.Keys) {
				Destroy (this.infoUIMap[key]);
			}
			this.infoUIMap.Clear();
		}

		foreach (string key in this.infoMap.Keys) {
			GameObject eachKeyValue = Instantiate (KeyValue, new Vector3(40f,40f,0f), Quaternion.identity) as GameObject;
			//eachKeyValue.transform.SetParent (GameObject.Find("InformationPanel").transform);
			eachKeyValue.transform.SetParent (this.informationWindowInstance);

			this.infoUIMap.Add (key, eachKeyValue);

			Transform keyTransform = eachKeyValue.transform.FindChild ("Key");
			keyTransform.GetComponent<Text> ().text = key;
			eachKeyValue.transform.name = string.Format ("KeyValue ({0})",key);

			Transform valueTransform = eachKeyValue.transform.FindChild ("Value");
			valueTransform.GetComponent<InputField> ().text = this.infoMap[key];

			eachKeyValue.GetComponent<RectTransform>().anchoredPosition = new Vector3 (0f, 0f-(counter*90f), 0f);
			eachKeyValue.transform.localScale = new Vector3 (3.5f,3.5f,3.5f);

			counter++;
		}
		updateInfo ();
	}

	public void updateInfo() {
		foreach (string key in this.infoUIMap.Keys) {
			changeValueInKey (key, getValueTextFromParentTransform(this.infoUIMap[key]));
			//this.UIMap [keyTransform].GetComponent<InputField> ().onEndEdit.AddListener ((value) => changeValueInKey(keyTransform.GetComponent<Text>().text, value));
			//Debug.Log (key + "  " + this.infoMap[key] + "  " + getValueTextFromParentTransform(this.infoUIMap[key]));
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
