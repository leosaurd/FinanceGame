using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticObjectManager : MonoBehaviour
{

	public static readonly Dictionary<string, StaticBlockStats> BlockStats = new()
	{
		{ "Health Plan", new StaticBlockStats(Mathf.FloorToInt(150 / 1.05f), 1, 4, 1) },
		{ "Disability Plan", new StaticBlockStats(Mathf.FloorToInt(200 / 1.05f), 0, 6, 1) },
		{ "Term Life Plan", new StaticBlockStats(Mathf.FloorToInt(300 / 1.05f), 2, 8, 1) },
		{ "Life Plan", new StaticBlockStats(Mathf.FloorToInt(450 / 1.05f), 1, 10, 1) },

		{ "Treasury Bills", new StaticBlockStats(Mathf.FloorToInt(150 / 1.05f), 5, 1, 1) },
		{ "Government Bonds", new StaticBlockStats(Mathf.FloorToInt(200 / 1.05f), 10, 1, 1) },
		{ "Savings Bonds", new StaticBlockStats(Mathf.FloorToInt(250 / 1.05f), 15, -1, 2) },
		{ "Fixed Deposit", new StaticBlockStats(Mathf.FloorToInt(350 / 1.05f), 30, -0.5f, 2) },
		{ "Dividend-paying stocks", new StaticBlockStats(Mathf.FloorToInt(450 / 1.05f), 50, -2, 2) },

		{ "ETF", new StaticBlockStats(Mathf.FloorToInt(150 / 1.05f), 15, -1, 2) },
		{ "REIT", new StaticBlockStats(Mathf.FloorToInt(200 / 1.05f), 25, -2, 2) },
		{ "Equity Mutual Fund", new StaticBlockStats(Mathf.FloorToInt(250 / 1.05f), 35, -3, 2) },
		{ "Emerging Markets Equities", new StaticBlockStats(Mathf.FloorToInt(300 / 1.05f), 50, -5, 3) },
		{ "High-Yield Bonds", new StaticBlockStats(Mathf.FloorToInt(350 / 1.05f), 75, -7, 3) },
		{ "Crypto Currency", new StaticBlockStats(Mathf.FloorToInt(450 / 1.05f), 100, -9, 3) },
	};
}
