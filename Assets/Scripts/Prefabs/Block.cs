using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
	public string name;
	public BlockType blockType;
	public IntRange defaultProfit;
	public IntRange defaultValue;
	public FloatRange defaultStability;

	public Block(BlockType blockType, IntRange defaultProfit, IntRange defaultValue, FloatRange defaultStability)
	{
		this.blockType = blockType;
		this.defaultProfit = defaultProfit;
		this.defaultValue = defaultValue;
		this.defaultStability = defaultStability;
	}

	public BlockInstance GenerateBlock()
	{
		float stability = defaultStability.Generate();
		int profit = defaultProfit.Generate();
		int value = defaultValue.Generate();

		return new BlockInstance(blockType, stability, value, profit);
	}
}
