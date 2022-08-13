using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueWarningScript : MonoBehaviour
{
    public static ValueWarningScript Instance { get; private set; }

	public Transform portfolioTransform;
	public Transform profitTransform;
	public Sprite stabilityUp;
	public Sprite stabilityDown;
	public GameObject stabilityChildPrefab;

	// Start is called before the first frame update
	void Awake()
    {
        if (Instance == null) Instance = this;
		portfolioTransform = transform.Find("ValueStability");
		profitTransform = transform.Find("ProfitStability");
		stabilityDown = Resources.Load<Sprite>("StabilityDown");
		stabilityUp = Resources.Load<Sprite>("StabilityUp");
	}



    //Things added here
    public void PointerEnter(BlockInstance block)
    {
		ProfitValue(block);
		PortfolioValue(block);
	}

    public void PointerExit()
    {
		for (int i = 0; i < profitTransform.childCount; i++)
		{
			Destroy(profitTransform.GetChild(i).gameObject);
		}
		for (int i = 0; i < portfolioTransform.childCount; i++)
		{
			Destroy(portfolioTransform.GetChild(i).gameObject);
		}
	}

	public void ProfitValue(BlockInstance block)
    {
		int numberOfSprites;
		//TODO-Trent
		float ratio = block.profit/50f;

		if (ratio > 0.66)
		{
			numberOfSprites = 3;
		}
		else if (ratio > 0.33)
		{
			numberOfSprites = 2;
		}
		else
		{
			numberOfSprites = 1;
		}

		for (int i = 0; i < numberOfSprites; i++)
		{
			GameObject stabilityChild = Instantiate(stabilityChildPrefab, profitTransform);
			stabilityChild.GetComponent<Image>().sprite = stabilityUp;
			stabilityChild.transform.localPosition = new Vector3(-15 * i, 0, 0);
		}
	}

	public void PortfolioValue(BlockInstance block)
    {
		int numberOfSprites;
		float ratio = block.cost/(500*StaticBlockStats.roundScaling);

		if (ratio > 0.66)
		{
			numberOfSprites = 3;
		}
		else if (ratio > 0.33)
		{
			numberOfSprites = 2;
		}
		else
		{
			numberOfSprites = 1;
		}

		for (int i = 0; i < numberOfSprites; i++)
		{
			GameObject stabilityChild = Instantiate(stabilityChildPrefab, portfolioTransform);
			stabilityChild.GetComponent<Image>().sprite = stabilityDown;
			stabilityChild.transform.localPosition = new Vector3(-15 * i, 0, 0);
		}
	}
}
