using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		new Block(BlockType.Insurance, new IntRange(-1000, -200), new IntRange(50, 500), new FloatRange(0.05f, 0.2f)),
		new Block(BlockType.Risky, new IntRange(50, 500), new IntRange(100, 500), new FloatRange(-0.3f, -0.1f)),
		new Block(BlockType.Neutral, new IntRange(50, 200), new IntRange(50, 250), new FloatRange(-0.15f, 0.15f)),
		new Block(BlockType.Valuable, new IntRange(200, 1000), new IntRange(50, 500), new FloatRange(-0.2f, 0.05f)),
	};
}
