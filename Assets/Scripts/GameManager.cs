using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{

	public static GameManager Instance { get; private set; }

	void Awake()
	{
		if (!Instance) Instance = this;
	}

	private float stability = 0;
	public float Stability
	{
		get { return stability; }
		set
		{
			stability = value;
			if (value < -1)
			{
				stability = -1;
				EndGame(GameOverReason.Stability);
			}
		}
	}
	public float portfolioValue = 0;
	public int profits = 0;
	public int towerHeight = 0;

	public int totalEarnings = 0;

	//Chance for event to occur
	public float eventChance = 0f;

	//How long the event will be for.
	public int eventDuration = 0;

	//Whether there is an event ongoing
	public bool eventOccuring = false;

	//saved value of Stability
	public float originalStab = 0;

	//List of booleans to check if a block has already been modified
	public List<bool> alteredStability = new List<bool>();

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
		//If the generated number is less than or equal to event chance AND there is no event happening, then generate an event
		if (Random.Range(0, 100) <= eventChance && !eventOccuring)
		{
			//Randomly picks from one of the 5 events available in EventType.
			EventType eventType = (EventType)Random.Range(0, 5);
			string description = EventGenerator.GenerateEvent(eventType);
			string title = EventGenerator.typeToTitle[eventType].ToUpper();

			if (eventType == EventType.BlockRemoval || eventType == EventType.BlockAddition)
			{
				EventUIManager.Instance.ShowEvent(title, description, () =>
				{
					//If the event is to add a block.
					if (EventGenerator.eventRecord == EventType.BlockAddition)
					{
						//Add a random block here.
						BlockType blockType = (BlockType)Random.Range(0, 3);
						string name = NameGenerator.GenerateName(blockType);
						BlockInstance blockToAdd = new(name, blockType, StaticObjectManager.BlockStats[name]);
						profits += blockToAdd.profit;
						Stability += blockToAdd.stability;
						ownedBlocks.Add(blockToAdd);
						TowerAnimator.Instance.AddBlockToTower(blockToAdd);

						eventOccuring = false;
						eventDuration = 0;
					}
					//If the event is to remove a block.
					else if (EventGenerator.eventRecord == EventType.BlockRemoval)
					{
						//Remove a random block
						int blockToRemoveIndex = Random.Range(0, ownedBlocks.Count);
						BlockInstance blockToRemove = ownedBlocks[blockToRemoveIndex];
						TowerAnimator.Instance.RemoveBlockFromTower(blockToRemove);
						ownedBlocks.RemoveAt(blockToRemoveIndex);
						profits -= blockToRemove.profit;
						Stability -= blockToRemove.stability;
						eventOccuring = false;
						eventDuration = 0;
					}
				});
			}
			else
				EventUIManager.Instance.ShowEvent(title, description);

			//Sets event occuring to true.
			eventOccuring = true;
			//Resets event chance.
			eventChance = 0f;
			//sets the duration of the event.
			eventDuration = EventGenerator.selectRounds;
		}
		TowerAnimator.Instance.AddBlockToTower(block);

		for (int i = 0; i < ownedBlocks.Count; i++)
		{
			if (alteredStability.Count < ownedBlocks.Count)
			{
				alteredStability.Add(false);
			}
			//Change profit by multiplier from Generated Event.
			float multiplier = 1f;
			//If event is occuring
			if (eventOccuring)
			{

				//If the event is to nullify profit.
				if (EventGenerator.eventRecord == EventType.BlockNullification)
				{
					//If the block matches the record, select all is set, or the name matches the group.
					if (SelectBlock(i))
						multiplier = 0;
				}
				//If event is a multiplier event
				else if (isMultiplier())
				{
					//If event is a profit-altering event
					if (EventGenerator.selectIndex == 1)
					{
						if (SelectBlock(i))
							multiplier = EventGenerator.selectMult;
					}
					//If event is a Stability-altering event
					else if (EventGenerator.selectIndex == 0)
					{
						if (SelectBlock(i))
						{
							//There was a *= here LOL - must've been super sleepy
							multiplier = EventGenerator.selectMult;
							if (!alteredStability[i])
							{
								Stability += (ownedBlocks[i].stability * multiplier) - ownedBlocks[i].stability;
								alteredStability[i] = true;
							}
						}
					}
				}
			}
			portfolioValue += (ownedBlocks[i].profit) * multiplier;
			totalEarnings += Mathf.RoundToInt((ownedBlocks[i].profit) * multiplier);

		}

		//If there is an ongoing event
		if (eventOccuring)
		{
			eventDuration--;
			//If the event is 0, set the event occuring to false.
			if (eventDuration == 0)
			{
				//Clear any Stability changes
				alteredStability.Clear();
				Stability = 0;
				for (int i = 0; i < ownedBlocks.Count; i++)
				{
					Stability += ownedBlocks[i].stability;
				}
				eventOccuring = false;
			}
		}

		if (Stability < -1)
		{
			Stability = -1;
			EndGame(GameOverReason.Stability);
		}
		else if (Stability > 1)
		{
			Stability = 1;
		}

		SessionManager.Instance.Session.Stability.Add(Stability);
		SessionManager.Instance.Session.TotalEarnings = totalEarnings;
		SessionManager.Instance.Session.Tower = ownedBlocks.ToArray();
		SessionManager.Instance.Session.InsuranceCount = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.Insurance).Count;
		SessionManager.Instance.Session.LowRiskCount = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.LowRiskInvestment).Count;
		SessionManager.Instance.Session.HighRiskCount = ownedBlocks.FindAll((BlockInstance b) => b.blockType == BlockType.HighRiskInvestment).Count;
		SessionManager.Instance.SaveSession();
	}


	public void EndGame(GameOverReason reason)
	{
		if (reason == GameOverReason.Stability)
		{
			SessionManager.Instance.EndSession(SessionEndReason.GameOverStability);
		}
		else
		{
			SessionManager.Instance.EndSession(SessionEndReason.GameOverPoor);
		}

		GameOver.Instance.ShowGameover(reason);
	}

	public void RetunToMainMenu()
	{
		SessionManager.Instance.EndSession(SessionEndReason.MainMenu);
		SceneManager.LoadScene("MainMenu");
	}

	//If event matches BlockType, or Name matches
	public bool SelectBlock(int i)
	{
		return ((EventGenerator.blockRecord == ownedBlocks[i].blockType) || EventGenerator.blockRecord == BlockType.All || EventGenerator.selectGroup.Equals(ownedBlocks[i].name));
	}

	//If the event is a Multiplier event
	public bool isMultiplier()
	{
		return EventGenerator.eventRecord == EventType.Fractional || EventGenerator.eventRecord == EventType.Multiplier;
	}



	//Rounding function to round values down for easier reading.
	public int RoundDownTwoSF(float d)
	{
		if (d == 0) return 0;
		float scale = Mathf.Pow(10, Mathf.Floor(Mathf.Log10(Mathf.Abs(d))) + 1);
		return Mathf.RoundToInt(scale * (float)System.Math.Round(d / scale, 2));
	}
}


public enum GameOverReason
{
	Stability,
	Poor,
}
