using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockAnimator : MonoBehaviour
{
	public float targetPosition;
	public BlockInstance block;
	public float speed = 0;


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
		// TODO Check if events affect profit?
		profitObject.GetComponent<TextMeshPro>().text = (profit < 0 ? "-$" : "+$") + Mathf.Abs(profit).ToString("N0");

	}

	private void FixedUpdate()
	{
		if (transform.localPosition.y > targetPosition)
		{
			speed += 0.025f;

			if (transform.localPosition.y - speed < targetPosition)
			{
				transform.localPosition = new Vector2(transform.localPosition.x, targetPosition);
				/*
								float stabilityModifier = 1 + block.stability * -1;

								CameraAnimator.Instance.ScreenShake(10 * stabilityModifier / 2, stabilityModifier / 5);*/
			}
			else
				transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - speed);
		}

	}
}
