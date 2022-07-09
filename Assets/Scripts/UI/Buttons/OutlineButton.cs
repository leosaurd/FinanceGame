using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using static UnityEngine.UI.Button;

public class OutlineButton : MonoBehaviour, Button
{
	float fill = 0;
	float displayFill = 0;
	private Image image;
	public ButtonClickedEvent Actions;

	void Awake()
	{
		image = GetComponentInChildren<Image>();
	}

	void FixedUpdate()
	{
		// Animate the motion of the slider
		float diff = fill - displayFill;

		float speedMod = 1;
		if ((displayFill > 0.13 && displayFill < 0.37) || (displayFill > 0.63 && displayFill < 0.87))
		{
			speedMod = 2;
		}

		displayFill += (diff / 15) * speedMod;

		if (Mathf.Abs(diff) < 0.005) displayFill = fill;

		image.fillAmount = displayFill;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		fill = 1;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		fill = 0;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Actions.Invoke();
	}
}
