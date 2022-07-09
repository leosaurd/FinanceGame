using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StabilityMeter : MonoBehaviour
{
	private const int maxY = 130;

	private GameManager gameManager;

	public Gradient stabilityTextGradient;

	private Transform meter;
	private Image warningImage;
	private TextMeshProUGUI stabilityText;

	public float displayValue = 0;

	private void Awake()
	{
		meter = transform.Find("Meter");
		stabilityText = transform.Find("ValueText").GetComponent<TextMeshProUGUI>();
		warningImage = transform.Find("Warning").GetComponent<Image>();
	}

	private void Start()
	{
		gameManager = GameManager.Instance;
	}



	void FixedUpdate()
	{
		// Animate the motion of the slider
		float diff = gameManager.Stability - displayValue;

		displayValue += diff / 20;

		if (Mathf.Abs(diff) < 0.005) displayValue = gameManager.Stability;

		meter.localPosition = new Vector2(meter.localPosition.x, maxY * displayValue - 9.5f);

		if (displayValue < -0.33f)
			stabilityText.text = "Low";
		else if (displayValue < 0.33f)
			stabilityText.text = "Average";
		else
			stabilityText.text = "High";

		if (displayValue < -0.5f)
			warningImage.enabled = true;
		else
			warningImage.enabled = false;

		stabilityText.color = stabilityTextGradient.Evaluate((displayValue + 1) / 2);

	}
}
