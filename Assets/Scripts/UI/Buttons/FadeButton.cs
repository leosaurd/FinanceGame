using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class FadeButton : MonoBehaviour, Button
{
	float opacity = 0;
	float displayOpacity = 0;
	public float maxOpacity = 1;
	private Image image;
	public ButtonClickedEvent Actions;
	public BlockInstance block;

	void Awake()
	{
		image = GetComponent<Image>();
	}

	void FixedUpdate()
	{
		// Animate the motion of the slider
		float diff = opacity - displayOpacity;

		displayOpacity += diff / 10;

		if (Mathf.Abs(diff) < 0.005) displayOpacity = opacity;

		image.color = new Color(1, 1, 1, displayOpacity);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ValueWarningScript.Instance.PointerEnter(block);
		opacity = maxOpacity;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ValueWarningScript.Instance.PointerExit();
		opacity = 0;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Actions.Invoke();
	}
}
