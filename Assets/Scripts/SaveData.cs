using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    //Testing locale for savedata management lol
    public Hashtable gamedata = new Hashtable();
    /*
     *     private static readonly Dictionary<BlockType, string[]> nameList = new()
    {   Insurance, High Risk Investment, Low Risk Investment
        {BlockType.Insurance, new string[]{"Health Plan", "Disability Plan", "Term Life Plan", "Life Plan"} },
        {BlockType.LowRiskInvestment, new string[]{ "Treasury Bills", "Government Bonds", "Savings Bonds", "Fixed Deposit", "Dividend-paying stocks" } },
        {BlockType.HighRiskInvestment, new string[]{ "ETF", "REIT", "Equity Mutual Fund", "Emerging Markets Equities", "High-Yield Bonds","Cryptocurrencies" } },
    };
     */
    public void saveData()
    {
        GameManager GM = GameManager.Instance;
        List<BlockInstance> blockList = GM.ownedBlocks;

        //Saves the total number of blocks a player has placed
        gamedata.Add("TotalPlayerBlockCount", blockList.Count);

        //Saves the average stability score
        gamedata.Add("AVGStability", GM.Stability);

        //Saves the time it took for a game
        gamedata.Add("GameTime", GM.gameTime);
        
        //Saves the number of investment blocks placed
        gamedata.Add("InsuranceBlocksPlaced", calculateTotal(blockList, BlockType.Insurance));

        //Saves the number of High risk investment blocks placed
        gamedata.Add("HRIBlocksPlaced", calculateTotal(blockList, BlockType.HighRiskInvestment));

        //Saves the number of Low risk investment blocks placed
        gamedata.Add("LRIBlocksPlaced", calculateTotal(blockList, BlockType.LowRiskInvestment));

        //Number of games played
        gamedata.Add("GamesPlayed", GM.gameCount);

        //Total earnings
        gamedata.Add("TotalEarnings", GM.portfolioValue);

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
}
