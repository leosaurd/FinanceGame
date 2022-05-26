using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectManager : MonoBehaviour
{
	private static StaticObjectManager instance;

	public static StaticObjectManager GetInstance()
	{
		return instance;
	}

	void Awake()
	{
		if (!instance)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

}
