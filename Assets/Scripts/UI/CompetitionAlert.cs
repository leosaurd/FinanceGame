using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class CompetitionAlert : MonoBehaviour {
	public static CompetitionAlert Instance { get; private set; }
	public GameObject Register;

	public void Awake() {
		Instance = this;
	} 

	public void DisplayCompetition(string response) {
		CompetitionInfo info = JsonUtility.FromJson<CompetitionInfo>(response);
		transform.GetChild(0).Find("Background").Find("Ends").GetComponent<TextMeshProUGUI>().text = "Ends\n" + info.end_date;
		transform.GetChild(0).Find("Background").Find("Description").GetComponent<TextMeshProUGUI>().text = info.details;
		transform.GetChild(0).Find("Background").Find("Title").GetComponent<TextMeshProUGUI>().text = info.title;
		transform.GetChild(0).gameObject.SetActive(true);
	}

	public void CloseCompetition() {
		transform.GetChild(0).gameObject.SetActive(true);
		if (!PlayerInfoHelper.IsRegistered()) {
			Register.SetActive(true);
		}
	}

	

	public void OpenURL(string url) {
		Application.OpenURL(url);
	}
}

struct CompetitionInfo {
	public string start_date;
	public string end_date;
	public string details;
	public string title;
}
