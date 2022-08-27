using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundText : MonoBehaviour
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
		if (gameManager.ownedBlocks.Count < 0)
		{
			text += "-";
		}
		text += "Round " + Mathf.Abs(gameManager.ownedBlocks.Count).ToString("N0");
		textComponent.text = text;
	}
}
