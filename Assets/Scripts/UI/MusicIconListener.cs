using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicIconListener : MonoBehaviour
{
	private Image image;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	private void FixedUpdate()
	{
		image.sprite = SettingsManager.Instance.soundSprites[SettingsManager.Instance.music == false ? 0 : 1];
	}
}
