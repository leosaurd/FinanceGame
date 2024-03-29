using TMPro;
using UnityEngine;

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
		//Updated the thing to match the stability/portfolio value warning.
		if (gameManager.Stability < -0.7 || gameManager.portfolioValue - (250 * StaticBlockStats.roundScaling) < (250 * StaticBlockStats.roundScaling))
		{
			warningObj.gameObject.SetActive(true);
		}
		else
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
