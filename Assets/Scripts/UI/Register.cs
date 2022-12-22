using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public GameObject RegisterSection;
    public GameObject LinkDeviceSection;
	public GameObject SuccessSection;
    public void RegisterUser() {
		string displayName = RegisterSection.transform.Find("DisplayName").GetComponent<TMP_InputField>().text;
		string email = RegisterSection.transform.Find("Email").GetComponent<TMP_InputField>().text;
		string firstName = RegisterSection.transform.Find("FirstName").GetComponent<TMP_InputField>().text;
		string lastName = RegisterSection.transform.Find("LastName").GetComponent<TMP_InputField>().text;
		string mobile = RegisterSection.transform.Find("Mobile").GetComponent<TMP_InputField>().text;
		bool agree = RegisterSection.transform.Find("Agree").GetComponent<Toggle>().isOn;

		TextMeshProUGUI errorText = RegisterSection.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>();
		errorText.text = "";

		if (displayName == "" || email == "" || firstName == "" || lastName == "" || mobile == "" || !agree) {
			errorText.text = "Please fill all the fields";
			return;
		}

		if (name.Length < 2 || firstName.Length < 2 || lastName.Length < 2) {
			errorText.text = "Names must be at least 2 characters long.";
			return;
		}

		// if email does not match email regex
		if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")) {
			errorText.text = "Invalid email address.";
			return;
		}

		StartCoroutine(WebRequest.GET("/api/v1/leaderboard/valid_name?name=" + name,
			(string response) => {
				Response result = JsonUtility.FromJson<Response>(response);
				if (result.valid) {
					errorText.text = "";

					PlayerInfo playerInfo = new() {
						deviceID = SessionManager.Instance.ID,
						displayName = displayName,
						email = email,
						firstName = firstName,
						lastName = lastName,
						mobile = mobile,
						agreeTerms = agree
					};

					string jsonString = JsonUtility.ToJson(playerInfo);

					StartCoroutine(WebRequest.POST("/api/v1/register", jsonString,
						(string response) => {
							PlayerInfo serverPlayerInfo = JsonUtility.FromJson<PlayerInfo>(response);
							PlayerInfoHelper.SavePlayerInfo(serverPlayerInfo);
							RegisterSection.SetActive(false);
							SuccessSection.SetActive(true);
						},
						(long status) => {
							if (status == 400) {
								errorText.text = "Make sure all fields are filled out correctly";
							}
							else {
								Debug.Log("Failed to register: " + status);
								errorText.text = "Something went wrong while registering";
							}
						}
					));
				}
				else {
					if (result.reason != null)
						errorText.text = result.reason;
					else
						errorText.text = "Inavlid Name";
				}

			},
			(long status) => {
				Debug.Log("Failed to validate name: " + status);
				errorText.text = "Server Error! Failed to validate name";
			}
			));
	}

    public void LinkDevice() {
		string email = LinkDeviceSection.transform.Find("Email").GetComponent<TMP_InputField>().text;
		TextMeshProUGUI errorText = LinkDeviceSection.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>();
		errorText.text = "";

		// if email does not match email regex
		if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")) {
			errorText.text = "Invalid email address.";
			return;
		}

		string deviceID = SessionManager.Instance.ID;

		string json = "{\"email\": \"" + email + "\", \"deviceID\": \"" + deviceID + "\"}";
		

		StartCoroutine(WebRequest.POST("/api/v1/is_registered", json,
			(string response) => {
				PlayerInfo serverPlayerInfo = JsonUtility.FromJson<PlayerInfo>(response);
				PlayerInfoHelper.SavePlayerInfo(serverPlayerInfo);
				LinkDeviceSection.SetActive(false);
				SuccessSection.SetActive(true);
			},
			(long status) => {
				if (status == 404) 
					errorText.text = "That email has not been registered";
				else
					errorText.text = "Something went wrong while linking device";
				
			}
		));
	}
}
struct Response {
	public bool valid;
	public string reason;
}