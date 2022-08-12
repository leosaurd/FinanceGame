using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueText : MonoBehaviour
{
	private TextMeshProUGUI textComponent;
	private GameManager gameManager;
	//adding in warning
	public Transform warningObj;

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
		//Lazy adding in of warning atm - need to discuss parameters.
		if(gameManager.portfolioValue < 7000)
        {
			warningObj.gameObject.SetActive(true);
        } else
        {
			warningObj.gameObject.SetActive(false);
		}
		if (gameManager.portfolioValue < 0)
		{
			text += "-";
		}
		text += "$" + Mathf.Abs(gameManager.portfolioValue).ToString("N0");
		textComponent.text = text;
	}
}
