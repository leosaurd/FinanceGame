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

	public Block[] blocks =
	{
		new Block(BlockType.Insurance, new IntRange(1000, 5000), new IntRange(-500, 50), new FloatRange(0.05f, 0.2f)),
		new Block(BlockType.Risky, new IntRange(50, 1000), new IntRange(100, 500), new FloatRange(-0.05f, -0.3f)),
	};
}
