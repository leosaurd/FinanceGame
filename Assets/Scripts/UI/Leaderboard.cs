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

	public TextMeshProUGUI pageText;

	public GameObject leaderboardPrefab;

	LeaderboardData leaderboardData = new();
	int page = 1;

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

	public void NextPage() {
		if (page < leaderboardData.pageCount && page < 10) {
			page++;
			GetLeaderboard();
		}
	}

	public void PreviousPage() {
		if (page > 1) {
			page--;
			GetLeaderboard();
		}
	}


	void CreateLeaderboard() {
		scoreObjects.ForEach(o => { Destroy(o); });
		scoreObjects.Clear();
		int totalPageCount = leaderboardData.pageCount > 10 ? 10 : leaderboardData.pageCount;
		pageText.text = page + "/" + totalPageCount;
		loader.isActive = true;


		for (int i = 0; i < leaderboardData.leaderboard.Length; i++) {
			GameObject obj = Instantiate(leaderboardPrefab, transform);


			obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -25f * (i));

			int place = (page - 1) * 10 + i + 1;
			obj.transform.Find("Place").GetComponent<TextMeshProUGUI>().text = place.ToString();
			obj.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = leaderboardData.leaderboard[i].name;
			obj.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = "$" + (leaderboardData.leaderboard[i].portfolio_value).ToString("N0");
			scoreObjects.Add(obj);
		}


		loader.isActive = false;
	}

	void GetLeaderboard() {
		StartCoroutine(WebRequest.GET("/api/v1/leaderboard?page=" + page,
			(string result) => {
				leaderboardData = JsonUtility.FromJson<LeaderboardData>(result);
				CreateLeaderboard();
			},
			(long status) => {
				Debug.Log("Failed to get leaderboard: " + status);
			}));
	}
}

[System.Serializable]
class LeaderboardData {
	public ScoreData[] leaderboard;
	public int pageCount;
}

[System.Serializable]
struct ScoreData {
	public string name;
	public int portfolio_value;
}