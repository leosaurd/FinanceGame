using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXButton : MonoBehaviour
{
	public void ToggleMute()
	{
		SettingsManager.Instance.sfx = !SettingsManager.Instance.sfx;
	}
}
