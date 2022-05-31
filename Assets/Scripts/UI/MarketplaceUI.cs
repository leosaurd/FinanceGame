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

    public void RefreshShop()
    {
        for (int i = 0; i < shop.Count; i++)
		{
            Destroy(shop[i]);
		}

        StaticObjectManager SOM = StaticObjectManager.GetInstance();
        shop.Clear();
        // TODO randomise order it grabs blocktype from SOM
        // TODO Showcase
        for ( int i = 0; i < SOM.blocks.Length; i++)
        {
            Block b = SOM.blocks[i];
            BlockInstance blockInstance = b.GenerateBlock();
            GameObject item = Instantiate(ShopItemPrefab, transform);
            item.transform.localPosition = new Vector3(-15, 49f - 98 * i, 0);
            item.GetComponent<ShopItem>().block = blockInstance;
            shop.Add(item);
        }
    }

    public void Buy(BlockInstance block)
	{
        GameManager GM = GameManager.GetInstance();
        GM.profits += block.profit;
        GM.portfolioValue -= block.cost;
        GM.stability += block.stability;
        RefreshShop();
	}
}
