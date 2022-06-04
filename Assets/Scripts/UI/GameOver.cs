using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class GameOver : MonoBehaviour
{
	public static GameOver Instance { get; private set; }

    private static TextMeshProUGUI message;

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

			message = transform.GetChild(0).Find("Message").GetComponent<TextMeshProUGUI>();
			
		}
		else
		{
			Destroy(gameObject);
		}
		
	}

	public void ShowGameover(GameOverReason reason)
	{
		message.text = gameoverMessages[reason];
		// Leaderboard stuffs?
		StartCoroutine(FadeIn());
	}

	IEnumerator FadeIn()
	{
		CanvasGroup canvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;

		int steps = 20;
		for (int i = 0; i <= steps; i++)
		{
			canvasGroup.alpha = i / (float)steps;
			yield return new WaitForFixedUpdate();
		}
	}
}
