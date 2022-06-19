using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	void Awake()
	{
		if (!Instance) Instance = this;
	}

	public float stability = 0;
	public int portfolioValue = 0;
	public int profits = 0;

	public List<BlockInstance> ownedBlocks = new List<BlockInstance>();

	public void AddBlock(BlockInstance block)
	{
		ownedBlocks.Add(block);

		TowerAnimator.Instance.AddBlockToTower(block);

		for (int i = 0; i < ownedBlocks.Count; i++)
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

	public int GetScore()
	{
		return Mathf.RoundToInt(ownedBlocks.Count * profits);
	}

	public void EndGame(GameOverReason reason)
	{
		GameOver.Instance.ShowGameover(reason);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene("Game");
	}
}

public enum GameOverReason
{
	Stability,
	Poor,
}
