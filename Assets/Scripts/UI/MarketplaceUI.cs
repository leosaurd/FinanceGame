using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketplaceUI : MonoBehaviour
{
    private static MarketplaceUI instance;

    public GameObject ShopItemPrefab;

    private List<GameObject> shop = new();

    public static MarketplaceUI GetInstance()
	{
        return instance;
	}

	private void Awake()
	{
		if(instance == null)
		{
            instance = this;
		}
		else
		{
            Destroy(gameObject);
		}
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
            if (names.Contains(name))
            {
                while (names.Contains(name))
                {
                    blockType = (BlockType)Random.Range(0, 3);
                    name = NameGenerator.GenerateName(blockType);
                }
            }
            else
            {
                names.Add(name);
                blocks.Add(new BlockInstance(name, blockType, StaticObjectManager.BlockStats[name].GenerateStats()));
            }
        }
        return blocks;
    }

    private void CreateShopGameobjects(List<BlockInstance> blocks)
	{
        bool canBuySomething = false;

        // Create actual gameobjects for the shop
        for (int i = 0; i < blocks.Count; i++)
        {
            BlockInstance instance = blocks[i];

            if (instance.cost < GameManager.GetInstance().portfolioValue) canBuySomething = true;

            GameObject item = Instantiate(ShopItemPrefab, transform);
            item.transform.localPosition = new Vector3(-15, 49f - 98 * i, 0);
            item.GetComponent<ShopItem>().block = instance;
            shop.Add(item);
        }

        if (!canBuySomething)
        {
            GameManager.GetInstance().EndGame(GameOverReason.Poor);
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
        GameManager GM = GameManager.GetInstance();
        GM.profits += block.profit;
        GM.portfolioValue -= block.cost;
        GM.stability += block.stability;
        GM.AddBlock(block);
        RefreshShop();
	}
}
