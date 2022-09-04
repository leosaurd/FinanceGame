using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketplaceUI : MonoBehaviour
{
	public static MarketplaceUI Instance { get; private set; }

	public ShopItem[] ShopItems;

	private int[] indexes = { 0, 1, 2, 3 };

	public T[] ReorderArray<T>(T[] arr)
	{
		T[] outArray = (T[])arr.Clone();
		for (int i = 0; i < arr.Length; i++)
		{
			int newLocation = Random.Range(0, arr.Length);
			(outArray[i], outArray[newLocation]) = (outArray[newLocation], outArray[i]);
		}
		return outArray;
	}

	private void Awake()
	{
		if (Instance == null) Instance = this;
	}

	void Start()
	{
		RefreshShop();
	}


	private List<BlockInstance> GenerateNewItems()
	{
		// Generate new items for the shop
		List<string> names = new();
		List<BlockInstance> blocks = new();

		for (int i = 0; i < 4; i++)
		{
			BlockType blockType = (BlockType)Random.Range(0, 3);
			string name = NameGenerator.GenerateName(blockType);

			//Lazymans method for non-repeating names
			while (names.Contains(name))
			{
				blockType = (BlockType)Random.Range(0, 3);
				name = NameGenerator.GenerateName(blockType);
			}
			names.Add(name);
			blocks.Add(new BlockInstance(name, blockType, StaticObjectManager.BlockStats[name]));
		}
		return blocks;
	}

	private void CreateShopGameobjects(List<BlockInstance> blocks)
	{
		bool canBuySomething = false;
		bool canBuyStability = false;
		bool canBuyCost = false;

		int[] colorIndexes = ReorderArray(indexes);

		for (int i = 0; i < ShopItems.Length; i++)
		{
			BlockInstance instance = blocks[i];
			instance.towerColor = (TowerColor)colorIndexes[i];
			ShopItems[i].Refresh(instance);

			if (instance.cost < GameManager.Instance.portfolioValue && GameManager.Instance.Stability + instance.stability > -1) canBuySomething = true;
			if (instance.cost < GameManager.Instance.portfolioValue) canBuyCost = true;
			if (GameManager.Instance.Stability + instance.stability > -1) canBuyStability = true;
		}

		if (!canBuySomething)
		{
			if (canBuyStability)
			{
				GameManager.Instance.EndGame(GameOverReason.Poor);
			}
			else if (canBuyCost)
			{
				GameManager.Instance.EndGame(GameOverReason.Stability);
			}
			else
			{
				GameManager.Instance.EndGame(GameOverReason.Stability);
			}
		}
	}


	public void RefreshShop()
	{
		List<BlockInstance> blocks = GenerateNewItems();
		CreateShopGameobjects(blocks);
	}

	public void Buy(BlockInstance block)
	{
		GameManager GM = GameManager.Instance;
		GM.profits += block.profit;
		GM.portfolioValue -= block.cost;
		GM.Stability += block.stability;
		GM.AddBlock(block);
	}


}
