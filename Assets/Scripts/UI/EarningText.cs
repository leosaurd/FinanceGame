using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarningText : MonoBehaviour
{
	private TextMeshProUGUI textComponent;
	private GameManager gameManager;

	void Awake()
	{
		textComponent = GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		gameManager = GameManager.Instance;
	}

	void Update()
	{
		string text = "";
		if (gameManager.totalEarnings < 0)
		{
			text += "-";
		}
		text += "EARNINGS:\n$" + Mathf.Abs(gameManager.totalEarnings).ToString("N0");
		textComponent.text = text;
	}
}
