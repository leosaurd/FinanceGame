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

	List<ScoreData> leaderboard = new();
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

	void CreateLeaderboard() {
		scoreObjects.ForEach(o => { Destroy(o); });
		scoreObjects.Clear();
		loader.isActive = true;


		for (int i = 0; i < leaderboard.Count; i++) {
			GameObject obj = Instantiate(leaderboardPrefab, transform);


			obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -25f * (i));

			obj.transform.Find("Place").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
			obj.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = leaderboard[i].name;
			obj.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = "$" + (leaderboard[i].portfolio_value).ToString("N0");
			scoreObjects.Add(obj);
		}


		loader.isActive = false;
	}

	void GetLeaderboard() {
		/*StartCoroutine(WebRequest.GET("/api/v1/leaderboard",
			(string result) => {
				ScoreDataCollection sdc = JsonUtility.FromJson<ScoreDataCollection>("{\"scoreData\":" + result + "}");
				leaderboard = sdc.scoreData.ToList();
				CreateLeaderboard();
			},
			(string status) => {
				Debug.Log("Failed to get leaderboard: " + status);
			}));*/
	}

	/*public void SubmitScore(string name, string email = null, string first_name = null, string last_name = null, string mobile = null, bool agree = false) {

		SubmitData submitData = new() {
			name = name,
			first_name = first_name,
			last_name = last_name,
			email = email,
			mobile = mobile,
			agree_terms = agree,
			device_id = SessionManager.Instance.ID,
			game_id = SessionManager.Instance.previous_game_id
		};

		string jsonString = JsonUtility.ToJson(submitData);

		StartCoroutine(
			WebRequest.POST("/api/v1/leaderboard/" + SessionManager.Instance.ID, jsonString,
			(string response) => { *//*GetLeaderboard();*//* },
			(long status) => { Debug.Log("Failed to submit: " + status); })
			);

	}*/
}

class SubmitData {
	public string game_secret = WebRequest.SECRET;
	public string name;
	public string game_id;
	public string device_id;
	public string email = null;
	public string first_name = null;
	public string last_name = null;
	public string mobile = null;
	public bool agree_terms = false;
}

[System.Serializable]
class ScoreDataCollection {
	public ScoreData[] scoreData;
}

[System.Serializable]
struct ScoreData {
	public string name;
	public int portfolio_value;
}