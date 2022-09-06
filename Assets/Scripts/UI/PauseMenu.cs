using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public static PauseMenu Instance { get; private set; }

	private CanvasGroup canvasGroup;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		canvasGroup = GetComponent<CanvasGroup>();
	}


	IEnumerator FadeIn()
	{
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;

		int steps = 30;
		for (int i = 0; i <= steps; i++)
		{
			canvasGroup.alpha = i / (float)steps;
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator FadeOut()
	{
		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;

		int steps = 30;
		for (int i = steps; i >= 0; i--)
		{
			canvasGroup.alpha = i / (float)steps;
			yield return new WaitForFixedUpdate();
		}

		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;
	}

	public void Open()
	{
		StartCoroutine(FadeIn());
	}

	public void OnClose()
	{
		StartCoroutine(FadeOut());
	}

	public void OpenTutorial()
	{

	}
}
