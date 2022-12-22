using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
	public GameObject Register;
	public void Start() {
		StartCoroutine(WebRequest.GET("/api/v1/competition", (string response) => {
			CompetitionAlert.Instance.DisplayCompetition(response);
		}, (long status) => {
			if (status == 404) {
				if (!PlayerInfoHelper.IsRegistered()) {
					Register.SetActive(true);
				}
			}
		}));
	}

	public void StartGame() {
		SceneManager.LoadScene("Game");
		SessionManager.Instance.StartSession();
	}
}
