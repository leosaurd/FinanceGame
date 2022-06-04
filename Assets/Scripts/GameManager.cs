using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	public GameObject blockPrefab;
	public GameObject blockParent;

	public static GameManager GetInstance(){
		return instance;
	}

    void Awake(){
		if(!instance){
			instance = this;
		}
		else{
			Destroy(gameObject);
		}
	}

	public float stability = 0;
	public int portfolioValue = 0;
	public int profits = 0;

	public List<BlockInstance> ownedBlocks = new List<BlockInstance>();

	public void AddBlock(BlockInstance block)
	{
		ownedBlocks.Add(block);

		GameObject blockObj = Instantiate(blockPrefab, blockParent.transform);

		blockObj.transform.localPosition = new Vector3(0, ownedBlocks.Count, 0);
		blockObj.transform.Find("Name").GetComponent<TextMeshPro>().text = block.name;

		if(ownedBlocks.Count > 7)
		{
			Vector3 prevPos = blockParent.transform.localPosition;
			blockParent.transform.localPosition = new Vector3(prevPos.x, prevPos.y - 1, prevPos.z);
		}

		for(int i = 0; i < ownedBlocks.Count; i++)
        {
			portfolioValue += (ownedBlocks[i].profit);
        }

		if (stability < -1)
		{
			stability = -1;
			EndGame(GameOverReason.Stability);
		}
		else if (stability > 1)
		{
			stability = 1;
		}
	}

	public void EndGame(GameOverReason reason)
	{
		GameOver.Instance.ShowGameover(reason);
	}
}

public enum GameOverReason
{
	Stability,
	Poor,
}
