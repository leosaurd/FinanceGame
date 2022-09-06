using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SubmitScore : MonoBehaviour {

	GameObject target;
	bool details;

	private void Awake() {
		StartCoroutine(WebRequest.GET("/api/v1/competition/ongoing",
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
		StartCoroutine(WebRequest.GET("/api/v1/leaderboard/" + SessionManager.Instance.ID,
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
			}));

	}

	public void Hide() {
		target.SetActive(false);
		if (details) {
			target.transform.Find("DisplayName").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("FirstName").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("LastName").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("Email").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("Mobile").GetComponent<TMP_InputField>().text = "";
			target.transform.Find("Agree").GetComponent<Toggle>().isOn = false;
		}
		else {
			target.transform.Find("DisplayName").GetComponent<TMP_InputField>().text = "";
		}
	}


	public void TrySubmitDetails() {
		transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";

		string name = target.transform.Find("DisplayName").GetComponent<TMP_InputField>().text;
		string first_name = target.transform.Find("FirstName").GetComponent<TMP_InputField>().text;
		string last_name = target.transform.Find("LastName").GetComponent<TMP_InputField>().text;
		string email = target.transform.Find("Email").GetComponent<TMP_InputField>().text;
		string mobile = target.transform.Find("Mobile").GetComponent<TMP_InputField>().text;
		bool agree = target.transform.Find("Agree").GetComponent<Toggle>().isOn;

		if (name == "" || first_name == "" || last_name == "" || email == "" || mobile == "") {
			transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Some fields are incomplete.";
			return;
		}

		if (!agree) {
			transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "You must agree to the terms and conditions, and privacy policy.";
			return;
		}

		StartCoroutine(WebRequest.GET("/api/v1/valid_name?name=" + name,
			(string response) => {
				Response result = JsonUtility.FromJson<Response>(response);
				if (result.valid) {
					transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";
					Leaderboard.Instance.SubmitScore(name, first_name, last_name, email, mobile, agree);
				}
				else {
					if (result.reason != null)
						transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = result.reason;
					else
						transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Inavlid Name";
				}
			},
			(string status) => {
				Debug.Log("Failed to validate name: " + status);
				transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Server Error! Failed to validate name";
			}
			));
	}

	public void TrySubmitNoDetails() {
		string name = target.GetComponentInChildren<TMP_InputField>().text;

		StartCoroutine(WebRequest.GET("/api/v1/valid_name?name=" + name,
			(string response) => {
				Response result = JsonUtility.FromJson<Response>(response);
				if (result.valid) {
					transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";
					Leaderboard.Instance.SubmitScore(name);
				}
				else {
					if (result.reason != null)
						transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = result.reason;
					else
						transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Inavlid Name";
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