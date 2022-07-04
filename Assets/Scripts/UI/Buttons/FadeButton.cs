using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using static UnityEngine.UI.Button;

public class FadeButton : MonoBehaviour, Button
{
	float opacity = 0;
	float displayOpacity = 0;
	private Image image;
	public ButtonClickedEvent Actions;

	void Awake()
	{
		image = GetComponent<Image>();
	}

	void FixedUpdate()
	{
		// Animate the motion of the slider
		float diff = opacity - displayOpacity;

		displayOpacity += diff / 10;

		if (Mathf.Abs(diff) < 0.005) displayOpacity = opacity;

		image.color = new Color(1, 1, 1, displayOpacity);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		opacity = 1;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		opacity = 0;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Actions.Invoke();
	}
}
