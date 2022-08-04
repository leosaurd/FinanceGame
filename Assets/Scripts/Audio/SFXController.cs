using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
	AudioSource audioSource;

	private float volume;

	public float delay;

	public int InRowCount;
	public bool prevRiskIncrease;
	public bool overridePitch;

	void Start()
	{
		if (SettingsManager.Instance.sfx == false)
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);

		audioSource = GetComponent<AudioSource>();

		float pitch = Random.Range(0.95f, 1.05f);
		float volumeChange = Random.Range(-0.05f, 0.05f);
		volume = 0.25f + volumeChange;



		if (overridePitch)
		{
			if (prevRiskIncrease)
			{
				pitch = 1f - (InRowCount / 30f);
				volume = 0.10f;
			}
			else
			{
				pitch = 1f + (InRowCount / 30f);
				volume = 0.10f;
			}

		}

		audioSource.pitch = pitch;

		audioSource.PlayDelayed(delay);
		StartCoroutine(Kill());
	}

	private void FixedUpdate()
	{
		if (SettingsManager.Instance.sfx)
		{
			audioSource.volume = volume;
		}
		else
		{
			audioSource.volume = 0f;
		}
	}

	IEnumerator Kill()
	{
		yield return new WaitWhile(() => audioSource.isPlaying);
		Destroy(gameObject);
	}

}
