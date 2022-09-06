using UnityEngine;

public class SFXButton : MonoBehaviour
{
	public void ToggleMute()
	{
		SettingsManager.Instance.sfx = !SettingsManager.Instance.sfx;
	}
}
