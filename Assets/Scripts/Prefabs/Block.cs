using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
	public string name;
	public BlockType blockType;
	public IntRange defaultProfit;
	public IntRange defaultCost;
	public FloatRange defaultStability;

	public Block(BlockType blockType, IntRange defaultProfit, IntRange defaultCost, FloatRange defaultStability)
	{
		this.blockType = blockType;
		this.defaultProfit = defaultProfit;
		this.defaultCost = defaultCost;
		this.defaultStability = defaultStability;
	}

	public BlockInstance GenerateBlock()
	{
		float stability = defaultStability.Generate();
		int profit = defaultProfit.Generate();
		int value = defaultCost.Generate();

		return new BlockInstance(blockType, stability, value, profit);
	}

}
