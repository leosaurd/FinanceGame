using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticObjectManager : MonoBehaviour
{

	public static readonly Dictionary<string, StaticBlockStats> BlockStats = new()
	{
		{ "Health Plan", new StaticBlockStats(150, 1, 4, 1) },
		{ "Disability Plan", new StaticBlockStats(200, 0, 6, 1) },
		{ "Term Life Plan", new StaticBlockStats(300, 2, 8, 1) },
		{ "Life Plan", new StaticBlockStats(450, 1, 10, 1) },

		{ "Treasury Bills", new StaticBlockStats(150, 5, 1, 1) },
		{ "Government Bonds", new StaticBlockStats(200, 10, 1, 1) },
		{ "Savings Bonds", new StaticBlockStats(250, 15, -1, 2) },
		{ "Fixed Deposit", new StaticBlockStats(350, 30, -0.5f, 2) },
		{ "Dividend-paying stocks", new StaticBlockStats(450, 50, -2, 2) },

		{ "ETF", new StaticBlockStats(150, 15, -1, 2) },
		{ "REIT", new StaticBlockStats(200, 25, -2, 2) },
		{ "Equity Mutual Fund", new StaticBlockStats(250, 35, -3, 2) },
		{ "Emerging Markets Equities", new StaticBlockStats(300, 50, -5, 3) },
		{ "High-Yield Bonds", new StaticBlockStats(350, 75, -7, 3) },
		{ "Cryptocurrencies", new StaticBlockStats(450, 100, -9, 3) },
	};
}
