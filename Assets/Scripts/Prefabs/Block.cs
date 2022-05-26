using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
	public string name;
	public BlockType blockType;
	public IntRange defaultCost;
	public IntRange defaultProfit;
	public FloatRange defaultStability;

	public Block(BlockType blockType, IntRange defaultCost, IntRange defaultProfit, FloatRange defaultStability)
	{
		this.blockType = blockType;
		this.defaultCost = defaultCost;
		this.defaultProfit = defaultProfit;
		this.defaultStability = defaultStability;
	}

	public BlockInstance GenerateBlock()
	{
		float stability = defaultStability.Generate();
		int profit = defaultProfit.Generate();
		int cost = defaultCost.Generate();

		return new BlockInstance(blockType, stability, profit, cost);
	}
}
