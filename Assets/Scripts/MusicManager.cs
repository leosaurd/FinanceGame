using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public static MusicManager Instance { get; private set; }

	public AudioSource audioSource;
	private float volume = 0.25f;
	// Start is called before the first frame update
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			audioSource = GetComponent<AudioSource>();
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (SettingsManager.Instance.music)
		{
			audioSource.volume = volume;
		}
		else
		{
			audioSource.volume = 0f;
		}
	}
}
