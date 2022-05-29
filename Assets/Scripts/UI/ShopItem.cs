using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public BlockInstance block;

	private Transform imageObj;
	private Transform nameObj;
	private Transform costObj;
	private Transform profitObj;
	private Transform stabilityObj;

    void Start()
	{
		imageObj = transform.Find("Image");
		nameObj = transform.Find("Name");
		costObj = transform.Find("Cost");
		profitObj = transform.Find("Profits");
		stabilityObj = transform.Find("Stability");

		nameObj.GetComponent<TextMeshProUGUI>().text = block.name;

		string costText = "COST ";
		costText += "$" + Mathf.Abs(block.value);
		costObj.GetComponent<TextMeshProUGUI>().text = costText;

		string profitText = "PROFITS ";
		if (block.profit < 0)
			profitText += "-";
		profitText += "$" + Mathf.Abs(block.profit);
		profitObj.GetComponent<TextMeshProUGUI>().text = profitText;


	}

	public void Buy()
	{
		MarketplaceUI.GetInstance().Buy(block);
	}
	
}
