using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour {
	public static GameOver Instance {
		get; private set;
	}

	private static TextMeshProUGUI reasonComponent;
	private static TextMeshProUGUI[] scoreComponents;

	private static readonly Dictionary<GameOverReason, string> gameoverMessages = new()
	{
		{GameOverReason.Stability, "Your portfolio is too unstable." },
		{GameOverReason.Poor, "You cannot afford to expand your portfolio." }
	};

	private void Awake() {
		if (Instance == null) {
			Instance = this;

			reasonComponent = transform.Find("Stage1").Find("Reason").GetComponent<TextMeshProUGUI>();
			scoreComponents = new TextMeshProUGUI[3];
			scoreComponents[0] = transform.Find("Registered").Find("Main").Find("Score").GetComponent<TextMeshProUGUI>();
			scoreComponents[1] = transform.Find("Not Registered").Find("Main").Find("Score").GetComponent<TextMeshProUGUI>();
			scoreComponents[2] = transform.Find("Register").Find("Container").Find("Info").Find("Score").GetComponent<TextMeshProUGUI>();
		}
		else {
			Destroy(gameObject);
		}
	}

	public void ShowGameover(GameOverReason reason) {
		GameManager gm = GameManager.Instance;
		reasonComponent.text = gameoverMessages[reason];
		foreach (TextMeshProUGUI v in scoreComponents) {
			v.text = "$" + gm.totalEarnings.ToString("N0");
		}

		StartCoroutine(FadeIn());
	}

	public void Stage1Next() {
		/*if (!PlayerInfoHelper.IsRegistered()) {
			transform.Find("Stage1").gameObject.SetActive(false);
			transform.Find("Register").gameObject.SetActive(true);
		}*/
		

		PlayerInfo playerInfo = PlayerInfoHelper.GetPlayerInfo();
		string playerID = playerInfo.playerID;
		string displayName = playerInfo.displayName;
		string first_name = playerInfo.firstName;
		string last_name = playerInfo.lastName;
		string email = playerInfo.email;
		string mobile = playerInfo.mobile;
		string deviceID = SessionManager.Instance.ID;
		string gameID = SessionManager.Instance.Session.game_id;
		string secret = WebRequest.SECRET;
		string json = "{\"player_id\":\"" + playerID + "\",\"display_name\":\"" + displayName + "\",\"first_name\":\"" + first_name + "\",\"last_name\":\"" + last_name + "\",\"email\":\"" + email + "\",\"mobile\":\"" + mobile + "\",\"deviceID\":\"" + deviceID + "\",\"gameID\":\"" + gameID + "\",\"secret\":\"" + secret + "\"}";

		StartCoroutine(WebRequest.POST("/api/v2/leaderboard", json,
			(string response) => {
				LoadRegistered();
				transform.Find("Registered").Find("Main").Find("Message").GetComponent<TextMeshProUGUI>().text = "Your score has been submitted!";
			},
			(long status) => {
				LoadRegistered();
				transform.Find("Registered").Find("Main").Find("Message").GetComponent<TextMeshProUGUI>().text = "Failed to submit your score";
			}
		));
	}

	private void LoadRegistered() {
		transform.Find("Stage1").gameObject.SetActive(false);
		transform.Find("Registered").gameObject.SetActive(true);
	}

	IEnumerator FadeIn() {
		CanvasGroup canvasGroup = transform.Find("Stage1").GetComponent<CanvasGroup>();
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;

		int steps = 30;
		for (int i = 0; i <= steps; i++) {
			canvasGroup.alpha = i / (float)steps;
			yield return new WaitForFixedUpdate();
		}
	}
}
