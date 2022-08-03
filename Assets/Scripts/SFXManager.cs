using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
	public static SFXManager Instance { get; private set; }

	private Dictionary<SFX, AudioClip> audioMap = new();

	private GameObject AudioSource;

	private bool prevRiskIncrease = false;
	private int InRowCount = 0;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);

			AudioSource = (GameObject)Resources.Load("Audio Source");

			audioMap.Add(SFX.beneficialEvent, (AudioClip)Resources.Load("beneficial events"));
			audioMap.Add(SFX.blockDrop, (AudioClip)Resources.Load("block drop"));
			audioMap.Add(SFX.button, (AudioClip)Resources.Load("button"));
			audioMap.Add(SFX.decreaseRisk, (AudioClip)Resources.Load("decrease risk"));
			audioMap.Add(SFX.gameOver, (AudioClip)Resources.Load("game over"));
			audioMap.Add(SFX.harmfulEvent, (AudioClip)Resources.Load("harmful events"));
			audioMap.Add(SFX.increaseRisk, (AudioClip)Resources.Load("increase risk"));
		}
		else Destroy(gameObject);
	}

	public void PlaySFX(SFX sfx, float delay)
	{
		GameObject audioSource = Instantiate(AudioSource, null);
		audioSource.GetComponent<AudioSource>().clip = audioMap[sfx];
		audioSource.GetComponent<SFXController>().delay = delay;

		if (sfx == SFX.increaseRisk)
		{
			if (prevRiskIncrease)
			{
				InRowCount++;
			}
			else
			{
				prevRiskIncrease = true;
				InRowCount = 0;
			}
			audioSource.GetComponent<SFXController>().InRowCount = InRowCount;
			audioSource.GetComponent<SFXController>().prevRiskIncrease = prevRiskIncrease;
			audioSource.GetComponent<SFXController>().overridePitch = true;

		}
		else if (sfx == SFX.decreaseRisk)
		{
			if (prevRiskIncrease)
			{
				prevRiskIncrease = false;
				InRowCount = 0;
			}
			else
			{
				InRowCount++;
			}
			audioSource.GetComponent<SFXController>().InRowCount = InRowCount;
			audioSource.GetComponent<SFXController>().prevRiskIncrease = prevRiskIncrease;
			audioSource.GetComponent<SFXController>().overridePitch = true;
		}

	}

	public void PlaySFX(SFX sfx)
	{
		PlaySFX(sfx, 0);
	}
}
public enum SFX
{
	beneficialEvent,
	blockDrop,
	button,
	decreaseRisk,
	gameOver,
	harmfulEvent,
	increaseRisk
}