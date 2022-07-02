using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAssetTextManager : MonoBehaviour
{
	public static NewAssetTextManager Instance { get; private set; }

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
