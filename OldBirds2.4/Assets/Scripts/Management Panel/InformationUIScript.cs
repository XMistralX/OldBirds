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

	public RectTransform addNewKeyValuePanel;

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

	private Transform addNewKeyValuePanelInstance;

	private bool enabled = true;

	void Update() {
		updateInfo ();
		mouseOverBird ();
	}

	void Start () {	
		if (informationWindowObject) {
			initPanel ();

			this.infoMap = new Dictionary<string, string> ();
			this.infoUIMap = new Dictionary<string, GameObject> ();
			this.infoMap.Add ("Name", birdName);
			this.infoMap.Add ("Status", birdStatus);
			this.infoMap.Add ("Next Event", nextEvent);
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
		Debug.Log ("added <"+ key + ", " + value + ">");
		destroyPanel ();
		initPanel ();
		renderUI ();
	}

	public void changeValueInKey(string key, string value) {
		this.infoMap [key] = value;
	}

	public void removeKey(string key) {
		this.infoMap.Remove (key);
		Debug.Log ("removed <"+ key + ">");
		destroyPanel ();
		initPanel ();
		renderUI ();
	}

	public string getKeyTextFromParentObject(GameObject parent) {
		return parent.transform.FindChild ("Key").GetComponent<Text> ().text;
	}

	public string getValueTextFromParentTransform(GameObject parent) {
		return parent.transform.FindChild ("Value").GetComponent<InputField>().text;
	}
	private void mouseOverBird(){
		if (managementScript.getSelectionList ().Count <= 0) {
			setModifiable(false);
			if (managementScript.getPointerObject () != null && managementScript.getPointerObject ().tag == "Bird" && managementScript.getPointerObject () == this.gameObject && this.enabled == false) {
				show ();
			} else if (managementScript.getPointerObject () == null && this.enabled == true) {
				hide ();
			} else if (managementScript.getPointerObject () != null && managementScript.getPointerObject ().tag != "Bird" && this.enabled == true) {
				hide ();
			}
		} else {
			foreach (GameObject bird in managementScript.getSelectionList()) {
				if (bird == this.gameObject) {
					show ();
					setModifiable (true);
				} else {
					setModifiable (false);
				}
			}
		}
	}

	private void destroyPanel() {
		if (this.infoUIMap.Keys.Count > 0) {
			foreach (string key in this.infoMap.Keys) {
				Destroy (this.infoUIMap[key].gameObject);
			}
			this.infoUIMap.Clear();
		}
		if (this.addNewKeyValuePanelInstance) {
			Destroy (this.addNewKeyValuePanelInstance.gameObject);
		}
		if (this.informationWindowInstance) {
			Destroy (this.informationWindowInstance.gameObject);
		}
	}

	private void initPanel() {
		if (informationWindowObject) {
			informationWindowInstance = Instantiate (informationWindowObject);
			informationWindowObject.transform.name = string.Format ("InformationPanel");
			informationWindowInstance.SetParent (GameObject.Find ("Canvas").transform);
			informationWindowInstance.GetComponent<RectTransform> ().localPosition = new Vector3 (0f, 0f, 0f);
		}
	}


	private void renderUI() {
		int counter = 0;

		if (this.infoUIMap.Keys.Count > 0) {
			foreach (string key in this.infoUIMap.Keys) {
				Destroy (this.infoUIMap[key].gameObject);
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

			Transform destroyButton = eachKeyValue.transform.FindChild ("Destroy");
			destroyButton.GetComponent<Button> ().onClick.AddListener( () => removeKey(keyTransform.GetComponent<Text> ().text) );
			destroyButton.gameObject.SetActive (false);

			counter++;
		}
		Transform addButton = this.informationWindowInstance.FindChild ("Add");
		addButton.GetComponent<Button> ().onClick.AddListener( () => showAddNewKeyValuePanel() );
		addButton.gameObject.SetActive (false);


		updateInfo ();
	}

	private void showAddNewKeyValuePanel() {
		destroyPanel ();
		this.addNewKeyValuePanelInstance = Instantiate (this.addNewKeyValuePanel) as Transform;
		this.addNewKeyValuePanelInstance.SetParent (GameObject.Find ("Canvas").transform);
		this.addNewKeyValuePanelInstance.GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
		this.addNewKeyValuePanelInstance.GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);

		Transform addButton = this.addNewKeyValuePanelInstance.FindChild ("Add");
		Transform cancelButton = this.addNewKeyValuePanelInstance.FindChild ("Cancel");
		Transform keyField = this.addNewKeyValuePanelInstance.FindChild ("KeyField");
		Transform valueField = this.addNewKeyValuePanelInstance.FindChild ("ValueField");


		cancelButton.GetComponent<Button> ().onClick.AddListener( () => hide() );

		addButton.GetComponent<Button> ().onClick.AddListener( () => addNewKeyValue(keyField.GetComponent<InputField> ().text, valueField.GetComponent<InputField> ().text) );
	}

	private void setModifiable(bool isActive) {
		setAddButtonActive (isActive);
		setDestroyButtonActive (isActive);
	}

	private void setAddButtonActive(bool isActive) {
		if(this.informationWindowInstance) {
			this.informationWindowInstance.FindChild ("Add").gameObject.SetActive(isActive);
		}
	}

	private void setDestroyButtonActive(bool isActive) {
		if(this.informationWindowInstance) {
			foreach (string key in this.infoUIMap.Keys) {
				this.infoUIMap[key].transform.FindChild ("Destroy").gameObject.SetActive(isActive);
			}
		}
	}

	public void updateInfo() {
		foreach (string key in this.infoUIMap.Keys) {
			changeValueInKey (key, getValueTextFromParentTransform(this.infoUIMap[key]));
			//this.UIMap [keyTransform].GetComponent<InputField> ().onEndEdit.AddListener ((value) => changeValueInKey(keyTransform.GetComponent<Text>().text, value));
			//Debug.Log (key + "  " + this.infoMap[key] + "  " + getValueTextFromParentTransform(this.infoUIMap[key]));
		}
	}

	public void show() {
		if(!enabled) {
			this.enabled = true;
			enableWindow();
		}
	}

	public void hide() {
		if(enabled) {
			this.enabled = false;
			disableWindow();
		}
	}

	public void toggle() {
		if(enabled) {
			hide();
		} else {
			show();
		}

	}

	public void setShow(bool show) {
		this.enabled = show;
		if(show) enableWindow();
		else disableWindow();
	}


	void enableWindow ()
	{
		if(informationWindowInstance) {
			destroyPanel();
			initPanel();
			renderUI();
		}
		else {
			initPanel();
			renderUI();
		}
	}

	void disableWindow ()
	{
		if (informationWindowInstance || addNewKeyValuePanelInstance) {
			destroyPanel ();
		}
	}
}
