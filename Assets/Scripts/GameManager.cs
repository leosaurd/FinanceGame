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

	public float gameTime = 0;
	public float gameCount = 0;
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
	}


	public void EndGame(GameOverReason reason)
	{
		//Added a gameCount here
		gameCount++;
		//Added a timer here - Needs fixing TODO for more than 1 game
		gameTime = (Time.timeSinceLevelLoad - gameTime);
		//Attempting to save data
		saveData();
		GameOver.Instance.ShowGameover(reason);
	}

	public void RetunToMainMenu()
	{
		gameTime = 0;
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

	public void saveData()
	{
		Hashtable gamedata = new Hashtable();
		string path = "test.csv";

		StreamWriter writer = new StreamWriter(path, true);

		string saveText = "";
		List<BlockInstance> blockList = ownedBlocks;

		//Saves the total number of blocks a player has placed
		gamedata.Add("TotalPlayerBlockCount", blockList.Count);

		//Saves the average stability score
		gamedata.Add("AVGStability", Stability);

		//Saves the time it took for a game
		gamedata.Add("GameTime", gameTime);

		//Saves the number of investment blocks placed
		gamedata.Add("InsuranceBlocksPlaced", calculateTotal(blockList, BlockType.Insurance));

		//Saves the number of High risk investment blocks placed
		gamedata.Add("HRIBlocksPlaced", calculateTotal(blockList, BlockType.HighRiskInvestment));

		//Saves the number of Low risk investment blocks placed
		gamedata.Add("LRIBlocksPlaced", calculateTotal(blockList, BlockType.LowRiskInvestment));

		//Number of games played
		gamedata.Add("GamesPlayed", gameCount);

		//Total earnings
		gamedata.Add("TotalEarnings", totalEarnings);

		//Name Data 
		gamedata.Add("HealthPlan", calculateTotal(blockList, "Health Plan"));
		gamedata.Add("DisabiltyPlan", calculateTotal(blockList, "Disability Plan"));
		gamedata.Add("TermLifePlan", calculateTotal(blockList, "Term Life Plan"));
		gamedata.Add("LifePlan", calculateTotal(blockList, "Life Plan"));
		gamedata.Add("TreasuryBills", calculateTotal(blockList, "Treasury Bills"));
		gamedata.Add("GovtBonds", calculateTotal(blockList, "Government Bonds"));
		gamedata.Add("SavingBonds", calculateTotal(blockList, "Savings Bonds"));
		gamedata.Add("FixedDeposit", calculateTotal(blockList, "Fixed Deposit"));
		gamedata.Add("DividendPayingStocks", calculateTotal(blockList, "Dividend-paying stocks"));
		gamedata.Add("ETF", calculateTotal(blockList, "ETF"));
		gamedata.Add("REIT", calculateTotal(blockList, "REIT"));
		gamedata.Add("EquityMutualFund", calculateTotal(blockList, "Equity Mutual Fund"));
		gamedata.Add("EmergingMarketEquities", calculateTotal(blockList, "Emerging Markets Equities"));
		gamedata.Add("HighYieldBonds", calculateTotal(blockList, "High-Yield Bonds"));
		gamedata.Add("CryptoCurrency", calculateTotal(blockList, "Cryptocurrencies"));


		foreach(DictionaryEntry entry in gamedata)
        {
			//Attempting to save the string
			saveText += entry.Key + "," + entry.Value + "\n";
        }
		//This is what will save the string to the server most likely.
		writer.Write(saveText + "\n");
		writer.Close();
		
	}

	public int calculateTotal(List<BlockInstance> l, BlockType b)
	{
		int count = 0;
		for (int i = 0; i < l.Count; i++)
		{
			if (l[i].blockType == b)
			{
				count++;
			}
		}

		return count;
	}

	public int calculateTotal(List<BlockInstance> l, string s)
	{
		int count = 0;
		for (int i = 0; i < l.Count; i++)
		{
			if (l[i].name == s)
			{
				count++;
			}
		}

		return count;
	}

	//Rounding function to round values down for easier reading.
	public int RoundDownTwoSF(double d)
    {
		if (d == 0.0) return (int)d;
		double scale = System.Math.Pow(10, System.Math.Floor(System.Math.Log10(System.Math.Abs(d))) + 1);
		double s = scale * System.Math.Round(d / scale, 2);
		return (int)s;
    }
}


public enum GameOverReason
{
	Stability,
	Poor,
}
