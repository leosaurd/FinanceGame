using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class CompetitionAlert : MonoBehaviour {

	public void Awake() {
		StartCoroutine(WebRequest.GET("/api/v1/competition/ongoing", (string response) => {
			Debug.Log(response);
			CompetitionInfo info = JsonUtility.FromJson<CompetitionInfo>(response);
			transform.GetChild(0).Find("Background").Find("Ends").GetComponent<TextMeshProUGUI>().text = "Ends\n" + info.end_date;
			transform.GetChild(0).Find("Background").Find("Description").GetComponent<TextMeshProUGUI>().text = info.details;
			transform.GetChild(0).gameObject.SetActive(true);
		}, (string status) => { }));
	}

	public void OpenURL(string url) {
		Application.OpenURL(url);
	}
}

struct CompetitionInfo {
	public string start_date;
	public string end_date;
	public string details;
}
