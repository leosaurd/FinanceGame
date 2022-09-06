using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockAnimator : MonoBehaviour
{
	public float targetPosition;
	public BlockInstance block;
	public SpriteRenderer glowRenderer;
	public SpriteRenderer iconRenderer;
	public float targetGlow = 1;
	public float glow = 1;
	public bool firstFall = true;

	private TextMeshPro profitTextMesh;

	public void Start()
	{
		SpriteRenderer spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
		spriteRenderer.sprite = TowerColorUtils.GetBlockSprite(block.towerColor, block.height);
		spriteRenderer.sortingOrder = GameManager.Instance.ownedBlocks.Count;

		Transform nameObject = transform.Find("Name");
		Transform profitObject = transform.Find("Profit");

		float textHeight = 2 * block.height;

		nameObject.localPosition = new Vector3(nameObject.localPosition.x, textHeight, 0);
		profitObject.localPosition = new Vector3(profitObject.localPosition.x, textHeight, 0);

		nameObject.GetComponent<TextMeshPro>().text = block.name;

		int profit = block.profit;
		profitTextMesh = profitObject.GetComponent<TextMeshPro>();
		profitTextMesh.text = (profit < 0 ? "-$" : "+$") + Mathf.Abs(profit).ToString("N0");


		Transform glowObject = transform.Find("Glow");
		Transform iconObject = transform.Find("Icon");
		glowRenderer = glowObject.GetComponent<SpriteRenderer>();
		glowRenderer.sprite = TowerColorUtils.GetGlowSprite(block.height);
		switch (block.height)
		{
			case 1:
				glowObject.localPosition = new Vector2(0, 3.25f);
				iconObject.localPosition = new Vector2(0.8f, 1.5f);
				break;
			case 2:
				glowObject.localPosition = new Vector2(0, 4.2f);
				iconObject.localPosition = new Vector2(0.8f, 3.4f);
				break;
			case 3:
				glowObject.localPosition = new Vector2(0, 5.1f);
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

	private void ProfitUpdate()
	{
		if (GameManager.Instance.lastingEvent != null && GameManager.Instance.lastingEvent.Type == EventType.BlockNullification && GameManager.Instance.lastingEvent.AffectedGroup == block.blockType)
		{
			profitTextMesh.text = "$0";
		}
		else
		{
			int profit = block.profit;
			profitTextMesh.text = (profit < 0 ? "-$" : "+$") + Mathf.Abs(profit).ToString("N0");
		}
	}

	private void FixedUpdate()
	{
		//ProfitUpdate();

		if (transform.localPosition.y > targetPosition)
		{
			float speed = 0.33f;
			if (firstFall && transform.localPosition.y - speed * 10 < targetPosition)
			{
				firstFall = false;
				SFXManager.Instance.PlaySFX(SFX.blockDrop);
			}

			if (transform.localPosition.y - speed < targetPosition)
			{
				transform.localPosition = new Vector2(transform.localPosition.x, targetPosition);
				targetGlow = 0;

			}
			else
			{
				transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - speed);
			}
		}

		if (glowRenderer.color.a != targetGlow)
		{
			float diff = targetGlow - glowRenderer.color.a;
			if (Mathf.Abs(diff) < 0.05f) glowRenderer.color = new Color(1, 1, 1, targetGlow);
			else glowRenderer.color = new Color(1, 1, 1, glowRenderer.color.a + diff / 15);
		}
	}
}
