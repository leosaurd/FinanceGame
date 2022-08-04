using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
	public BlockInstance block;
	private Transform imageObj;
	private Transform nameObj;
	private Transform costObj;
	private Transform profitObj;
	private Transform buttonObj;
	private Transform stabilityObj;
	private Transform warningObj;
	public GameObject stabilityChildPrefab;

	public Sprite image;
	public Color textColor;


	void Start()
	{
		imageObj = transform.Find("Image");
		nameObj = transform.Find("Name");
		costObj = transform.Find("CostValue");
		profitObj = transform.Find("ProfitsValue");
		stabilityObj = transform.Find("Stability Images");
		buttonObj = transform.Find("Button");
		warningObj = transform.Find("Warning");




		image = TowerColorUtils.GetIconSprite[block.blockType];

		textColor = TowerColorUtils.GetTextColor[block.towerColor];


		imageObj.GetComponent<Image>().sprite = image;
		nameObj.GetComponent<TextMeshProUGUI>().color = textColor;

		nameObj.GetComponent<TextMeshProUGUI>().text = block.name;

		string costText = "$" + block.cost;
		costObj.GetComponent<TextMeshProUGUI>().text = costText;
		if (block.cost > GameManager.Instance.portfolioValue)
		{
			costObj.GetComponent<TextMeshProUGUI>().color = Color.red;
			warningObj.gameObject.SetActive(true);
		}

		if (GameManager.Instance.Stability + block.stability <= -1)
		{
			warningObj.gameObject.SetActive(true);
		}



		string profitText = "";
		if (block.profit < 0)
			profitText += "-";
		profitText += "$" + Mathf.Abs(block.profit);
		profitObj.GetComponent<TextMeshProUGUI>().text = profitText;


		Sprite stabilityImage;
		int numberOfSprites;

		if (block.stability < 0)
		{
			stabilityImage = Resources.Load<Sprite>("StabilityDown");
			buttonObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("MarketplaceItemOutlineRed");
		}
		else
		{
			stabilityImage = Resources.Load<Sprite>("StabilityUp");
			buttonObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("MarketplaceItemOutlineGreen");
		}

		if (Mathf.Abs(block.stability) > 0.35)
		{
			numberOfSprites = 3;
			buttonObj.GetComponent<FadeButton>().maxOpacity = 1;
		}
		else if (Mathf.Abs(block.stability) > 0.20)
		{
			numberOfSprites = 2;
			buttonObj.GetComponent<FadeButton>().maxOpacity = 0.75f;
		}
		else
		{
			buttonObj.GetComponent<FadeButton>().maxOpacity = 0.5f;
			numberOfSprites = 1;
		}

		for (int i = 0; i < numberOfSprites; i++)
		{
			GameObject stabilityChild = Instantiate(stabilityChildPrefab, stabilityObj);
			stabilityChild.GetComponent<Image>().sprite = stabilityImage;
			stabilityChild.transform.localPosition = new Vector3(-15 * i, 0, 0);
		}
	}

	public void Buy()
	{
		if (GameManager.Instance.portfolioValue - block.cost > 0 && GameManager.Instance.Stability + block.stability > -1)
			MarketplaceUI.Instance.Buy(block);
	}

}
