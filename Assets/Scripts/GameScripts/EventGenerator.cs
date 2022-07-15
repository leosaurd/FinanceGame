using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{

	public static string[] selector = new string[] { "stability", "profit" };
	public static int[] multiplier = new int[] { 2, 5, 10 };
	public static int[] rounds = new int[] { 1, 2, 5 };
	public static string[] blockSelector = new string[] {
		"", " of the Insurance Type", " of the Low Risk Type", " of the High Risk Type",
		" that are ETFs", " that are REITs", " that are Equity Mutual Funds", " that are Emerging Markets Equities", " that are High-Yield Bonds"," that are Cryptocurrencies",
		" that are Treasury Bills", " that are Government Bonds", " that are Savings Bonds", " that are Fixed Deposit", " that are Dividend-paying stocks",
		" that are Health Plans", " that are Disability Plans", " that are Term Life Plan", " that are Life Plans"
	};

	//Variables used in GameManager
	public static float selectMult;
	public static int selectRounds;
	public static EventType eventRecord;
	public static BlockType blockRecord;
	public static int selectIndex;
	public static string selectGroup;

	//Variables used locally - Not sure if GameManager needs them.
	public static int blockIndex;
	public static string selectBlocks;
	public static string selectType;

	//Not sure whether I want to use a dictionary for the event.
	private static readonly Dictionary<EventType, string> eventList = new()
	{
		{ EventType.Multiplier, "Multiply {0} by {1} for the next {2} round(s) for all blocks{3}." },
		{ EventType.Fractional, "Divide {0} by {1} for the next {2} rounds(s) for all blocks{3}." },
		{ EventType.BlockRemoval, "Removes a random block from the tower." },
		{ EventType.BlockAddition, "Adds a random block to the tower."},
		{ EventType.BlockNullification, "all blocks{3} no longer generate profit for a single round."}
	};

	public static readonly Dictionary<EventType, string> typeToTitle = new()
	{
		{ EventType.Multiplier, "Multplier event" },
		{ EventType.Fractional, "Divider event" },
		{ EventType.BlockRemoval, "Block Removal Event" },
		{ EventType.BlockAddition, "Block Addition Event" },
		{ EventType.BlockNullification, "Profit Nullification Event" },
	};

	//Where all altering factors should be for ownedblocks.
	public static string GenerateEvent(EventType eventType)
	{
		//If there is an ongoing event, do not do anything. This code SHOULD NOT OCCUR, as Generate Event should only be called when eventoccuring is false.
		/*if (GameManager.Instance.eventOccuring)
		{
			return "";
		}*/

		//Randomly generates an index - One for block selection, utilizing a switch case to select Options.
		blockIndex = Random.Range(0, blockSelector.Length);
		selectIndex = Random.Range(0, selector.Length);


		selectType = selector[selectIndex];
		selectRounds = rounds[Random.Range(0, rounds.Length)];
		selectBlocks = blockSelector[Random.Range(0, blockSelector.Length)];
		eventRecord = eventType;
		selectGroup = "";

		switch (blockIndex)
		{
			case 0:
				blockRecord = BlockType.All;
				break;
			case 1:
				blockRecord = BlockType.Insurance;
				break;
			case 2:
				blockRecord = BlockType.LowRiskInvestment;
				break;
			case 3:
				blockRecord = BlockType.HighRiskInvestment;
				break;
			case 4:
				selectGroup = "ETF";
				blockRecord = BlockType.None;
				break;
			case 5:
				selectGroup = "REIT";
				blockRecord = BlockType.None;
				break;
			case 6:
				selectGroup = "Equity Mutual Fund";
				blockRecord = BlockType.None;
				break;
			case 7:
				selectGroup = "Emerging Market Equities";
				blockRecord = BlockType.None;
				break;
			case 8:
				selectGroup = "High-Yield Bonds";
				blockRecord = BlockType.None;
				break;
			case 9:
				selectGroup = "Cryptocurrencies";
				blockRecord = BlockType.None;
				break;
			case 10:
				selectGroup = "Treasury Bills";
				blockRecord = BlockType.None;
				break;
			case 11:
				selectGroup = "Government Bonds";
				blockRecord = BlockType.None;
				break;
			case 12:
				selectGroup = "Savings Bonds";
				blockRecord = BlockType.None;
				break;
			case 13:
				selectGroup = "Fixed Deposit";
				blockRecord = BlockType.None;
				break;
			case 14:
				selectGroup = "Dividend-paying stocks";
				blockRecord = BlockType.None;
				break;
			case 15:
				selectGroup = "Health Plan";
				blockRecord = BlockType.None;
				break;
			case 16:
				selectGroup = "Disability Plan";
				blockRecord = BlockType.None;
				break;
			case 17:
				selectGroup = "Term Life Plan";
				blockRecord = BlockType.None;
				break;
			case 18:
				selectGroup = "Life Plan";
				blockRecord = BlockType.None;
				break;
			default:
				blockRecord = BlockType.None;
				break;
		}



		//String that is generated.
		//Fixing the multiplier on the string for real this time lol

		selectMult = multiplier[Random.Range(0, multiplier.Length)];
		string printList = string.Format(eventList[eventType], selectType, selectMult, selectRounds, selectBlocks);

		if (eventType == EventType.Fractional)
		{
			selectMult = (float)1 / selectMult;
		}

		return printList;
	}
}
