using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilityMeter : MonoBehaviour
{
	private const int maxY = 225;

	private GameManager gameManager;

	public float displayValue = 0;

	private void Start()
	{
		gameManager = GameManager.Instance;
	}



	void FixedUpdate()
	{
		// Animate the motion of the slider
		float diff = gameManager.stability - displayValue;

		displayValue += diff / 20;

		if (Mathf.Abs(diff) < 0.005) displayValue = gameManager.stability;

		transform.localPosition = new Vector2(transform.localPosition.x, maxY * displayValue);
	}
}
