using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockInstance
{
	public float stability;
	public int cost;
	public int profit;
	public BlockType blockType;
	public string name;
	public int height;
	public TowerColor towerColor;
	public Guid id;
	public BlockInstance(string name, BlockType blockType, StaticBlockStats defaultStats)
	{
		StaticBlockStats stats = defaultStats.GenerateStats();
		this.blockType = blockType;
		this.stability = stats.stability;
		this.profit = stats.profit;
		this.cost = stats.cost;
		this.name = name;
		this.height = stats.height;
		id = new Guid();
	}
}
