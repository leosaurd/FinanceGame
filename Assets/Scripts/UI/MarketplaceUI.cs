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
        RefreshShopRandomized();
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
            //BlockInstance blockInstance = b.GenerateBlock();
            BlockInstance staticBlockInstance = NameGenerator.GenerateStaticBlock(b.blockType);
            GameObject item = Instantiate(ShopItemPrefab, transform);
            item.transform.localPosition = new Vector3(-15, 49f - 98 * i, 0);
            item.GetComponent<ShopItem>().block = staticBlockInstance;
            shop.Add(item);
        }
        shop = Shuffle(shop);
    }

    //A way to force randomization in the shop without exclusivity.
    public void RefreshShopRandomized()
    {
        for (int i = 0; i < shop.Count; i++)
        {
            Destroy(shop[i]);
        }
        shop.Clear();
        List<BlockInstance> l = new List<BlockInstance>();
        for (int i = 0; i < 4; i++)
        {
            //TODO non-repeating names
            BlockInstance staticBlockInstance = NameGenerator.GenerateStaticBlock((BlockType)Random.Range(0,3));

            //Lazymans method for non-repeating names
            if (l.Contains(staticBlockInstance))
            {
                while (l.Contains(staticBlockInstance))
                {
                    staticBlockInstance = NameGenerator.GenerateStaticBlock((BlockType)Random.Range(0, 3));
                }
            }
            else
            {
                l.Add(staticBlockInstance);
            }
            
            GameObject item = Instantiate(ShopItemPrefab, transform);
            item.transform.localPosition = new Vector3(-15, 49f - 98 * i, 0);
            item.GetComponent<ShopItem>().block = staticBlockInstance;
            shop.Add(item);
        }
        shop = Shuffle(shop);
    }

    public void Buy(BlockInstance block)
	{
        GameManager GM = GameManager.GetInstance();
        GM.profits += block.profit;
        GM.portfolioValue -= block.cost;
        GM.stability += block.stability;
        GM.AddBlock(block);
        RefreshShopRandomized();
	}

    //Shuffle method
    private List<GameObject> Shuffle(List<GameObject> l)
    {
        //saving vector positions before shuffling.
        List<Vector3> v = new List<Vector3>();

        for (int i = 0; i < l.Count; i++)
        {
            v.Add(l[i].transform.position);
        }
        //shuffle
        for (int i = 0; i < l.Count; i++)
        {
            GameObject temp = l[i];
            int randomIndex = Random.Range(i, l.Count);
            l[i] = l[randomIndex];
            //inherit position?
            l[randomIndex] = temp;
        }
        //Inherit original positions
        for (int i = 0; i < l.Count; i++)
        {
            l[i].transform.position = v[i];
        }

        return l;
    }
}
