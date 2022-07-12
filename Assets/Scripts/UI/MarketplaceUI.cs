using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketplaceUI : MonoBehaviour
{
	public static MarketplaceUI Instance { get; private set; }

	public GameObject ShopItemPrefab;

	public Transform[] ShopItemLocations;

	private List<GameObject> shop = new();

	public int[] indexes = { 0, 1, 2, 3 };

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

	private void ClearShop()
	{
		// Clear old shop
		for (int i = 0; i < shop.Count; i++)
		{
			Destroy(shop[i]);
		}
		shop.Clear();
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

		int[] colorIndexes = ReorderArray(indexes);

		// Create actual gameobjects for the shop
		for (int i = 0; i < blocks.Count; i++)
		{
			BlockInstance instance = blocks[i];
			//Rounding is here.
			instance.profit = GameManager.Instance.RoundDownTwoSF(instance.profit);
			instance.cost = GameManager.Instance.RoundDownTwoSF(instance.cost);

			instance.towerColor = (TowerColor)colorIndexes[i];

			if (instance.cost < GameManager.Instance.portfolioValue) canBuySomething = true;

			GameObject item = Instantiate(ShopItemPrefab, ShopItemLocations[i]);
			ShopItem shopItem = item.GetComponent<ShopItem>();
			shopItem.block = instance;
			shop.Add(item);
		}

		if (!canBuySomething)
		{
			GameManager.Instance.EndGame(GameOverReason.Poor);
		}
	}


	public void RefreshShop()
	{
		ClearShop();
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
		RefreshShop();
	}
}
