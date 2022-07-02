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
	public float portfolioValue = 0;
	public int profits = 0;
	public int towerHeight = 0;

	//Chance for event to occur
	public float eventChance = 0f;

	//How long the event will be for.
	public int eventDuration = 0;

	//Whether there is an event ongoing
	public bool eventOccuring = false;

	//Whether the stability has already been multiplied 
	public bool stabMultiplied = false;

	public List<BlockInstance> ownedBlocks = new List<BlockInstance>();

	public void AddBlock(BlockInstance block)
	{
		ownedBlocks.Add(block);

		//Only if there is no event happening.
		if (!eventOccuring)
		{
			//Any time a block is added, the chance for an event increases.
			eventChance += 5;
		}
		//If the generated number is less than or equal to event chance, then generate an event.
		if (Random.Range(0, 100) <= eventChance)
		{
			//Randomly picks from one of the 5 events available in EventType.
			EventGenerator.Instance.GenerateEvent((EventType)Random.Range(0, 5));
			//Sets event occuring to true.
			eventOccuring = true;
			//Resets event chance.
			eventChance = 0f;
			eventDuration = EventGenerator.Instance.selectRounds;
		}
		TowerAnimator.Instance.AddBlockToTower(block);

		for (int i = 0; i < ownedBlocks.Count; i++)
		{
			//Change profit by multiplier from Generated Event.
			float multiplier = 1f;
            if (eventOccuring)
            {
				if (EventGenerator.Instance.selectIndex == 1)
				{
					//If the block matches the record, or it affects all blocks
					if (EventGenerator.Instance.blockRecord == ownedBlocks[i].blockType || EventGenerator.Instance.blockIndex == 0)
					{
						//If the event is a multiplicative/divisive event, set the multiplier accordingly.
						if (EventGenerator.Instance.eventRecord == EventType.Fractional || EventGenerator.Instance.eventRecord == EventType.Multiplier)
						{
							multiplier = EventGenerator.Instance.selectMult;
						}

						//If the event is to nullify a value, then remove it.
						if (EventGenerator.Instance.eventRecord == EventType.BlockNullification)
						{
							multiplier = 0;
						}

						//If the event is to add a block
						if (EventGenerator.Instance.eventRecord == EventType.BlockAddition)
						{
							//Add a random block here.
							eventOccuring = false;
							eventDuration = 0;
						}

						//If the event is to remove a block
						if (EventGenerator.Instance.eventRecord == EventType.BlockRemoval)
						{
							//Add a random block here.
							eventOccuring = false;
							eventDuration = 0;
						}
					}
				}
				//Stability portion is here.
				else
				{
					if (EventGenerator.Instance.blockRecord == ownedBlocks[i].blockType || EventGenerator.Instance.blockIndex == 0)
					{
						//If the event is a multiplicative/divisive event, set the multiplier accordingly.
						if (EventGenerator.Instance.eventRecord == EventType.Fractional || EventGenerator.Instance.eventRecord == EventType.Multiplier)
						{
							if (!stabMultiplied)
							{
								ownedBlocks[i].stability = ownedBlocks[i].stability * EventGenerator.Instance.selectMult;
							}
						}
					}
				}
			}

			//If the event occuring is a stability event, then set the multiplication value to true.
			if (EventGenerator.Instance.selectIndex == 0)
			{
				stabMultiplied = true;
			}
			portfolioValue += (ownedBlocks[i].profit) * multiplier;
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

		//If there is an ongoing event
		if (eventOccuring)
		{
			//If the event is 0, set the event occuring to false.
			if (eventDuration == 0)
			{
				//If the event was a stability event, reset the values.
				if(stabMultiplied)
                {
					for (int i = 0; i < ownedBlocks.Count; i++)
					{
						if (EventGenerator.Instance.blockRecord == ownedBlocks[i].blockType || EventGenerator.Instance.blockIndex == 0)
						{
							//If the event is a multiplicative/divisive event, revert the multiplier accordingly.
							if (EventGenerator.Instance.eventRecord == EventType.Fractional || EventGenerator.Instance.eventRecord == EventType.Multiplier)
							{
								ownedBlocks[i].stability = ownedBlocks[i].stability / EventGenerator.Instance.selectMult;
							}
						}
					}
					stabMultiplied = false;
				}

				eventOccuring = false;
			}
			else
			{
				//If not, reduce duration by 1, since we added a block.
				eventDuration--;
			}
			
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
