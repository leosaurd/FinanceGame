using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class ColorButton : MonoBehaviour, Button
{
	public Color defaultColor;
	public Color hoveredColor;
	private Color currentColor;
	public Color targetColor;

	public bool onCooldown = false;

	private Image image;
	public ButtonClickedEvent Actions;

	void Awake()
	{
		image = GetComponentInChildren<Image>();
		currentColor = defaultColor;
		targetColor = defaultColor;
	}

	void FixedUpdate()
	{
		float rDiff = targetColor.r - currentColor.r;
		float gDiff = targetColor.g - currentColor.g;
		float bDiff = targetColor.b - currentColor.b;

		float speed = 5;

		if (Mathf.Abs(rDiff + gDiff + bDiff) < 0.05) currentColor = targetColor;

		currentColor = new Color(currentColor.r + rDiff / speed, currentColor.g + gDiff / speed, currentColor.b + bDiff / speed);

		image.color = currentColor;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		targetColor = hoveredColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		// Return to normal
		targetColor = defaultColor;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!onCooldown)
		{
			onCooldown = true;
			StartCoroutine(Cooldown());
			Actions.Invoke();
		}
	}

	IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(0.25f);
		onCooldown = false;
	}
}
