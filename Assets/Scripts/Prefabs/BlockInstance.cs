using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInstance
{
    public float stability;
    public int cost;
    public int profit;
    public BlockType blockType;
    public string name;

    public BlockInstance(string name, BlockType blockType, StaticBlockStats defaultStats)
	{
        StaticBlockStats stats = defaultStats.GenerateStats();
        this.blockType = blockType;
        this.stability = stats.stability;
        this.profit = stats.profit;
        this.cost = stats.cost;
        this.name = name;
	}
}
