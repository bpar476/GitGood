using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextButtonDialogController : MonoBehaviour {
	public InputField inputField;
	public Text promptText;
    public Button submitButton;

    // Use this for initialization
    void Awake () {
		inputField = gameObject.transform.Find("UserResponse").gameObject.GetComponent<InputField>();
		promptText = gameObject.transform.Find("Prompt").gameObject.GetComponent<Text>();
		submitButton = gameObject.transform.Find("SubmitButton").gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
