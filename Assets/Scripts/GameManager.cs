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
	public int towerHeight = 0;

	//Chance for event to occur
	public float eventchance = 0f;
	public int eventduration = 0;

	public List<BlockInstance> ownedBlocks = new List<BlockInstance>();

	public void AddBlock(BlockInstance block)
	{
		ownedBlocks.Add(block);



		//Any time a block is added, the chance for an event increases.
		eventchance += 5;
		//If the generated number is less than or equal to event chance, then generate an event.
		if (Random.Range(0, 100) <= eventchance)
		{
			//Randomly picks from one of the 5 events available in EventType.
			EventGenerator.GenerateEvent((EventType)Random.Range(0, 5));
		}
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

		//If there is an ongoing event, count down.
		if (eventduration > 0)
		{
			eventduration--;
		}
	}

	public int GetScore()
	{
		return Mathf.RoundToInt(towerHeight * profits);
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
