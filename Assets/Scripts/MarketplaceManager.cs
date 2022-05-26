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

		RefreshShop();
	}

	public List<BlockStats> shop = new();

	public void RefreshShop()
	{
		// Generate new blocks for the shop
	}
}
