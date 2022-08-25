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


	private bool canBuy = true;

	void Start()
	{
		imageObj = transform.Find("Image");
		nameObj = transform.Find("Name");
		costObj = transform.Find("CostValue");
		profitObj = transform.Find("ProfitsValue");
		stabilityObj = transform.Find("Stability Images");
		buttonObj = transform.Find("Button");
		warningObj = transform.Find("Warning");


		DoFormatting();

		image = TowerColorUtils.GetIconSprite[block.blockType];
		imageObj.GetComponent<Image>().sprite = image;

		textColor = TowerColorUtils.GetTextColor[block.towerColor];
		nameObj.GetComponent<TextMeshProUGUI>().color = textColor;
		nameObj.GetComponent<TextMeshProUGUI>().text = block.name;


		DoCost();
		DoProfit();
		DoStability();

		buttonObj.GetComponent<FadeButton>().block = this.block;

		transform.Find("GreyOverlay").gameObject.SetActive(!canBuy);
	}

	private void DoProfit()
	{
		string profitText = "";
		if (block.profit < 0)
			profitText += "-";
		profitText += "$" + Mathf.Abs(block.profit);

		if (block.affectedByEvent && block.affectedField == EventField.profit)
		{
			if (block.beneficialEvent)
			{
				profitObj.GetComponent<TextMeshProUGUI>().color = new Color32(33, 150, 243, 255);
			}
			else
			{
				profitObj.GetComponent<TextMeshProUGUI>().color = new Color32(244, 67, 54, 255);
			}
		}

		profitObj.GetComponent<TextMeshProUGUI>().text = profitText;
	}

	private void DoStability()
	{
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
		else if (Mathf.Abs(block.stability) > 0.15)
		{
			numberOfSprites = 2;
			buttonObj.GetComponent<FadeButton>().maxOpacity = 0.75f;
		}
		else
		{
			buttonObj.GetComponent<FadeButton>().maxOpacity = 0.5f;
			numberOfSprites = 1;

		}
		float width = GetComponent<RectTransform>().rect.width;
		float unit = width / 24;

		for (int i = 0; i < numberOfSprites; i++)
		{
			GameObject stabilityChild = Instantiate(stabilityChildPrefab, stabilityObj);
			stabilityChild.GetComponent<Image>().sprite = stabilityImage;

			RectTransform rect = stabilityChild.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, unit);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, unit);
			rect.anchoredPosition = new Vector2(-unit * i, 0);

		}

		if (GameManager.Instance.Stability + block.stability <= -1)
		{
			canBuy = false;
			warningObj.gameObject.SetActive(true);
		}
	}

	private void DoCost()
	{
		string costText = "$" + block.cost;
		costObj.GetComponent<TextMeshProUGUI>().text = costText;

		if (block.affectedByEvent && block.affectedField == EventField.cost)
		{
			if (block.beneficialEvent)
			{
				costObj.GetComponent<TextMeshProUGUI>().color = new Color32(33, 150, 243, 255);
			}
			else
			{
				costObj.GetComponent<TextMeshProUGUI>().color = new Color32(244, 67, 54, 255);
			}
		}

		if (block.cost > GameManager.Instance.portfolioValue)
		{
			canBuy = false;
			warningObj.gameObject.SetActive(true);
		}
	}

	void DoFormatting()
	{
		float width = GetComponent<RectTransform>().rect.width;
		float unit = width / 24;
		float right = 0;

		ItemHint.UpdatePositions(unit);

		// image - 2
		// Name - 7
		// Gap - 1
		// Cost - 4
		// Gap - 1
		// Profit - 3
		// Gap - 2
		// Stability - 3
		// Gap - 1

		float size = unit * 2;
		RectTransform imageRect = imageObj.GetComponent<RectTransform>();
		imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
		imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
		right += size;

		size = unit * 7;
		RectTransform nameRect = nameObj.GetComponent<RectTransform>();
		nameRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
		nameRect.anchoredPosition = new Vector2(right, nameRect.anchoredPosition.y);
		right += size;

		size = unit * 4;
		RectTransform costRect = costObj.GetComponent<RectTransform>();
		costRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
		costRect.anchoredPosition = new Vector2(right + unit, costRect.anchoredPosition.y);
		right += size + unit;

		size = unit * 3;
		RectTransform profitRect = profitObj.GetComponent<RectTransform>();
		profitRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
		profitRect.anchoredPosition = new Vector2(right + unit, profitRect.anchoredPosition.y);
		right += size + unit;

		size = unit * 3;
		RectTransform stabilityRect = stabilityObj.GetComponent<RectTransform>();
		stabilityRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
		stabilityRect.anchoredPosition = new Vector2(right + unit * 2, stabilityRect.anchoredPosition.y);
		right += size + unit * 2;
	}

	public void Buy()
	{
		if (GameManager.Instance.portfolioValue - block.cost > 0 && GameManager.Instance.Stability + block.stability > -1)
			MarketplaceUI.Instance.Buy(block);
	}

}
