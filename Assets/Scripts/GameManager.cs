using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;

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

		TowerAnimator.Instance.AddBlockToTower(block);

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
