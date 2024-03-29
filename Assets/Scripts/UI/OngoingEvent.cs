using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OngoingEvent : MonoBehaviour
{
	private Image imageComponent;
	private TextMeshProUGUI titleComponent;
	private TextMeshProUGUI roundComponent;

	private CanvasGroup canvasGroup;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();

		imageComponent = transform.Find("Image").GetComponent<Image>();
		titleComponent = transform.Find("Title").GetComponent<TextMeshProUGUI>();
		roundComponent = transform.Find("RoundsLeft").GetComponent<TextMeshProUGUI>();
	}

	private void FixedUpdate()
	{
		if (GameManager.Instance.lastingEvent != null)
		{
			canvasGroup.alpha = 1;
			imageComponent.sprite = TowerColorUtils.GetIsoIconSprite[GameManager.Instance.lastingEvent.AffectedGroup];

			titleComponent.text = GameManager.Instance.lastingEvent.Title;
			if (GameManager.Instance.lastingEvent.Duration - 1 == 0)
			{
				roundComponent.text = string.Format("Last Turn");
			}
			else
			{
				roundComponent.text = string.Format("{0} Turn{1} Left", GameManager.Instance.lastingEvent.Duration, GameManager.Instance.lastingEvent.Duration != 1 ? "s" : "");
			}

		}
		else
		{
			canvasGroup.alpha = 0;
		}
	}
}
