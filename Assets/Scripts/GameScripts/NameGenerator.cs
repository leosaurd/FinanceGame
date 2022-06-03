using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator
{
    //Multiplier for the stability values
    public static float stabilityMultiplier = 0.5f;
    //Base multiplier for profit values
    public static int profitValue = 20;
    public static List<BlockInstance> usedValues = new List<BlockInstance>();
    public static string GenerateName(BlockType blocktype)
    {
        //Get value from nameValues, return one from the array.
        //Generate name here.
        string[] names = nameValues[blocktype];
        return names[Random.Range(0, names.Length)];
    }
    
    private static readonly Dictionary<BlockType, string[]> nameValues = new Dictionary<BlockType, string[]>()
    {
        {BlockType.Insurance, new string[] {"InsuranceA", "InsuranceB"} },
        {BlockType.LowRiskInvestment, new string[] {"LRIA", "LRIB"} },
        {BlockType.HighRiskInvestment, new string[] {"HRIA", "HRIB"} },
        //{BlockType.Risky, new string[] {"RiskyA", "RiskyB"} },
        //{BlockType.Neutral, new string[] {"NeutralA", "NeutralB"} },
        //{BlockType.Valuable, new string[] {"ValuableA", "ValuableB"} }
    };

    public static BlockInstance GenerateStaticBlock(BlockType blocktype)
    {
        //Get value from nameValues, return one from the array.
        //Generate name here.

        List<BlockInstance> blks = new List<BlockInstance>();
        blks.AddRange(nameStaticValues[blocktype]);
        BlockInstance b = blks[Random.Range(0, blks.Count)];
        return b;
    }
    //Predefined Block Types
    private static readonly Dictionary<BlockType, BlockInstance[]> nameStaticValues = new Dictionary<BlockType, BlockInstance[]>()
    {
        {BlockType.Insurance, new BlockInstance[] 
            {//Name, BlockType, Stability, Cost, Profit
                new BlockInstance("Health Plan", BlockType.Insurance, 0.20f*stabilityMultiplier, 150, 1*profitValue), 
                new BlockInstance("Disability Plan", BlockType.Insurance, 0.3f*stabilityMultiplier, 200, 0*profitValue),
                new BlockInstance("Term Plan", BlockType.Insurance, 0.4f*stabilityMultiplier, 300, 2*profitValue),
                new BlockInstance("Life Plan", BlockType.Insurance, 0.5f*stabilityMultiplier, 450, 1*profitValue)

            } 
        },
        {BlockType.LowRiskInvestment, new BlockInstance[]
            {
                new BlockInstance("Treasury Bills", BlockType.LowRiskInvestment, 0.10f*stabilityMultiplier, 150, 5*profitValue),
                new BlockInstance("Government Bonds", BlockType.LowRiskInvestment, 0.10f*stabilityMultiplier, 200, 10*profitValue),
                new BlockInstance("Savings Bonds", BlockType.LowRiskInvestment, 0.10f*stabilityMultiplier, 250, 15*profitValue),
                new BlockInstance("Fixed Deposit", BlockType.LowRiskInvestment, 0.00f*stabilityMultiplier, 350, 30*profitValue),
                new BlockInstance("Stock Dividends", BlockType.LowRiskInvestment, -0.20f*stabilityMultiplier, 450, 50*profitValue)

            }
        },
        {BlockType.HighRiskInvestment, new BlockInstance[]
            {
                new BlockInstance("ETF", BlockType.HighRiskInvestment, -0.10f*stabilityMultiplier, 150, 15*profitValue),
                new BlockInstance("REIT", BlockType.HighRiskInvestment, -0.20f*stabilityMultiplier, 200, 25*profitValue),
                new BlockInstance("Equity Mutual Fund", BlockType.HighRiskInvestment, -0.30f*stabilityMultiplier, 250, 35*profitValue),
                new BlockInstance("Emerging Markets Equities", BlockType.HighRiskInvestment, -0.50f*stabilityMultiplier, 300, 50*profitValue),
                new BlockInstance("High-Yield Bonds", BlockType.HighRiskInvestment, -0.70f*stabilityMultiplier, 350, 75*profitValue),
                new BlockInstance("Cryptocurrencies", BlockType.HighRiskInvestment, -0.90f*stabilityMultiplier, 450, 100*profitValue),

            }
        }
    };
}
