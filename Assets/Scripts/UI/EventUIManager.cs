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

	public Dictionary<EventType, EventUIPreset> presets = new();

#nullable enable
	private Action? okCallback;


	private bool overrideFadeOut = false;

	public Transform blockObject;



	private void Awake()
	{
		if (Instance == null) Instance = this;

		canvasGroup = GetComponent<CanvasGroup>();
		Body = transform.Find("Body");
		blockObject = Body.Find("EventBlock");

		LoadPresets();
	}

	public void LoadPresets()
	{
		presets.Add(EventType.CostIncrease, new EventUIPreset(Resources.Load<Sprite>("EventMultiplier"), new Color32(221, 114, 27, 255), new Color32(244, 138, 59, 255)));
		presets.Add(EventType.ProfitIncrease, new EventUIPreset(Resources.Load<Sprite>("EventMultiplier"), new Color32(221, 114, 27, 255), new Color32(244, 138, 59, 255)));
		presets.Add(EventType.CostDecrease, new EventUIPreset(Resources.Load<Sprite>("EventDivider"), new Color32(0, 146, 69, 255), new Color32(49, 175, 100, 255)));
		presets.Add(EventType.BlockNullification, new EventUIPreset(Resources.Load<Sprite>("EventNullification"), new Color32(94, 192, 233, 255), new Color32(148, 213, 240, 255)));
		presets.Add(EventType.BlockAddition, new EventUIPreset(Resources.Load<Sprite>("EventAddition"), new Color32(0, 113, 187, 255), new Color32(94, 192, 233, 255)));
		presets.Add(EventType.BlockRemoval, new EventUIPreset(Resources.Load<Sprite>("EventRemoval"), new Color32(0, 113, 187, 255), new Color32(94, 192, 233, 255)));
	}

	public void ShowEvent(GameEvent gameEvent, Action? callback)
	{
		if (gameEvent.IsBeneficial)
		{
			SFXManager.Instance.PlaySFX(SFX.beneficialEvent, 0.25f);
		}
		else
		{
			SFXManager.Instance.PlaySFX(SFX.harmfulEvent, 0.25f);
		}

		EventUIPreset eventPreset = presets[gameEvent.Type];
		if (gameEvent is InstantEvent)
		{
			overrideFadeOut = true;
		}
		else
		{
			overrideFadeOut = false;
		}

		Body.Find("Background").GetComponent<Image>().sprite = eventPreset.background;

		Body.Find("Title").GetComponent<TextMeshProUGUI>().text = gameEvent.Title.ToUpper();

		Body.Find("Description").GetComponent<TextMeshProUGUI>().text = gameEvent.Description.Replace("__", string.Format("#{0}", ColorUtility.ToHtmlStringRGB(eventPreset.buttonColor)));

		Color glowColor = eventPreset.buttonColor;
		glowColor.a = 0.7f;

		Body.Find("OkButton").Find("Glow").GetComponent<Image>().color = glowColor;

		Body.Find("OkButton").GetComponent<ColorButton>().defaultColor = eventPreset.buttonColor;
		Body.Find("OkButton").GetComponent<ColorButton>().targetColor = eventPreset.buttonColor;
		Body.Find("OkButton").GetComponent<ColorButton>().hoveredColor = eventPreset.buttonHoveredColor;

		okCallback = callback;

		StartCoroutine(FadeIn());
	}


	private void FixedUpdate()
	{
		if (GameManager.Instance.IsGameOver)
		{
			blockObject.gameObject.SetActive(false);
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
			canvasGroup.alpha = 0;
		}
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
		blockObject.gameObject.SetActive(false);
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




	public void OnClose()
	{
		if (overrideFadeOut)
		{
			overrideFadeOut = false;

			blockObject.gameObject.SetActive(true);
			Body.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>("EventBlank");
			Body.Find("Title").GetComponent<TextMeshProUGUI>().text = "";
			Body.Find("Description").GetComponent<TextMeshProUGUI>().text = "";
		}
		else
		{
			StartCoroutine(FadeOut());
		}
	}

}

[System.Serializable]
public class EventUIPreset
{
	public Sprite background;
	public Color buttonColor;
	public Color buttonHoveredColor;


	public EventUIPreset(Sprite background, Color buttonColor, Color buttonHoveredColor)
	{
		this.background = background;
		this.buttonColor = buttonColor;
		this.buttonHoveredColor = buttonHoveredColor;
	}
}