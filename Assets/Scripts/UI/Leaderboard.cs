using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Leaderboard : MonoBehaviour {
	public static Leaderboard Instance {
		get; private set;
	}
	Loader loader;

	public GameObject leaderboardPrefab;

	List<ScoreData> scores = new();
	List<GameObject> scoreObjects = new();

	private void Awake() {
		if (!Instance)
			Instance = this;
		loader = GetComponentInChildren<Loader>();
	}


	// Start is called before the first frame update
	void Start() {
		GetLeaderboard();
	}


	public void Refresh() {
		GetLeaderboard();
	}

	public void CanSubmit(Action<bool> callback) {
		StartCoroutine(WebRequest.GET("/api/v1/leaderboard/" + SessionManager.Instance.ID,
			(string response) => {
				GET_leaderboard_player_id result = JsonUtility.FromJson<GET_leaderboard_player_id>(response);
				// If bet old score
				if (GameManager.Instance.totalEarnings > result.portfolio_value) {
					// If they are in top 10
					// If they are in top 10
					if (GameManager.Instance.totalEarnings > scores[scores.Count - 1].score || scores.Count < 10) {
						callback(true);
						return;
					}
				}

				callback(false);
			},
			(string status) => {
				// If no leaderboard entries
				if (status == "404") {
					// If they are in top 10
					if (GameManager.Instance.totalEarnings > scores[scores.Count - 1].score || scores.Count < 10) {
						callback(true);
						return;
					}
				}
				else {
					Debug.Log("Failed to get player leaderboard data: " + status);
				}
				callback(false);
			}));
	}

	void CreateLeaderboard() {
		scoreObjects.ForEach(o => { Destroy(o); });
		scoreObjects.Clear();
		loader.isActive = true;

		scores.AddRange(scores);

		for (int i = 0; i < scores.Count; i++) {
			GameObject obj = Instantiate(leaderboardPrefab, transform);


			obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -25f * (i));

			obj.transform.Find("Place").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
			obj.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = scores[i].name;
			obj.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = "$" + (scores[i].score).ToString("N0");
			scoreObjects.Add(obj);
		}


		loader.isActive = false;
	}

	void GetLeaderboard() {
		StartCoroutine(WebRequest.GET("/api/v1/leaderboard",
			(string result) => {
				ScoreDataCollection sdc = JsonUtility.FromJson<ScoreDataCollection>("{\"scoreData\":" + result + "}");
				scores = sdc.scoreData.ToList();
				CreateLeaderboard();
			},
			(string status) => {
				Debug.Log("Failed to get leaderboard: " + status);
			}));
	}

	public void SubmitScore(string name, string email = null, string first_name = null, string last_name = null, string mobile = null, bool? agree = null) {

		SubmitData submitData = new() {
			name = name,
			email = email,
			first_name = first_name,
			last_name = last_name,
			mobile = mobile,
			agree = agree,
			game_id = SessionManager.Instance.previous_game_id
		};
		string jsonString = JsonUtility.ToJson(submitData);

		StartCoroutine(
			WebRequest.POST("/api/v1/leaderboard/" + SessionManager.Instance.ID, jsonString,
			(string response) => { GetLeaderboard(); },
			(long status) => { Debug.Log("Failed to submit: " + status); })
			);

	}
}

class SubmitData {
	public string game_secret = WebRequest.SECRET;
	public string name;
	public string game_id;
	public int portfolio_value;
	public string email = null;
	public string first_name = null;
	public string last_name = null;
	public string mobile = null;
	public bool? agree = null;
}

[System.Serializable]
class ScoreDataCollection {
	public ScoreData[] scoreData;
}

[System.Serializable]
struct ScoreData {
	public string score_id;
	public string name;
	public int score;
}