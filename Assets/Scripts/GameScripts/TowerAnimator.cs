using System.Collections.Generic;
using UnityEngine;

public class TowerAnimator : MonoBehaviour
{
	public static TowerAnimator Instance { get; private set; }
	public GameObject blockPrefab;
	public float targetPos = -0.775f;

	public List<GameObject> tower = new List<GameObject>();

	private void Awake()
	{
		if (Instance == null) Instance = this;
	}

	public void FixedUpdate()
	{
		// Animate the motion of the tower
		float diff = targetPos - transform.localPosition.y;

		transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + diff / 15);

		if (Mathf.Abs(diff) < 0.005) transform.localPosition = new Vector2(transform.localPosition.x, targetPos);
	}
	public void RemoveBlockFromTower(BlockInstance block)
	{
		int index = tower.FindIndex(0, (GameObject obj) => obj.GetComponent<BlockAnimator>().block.id == block.id);
		Destroy(tower[index]);
		tower.RemoveAt(index);

		// Move all blocks above it down by 0.64f * block.height
		for (int i = 0; i < tower.Count; i++)
		{
			GameObject obj = tower[i];
			obj.transform.Find("Background").GetComponent<SpriteRenderer>().sortingOrder = i;
			if (i >= index)
				obj.GetComponent<BlockAnimator>().targetPosition -= 0.64f * block.height;

		}

		GameManager.Instance.towerHeight -= block.height;

		if (GameManager.Instance.towerHeight > 4)
		{
			targetPos += 0.64f * block.height;
		}
	}
	public void AddBlockToTower(BlockInstance block)
	{
		GameObject blockObj = Instantiate(blockPrefab, transform);

		blockObj.transform.position = new Vector2(blockObj.transform.localPosition.x, 6);

		BlockEffectManager.Instance.NewEffect();
		NewAssetTextManager.Instance.NewEffect();

		BlockAnimator blockAnimator = blockObj.GetComponent<BlockAnimator>();
		blockAnimator.targetPosition = GameManager.Instance.towerHeight * 0.64f;
		blockAnimator.block = block;
		GameManager.Instance.towerHeight += block.height;



		if (GameManager.Instance.towerHeight > 4)
		{
			targetPos -= 0.64f * block.height;
		}
		tower.Add(blockObj);
	}
}
