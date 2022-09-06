using TMPro;
using UnityEngine;

public class ProfitText : MonoBehaviour
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
		if (gameManager.profits < 0)
		{
			text += "-";
		}
		text += "$" + Mathf.Abs(gameManager.profits).ToString("N0");
		textComponent.text = text;
	}
}
