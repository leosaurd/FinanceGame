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
	private Transform stabilityObj;

	public Sprite[] stabilityImages;
	public Sprite image;
	public Color textColor;


	void Start()
	{
		imageObj = transform.Find("Image");
		nameObj = transform.Find("Name");
		costObj = transform.Find("CostValue");
		profitObj = transform.Find("ProfitsValue");
		stabilityObj = transform.Find("Stability Image");


		textColor = TowerColorUtils.GetTextColor[block.towerColor];
		image = TowerColorUtils.GetCubeSprite[block.towerColor];

		imageObj.GetComponent<Image>().sprite = image;
		nameObj.GetComponent<TextMeshProUGUI>().color = textColor;

		nameObj.GetComponent<TextMeshProUGUI>().text = block.name;

		string costText = "$" + block.cost;
		costObj.GetComponent<TextMeshProUGUI>().text = costText;
		if (block.cost > GameManager.Instance.portfolioValue)
		{
			costObj.GetComponent<TextMeshProUGUI>().color = Color.red;
		}

		string profitText = "";
		if (block.profit < 0)
			profitText += "-";
		profitText += "$" + Mathf.Abs(block.profit);
		profitObj.GetComponent<TextMeshProUGUI>().text = profitText;


		Image stabilityImage = stabilityObj.GetComponent<Image>();
		if (block.stability < -0.35)
		{
			stabilityImage.sprite = stabilityImages[0];
		}
		else if (block.stability < -0.20)
		{
			stabilityImage.sprite = stabilityImages[1];
		}
		else if (block.stability < 0)
		{
			stabilityImage.sprite = stabilityImages[2];
		}
		else if (block.stability < 0.20)
		{
			stabilityImage.sprite = stabilityImages[3];
		}
		else if (block.stability < 0.35)
		{
			stabilityImage.sprite = stabilityImages[4];
		}
		else
		{
			stabilityImage.sprite = stabilityImages[5];
		}
	}

	public void Buy()
	{
		if (GameManager.Instance.portfolioValue - block.cost > 0)
			MarketplaceUI.Instance.Buy(block);
	}

}
