using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
	public static SettingsManager Instance { get; private set; }

	public bool music = true;
	public bool sfx = true;


	public Sprite[] soundSprites;

	private void Awake()
	{
		if (Instance == null) Instance = this;
	}


}
