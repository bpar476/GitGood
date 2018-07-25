using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextButtonDialogController : MonoBehaviour {
	public InputField inputField;
	public Text titleText;
    public Text promptText;
    public Button submitButton;

    // Use this for initialization
    void Awake () {
		inputField = gameObject.transform.Find("UserResponse").gameObject.GetComponent<InputField>();
		titleText = gameObject.transform.Find("Prompt").gameObject.GetComponent<Text>();
		promptText = gameObject.transform.Find("UserResponse/Prefix").gameObject.GetComponent<Text>();
		submitButton = gameObject.transform.Find("SubmitButton").gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
