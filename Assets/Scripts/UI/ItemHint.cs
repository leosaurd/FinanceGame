using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHint : MonoBehaviour
{
	private static RectTransform nameRect;
	private static RectTransform costRect;
	private static RectTransform profitRect;
	private static RectTransform stabilityRect;


	private void Awake()
	{
		nameRect = transform.Find("Name").GetComponent<RectTransform>();
		costRect = transform.Find("Cost").GetComponent<RectTransform>();
		profitRect = transform.Find("Profit").GetComponent<RectTransform>();
		stabilityRect = transform.Find("Stability").GetComponent<RectTransform>();
	}

	public static void UpdatePositions(float unit)
	{
		nameRect.anchoredPosition = new Vector2(unit * 2, nameRect.anchoredPosition.y);
		costRect.anchoredPosition = new Vector2(unit * 10, costRect.anchoredPosition.y);
		profitRect.anchoredPosition = new Vector2(unit * 15f, profitRect.anchoredPosition.y);
		stabilityRect.anchoredPosition = new Vector2(unit * 20f, stabilityRect.anchoredPosition.y);
	}
}
