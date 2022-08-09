using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventBlockScript : MonoBehaviour
{
	public BlockInstance block;
	public SpriteRenderer iconRenderer;

	private TextMeshPro profitTextMesh;

	// Start is called before the first frame update
	public void updateGraphics()
    {
		SpriteRenderer spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
		spriteRenderer.sprite = TowerColorUtils.GetBlockSprite(block.towerColor, block.height);

		Transform nameObject = transform.Find("Name");
		Transform profitObject = transform.Find("Profit");

		float textHeight = 2 * block.height;

		nameObject.localPosition = new Vector3(nameObject.localPosition.x, textHeight, 0);
		profitObject.localPosition = new Vector3(profitObject.localPosition.x, textHeight, 0);

		nameObject.GetComponent<TextMeshPro>().text = block.name;

		int profit = block.profit;
		profitTextMesh = profitObject.GetComponent<TextMeshPro>();
		profitTextMesh.text = (profit < 0 ? "-$" : "+$") + Mathf.Abs(profit).ToString("N0");

		Transform iconObject = transform.Find("Icon");
		switch (block.height)
		{
			case 1:
				iconObject.localPosition = new Vector2(0.8f, 1.5f);
				break;
			case 2:
				iconObject.localPosition = new Vector2(0.8f, 3.4f);
				break;
			case 3:
				iconObject.localPosition = new Vector2(0.8f, 5.3f);
				break;
		}


		iconRenderer = iconObject.GetComponent<SpriteRenderer>();
		switch (block.blockType)
		{
			case BlockType.Insurance:
				iconRenderer.sprite = Resources.Load<Sprite>("InsuranceIconIso");
				break;
			case BlockType.LowRiskInvestment:
				iconRenderer.sprite = Resources.Load<Sprite>("LowRiskIconIso");
				break;
			case BlockType.HighRiskInvestment:
				iconRenderer.sprite = Resources.Load<Sprite>("HighRiskIconIso");
				break;
		}
	}
}
