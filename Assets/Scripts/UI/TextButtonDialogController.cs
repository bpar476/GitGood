using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextButtonDialogController : MonoBehaviour {
	public InputField inputField;
	public Text titleText;
    public Text promptText;
	public string ErrorText {
		get {
			return _errorString;
		}
		set {
			_errorString = value;
			_errorText.text = value;
		}
	}
    public Button submitButton;

	private Text _errorText;
	private string _errorString;

    // Use this for initialization
    void Awake () {
		inputField = gameObject.transform.Find("UserResponse").gameObject.GetComponent<InputField>();
		titleText = gameObject.transform.Find("Prompt").gameObject.GetComponent<Text>();
		promptText = gameObject.transform.Find("UserResponse/Prefix").gameObject.GetComponent<Text>();
		_errorText = gameObject.transform.Find("ErrorText").gameObject.GetComponent<Text>();
		submitButton = gameObject.transform.Find("SubmitButton").gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
