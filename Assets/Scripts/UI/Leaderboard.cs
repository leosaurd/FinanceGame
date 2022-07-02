using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Leaderboard : MonoBehaviour
{
	public static Leaderboard Instance { get; private set; }
	readonly string secret = "b7c97be8-cf53-4500-9c27-7d262fd07510";
	Loader loader;

	public GameObject leaderboardPrefab;

	List<ScoreData> scores = new();
	List<GameObject> scoreObjects = new();

	private void Awake()
	{
		if (!Instance) Instance = this;
		loader = GetComponentInChildren<Loader>();
	}


	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(GetLeaderboard());
	}

	public bool IsTop5()
	{
		if (scores.Count < 5) return true;
		return GameManager.Instance.totalEarnings > scores[scores.Count - 1].score;
	}

	void CreateLeaderboard()
	{
		scoreObjects.ForEach(o => { Destroy(o); });
		scoreObjects.Clear();
		loader.isActive = true;

		for (int i = 0; i < scores.Count; i++)
		{
			GameObject obj = Instantiate(leaderboardPrefab, transform);


			obj.GetComponent<RectTransform>().localPosition = new Vector3(0, 82.5f - 32.5f * (i), 0);

			obj.transform.Find("Place").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
			obj.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = scores[i].name;
			obj.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = "$" + (scores[i].score).ToString("N0");
			scoreObjects.Add(obj);
		}


		loader.isActive = false;
	}

	IEnumerator GetLeaderboard()
	{
		using UnityWebRequest webRequest = UnityWebRequest.Get("https://test.trentshailer.com/leaderboard");
		yield return webRequest.SendWebRequest();
		if (webRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(webRequest.error);
		}
		else
		{
			Debug.Log("Received: " + webRequest.downloadHandler.text);

			ScoreDataCollection sdc = JsonUtility.FromJson<ScoreDataCollection>("{\"scoreData\":" + webRequest.downloadHandler.text + "}");
			scores = sdc.scoreData.ToList();
			CreateLeaderboard();
		}
	}

	public void SubmitScore(string name)
	{
		StartCoroutine(Submit(name));
	}

	IEnumerator Submit(string name)
	{
		WWWForm form = new WWWForm();
		form.AddField("secret", secret);
		form.AddField("name", name);
		form.AddField("score", GameManager.Instance.totalEarnings);

		using UnityWebRequest webRequest = UnityWebRequest.Post("https://test.trentshailer.com/leaderboard/add", form);

		yield return webRequest.SendWebRequest();

		if (webRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(webRequest.error);
		}
		else
		{
			Debug.Log("Successful score submission");
			StartCoroutine(GetLeaderboard());
		}
	}
}
[System.Serializable]
class ScoreDataCollection
{
	public ScoreData[] scoreData;
}

[System.Serializable]
struct ScoreData
{
	public string score_id;
	public string name;
	public int score;
}