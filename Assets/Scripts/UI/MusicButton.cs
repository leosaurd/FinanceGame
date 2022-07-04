using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButton : MonoBehaviour
{
	public void ToggleMute()
	{
		SettingsManager.Instance.music = !SettingsManager.Instance.music;
	}
}
