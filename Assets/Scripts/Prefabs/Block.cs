using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block
{
	public string name;
	public BlockType blockType;
	public IntRange defaultCost;
	public IntRange defaultProfit;
	public FloatRange defaultStability;

	public int cost;
	public int profit;
	public float stability;

	public Block(BlockType blockType, IntRange defaultCost, IntRange defaultProfit, FloatRange defaultStability)
	{
		this.blockType = blockType;
		this.defaultCost = defaultCost;
		this.defaultProfit = defaultProfit;
		this.defaultStability = defaultStability;
	}
}
