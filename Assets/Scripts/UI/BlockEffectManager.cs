using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEffectManager : MonoBehaviour
{
	public static BlockEffectManager Instance { get; private set; }

	public GameObject prefab;

	private void Awake()
	{
		if (Instance == null) Instance = this;
	}

	public void NewEffect()
	{
		Instantiate(prefab, transform);
	}
}
