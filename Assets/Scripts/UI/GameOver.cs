using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class GameOver : MonoBehaviour
{
	public static GameOver Instance { get; private set; }

	private static TextMeshProUGUI reasonComponent;
	private static TextMeshProUGUI scoreComponent;

	private static readonly Dictionary<GameOverReason, string> gameoverMessages = new()
	{
		{GameOverReason.Stability, "Your portfolio is too unstable." },
		{GameOverReason.Poor, "You cannot afford to expand your portfolio." }
	};

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			reasonComponent = transform.Find("Stage1").Find("Reason").GetComponent<TextMeshProUGUI>();
			scoreComponent = transform.Find("Stage2").Find("Main").Find("GameOverSection").Find("Score").GetComponent<TextMeshProUGUI>();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void ShowGameover(GameOverReason reason)
	{
		GameManager gm = GameManager.Instance;
		reasonComponent.text = gameoverMessages[reason];
		scoreComponent.text = "$" + gm.totalEarnings.ToString("N0");

		if (Leaderboard.Instance.IsTop5())
		{
			transform.Find("Stage2").Find("Main").Find("GameOverSection").Find("SubmitScoreBtn").gameObject.SetActive(true);
		}
		else
		{
			transform.Find("Stage2").Find("Main").Find("GameOverSection").Find("SubmitScoreBtn").gameObject.SetActive(false);
		}

		StartCoroutine(FadeIn());
	}

	IEnumerator FadeIn()
	{
		CanvasGroup canvasGroup = transform.Find("Stage1").GetComponent<CanvasGroup>();
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;

		int steps = 30;
		for (int i = 0; i <= steps; i++)
		{
			canvasGroup.alpha = i / (float)steps;
			yield return new WaitForFixedUpdate();
		}
	}
}
