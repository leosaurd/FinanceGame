using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticObjectManager : MonoBehaviour
{

	public static readonly Dictionary<string, StaticBlockStats> BlockStats = new()
	{
		{ "Health Plan", new StaticBlockStats(150, -35, 2) },
		{ "Disability Plan", new StaticBlockStats(200, -50, 3) },
		{ "Term Life Plan", new StaticBlockStats(300, -75, 4) },
		{ "Life Plan", new StaticBlockStats(450, -100, 5) },

		{ "Treasury Bills", new StaticBlockStats(150, 5, 1) },
		{ "Government Bonds", new StaticBlockStats(200, 10, 1) },
		{ "Savings Bonds", new StaticBlockStats(250, 15, -1) },
		{ "Fixed Deposit", new StaticBlockStats(350, 30, -0.5f) },
		{ "Dividend-paying stocks", new StaticBlockStats(450, 50, -2) },

		{ "ETF", new StaticBlockStats(150, 15, -1) },
		{ "REIT", new StaticBlockStats(200, 25, -2) },
		{ "Equity Mutual Fund", new StaticBlockStats(250, 35, -3) },
		{ "Emerging Markets Equities", new StaticBlockStats(300, 50, -5) },
		{ "High-Yield Bonds", new StaticBlockStats(350, 75, -7) },
		{ "Cryptocurrencies", new StaticBlockStats(450, 100, -9) },
	};
}
