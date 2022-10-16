using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SubmitScore : MonoBehaviour {
	public UnityEvent OnSubmit;
	GameObject target;
	bool details;

	private void Awake() {
		StartCoroutine(WebRequest.GET("/api/v1/competition",
			(string response) => {
				target = transform.Find("Details").gameObject;
				details = true;
			},
			(string status) => {
				target = transform.Find("NoDetails").gameObject;
				details = false;
			}));
	}



	public void Show() {
		target.SetActive(true);
		/*StartCoroutine(WebRequest.GET("/api/v1/leaderboard/" + SessionManager.Instance.ID,
			(string response) => {
				GET_leaderboard_player_id result = JsonUtility.FromJson<GET_leaderboard_player_id>(response);

				if (details) {
					target.transform.Find("DisplayName").GetComponent<TMP_InputField>().text = result.name;
					if (result.first_name != null)
						target.transform.Find("FirstName").GetComponent<TMP_InputField>().text = result.first_name;
					if (result.last_name != null)
						target.transform.Find("LastName").GetComponent<TMP_InputField>().text = result.last_name;
					if (result.email != null)
						target.transform.Find("Email").GetComponent<TMP_InputField>().text = result.email;
					if (result.mobile != null)
						target.transform.Find("Mobile").GetComponent<TMP_InputField>().text = result.mobile;
				}
				else {
					target.transform.Find("DisplayName").GetComponent<TMP_InputField>().text = result.name;
				}
			},
			(string status) => {
				if (status == "500") {
					Debug.Log("Failed to get player leaderboard data: " + status);
				}
			}));*/

	}

	public void Hide() {
		if (details) {
			target.transform.Find("DisplayName").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("FirstName").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("LastName").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("Email").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("Mobile").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("Agree").GetComponent<Toggle>().isOn = false;
			target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";
		}
		else {
			target.transform.Find("DisplayName").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";
		}
		target.SetActive(false);
	}


	public void TrySubmitDetails() {
		target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";

		string name = target.transform.Find("DisplayName").GetComponent<TMP_InputField>().text;
		string first_name = target.transform.Find("FirstName").GetComponent<TMP_InputField>().text;
		string last_name = target.transform.Find("LastName").GetComponent<TMP_InputField>().text;
		string email = target.transform.Find("Email").GetComponent<TMP_InputField>().text;
		string mobile = target.transform.Find("Mobile").GetComponent<TMP_InputField>().text;
		bool agree = target.transform.Find("Agree").GetComponent<Toggle>().isOn;

		if (name == "" || first_name == "" || last_name == "" || email == "" || mobile == "") {
			target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Some fields are incomplete.";
			return;
		}


		if (name.Length < 2 || first_name.Length < 2 || last_name.Length < 2) {
			target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Names must be at least 2 characters long.";
			return;
		}

		// if email does not match email regex
		if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")) {
			target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Invalid email address.";
			return;
		}

		if (!agree) {
			target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "You must agree to the terms and conditions, and privacy policy.";
			return;
		}

		StartCoroutine(WebRequest.GET("/api/v1/leaderboard/valid_name?name=" + name,
			(string response) => {
				Response result = JsonUtility.FromJson<Response>(response);
				if (result.valid) {
					target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";
					OnSubmit.Invoke();
					Submit(name, email, first_name, last_name, mobile, agree);
				}
				else {
					if (result.reason != null)
						target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = result.reason;
					else
						target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Inavlid Name";
				}

			},
			(string status) => {
				Debug.Log("Failed to validate name: " + status);
				target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Server Error! Failed to validate name";
			}
			));
	}

	public void Submit(string name, string email = null, string first_name = null, string last_name = null, string mobile = null, bool agree = false) {

		SubmitData submitData = new() {
			name = name,
			first_name = first_name,
			last_name = last_name,
			email = email,
			mobile = mobile,
			agree_terms = agree,
			device_id = SessionManager.Instance.ID,
			game_id = SessionManager.Instance.Session.game_id
		};

		string jsonString = JsonUtility.ToJson(submitData);

		StartCoroutine(
			WebRequest.POST("/api/v1/leaderboard", jsonString,
			(string response) => { },
			(long status) => {
				Debug.Log("Failed to submit: " + status);
			})
			);

	}

	public void TrySubmitNoDetails() {

		target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";
		string name = target.GetComponentInChildren<TMP_InputField>().text;

		StartCoroutine(WebRequest.GET("/api/v1/leaderboard/valid_name?name=" + name,
			(string response) => {
				Response result = JsonUtility.FromJson<Response>(response);
				if (result.valid) {
					target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";
					OnSubmit.Invoke();
					Submit(name);
				}
				else {
					if (result.reason != null)
						target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = result.reason;
					else
						target.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Inavlid Name";
				}
			},
			(string status) => {
				Debug.Log("Failed to validate name: " + status);
			}
			));

	}
}

struct GET_leaderboard_player_id {
	public string name;
	public int portfolio_value;
	public string email;
	public string first_name;
	public string last_name;
	public string mobile;
}

struct Response {
	public bool valid;
	public string reason;
}