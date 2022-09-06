using UnityEngine;
using UnityEngine.UI;

public class SFXIconListener : MonoBehaviour
{
	private Image image;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	private void FixedUpdate()
	{
		image.sprite = SettingsManager.Instance.soundSprites[SettingsManager.Instance.sfx == false ? 0 : 1];
	}
}
