using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerAnimator : MonoBehaviour
{
	public static TowerAnimator Instance { get; private set; }
	public GameObject blockPrefab;

	public float targetPos = -4;

	public List<GameObject> tower = new List<GameObject>();

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	public void FixedUpdate()
	{
		// Animate the motion of the tower
		float diff = targetPos - transform.localPosition.y;

		transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + diff / 15);

		if (Mathf.Abs(diff) < 0.005) transform.localPosition = new Vector2(transform.localPosition.x, targetPos);
	}

	public void AddBlockToTower(BlockInstance block)
	{
		GameObject blockObj = Instantiate(blockPrefab, transform);
		float startingPosition = 10;
		if(tower.Count > 5)
		{
			startingPosition = tower.Count + 5;
		}

		blockObj.transform.localPosition = new Vector2(blockObj.transform.localPosition.x, startingPosition);


		BlockAnimator blockAnimator = blockObj.GetComponent<BlockAnimator>();
		blockAnimator.targetPosition = tower.Count;
		blockAnimator.block = block;

		

		if (tower.Count > 5)
		{
			targetPos -= 1;
		}
		tower.Add(blockObj);
	}
}
