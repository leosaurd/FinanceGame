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

    void Start()
	{
		imageObj = transform.Find("Image");
		nameObj = transform.Find("Name");
		costObj = transform.Find("Cost");
		profitObj = transform.Find("Profits");
		stabilityObj = transform.Find("Stability");

		nameObj.GetComponent<TextMeshProUGUI>().text = block.name;

		string costText = "COST ";
		costText += "$" + block.cost;
		costObj.GetComponent<TextMeshProUGUI>().text = costText;

		string profitText = "PROFITS ";
		if (block.profit < 0)
			profitText += "-";
		profitText += "$" + Mathf.Abs(block.profit);
		profitObj.GetComponent<TextMeshProUGUI>().text = profitText;


		Image stabilityImage = stabilityObj.GetComponentInChildren<Image>();
		if (block.stability < -0.40)
		{
			stabilityImage.sprite = stabilityImages[0];
		}
		else if (block.stability < -0.25)
		{
			stabilityImage.sprite = stabilityImages[1];
		}
		else if (block.stability < -0.1)
		{
			stabilityImage.sprite = stabilityImages[2];
		}
		else if (block.stability < 0.10)
		{
			stabilityImage.sprite = stabilityImages[3];
		}
		else if (block.stability < 0.25)
		{
			stabilityImage.sprite = stabilityImages[4];
		}
		else if(block.stability < 0.40)
		{
			stabilityImage.sprite = stabilityImages[5];
		}
		else
		{
			stabilityImage.sprite = stabilityImages[6];
		}
	}

	public void Buy()
	{
		MarketplaceUI.GetInstance().Buy(block);
	}
	
}
