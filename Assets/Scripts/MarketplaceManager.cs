using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketplaceManager : MonoBehaviour
{
	private static MarketplaceManager instance;

	public static MarketplaceManager GetInstance()
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

	private void Start()
	{
		RefreshShop();
	}

	public List<BlockInstance> shop = new();

	public void RefreshShop()
	{
		StaticObjectManager SOM = StaticObjectManager.GetInstance();
		shop.Clear();
		foreach (Block b in SOM.blocks)
		{
			BlockInstance blockInstance = b.GenerateBlock();
			shop.Add(blockInstance);
		}
	}
}
