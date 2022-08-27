using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator
{
    private static readonly Dictionary<BlockType, string[]> nameList = new()
    {
        {BlockType.Insurance, new string[]{"Health Plan", "Disability Plan", "Term Life Plan", "Life Plan"} },
        {BlockType.LowRiskInvestment, new string[]{ "Treasury Bills", "Government Bonds", "Savings Bonds", "Fixed Deposit", "Dividend-paying stocks" } },
        {BlockType.HighRiskInvestment, new string[]{ "ETF", "REIT", "Equity Mutual Fund", "Emerging Markets Equities", "High-Yield Bonds","Crypto Currency" } },
    };

    public static string GenerateName(BlockType blockType)
	{
        string[] names = nameList[blockType];
        int index = Random.Range(0, names.Length);
        return names[index];
	}
}
