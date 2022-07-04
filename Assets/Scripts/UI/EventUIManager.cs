using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUIManager : MonoBehaviour
{
	public static EventUIManager Instance { get; private set; }

	private CanvasGroup canvasGroup;
	private Transform Body;

	[SerializeField]
	public EventUIPreset[] presets;

	private Action okCallback;

	private void Awake()
	{
		if (Instance == null) Instance = this;

		canvasGroup = GetComponent<CanvasGroup>();
		Body = transform.Find("Body");
	}
	public void ShowEvent(string Title, string Description)
	{
		int eventPresetIndex = UnityEngine.Random.Range(0, 3);
		EventUIPreset eventPreset = presets[eventPresetIndex];

		Body.Find("Background").GetComponent<Image>().sprite = eventPreset.image;

		Body.Find("Title").GetComponent<TextMeshProUGUI>().text = Title.ToUpper();
		Body.Find("Title").GetComponent<TextMeshProUGUI>().color = eventPreset.textColor;
		Body.Find("Description").GetComponent<TextMeshProUGUI>().text = Description;
		Body.Find("Description").GetComponent<TextMeshProUGUI>().color = eventPreset.textColor;

		Body.Find("OkButton").GetComponent<ColorButton>().defaultColor = eventPreset.buttonColor;
		Body.Find("OkButton").GetComponent<ColorButton>().hoveredColor = eventPreset.buttonHoveredColor;

		okCallback = null;

		StartCoroutine(FadeIn());
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
		okCallback?.Invoke();
	}

	public void ShowEvent(string Title, string Description, Action callback)
	{
		ShowEvent(Title, Description);
		okCallback = callback;
	}

	public void OnClose()
	{
		StartCoroutine(FadeOut());
	}

}

[System.Serializable]
public class EventUIPreset
{
	public Sprite image;
	public Color textColor;
	public Color buttonColor;
	public Color buttonHoveredColor;

	public EventUIPreset(Sprite image, Color textColor, Color buttonColor, Color buttonHoveredColor)
	{
		this.image = image;
		this.textColor = textColor;
		this.buttonColor = buttonColor;
		this.buttonHoveredColor = buttonHoveredColor;
	}
}